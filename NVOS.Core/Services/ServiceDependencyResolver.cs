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
    public class ServiceDependencyResolver
    {
        private BidirectionalGraph<Type, Edge<Type>> graph;
        private List<Type> knownTypes;

        public ServiceDependencyResolver()
        {
            graph = new BidirectionalGraph<Type, Edge<Type>>();
            knownTypes = new List<Type>();
        }

        public ServiceDependencyResolver(params Type[] types)
        {
            graph = new BidirectionalGraph<Type, Edge<Type>>();
            knownTypes = new List<Type>();
            Register(types);
        }

        private IEnumerable<Type> GetRawDependencies(Type type)
        {
            foreach(ServiceDependencyAttribute attr in type.GetCustomAttributes<ServiceDependencyAttribute>())
            {
                if (attr.Type == type)
                    throw new Exception($"Self-referential dependency detected: {attr.Type.Name}");

                if (!attr.IsRequired)
                    continue;

                yield return attr.Type;
            }
        }

        private void RecursiveResolveDependencies(List<Type> resolved, List<Type> unresolved, Type source)
        {
            unresolved.Add(source);
            foreach (Edge<Type> edge in graph.OutEdges(source))
            {
                Type dependency = edge.Target;
                if (!resolved.Contains(dependency))
                {
                    if (unresolved.Contains(dependency))
                        throw new Exception($"Circular dependency detected: {source.Name} -> {dependency.Name}");

                    RecursiveResolveDependencies(resolved, unresolved, dependency);
                }
            }

            resolved.Add(source);
            unresolved.Remove(source);
        }

        private void RecursiveResolveDependent(List<Type> resolved, List<Type> unresolved, Type source)
        {
            unresolved.Add(source);
            foreach(Edge<Type> edge in graph.InEdges(source))
            {
                Type dependent = edge.Source;
                if (!resolved.Contains(dependent))
                {
                    if (unresolved.Contains(dependent))
                        throw new Exception($"Circular dependency detected: {dependent.Name} -> {source.Name}");

                    RecursiveResolveDependent(resolved, unresolved, dependent);
                }
            }

            resolved.Add(source);
            unresolved.Remove(source);
        }

        private void BuildType(Type type)
        {
            graph.AddVertex(type);
            foreach(Type dependency in GetRawDependencies(type))
            {
                graph.AddVerticesAndEdge(new Edge<Type>(type, dependency));

                if (!knownTypes.Contains(dependency))
                {
                    knownTypes.Add(dependency);
                    BuildType(dependency);
                }
            }
        }

        public void Rebuild()
        {
            graph.Clear();
            foreach (Type type in knownTypes.ToArray())
                BuildType(type);
        }

        public void Register(params Type[] types)
        {
            foreach(Type type in types)
            {
                if (!knownTypes.Contains(type))
                    knownTypes.Add(type);
            }

            Rebuild();
        }

        public void Unregister(params Type[] types)
        {
            foreach (Type type in types)
                knownTypes.Remove(type);

            Rebuild();
        }

        public List<Type> ResolveDependencyOrder(Type type)
        {
            List<Type> resolved = new List<Type>();
            List<Type> seen = new List<Type>();
            RecursiveResolveDependencies(resolved, seen, type);
            return resolved;
        }

        public List<Type> ResolveInverseDependencyOrder(Type type)
        {
            List<Type> resolved = new List<Type>();
            List<Type> seen = new List<Type>();
            RecursiveResolveDependent(resolved, seen, type);
            return resolved;
        }
    }
}
