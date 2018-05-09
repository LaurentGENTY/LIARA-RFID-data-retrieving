using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Antennas
    {
        private Dictionary<string, string> list;

        public Antennas()
        {
            this.list = new Dictionary<string, string>();

            this.list.Add("RFID1", "000000000000000000000000000000000000");
            this.list.Add("RFID2", "000000000000000000000000000000000000");
            this.list.Add("RFID3", "000000000000000000000000000000000000");
            this.list.Add("RFID4", "000000000000000000000000000000000000");
            this.list.Add("RFID5", "64c8ed35-aa24-4968-842e-36058906539d");
            this.list.Add("RFID6", "40e7822a-37db-4bb0-b639-6081fb7d69a6");
            this.list.Add("RFID7", "082a43e9-5492-4ebe-9204-77f61980148d");
            this.list.Add("RFID8", "715f6d37-b313-479c-bfa0-d7542cb3e28c");
        }

        public Dictionary<string,string> getList()
        {
            return this.list;
        }
    }
}
