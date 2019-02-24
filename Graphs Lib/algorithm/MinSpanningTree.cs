using graph;
using graph.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public class MinSpanningTree : IncrementalEdgeEvaluator {

        public override int Compare(Edge x, Edge y) {
            CompareCount++;
            if (x.Weight > y.Weight)
                return +1;
            else if (x.Weight < y.Weight)
                return -1;
            return 0;
        }
    }
}

