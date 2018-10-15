using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NestedReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MyInfo nested = new MyInfo();

            JObject jsonFile = JObject.Parse(File.ReadAllText(@"test.json"));

            JsonOperation operation = new JsonOperation();
            var obj = operation.GetDataFromJson<MyInfo>(JsonConvert.SerializeObject(jsonFile));
            Console.WriteLine(JsonConvert.SerializeObject(obj));
        }
    }
}
