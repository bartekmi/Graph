using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph {
    public class Edge<T> {
        public double Weight;
        public Node<T> From;
        public Node<T> To;
        public Edge<T> BackEdge;
    }
}
