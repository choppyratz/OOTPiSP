using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    interface ISerialize
    {
        void serialize(string fileName, List<Car> list);
        List<Car> deserialize(string fileName);
    }
}
