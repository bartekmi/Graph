using graph;
using graph.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.algorithm {
    public class ShortestPathDijkstra : IncrementalEdgeEvaluator {

        public override int Compare(Edge x, Edge y) {

            double a = x.From.Distance + x.Weight;
            double b = y.From.Distance + y.Weight;

            CompareCount++;

            if (a > b)
                return +1;
            else if (a < b)
                return -1;
            return 0;
        }

        protected override void PostProcessTreeNode(Node parent, Node child, Edge edge) {
            child.Distance = parent.Distance + edge.Weight;
        }
    }
}

