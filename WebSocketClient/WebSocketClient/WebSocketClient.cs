using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
//using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WebSocketClient
{
    public partial class WebSocketClient : Form
    {
        private WebSocket client;

        private Antennas antennas;
        private Tags tags;
        private Distances distances;
        private Angles angles;

        private bool filter;

        public WebSocketClient()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.antennas = new Antennas();
            this.tags = new Tags();
            this.distances = new Distances();
            this.angles = new Angles();


            foreach (KeyValuePair<string, string> a in antennas.getList())
            {
                this.listAntennas.Items.Add(a.Key);

            }
            foreach (KeyValuePair<string, string> t in tags.getList())
            {
                this.listTags.Items.Add(t.Key);
            }
            foreach (double d in distances.getList())
            {
                this.listDistances.Items.Add(d);

            }
            foreach (double a in angles.getList())
            {
                this.listAngles.Items.Add(a);
            }

            filter = true;

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (serverUrl.Text != "" && serverUrl.Text != null)
            {



                /*
                  using (client = new WebSocket(serverUrl.Text))
                  {


                 */

                if ((this.filter == true && this.tagObject.Text != null && this.tagObject.Text != "") || this.filter == false)
                {
                    connectButton.Enabled = false;
                    disconnectButton.Enabled = true;
                    serverUrl.Enabled = false;

                    //init les premières lignes du CSV : colonnes SI LE FICHIER NEXISTE PAS
                    //faire une fonction qui vérifie les filtres ; verifie l'existence du fichier ; si oui delete TOUT sauf les colonnes
                    //si non créé les colonnes

                    //genere le filePath ici

                    client = new WebSocket(serverUrl.Text);

                    client.ConnectAsync();

                    client.OnOpen += (sender1, e1) =>
                    {
                        Console.WriteLine("CONNECTING TO " + serverUrl.Text + " .." + e1.ToString());
                    };

                    client.OnMessage += (sender1, e1) =>
                    {
                        if (e1.IsText)
                        {
                            JArray a = JArray.Parse(e1.Data);

                            //Console.WriteLine(a);
                            //Console.WriteLine("---------0--------");
                            //Console.WriteLine(a[0]);
                            //Console.WriteLine("---------1--------");
                            //Console.WriteLine(a[1]);

                            StringBuilder csv = new StringBuilder();

                            for (int i = 0; i < a.Count; i++)
                            {
                                //si l'objet est sélectionné et qu'il est dans une row d'un enregistrement ie. s'il est présent et qu'on veut le capter
                                //EXEMPLE : si on a le sel et le poivre : si je clique sur Sel alors je n'aurais que les parties de JArray qui sont pour le sel
                                if (a[i]["RFIDTagNames_ID_FK"].ToString() == this.labelIDObject.Text && this.filter == true)
                                {
                                    writingCSV(csv, a[i]);
                                }

                                //File.WriteAllText(filePath, csv.ToString());
                            }

                            this.Invoke((MethodInvoker)(() => messages.Items.Add(a.ToString())));

                            return;
                        }
                        if (e1.IsBinary)
                        {
                            Console.WriteLine("Server says: " + e1.RawData);
                            return;
                        }
                    };

                    client.OnClose += (sender1, e1) =>
                    {
                        Console.WriteLine("Code : " + e1.Code);
                        Console.WriteLine("Reason : " + e1.Reason);
                        Console.WriteLine("CLOSING ...");


                    };
                }
                else if (this.filter == true)
                {
                    Console.WriteLine("Please, select filters : object, distance and angle ..");
                }
            }
            else
            {
                Console.WriteLine("Please, enter an URL ..");
            }
        }

        private void writingCSV(StringBuilder csv, JToken jToken)
        {
            //liste de string que l'on va ajouter dans la row
            //text[0] = timestamp
            //text[1-8] = les rssi de chaque antenne

            string[] text = new string[9];

            //on recup le temps
            text[0] = jToken["TimeStamp"].ToString();


            //permet de recup le numéro de l'antenne afin de le mettre dans la case de CSV adéquat
            if (this.antennas.getRevert().ContainsKey(jToken["RFID_Antennas_ID_FK"].ToString()))
            {
                Console.WriteLine("good");
            }

            /*string second = image.ToString();*/


            var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", text);
            csv.AppendLine(newLine);

            Console.WriteLine(jToken.ToString());

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            disconnectButton.Enabled = false;
            connectButton.Enabled = true;
            serverUrl.Enabled = true;
            client.CloseAsync();
            //client = null;
        }

        private void listAntennas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listAntennas.SelectedItem.ToString();

            this.tagAntenna.Text = curItem;

            this.labelAntenna.Text = curItem;

            string idAntenna = this.antennas.getList()[curItem];

            this.labelIDAntenna.Text = idAntenna;

        }

        private void listTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listTags.SelectedItem.ToString();

            this.tagObject.Text = curItem;

            this.labelObject.Text = curItem;

            string idObject = this.tags.getList()[curItem];

            this.labelIDObject.Text = idObject;

            changeFileName();
        }

        private void tagAntenna_TextChanged(object sender, EventArgs e)
        {
            if (this.antennas.getList().ContainsKey(this.tagAntenna.Text))
            {
                this.labelAntenna.Text = this.tagAntenna.Text;

                string idAntenna = this.antennas.getList()[this.tagAntenna.Text];

                this.labelIDAntenna.Text = idAntenna;
            }

        }

        private void tagObject_TextChanged(object sender, EventArgs e)
        {
            if (this.tags.getList().ContainsKey(this.tagObject.Text))
            {
                this.labelObject.Text = this.tagObject.Text;

                string idObject = this.tags.getList()[this.tagObject.Text];

                this.labelIDObject.Text = idObject;
            }
        }

        private void listDistances_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listDistances.SelectedItem.ToString();

            this.currentDistance.Text = curItem;

            changeFileName();

        }

        private void listAngles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listAngles.SelectedItem.ToString();

            this.currentAngle.Text = curItem;

            changeFileName();

        }

        private void changeFileName()
        {
            string finalFile = "";

            if (this.tagObject.Text != null && this.tagObject.Text != "")
            {
                finalFile += this.tagObject.Text + "_";
            }

            if (this.currentDistance.Text != null && this.currentDistance.Text != "" && this.currentDistance.Text != "*****")
            {
                finalFile += this.currentDistance.Text + "_";
            }

            if (this.currentAngle.Text != null && this.currentAngle.Text != "" && this.currentAngle.Text != "***")
            {
                finalFile += this.currentAngle.Text;
            }

            this.fileName.Text = finalFile;
        }

        private void testButton(object sender, EventArgs e)
        {
            this.filter = !this.filter;
            if (!this.filter)
            {
                this.filters.Text = "OFF";
            }
            else
            {
                this.filters.Text = "ON";
            }
        }

        #region POUBELLE FAIL
        // POUBELLE FAILS
        private void serverUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void labelObject_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void cleanButton_Click(object sender, EventArgs e)
        {
            this.tagAntenna.Text = "";
            this.tagObject.Text = "";

            this.labelObject.Text = "";
            this.labelIDObject.Text = "";
            this.labelAntenna.Text = "";
            this.labelIDAntenna.Text = "";

            this.currentDistance.Text = "";
            this.currentAngle.Text = "";

            this.fileName.Text = "";

        }
    }
}

