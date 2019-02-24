using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.model {
    public class Node {
        public object Data;
        public int Index;
        public double Distance;     // Used for thinkgs like Dijkstra's Shortest Path
        public List<Edge> Outgoing = new List<Edge>();
    }
}
