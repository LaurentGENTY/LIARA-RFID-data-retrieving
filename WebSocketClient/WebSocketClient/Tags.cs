using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Tags
    {
        private Dictionary<string, string> list;

        public Tags()
        {
            this.list = new Dictionary<string, string>();

            this.list.Add("Sel", "00000000001B2A4601001994");
            this.list.Add("Chaudron1", "00000000001B2A4601001987");
            this.list.Add("Chaudron2", "000000000000000000002740");
            this.list.Add("Chaudron3", "000000000000000000002741");
            this.list.Add("BocalPates1", "000000000000000000002748");
            this.list.Add("BocalPates2", "000000000000000000002765");

        }

        public Dictionary<string, string> getList()
        {
            return this.list;
        }
    }
}
