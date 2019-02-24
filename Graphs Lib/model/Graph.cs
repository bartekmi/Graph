using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph {
    public class Graph<T> {
        public List<Node<T>> Nodes = new List<Node<T>>();
        public bool IsDirected = false;
        public bool AllowLoops = false;

        // Derived
        public int NodeCount { get { return Nodes.Count; } }
        public IEnumerable<Edge<T>> Edges { get { return Nodes.SelectMany(x => x.Outgoing); } }

        internal void Connect(int from, int to, double weight = 1.0) {

            if (!AllowLoops && from == to)
                throw new Exception("Loops not allowed");

            Edge<T> forward = ConnectPrivate(from, to, weight);

            if (!IsDirected) {
                Edge<T> back = ConnectPrivate(to, from, weight);
                forward.BackEdge = back;
                back.BackEdge = forward;
            }
        }

        private Edge<T> ConnectPrivate(int from, int to, double weight) {
            Edge<T> edge = new Edge<T>() {
                From = Nodes[from],
                To = Nodes[to],
                Weight = weight,
            };

            Nodes[from].Outgoing.Add(edge);
            return edge;
        }

        public bool HasEdge(int from, int to) {
            return Nodes[from].Outgoing.Any(x => x.To.Index == to);
        }

        public int CountEdges() {
            return Nodes.Sum(x => x.Outgoing.Count);
        }
    }
}
