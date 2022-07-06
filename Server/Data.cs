using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Data
    {
        public float speed;
        public int stopped;
        public bool isOn;

        public Data(float s, int st, bool state)
        {
            speed = s;
            isOn = state;
            stopped = st;
        }
    }
}
