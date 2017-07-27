using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(GetData(new []{"Values.Test2","OtherValues.Test4", "Values1.Test5", "Values3.Test8" }), Formatting.Indented));
        }

        public static Dictionary<string,object> GetData(string[] keys)
        {
            var data = new TestData
            {
                Values = new Dictionary<string, object> {{"Test", 1}, {"Test2", 2}},
                Values1 = new Dictionary<string, object> {{"Test5", 5}, {"Test6", 6}},
                Values3 = new Dictionary<string, object> {{"Test7", 7}, {"Test8", 8}},
                OtherValues = new[] {new SomeOtherStuff {Value = 3d, Label = "Test3"}, new SomeOtherStuff { Value = 4d, Label = "Test4" } }
            };
           
            var retVal =
            GetValue(data.Values, nameof(data.Values))
                .Union(GetValue(data.Values1, nameof(data.Values1)))
                .Union(GetValue(data.Values3, nameof(data.Values3)))
                    .ToDictionary(kvp => kvp.Key,kvp=> kvp.Value);

            foreach ( var thing in data.OtherValues)
            {
                retVal.Add($"{nameof(data.OtherValues)}.{thing.Label}", thing.Value);
            }

            return retVal.Where(kvp=>keys.Contains(kvp.Key)).ToDictionary(x=>x.Key,x=>x.Value);
        }

        private static Dictionary<string, object> GetValue(Dictionary<string, object> data, string name )
        {
            Dictionary<string, object> retVal = new Dictionary<string, object>();
            foreach (var kvp in data)
            {
              
                retVal.Add($"{name}.{kvp.Key}", kvp.Value);
            }
            return retVal;
        }
    }
    
    public class TestData
    {
        public Dictionary<string,object> Values { get; set; }
        public Dictionary<string,object> Values1 { get; set; }
        public Dictionary<string,object> Values3 { get; set; }
        public SomeOtherStuff[] OtherValues{get;set;}
    }

    public class SomeOtherStuff
    {
        public double Value { get; set; }
        public string Label { get; set; }
    }
}