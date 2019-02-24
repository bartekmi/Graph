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
        public Edge EdgeToParent;     // Some algorithms reduce the graph to a tree

        public override string ToString() {
            return string.Format("Node {0}. Data = {1}. Distance = {2}. Parent = {3}", Index, Data, Distance, 
                EdgeToParent == null ? "NONE" : EdgeToParent.To.Index.ToString());
        }
    }
}
