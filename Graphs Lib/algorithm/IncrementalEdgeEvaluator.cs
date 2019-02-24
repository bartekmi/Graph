using graph;
using graph.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public abstract class IncrementalEdgeEvaluator : IComparer<Edge> {

        public static int CompareCount = 0;
        protected virtual void PostProcessTreeNode(Node parent, Node child, Edge edge) { }

        public Graph Calculate(Graph graph, Node start) {
            Graph mst = new Graph();

            var nonTree = new HashSet<Node>(graph.Nodes);
            var tree = new HashSet<Node>();
            var oldToNew = new Dictionary<Node, Node>();
            var sortedOutgoingEdges = new SortedDictionary<Edge, Edge>((IComparer<Edge>)this);

            MoveNode(start, nonTree, tree, mst, oldToNew, sortedOutgoingEdges);

            while (nonTree.Count > 0) {
                Edge shortest = sortedOutgoingEdges.First().Value;

                Node newNode = MoveNode(shortest.To, nonTree, tree, mst, oldToNew, sortedOutgoingEdges);
                Node parentNode = oldToNew[shortest.From];
                parentNode.Outgoing.Add(new Edge() {
                    Weight = shortest.Weight,
                    From = parentNode,
                    To = newNode
                });

                PostProcessTreeNode(parentNode, newNode, shortest);
            }

            return mst;
        }

        private Node MoveNode(Node node, HashSet<Node> nonTree, HashSet<Node> tree, Graph mst,
            Dictionary<Node, Node> oldToNew, SortedDictionary<Edge, Edge> sortedOutgoingEdges) {

            nonTree.Remove(node);
            tree.Add(node);
            Node newNode = new Node() { Data = node.Data };
            mst.Nodes.Add(newNode);
            oldToNew[node] = newNode;

            foreach (Edge edge in node.Outgoing) {
                if (tree.Contains(edge.To))
                    sortedOutgoingEdges.Remove(edge.BackEdge);
                else
                    sortedOutgoingEdges.Add(edge, edge);
            }

            return newNode;
        }

        public abstract int Compare(Edge x, Edge y);
    }
}
