using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WindowsFormsApp1
{
    public class BinarySerializer : ISerialize
    {
        public void serialize(string fileName, List<Car> list)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, list);
            }

        }

        public List<Car> deserialize(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<Car> list = new List<Car> { };
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                List<Car> temp = new List<Car> { };
                temp = (List<Car>)formatter.Deserialize(fs);

                foreach (Car p in temp)
                {
                    list.Add(p);
                }
            }
            return list;
        }
    }
}
