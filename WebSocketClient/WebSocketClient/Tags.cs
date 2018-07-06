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
            this.list.Add("Poivre", "00000000001B2A4601001993");
            this.list.Add("Chaudron1", "00000000001B2A4601001987");
            this.list.Add("Chaudron2", "000000000000000000002740");
            this.list.Add("Chaudron3", "000000000000000000002741");
            this.list.Add("BocalPates1", "000000000000000000002748");
            this.list.Add("BocalPates2", "000000000000000000002765");
            this.list.Add("Sassis4", "00000000001B2A4601001975");
            this.list.Add("Cuillere1", "000000000000000000002720");
            this.list.Add("Cuillere2", "000000000000000000002721");
            this.list.Add("Couteau1", "000000000000000000002760");
            this.list.Add("Couteau2", "000000000000000000002761");


        }

        public Dictionary<string, string> getList()
        {
            return this.list;
        }
    }
}
