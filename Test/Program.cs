using NVOS.Core.Database;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            BidirectionalGraph<int, Edge<int>> graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(new Edge<int>(1, 2));
            graph.AddEdge(new Edge<int>(1, 3));
            foreach(Edge<int> e in graph.OutEdges(1))
            {
                Console.WriteLine(e);
            }
        }
    }
}
