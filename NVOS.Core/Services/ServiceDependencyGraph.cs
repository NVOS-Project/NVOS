using NVOS.Core.Services.Attributes;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Services
{
    public class ServiceDependencyGraph
    {
        private IEnumerable<Type> GetDependencies(Type type)
        {
            foreach(ServiceDependencyAttribute attr in type.GetCustomAttributes<ServiceDependencyAttribute>())
            {
                if (!attr.IsRequired)
                    continue;

                yield return attr.Type;
            }
        }

        private void BuildGraph(BidirectionalGraph<Type, Edge<Type>> graph, Type source, List<Type> seen = null)
        {
            if (seen == null)
                seen = new List<Type>();

            if (seen.Contains(source))
                return;

            seen.Add(source);
            foreach(Type dependency in GetDependencies(source))
            {
                graph.AddEdge(new Edge<Type>(source, dependency));
                BuildGraph(graph, dependency, seen);
            }
        }

        public BidirectionalGraph<Type, Edge<Type>> GetDependencyGraph(Type type)
        {
            BidirectionalGraph<Type, Edge<Type>> graph = new BidirectionalGraph<Type, Edge<Type>>();
            BuildGraph(graph, type);
            return graph;
        }
    }
}
