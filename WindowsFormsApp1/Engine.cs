using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]
    public class Engine : IEngine
    {
        public int power;
        public int torque;
        public int resourceStrength;
    }
}
