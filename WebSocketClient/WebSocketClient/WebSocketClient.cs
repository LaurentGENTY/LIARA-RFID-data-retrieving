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

        private DateTime beginning;
        private DateTime ending;

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

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
                //méthode propre pour utiliser le WebSocketClient
                /*
                  using (client = new WebSocket(serverUrl.Text))
                  {
                 */

                if ((this.filter == true && this.tagObject.Text != null && this.tagObject.Text != "") || this.filter == false)
                {
                    connectButton.Enabled = false;
                    disconnectButton.Enabled = true;
                    serverUrl.Enabled = false;

                    createFolder(this.tagObject.Text);

                    beginning = DateTime.Now;

                    this.Invoke((MethodInvoker)(() => progressBar.Value = 0));

                    //correspond au nombre d'échantillons ajoutés dans les lignes : on en veut 100 au maximum
                    int n = 0;

                    //init les premières lignes du CSV : colonnes SI LE FICHIER NEXISTE PAS
                    //faire une fonction qui vérifie les filtres ; verifie l'existence du fichier ; si oui delete TOUT sauf les colonnes
                    //si non créé les colonnes

                    client = new WebSocket(serverUrl.Text);

                    //variables d'écriture du fichier
                    StringBuilder format = new StringBuilder();
                    string filePath = "xp/"+ this.tagObject.Text + "/" + this.fileName.Text + this.formatFile.Text;

                    //on créer le fichier et on l'init de manière ASYNCHRONE : sinon l'écriture + ouverture peut provoquer des conflits
                    initFile(filePath, format);

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

                            bool canAppend = false;

                            //variables d'écriture du fichier
                            StringBuilder csv = new StringBuilder();
                            string filePathBis = "xp/" + this.tagObject.Text + "/" + this.fileName.Text + this.formatFile.Text;

                            //liste de string que l'on va ajouter dans la row
                            //text[0] = timestamp
                            //text[1-8] = les rssi de chaque antenne
                            string[] text = new string[9];

                            initText(text);

                            //on recup le temps : dans une row tous les timestamp sont les memes
                            //EDIT : si l'on récupère juste le timestamp et qu'on le met en toString on enlève les millisecond
                            //on doit donc passer par un dateTime et le reparse
                            var datetime = a[0]["TimeStamp"];
                            var result = datetime.ToObject<DateTime>();
                            text[0] = result.ToString("yyyy-MM-dd HH:mm:ss.fff");

                            Console.WriteLine(text[0]);

                            //pour toutes les cases du Json Array
                            for (int i = 0; i < a.Count; i++)
                            {
                                //si l'objet est sélectionné et qu'il est dans une row d'un enregistrement ie. s'il est présent et qu'on veut le capter
                                //EXEMPLE : si on a le sel et le poivre : si je clique sur Sel alors je n'aurais que les parties de JArray qui sont pour le sel
                                if (a[i]["RFIDTagNames_ID_FK"].ToString() == this.labelIDObject.Text && this.filter == true)
                                {
                                    fillText(a[i],text);
                                }

                                //On va verifier que l'antenne que l'on a selectionné recoit bien un signal : en effet si l'on fait des tests
                                //sur l'antenne 1 et que l'objet est assez loin pour ne pas le capter mais que les autres antennes le captent, on veut
                                //seulement les row que l'antenne choisie capte
                                if (a[i]["RFID_Antennas_ID_FK"].ToString() == this.labelIDAntenna.Text && this.filter == true)
                                {
                                    //Console.WriteLine("CAN APPEND");
                                    canAppend = true;
                                }
                            }

                            //a la fin du listage de toutes les cases du JArray on peut ajouter dans le fichier CSV la ligne QUE SI LA DITE
                            //ANTENNE A DETECTE
                            if (canAppend)
                            {
                                this.Invoke((MethodInvoker)(() => messages.Items.Add(a.ToString())));

                                if (this.fileName.Text != null && this.fileName.Text != "")
                                {
                                    //on append la ligne dans le stringbuilder
                                    //exemple : le sel a été detecté par l'antenne 1,2 et 3 au temps t1
                                    //newLine = t1;-25;-45,6;-55;-70;-70;-70;-70;-70;-70;
                                    var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", text);
                                    csv.AppendLine(newLine);

                                    //on ajoute la ligne dans le fichier csv
                                    this.Invoke((MethodInvoker)(() => File.AppendAllText(filePathBis, csv.ToString())));
                                    this.Invoke((MethodInvoker)(() => progressBar.PerformStep()));
                                    
                                    n++;
                                    ending = DateTime.Now;

                                    if((ending - beginning).TotalMilliseconds >= 300000)
                                    {
                                        double averageTime = (ending - beginning).TotalMilliseconds / n;
                                        MessageBox.Show("The retrieving took too much time. The session will be closed..\n There are : " + n + " samples \n For a total of : " + (ending - beginning).Seconds + "seconds \n And an average of : " + averageTime + " of milliseconds per record");
                                        this.Invoke((MethodInvoker)(() => disconnectButton_Click(sender, e)));

                                    }

                                    //si on a les échantillons que l'on souhauite
                                    if (n >= 100)
                                    {
                                        this.Invoke((MethodInvoker)(() => disconnectButton_Click(sender,e)));

                                        double averageTime = (ending - beginning).TotalMilliseconds / n;
                                        MessageBox.Show("The session worked perfectly. \n There are : " + n + " samples \n For a total of : " + (ending - beginning).Seconds + "seconds \n And an average of : "+ averageTime + " of milliseconds per record");

                                    }

                                    //le fichier sera donc constitué d'une liste de lignes avec les RSSI pour les antennes à des timestamp différents
                                }
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
                else if (this.filter == true)
                {
                    MessageBox.Show("Please, select filters : object, distance and angle ..");
                }
            }
            else
            {
                MessageBox.Show("Please, enter an URL ..");
            }
        }

        private void createFolder(string obj)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\xp\\" + obj + "\\";

            if (Directory.Exists(path))
            {
                Console.WriteLine("The directory exists. We're fine !");
            }
            else
            {
                Console.WriteLine("The directory doesn't exist : creation of the directory");
                Directory.CreateDirectory(path);
            }
        }

        private void initFile(string filePath, StringBuilder csv)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    sw.WriteLine("timestamp;rssi1;rssi2;rssi3;rssi4;rssi5;rssi6;rssi7;rssi8;");
                    sw.Flush();
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        private void initText(string[] text)
        {
            text[0] = "";

            //on met toutes les forces de signaux à -99 au début
            for (int i = 1; i < text.Count(); i++)
            {
                text[i] = "-70";
            }
        }

        private void fillText(JToken jToken, string[] text)
        {
            //permet de recup le numéro de l'antenne afin de le mettre dans la case de CSV adéquat
            //dans le current jToken (case du JArray de base) on a qu'une antenne
            if (this.antennas.getRevert().ContainsKey(jToken["RFID_Antennas_ID_FK"].ToString()))
            {
                //on récupère l'index de l'antenne pour le rajouter dans la string de la row
                int index = this.antennas.getRevert()[jToken["RFID_Antennas_ID_FK"].ToString()];

                //on stocke la valeur du RSSI
                string rssi = jToken["RSSIValue"].ToString();

                //PB : virgule sépare les float... cependant pour la suite de l'utilisation on prefere les .
                //donc si le string possede une virgule on le remplace par un point

                if (rssi.Contains(','))
                {
                    rssi = rssi.Replace(',', '.');
                }

                text[index] = rssi;
            }

            //Console.WriteLine(jToken.ToString());
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            disconnectButton.Enabled = false;
            connectButton.Enabled = true;
            serverUrl.Enabled = true;
            client.CloseAsync();
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

        private void label14_Click(object sender, EventArgs e)
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

        /*private void switchButton_Click(object sender, EventArgs e)
        {
            Form data = new data();
            data.Owner = this;
            data.Show();
            this.Hide();
        }*/

        private void clearButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all the data?? It will delete all the data files AND the resulted graphs",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                //Permet de clear tous les files de données
                //On va delete tous les fichiers dans les folders du nom des objets selectionnés :
                //On va delete le folder Sel/, le folder Pates/ ...

                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\xp\\";

                string[] filePaths = Directory.GetDirectories(path);


                foreach (string filePath in filePaths)
                {
                    //on récupère juste le nom du folder, pas le path
                    string folder = filePath.Split('\\').Last();

                    foreach (string obj in this.tags.getList().Keys)
                    {
                        if (obj == folder)
                        {
                            Directory.Delete(filePath, true);
                        }
                    }
                }
            }
        }
    }
}

