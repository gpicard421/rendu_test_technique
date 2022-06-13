namespace CurrencyConverter
{
    internal class Helpers
    {                    
        internal static string[] GetShortestPathForConversion(string from, string to, ExchangeRates rates)
        {
            IEnumerable<string> vertices = GetDistinctCurrencies(rates);
            Tuple<string, string>[] edges = rates.Select(rate => Tuple.Create(rate.FromCurrency, rate.ToCurrency)).ToArray();
            Graph<string> graph = new(vertices, edges);
            Algorithms algorithms = new();
            Func<string, IEnumerable<string>> shortestPath = algorithms.ShortestPathFunction(graph, from);
            return shortestPath(to).ToArray();
        }
        private static IEnumerable<string> GetDistinctCurrencies(ExchangeRates rates)
        {
            List<string> result = new();
            IEnumerable<string> fromCurrency = rates.Select(rate => rate.FromCurrency);
            IEnumerable<string> toCurrency = rates.Select(rate => rate.ToCurrency);
            return fromCurrency.Concat(toCurrency).Distinct();
        }
        //Breadth-First Search algorithm
        private class Graph<T>
        {
            internal Graph() { }
            internal Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
            {
                foreach (var vertex in vertices)
                    AddVertex(vertex);

                foreach (var edge in edges)
                    AddEdge(edge);
            }

            internal Dictionary<T, HashSet<T>> AdjacencyList { get; } = new ();

            private void AddVertex(T vertex)
            {
                AdjacencyList[vertex] = new HashSet<T>();
            }

            private void AddEdge(Tuple<T, T> edge)
            {
                if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
                {
                    AdjacencyList[edge.Item1].Add(edge.Item2);
                    AdjacencyList[edge.Item2].Add(edge.Item1);
                }
            }
        }

        private class Algorithms
        {            
            internal Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start)
            {
                Dictionary<T, T> previous = new();

                Queue<T> queue = new();
                queue.Enqueue(start);

                while (queue.Count > 0)
                {
                    var vertex = queue.Dequeue();
                    foreach (var neighbor in graph.AdjacencyList[vertex])
                    {
                        if (previous.ContainsKey(neighbor))
                            continue;

                        previous[neighbor] = vertex;
                        queue.Enqueue(neighbor);
                    }
                }

                Func<T, IEnumerable<T>> shortestPath = v => {
                    List<T> path = new() { };

                    T? current = v;
                    if (current == null)
                        throw new ArgumentNullException("current");
                    else
                        while (!current.Equals(start))
                        {
                            path.Add(current);
                            current = previous[current];
                        };

                    path.Add(start);
                    path.Reverse();

                    return path;
                };

                return shortestPath;
            }
        }
    }
}
