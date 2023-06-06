using Newtonsoft.Json;
using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Database.Serialization;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            JsonDbValueSerializer serializer = new JsonDbValueSerializer();
            object obj = LogLevel.WARN;
            string serialized = serializer.Serialize(obj);
            Console.WriteLine($"Serialized: {serialized}");
            Console.WriteLine($"Type: {obj.GetType()}");

            Console.WriteLine();
            object deserialized = serializer.Deserialize(serialized);
            Console.WriteLine($"Deserialized: {deserialized}");
            Console.WriteLine($"Type: {deserialized.GetType()}");

            Console.ReadLine();
        }
    }
}
