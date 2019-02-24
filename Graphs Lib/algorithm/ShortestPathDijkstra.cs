using graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public class ShortestPathDijkstra<T> : IncrementalEdgeEvaluator<T> {

        public override int Compare(Edge<T> x, Edge<T> y) {

            double a = x.From.Distance + x.Weight;
            double b = y.From.Distance + y.Weight;

            CompareCount++;

            if (a > b)
                return +1;
            else if (a < b)
                return -1;
            return 0;
        }
    }
}

