﻿using System;
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

        //Variables pour les échantillons
        int n;

        //Chronomètre
        private TW t;

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

                if (this.filter == true && this.tagObject.Text != null && this.tagObject.Text != "")
                {
                    connectButton.Enabled = false;
                    disconnectButton.Enabled = true;
                    serverUrl.Enabled = false;
                    this.messages.Items.Clear();
                    createFolder(this.tagObject.Text);

                    t = new TW(this);
                    t.Run();

                    beginning = DateTime.Now;

                    this.Invoke((MethodInvoker)(() => progressBar.Value = 0));

                    //correspond au nombre d'échantillons ajoutés dans les lignes : on en veut 100 au maximum
                    n = 0;

                    //init les premières lignes du CSV : colonnes SI LE FICHIER NEXISTE PAS
                    //faire une fonction qui vérifie les filtres ; verifie l'existence du fichier ; si oui delete TOUT sauf les colonnes
                    //si non créé les colonnes

                    client = new WebSocket(serverUrl.Text);

                    //variables d'écriture du fichier
                    StringBuilder format = new StringBuilder();
                    string filePath = "xp/"+ this.tagObject.Text + "/" + this.fileName.Text + this.formatFile.Text;

                    //on créer le fichier et on l'init de manière ASYNCHRONE : sinon l'écriture + ouverture peut provoquer des conflits
                    initFile(filePath);

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
                            text[0] = result.ToString("HH:mm:ss.fff");

                            Console.WriteLine(text[0]);

                            //pour toutes les cases du Json Array
                            for (int i = 0; i < a.Count; i++)
                            {
                   
                                //On va verifier que l'antenne que l'on a selectionné recoit bien un signal : en effet si l'on fait des tests
                                //sur l'antenne 1 et que l'objet est assez loin pour ne pas le capter mais que les autres antennes le captent, on veut
                                //seulement les row que l'antenne choisie capte
                                if (a[i]["RFID_Antennas_ID_FK"].ToString() == this.labelIDAntenna.Text && a[i]["RFIDTagNames_ID_FK"].ToString() == this.labelIDObject.Text && this.filter == true)
                                {
                                    //Console.WriteLine("CAN APPEND");
                                    fillText(a[i], text);
                                    canAppend = true;
                                }
                            }

                            //a la fin du listage de toutes les cases du JArray on peut ajouter dans le fichier CSV la ligne QUE SI LA DITE
                            //ANTENNE A DETECTE
                            if (canAppend)
                            {
                                Console.WriteLine(a.ToString());

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
                                    
                                    //On augmente le nombre d'échantillons prélevés
                                    n++;
                                    this.Invoke((MethodInvoker)(() => this.samples.Text = n.ToString()));

                                    //si on a les échantillons que l'on souhauite
                                    if (n >= int.Parse(this.samplesLimit.Text))
                                    {
                                        this.Invoke((MethodInvoker)(() => disconnectButton_Click(sender, e)));
                                        
                                        //On arrête le chrono
                                        t.Stop();

                                        double averageTime = (ending - beginning).TotalMilliseconds / n;
                                        MessageBox.Show("The session worked perfectly. \n There are : " + n + " samples \n For a total of : " + (ending - beginning).Seconds + "seconds \n And an average of : " + averageTime + " of milliseconds per record");
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
                else if (this.filter == true && (this.tagObject.Text == "" || this.tagAntenna.Text == ""))
                {
                    MessageBox.Show("Please, select filters : object");
                }

                // PARTIE SCENARIO -----------------------------------------------------------
                // ---------------------------------------------------------------------------

                //Si on veut enregistrer des scénarios et donc sans filtres
                if(this.filter == false && this.fileName.Text != "")
                {
                    connectButton.Enabled = false;
                    disconnectButton.Enabled = true;
                    serverUrl.Enabled = false;
                    this.messages.Items.Clear();

                    t = new TW(this);
                    t.Run();

                    n = 0;

                    client = new WebSocket(serverUrl.Text);

                    initScenar();

                    //cette liste va permettre de savoir quels objets sont détectés dans tout le scénario
                    //et de créer les CSV si l'objet n'a pas déjà été détecté
                    //Par exemple, on fait notre scénario et il y a au début 1 objet détecté, alors dans cette liste il y a 1 ID, et a chaque fois que l'on recevra
                    //des JSON Array, a[i]["TagName"] sera toujours dans objetsDetectes
                    //le but étant lorsqu'on détecte un nouvel objet, il ne soit pas dans cette liste et donc on créer le fichier et on refait le meme mecanisme
                    List<string> objetsDetectes = new List<string>();

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

                            //pour toutes les cases du Json Array
                            for (int i = 0; i < a.Count; i++)
                            {
                                //liste de string que l'on va ajouter dans la row
                                //text[0] = timestamp
                                //text[1-8] = les rssi de chaque antenne
                                string[] text = new string[9];

                                initText(text);

                                //variables d'écriture du fichier
                                StringBuilder csv = new StringBuilder();
                                string tempPath;

                                //On va lire le JSON Array : si l'id de l'objet ne fait pas partie de la liste objetDetectes
                                //alors il n'a pas été détecté durant la simulation
                                //et donc on va l'ajouter et créer le CSV

                                bool enFaitPartie = false;

                                //l'id de l'objet de la case du JSON Array
                                string idObjet = a[i]["RFIDTagNames_ID_FK"].ToString();

                                //on va vérifier dans les objets qui ont déjà été détecté durant le début de la connexion
                                foreach(string o in objetsDetectes)
                                {
                                    //si l'objet a déjà été détecté alors on va stocker son id dans une variable permettant de consister un path
                                    if(o == idObjet)
                                    {
                                        enFaitPartie = true;
                                    }
                                }

                                //On récupère le nom de l'objet pour créer le fichier adéquat grâce à l'id
                                string nomObjet = this.tags.getList().FirstOrDefault(x => x.Value == idObjet).Key;
                                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\xp\\" + this.fileName.Text + "\\" + nomObjet + this.formatFile.Text;
                                tempPath = path;

                                //Si enFaitPartie == false cela veut dire que depuis le ddébut la simulation l'objet n'a pas été détecté et donc on créer le csv (fichier)
                                if (!enFaitPartie)
                                {
                                    initFile(path);
                                    //on dit qu'il a été détecté durant la simulation et on l'ajoute à la liste des objets qui ont un CSV qui existe
                                    objetsDetectes.Add(a[i]["RFIDTagNames_ID_FK"].ToString());
                                }

                                //on remplit le tableau
                                Console.WriteLine(a[i]["RFID_Antennas_ID_FK"]);
                                fillText(a[i], text);

                                //on recup le temps
                                var datetime = a[i]["TimeStamp"];
                                var result = datetime.ToObject<DateTime>();
                                text[0] = result.ToString("HH:mm:ss.fff");

                                //on append la ligne dans le stringbuilder
                                //exemple : le sel a été detecté par l'antenne 1,2 et 3 au temps t1
                                //newLine = t1;-25;-45,6;-55;-70;-70;-70;-70;-70;-70;
                                var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", text);
                                csv.AppendLine(newLine);

                                //on ajoute la ligne dans le fichier csv
                                this.Invoke((MethodInvoker)(() => File.AppendAllText(tempPath, csv.ToString())));

                                //On augmente le nombre d'échantillons prélevés
                                n++;
                                this.Invoke((MethodInvoker)(() => this.samples.Text = n.ToString()));

                                //le fichier sera donc constitué d'une liste de lignes avec les RSSI pour les antennes à des timestamp différents
                            }

                            //a la fin d'un message on push le tableau dans la liste des messages
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

                        this.Invoke((MethodInvoker)(() => disconnectButton_Click(sender, e)));

                        //On arrête le chrono
                        t.Stop();

                        double averageTime = (ending - beginning).TotalMilliseconds / n;
                        MessageBox.Show("The session worked perfectly. \n There are : " + n + " samples \n For a total of : " + (ending - beginning).Seconds + "seconds \n And an average of : " + averageTime + " of milliseconds per record");


                    };



                }
                else if(this.filter == false && this.fileName.Text == "")
                {
                    MessageBox.Show("Please, enter a specific name of file ..");
                }
            }
            else
            {
                MessageBox.Show("Please, enter an URL ..");
            }
        }

        private void initScenar()
        {
            //variables d'écriture du fichier
            string filePath = this.fileName.Text + "\\";

            //créer le folder pour le scénar
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\xp\\" + filePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
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
                Directory.CreateDirectory(path+"\\"+"data\\");
                Directory.CreateDirectory(path + "\\" + "data\\2d\\");
                Directory.CreateDirectory(path + "\\" + "data\\2ddistanceangle\\");
                Directory.CreateDirectory(path + "\\" + "data\\2dlinregress\\");
                Directory.CreateDirectory(path + "\\" + "data\\3d\\");
                Directory.CreateDirectory(path + "\\" + "data\\heatmap\\");
                Directory.CreateDirectory(path + "\\" + "data\\deltas\\");
                Directory.CreateDirectory(path + "\\" + "data\\hist\\");

            }
        }

        private void initFile(string filePath)
        {

            if(File.Exists(filePath))
            {
                Console.WriteLine("The file of data already exists : it will be deleted ..");
                File.Delete(filePath);
            }

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

            //on met toutes les forces de signaux à -80 au début
            for (int i = 1; i < text.Count(); i++)
            {
                text[i] = "-80";
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
        }

        public void disconnectButton_Click(object sender, EventArgs e)
        {
            //On arrête le chrono
            t.Stop();

            disconnectButton.Enabled = false;
            connectButton.Enabled = true;
            serverUrl.Enabled = true;
            client.CloseAsync();
        }

        public void end()
        {
            //On arrête le chrono
            t.Stop();

            this.Invoke((MethodInvoker)(() => disconnectButton.Enabled = false));
            this.Invoke((MethodInvoker)(() => connectButton.Enabled = true));
            this.Invoke((MethodInvoker)(() => serverUrl.Enabled = true));
            this.Invoke((MethodInvoker)(() => client.CloseAsync()));

            ending = DateTime.Now;
            double averageTime = (ending - beginning).TotalMilliseconds / n;

            MessageBox.Show("The session is ended. The session will be closed..\n There are : " + n + " samples \n For a total of : " + (ending - beginning).Seconds + "seconds \n And an average of : " + averageTime + " of milliseconds per record");



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
                this.fileName.Enabled = true;
                this.fileName.Text = "";
                this.listAngles.Enabled = false;
                this.listDistances.Enabled = false;
                this.listAntennas.Enabled = false;
                this.listTags.Enabled = false;
                this.labelAntenna.Text = "";
                this.labelIDAntenna.Text = "";
                this.labelObject.Text = "";
                this.labelIDObject.Text = "";
                this.tagAntenna.Text = "";
                this.tagObject.Text = "";
                this.progressBar.Enabled = false;


            }
            else
            {
                this.filters.Text = "ON";
                this.fileName.Enabled = false;
                this.listAngles.Enabled = true;
                this.listDistances.Enabled = true;
                this.listAntennas.Enabled = true;
                this.listTags.Enabled = true;
                this.progressBar.Enabled = true;

            }
        }

        public int getTimeLimit()
        {
            int a = int.Parse(this.timeLimit.Text);

            return a;
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

        public void updateTime(string time)
        {
            this.Invoke((MethodInvoker)(() => this.time.Text = time));
        }

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

