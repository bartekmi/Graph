using graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public class MinSpanningTree<T> : IncrementalEdgeEvaluator<T> {

        public override int Compare(Edge<T> x, Edge<T> y) {
            CompareCount++;
            if (x.Weight > y.Weight)
                return +1;
            else if (x.Weight < y.Weight)
                return -1;
            return 0;
        }
    }
}

