using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Distances
    {
        private List<double> list;

        public Distances()
        {
            this.list = new List<double>();

            this.list.Add(5);
            this.list.Add(15);
            this.list.Add(30);
            this.list.Add(50);
            this.list.Add(75);
            this.list.Add(100);
            this.list.Add(200);
            this.list.Add(300);

        }

        public List<double> getList()
        {
            return this.list;
        }
    }
}
