using graph.algorithm;
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
        public MainWindow() {
            InitializeComponent();
            uxBtGenerate.Click += (s, e) => Generate();
        }

        private void Generate() {
            GraphBuilder<Point> builder = new GraphBuilder<Point>();
            Graph<Point> graph = new Graph<Point>() { };

            int nodeCount = int.Parse(uxTbNodeCount.Text);
            double interconnectedness = double.Parse(uxTbInterconnectedness.Text);

            builder.Build(graph, nodeCount, interconnectedness);

            SetGraphData(graph);
            MinSpanningTree<Point>.CompareCount = 0;
            Graph<Point> mst = new MinSpanningTree<Point>().Calculate(graph);

            int compareCount = MinSpanningTree<Point>.CompareCount;
            Console.WriteLine(string.Format("Edge Compare Count for {0}: {1}. Divided by n^2: {2}. Lg(n): {3}", 
                nodeCount, 
                compareCount, 
                compareCount / (nodeCount * nodeCount),
                Math.Log(nodeCount, 2.0)));

            PaintGraph(mst);
        }

        private void SetGraphData(Graph<Point> graph) {
            Random _random = new Random();

            foreach (Node<Point> node in graph.Nodes)
                node.Data = new Point() {
                    X = _random.NextDouble() * uxCanvas.ActualWidth,
                    Y = _random.NextDouble() * uxCanvas.ActualHeight,
                };

            foreach (Edge<Point> edge in graph.Edges) {
                Point a = edge.From.Data;
                Point b = edge.To.Data;
                double dx = b.X - a.X;
                double dy = b.Y - a.Y;

                edge.Weight = Math.Sqrt(dx * dx + dy * dy);

                if (uxRbMax.IsChecked == true)
                    edge.Weight = -edge.Weight;
            }
        }

        private void PaintGraph(Graph<Point> graph) {
            uxCanvas.Children.Clear();
            const double RADIUS = 10.0;

            // Paint the Nodes
            foreach (Node<Point> node in graph.Nodes) {

                Ellipse circle = new Ellipse() {
                    Fill = Brushes.Lime,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2.0,
                    Width = RADIUS * 2,
                    Height = RADIUS * 2
                };

                Canvas.SetLeft(circle, node.Data.X - RADIUS);
                Canvas.SetTop(circle, node.Data.Y - RADIUS);

                uxCanvas.Children.Add(circle);
            }

            // Paint the Edges
            foreach (Edge<Point> edge in graph.Edges) {

                Point from = edge.From.Data;
                Point to = edge.To.Data;

                if (edge.Weight > 2 * RADIUS) {
                    from = NudgeByRadius(edge.From.Data, edge.To.Data, RADIUS);
                    to = NudgeByRadius(edge.To.Data, edge.From.Data, RADIUS);
                }

                Line line = new Line() {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2.0,
                    X1 = from.X,
                    Y1 = from.Y,
                    X2 = to.X,
                    Y2 = to.Y,
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
    }
}
