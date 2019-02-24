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
        protected virtual void CleanWorkingData(Graph graph) {
            foreach (Node node in graph.Nodes)
                node.EdgeToParent = null;
        }
        protected virtual void PostProcessEdge(Edge edge) { }

        // Node.Parent values will be set
        public void Calculate(Graph graph, Node start) {
            CleanWorkingData(graph);

            var nonTree = new HashSet<Node>(graph.Nodes);
            var tree = new HashSet<Node>();
            var sortedOutgoingEdges = new SortedDictionary<Edge, Edge>((IComparer<Edge>)this);

            MoveNode(start, nonTree, tree, sortedOutgoingEdges);

            while (nonTree.Count > 0) {
                Edge shortest = sortedOutgoingEdges.First().Value;
                PostProcessEdge(shortest);

                MoveNode(shortest.To, nonTree, tree, sortedOutgoingEdges);
                shortest.To.EdgeToParent = shortest.BackEdge;
            }
        }

        private void MoveNode(Node node, HashSet<Node> nonTree, HashSet<Node> tree, SortedDictionary<Edge, Edge> sortedOutgoingEdges) {

            nonTree.Remove(node);
            tree.Add(node);

            foreach (Edge edge in node.Outgoing) {
                if (tree.Contains(edge.To))
                    sortedOutgoingEdges.Remove(edge.BackEdge);
                else
                    sortedOutgoingEdges.Add(edge, edge);
            }
        }

        public abstract int Compare(Edge x, Edge y);
    }
}
