using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using graph.model;

namespace graph {
    public class GraphBuilder {

        private Random _random = new Random();

        enum BuildStrategy {
            AddEdges,
            SubtractEdges,
        }

        // nodeCount: How many nodes
        // interconnectedness: 0 = tree, 1.0 = fully connected graph
        public void Build(Graph graph, int nodeCount, double interconnectedness, Func<int, object> dataProvider = null) {
            if (interconnectedness < 0.0 || interconnectedness > 1.0)
                throw new ArgumentException("Interconnectedness must be 0.0 <= x <= 1.0");

            BuildStrategy strategy = interconnectedness > 0.5 ? BuildStrategy.SubtractEdges : BuildStrategy.AddEdges;

            CreateNodes(graph, nodeCount, dataProvider);

            if (strategy == BuildStrategy.AddEdges)
                AddEdges(graph, interconnectedness);
            else
                FullConnectThenSubtract(graph, interconnectedness);
        }

        private void CreateNodes(Graph graph, int nodeCount, Func<int, object> dataProvider) {
            for (int ii = 0; ii < nodeCount; ii++) {
                graph.Nodes.Add(new Node() {
                    Data = dataProvider == null ? null : dataProvider(ii),
                    Index = ii
                });
            }
        }

        private void AddEdges(Graph graph, double interconnectedness) {
            // Randomly add nodes to make a tree
            for (int ii = 0; ii < graph.NodeCount; ii++) {
                if (ii > 0) {
                    int from = _random.Next(ii);
                    graph.Connect(from, ii);
                }
            }

            int fullConnectionCount = graph.NodeCount * (graph.NodeCount - 1) / 2;
            int addedAtCreation = graph.NodeCount - 1;        // These many edges were added at creation
            int toCreate = (int)((fullConnectionCount - addedAtCreation) * interconnectedness);
            int created = 0;

            while (created < toCreate) {
                int from = _random.Next(graph.NodeCount);
                int to = _random.Next(graph.NodeCount);

                if (!graph.AllowLoops && from == to)
                    continue;

                if (graph.HasEdge(from, to))
                    continue;

                graph.Connect(from, to);
                created++;
            }
        }

        // Note that this algorithm could leave disjoint graphs, especially if number of nodes is low
        private void FullConnectThenSubtract(Graph graph, double interconnectedness) {
            HashSet<long> edges = new HashSet<long>();

            for (int ii = 0; ii < graph.NodeCount - 1; ii++)
                for (int jj = ii + 1; jj < graph.NodeCount; jj++)
                    edges.Add(ii * graph.NodeCount + jj);       // Encode 2 integers in one long

            // We assume has order is random
            int fullConnectionCount = graph.NodeCount * (graph.NodeCount - 1) / 2;
            int toCreate = (int)(fullConnectionCount * interconnectedness);

            foreach (long item in edges.Take(toCreate)) { 
                int from = (int)(item / graph.NodeCount);
                int to = (int)(item % graph.NodeCount);

                graph.Connect(from, to);
            }
        }
    }
}
