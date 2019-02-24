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
    }
}
