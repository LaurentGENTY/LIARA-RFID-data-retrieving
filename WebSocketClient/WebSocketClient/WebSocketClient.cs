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

namespace WebSocketClient
{
    public partial class WebSocketClient : Form
    {
        private WebSocket client;

        private Antennas antennas;
        private Tags tags;

        private static UTF8Encoding encoding = new UTF8Encoding();

        public WebSocketClient()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.antennas = new Antennas();
            this.tags = new Tags();

            foreach (KeyValuePair<string, string> a in antennas.getList())
            {
                this.listAntennas.Items.Add(a.Key);

            }
            foreach (KeyValuePair<string, string> t in tags.getList())
            {
                this.listTags.Items.Add(t.Key);
            }



        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (serverUrl.Text != "" && serverUrl.Text != null)
            {

                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
                serverUrl.Enabled = false;

                using (client = new WebSocket(serverUrl.Text))
                {
                    //client.CloseAsync();

                    Console.WriteLine("CONNECTING TO " + serverUrl.Text + " ..");
                    client.ConnectAsync();

                    /*if (this.tagAntenna.Text == null || this.tagAntenna.Text == "")
                    {
                        Console.WriteLine("Please enter a valid Antenna ..");
                    }
                    if (this.tagObject.Text == null || this.tagObject.Text == "")
                    {
                        Console.WriteLine("Please enter a valid object ..");
                    }*/

                    client.OnMessage += (sender1, e1) =>
                    {
                        if (e1.IsText)
                        {
                            JArray a = JArray.Parse(e1.Data);

                            Console.WriteLine(e1.Data);

                            for (int i = 0; i < a.Count; i++)
                            {
                                Console.WriteLine(a[i].ToString());
                            }
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
            }
            else
            {
                Console.WriteLine("Please, enter an URL ..");
            }
        }



        private void disconnectButton_Click(object sender, EventArgs e)
        {
            disconnectButton.Enabled = false;
            connectButton.Enabled = true;
            serverUrl.Enabled = true;
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


        #endregion

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
    }
}

