using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    class JSONSerializer : ISerialize
    {
        public void serialize(string fileName, List<Car> list)
        {
            Dictionary<Type, List<Car>> dict = new Dictionary<Type, List<Car>> { };
            foreach (Car p in list)
            {
                List<Car> temp;
                try
                {
                    temp = dict[p.GetType()];
                }
                catch (System.Collections.Generic.KeyNotFoundException e)
                {
                    temp = new List<Car> { };
                }
                temp.Add(p);
                dict.Remove(p.GetType());
                dict.Add(p.GetType(), temp);
            }

            foreach (Type c in dict.Keys)
            {
                //Console.WriteLine(c.Name);
            }

            
            string output = JsonConvert.SerializeObject(dict, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }
        public List<Car> deserialize(string fileName)
        {
            Dictionary<Type, List<object>> info = JsonConvert.DeserializeObject<Dictionary<Type, List<object>>>(File.ReadAllText(fileName));
            List<Car> list = new List<Car> { };
            for (int i = 0; i < info.Values.Count; i++)
            {
                for (int j = 0; j < info.Values.ElementAt(i).Count; j++)
                {
                    var obj = (JObject)info.Values.ElementAt(i)[j];
                    info.Values.ElementAt(i)[j] = obj.ToObject(info.Keys.ElementAt(i));
                    list.Add((Car)info.Values.ElementAt(i)[j]);
                }
            }
            return list;
        }

        
    }
}
