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

        private Dictionary<string, int> revert;

        public Antennas()
        {
            this.list = new Dictionary<string, string>();

            //172.24.24.20
            this.list.Add("RFID1", "8c029971-95a1-48af-bf1e-ac42d9058e4c"); //TK1
            this.list.Add("RFID2", "ad27b222-0373-4a27-bc32-3765d340992a"); //TK1
            this.list.Add("RFID3", "89d79ead-cb88-46eb-9f70-8bf3c9d876b1"); //TK1 
            this.list.Add("RFID4", "44148ab2-683f-435d-9d45-9dcef2610733"); //TK1 
            
            //172.24.24.21
            this.list.Add("RFID5", "64c8ed35-aa24-4968-842e-36058906539d"); //TK2
            this.list.Add("RFID6", "40e7822a-37db-4bb0-b639-6081fb7d69a6"); //TK2
            this.list.Add("RFID7", "082a43e9-5492-4ebe-9204-77f61980148d"); //TK2
            this.list.Add("RFID8", "715f6d37-b313-479c-bfa0-d7542cb3e28c"); //TK2

            this.revert = new Dictionary<string, int>();

            this.revert.Add("8c029971-95a1-48af-bf1e-ac42d9058e4c", 1);
            this.revert.Add("ad27b222-0373-4a27-bc32-3765d340992a", 2);
            this.revert.Add("89d79ead-cb88-46eb-9f70-8bf3c9d876b1", 3); 
            this.revert.Add("44148ab2-683f-435d-9d45-9dcef2610733", 4);
            this.revert.Add("64c8ed35-aa24-4968-842e-36058906539d", 5);
            this.revert.Add("40e7822a-37db-4bb0-b639-6081fb7d69a6", 6);
            this.revert.Add("082a43e9-5492-4ebe-9204-77f61980148d", 7);
            this.revert.Add("715f6d37-b313-479c-bfa0-d7542cb3e28c", 8);

        }

        public Dictionary<string,string> getList()
        {
            return this.list;
        }

        public Dictionary<string, int> getRevert()
        {
            return this.revert;
        }
    }
}
