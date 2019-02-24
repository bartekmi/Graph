using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.model {
    public class Edge {
        public double Weight;
        public Node From;
        public Node To;
        public Edge BackEdge;

        public override string ToString() {
            return string.Format("EDGE From {0} to {1}. Weight = {2}", From.Index, To.Index, Weight);
        }
    }
}
