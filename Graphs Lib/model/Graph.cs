using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.model {
    public class Graph {
        public List<Node> Nodes = new List<Node>();
        public bool IsDirected = false;
        public bool AllowLoops = false;

        // Derived
        public int NodeCount { get { return Nodes.Count; } }
        public IEnumerable<Edge> Edges { get { return Nodes.SelectMany(x => x.Outgoing); } }
        public IEnumerable<Edge> TreeEdges {
            get {
                return Nodes.Select(x => x.EdgeToParent).Where(x => x != null);
            }
        }

        internal void Connect(int from, int to, double weight = 1.0) {

            if (!AllowLoops && from == to)
                throw new Exception("Loops not allowed");

            Edge forward = ConnectPrivate(from, to, weight);

            if (!IsDirected) {
                Edge back = ConnectPrivate(to, from, weight);
                forward.BackEdge = back;
                back.BackEdge = forward;
            }
        }

        private Edge ConnectPrivate(int from, int to, double weight) {
            Edge edge = new Edge() {
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
