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

            this.list.Add(0);
            this.list.Add(5);
            this.list.Add(10);
            this.list.Add(15);
            this.list.Add(25);
            this.list.Add(40);
            this.list.Add(60);
            this.list.Add(80);
            this.list.Add(100);
            this.list.Add(120);
            this.list.Add(140);
            this.list.Add(160);
            this.list.Add(180);
            this.list.Add(200);

        }

        public List<double> getList()
        {
            return this.list;
        }
    }
}
