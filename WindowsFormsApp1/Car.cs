using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]
    public class Car
    {
        public string name;
        public int power;
        public int wheelDiametr;
        public int bootVolume;
        public Engine engine;
    }
}
