using graph.algorithm;
using graph.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace graph {
    public partial class MainWindow : Window {

        private enum EdgePaintMode {
            None,
            Full,
            Tree
        }

        private int NodeCount { get { return int.Parse(uxTbNodeCount.Text); } }
        private double Interconnectedness { get { return double.Parse(uxTbInterconnectedness.Text); } }
        private int? Seed {
            get {
                return int.TryParse(uxTbSeed.Text, out int seed) ? seed : (int?)null;
            }
        }

        private Graph _graph;

        public MainWindow() {
            InitializeComponent();
            uxBtGenerate.Click += (s, e) => Generate();
        }

        #region Generate Graph
        private void Generate() {
            Random random = Seed == null ? new Random() : new Random(Seed.Value);

            GraphBuilder builder = new GraphBuilder(random);
            _graph = new Graph() { };
            builder.Build(_graph, NodeCount, Interconnectedness);

            SetGraphData(_graph, random);
            PaintGraph(EdgePaintMode.None);
        }

        private void SetGraphData(Graph graph, Random random) {

            foreach (Node node in graph.Nodes)
                node.Data = new Point() {
                    X = random.NextDouble() * uxCanvas.ActualWidth,
                    Y = random.NextDouble() * uxCanvas.ActualHeight,
                };

            foreach (Edge edge in graph.Edges) {
                Point a = (Point)edge.From.Data;
                Point b = (Point)edge.To.Data;
                double dx = b.X - a.X;
                double dy = b.Y - a.Y;

                edge.Weight = Math.Sqrt(dx * dx + dy * dy);

                if (uxRbMax.IsChecked == true)
                    edge.Weight = -edge.Weight;
            }
        }
        #endregion

        private void ReactToNodeClick(Node node) {
            Console.WriteLine("Clicked on Node: " + node.Data);

            MinSpanningTree.CompareCount = 0;
            //new MinSpanningTree().Calculate(_graph, node);
            new ShortestPathDijkstra().Calculate(_graph, node);

            int compareCount = MinSpanningTree.CompareCount;
            Console.WriteLine(string.Format("Edge Compare Count for {0}: {1}. Divided by n^2: {2}. Lg(n): {3}",
                NodeCount,
                compareCount,
                compareCount / (NodeCount * NodeCount),
                Math.Log(NodeCount, 2.0)));

            PaintGraph(EdgePaintMode.Tree);
        }

        #region Painting
        private const double RADIUS = 10.0;
        private void PaintGraph(EdgePaintMode mode) {
            uxCanvas.Children.Clear();
            PaintNodes();
            PaintEdges(mode);
            UpdateTooltips();
        }

        private void PaintNodes() {
            foreach (Node node in _graph.Nodes) {

                Ellipse circle = new Ellipse() {
                    Fill = Brushes.Lime,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2.0,
                    Width = RADIUS * 2,
                    Height = RADIUS * 2,
                    Tag = node
                };

                circle.MouseDown += (s, e) => ReactToNodeClick(((FrameworkElement)s).Tag as Node);

                Canvas.SetLeft(circle, ((Point)node.Data).X - RADIUS);
                Canvas.SetTop(circle, ((Point)node.Data).Y - RADIUS);

                uxCanvas.Children.Add(circle);
            }
        }

        private void PaintEdges(EdgePaintMode mode) {
            // If connectedness is reduced, just show all the edges and highlight the tree ones
            if (Interconnectedness < 1.0)
                mode = EdgePaintMode.Full;

            IEnumerable<Edge> edges;
            switch (mode) {
                case EdgePaintMode.None: edges = new List<Edge>(); break;
                case EdgePaintMode.Full: edges = _graph.Edges.Where(x => x.To.Index > x.From.Index); break;
                case EdgePaintMode.Tree: edges = _graph.TreeEdges; break;
                default:
                    throw new Exception();
            }

            foreach (Edge edge in edges) {
                Point from = (Point)edge.From.Data;
                Point to = (Point)edge.To.Data;

                if (edge.Weight > 2 * RADIUS) {
                    from = NudgeByRadius((Point)edge.From.Data, (Point)edge.To.Data, RADIUS);
                    to = NudgeByRadius((Point)edge.To.Data, (Point)edge.From.Data, RADIUS);
                }

                bool isTreeEdge = (edge == edge.From.EdgeToParent || edge.BackEdge == edge.To.EdgeToParent);

                Line line = new Line() {
                    Stroke = isTreeEdge ? Brushes.Red : Brushes.LightGray,
                    StrokeThickness = isTreeEdge ? 4.0 : 1.0,
                    X1 = from.X,
                    Y1 = from.Y,
                    X2 = to.X,
                    Y2 = to.Y,
                    Tag = edge,
                };
                uxCanvas.Children.Add(line);
            }
        }

        private Point NudgeByRadius(Point a, Point b, double radius) {
            Vector v = new Vector(b.X - a.X, b.Y - a.Y);
            v.Normalize();
            Point nudged = a + (v * radius);
            return nudged;
        }

        private void UpdateTooltips() {
            foreach (Shape shape in uxCanvas.Children) 
                shape.ToolTip = shape.Tag.ToString();
        }
        #endregion
    }
}
