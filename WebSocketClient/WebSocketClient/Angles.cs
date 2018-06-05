using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Angles
    {
        private List<double> list;

        public Angles()
        {
            this.list = new List<double>();

            this.list.Add(0);
            this.list.Add(23);
            this.list.Add(45);
            this.list.Add(90);

        }

        public List<double> getList()
        {
            return this.list;
        }
    }
}
