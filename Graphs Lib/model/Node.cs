using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph {
    public class Node<T> {
        public T Data;
        public int Index;
        public double Distance;     // Used for thinkgs like Dijkstra's Shortest Path
        public List<Edge<T>> Outgoing = new List<Edge<T>>();
    }
}
