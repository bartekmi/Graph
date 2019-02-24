using graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public abstract class IncrementalEdgeEvaluator<T> : IComparer<Edge> {

        public static int CompareCount = 0;
        protected virtual void PostProcessTreeNode(Node<T> parent, Node<T> child, Edge<T> edge) { }

        public Graph<T> Calculate<T>(Graph<T> graph) {
            Graph<T> mst = new Graph<T>();

            var nonTree = new HashSet<Node<T>>(graph.Nodes);
            var tree = new HashSet<Node<T>>();
            var oldToNew = new Dictionary<Node<T>, Node<T>>();
            var sortedOutgoingEdges = new SortedDictionary<Edge<T>, Edge<T>>((IComparer<Edge<T>>)this);

            Node<T> first = nonTree.FirstOrDefault();
            if (first == null)
                return mst;

            MoveNode(first, nonTree, tree, mst, oldToNew, sortedOutgoingEdges);

            while (nonTree.Count > 0) {
                Edge<T> shortest = sortedOutgoingEdges.First().Value;

                Node<T> newNode = MoveNode(shortest.To, nonTree, tree, mst, oldToNew, sortedOutgoingEdges);
                Node<T> parentNode = oldToNew[shortest.From];
                parentNode.Outgoing.Add(new Edge<T>() {
                    Weight = shortest.Weight,
                    From = parentNode,
                    To = newNode
                });

                PostProcessTreeNode(parentNode, newNode, shortest);
            }

            return mst;
        }

        private Node<T> MoveNode<T>(Node<T> node, HashSet<Node<T>> nonTree, HashSet<Node<T>> tree, Graph<T> mst,
            Dictionary<Node<T>, Node<T>> oldToNew, SortedDictionary<Edge<T>, Edge<T>> sortedOutgoingEdges) {

            nonTree.Remove(node);
            tree.Add(node);
            Node<T> newNode = new Node<T>() { Data = node.Data };
            mst.Nodes.Add(newNode);
            oldToNew[node] = newNode;

            foreach (Edge<T> edge in node.Outgoing) {
                if (tree.Contains(edge.To))
                    sortedOutgoingEdges.Remove(edge.BackEdge);
                else
                    sortedOutgoingEdges.Add(edge, edge);
            }

            return newNode;
        }

        public abstract int Compare(Edge<T> x, Edge<T> y);
    }
}
