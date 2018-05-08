namespace WebSocketClient
{
    partial class WebSocketClient
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.messages = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.serverUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tagAntenna = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tagObject = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listAntennas = new System.Windows.Forms.ListBox();
            this.listTags = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.labelAntenna = new System.Windows.Forms.Label();
            this.labelObject = new System.Windows.Forms.Label();
            this.labelIDAntenna = new System.Windows.Forms.Label();
            this.labelIDObject = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // messages
            // 
            this.messages.FormattingEnabled = true;
            this.messages.ItemHeight = 16;
            this.messages.Location = new System.Drawing.Point(12, 335);
            this.messages.Name = "messages";
            this.messages.Size = new System.Drawing.Size(776, 276);
            this.messages.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "URL : ";
            // 
            // serverUrl
            // 
            this.serverUrl.Location = new System.Drawing.Point(55, 29);
            this.serverUrl.Name = "serverUrl";
            this.serverUrl.Size = new System.Drawing.Size(265, 22);
            this.serverUrl.TabIndex = 4;
            this.serverUrl.Text = "ws://172.24.24.2:6093/";
            this.serverUrl.TextChanged += new System.EventHandler(this.serverUrl_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Actived Antenna";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tagAntenna
            // 
            this.tagAntenna.Location = new System.Drawing.Point(117, 87);
            this.tagAntenna.Name = "tagAntenna";
            this.tagAntenna.Size = new System.Drawing.Size(152, 22);
            this.tagAntenna.TabIndex = 7;
            this.tagAntenna.TextChanged += new System.EventHandler(this.tagAntenna_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tag Antenna : ";
            // 
            // tagObject
            // 
            this.tagObject.Location = new System.Drawing.Point(98, 142);
            this.tagObject.Name = "tagObject";
            this.tagObject.Size = new System.Drawing.Size(171, 22);
            this.tagObject.TabIndex = 10;
            this.tagObject.TextChanged += new System.EventHandler(this.tagObject_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Tag Object : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Listened Object";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "Listening to Antenna :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 17);
            this.label8.TabIndex = 11;
            this.label8.Text = "Filters";
            // 
            // listAntennas
            // 
            this.listAntennas.FormattingEnabled = true;
            this.listAntennas.ItemHeight = 16;
            this.listAntennas.Location = new System.Drawing.Point(448, 32);
            this.listAntennas.Name = "listAntennas";
            this.listAntennas.Size = new System.Drawing.Size(115, 292);
            this.listAntennas.TabIndex = 13;
            this.listAntennas.SelectedIndexChanged += new System.EventHandler(this.listAntennas_SelectedIndexChanged);
            // 
            // listTags
            // 
            this.listTags.FormattingEnabled = true;
            this.listTags.ItemHeight = 16;
            this.listTags.Location = new System.Drawing.Point(586, 32);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(202, 292);
            this.listTags.TabIndex = 14;
            this.listTags.SelectedIndexChanged += new System.EventHandler(this.listTags_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(445, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "List of Antennas :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(583, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 17);
            this.label10.TabIndex = 16;
            this.label10.Text = "List of Tags : ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 249);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(134, 17);
            this.label12.TabIndex = 18;
            this.label12.Text = "Listening to Object :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 223);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 17);
            this.label13.TabIndex = 19;
            this.label13.Text = "ID Antenna :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 276);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 17);
            this.label14.TabIndex = 20;
            this.label14.Text = "ID Object :";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(342, 9);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 31);
            this.connectButton.TabIndex = 21;
            this.connectButton.Text = "Open";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Location = new System.Drawing.Point(342, 46);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 31);
            this.disconnectButton.TabIndex = 22;
            this.disconnectButton.Text = "Close";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // labelAntenna
            // 
            this.labelAntenna.AutoSize = true;
            this.labelAntenna.Location = new System.Drawing.Point(165, 199);
            this.labelAntenna.Name = "labelAntenna";
            this.labelAntenna.Size = new System.Drawing.Size(103, 17);
            this.labelAntenna.TabIndex = 23;
            this.labelAntenna.Text = "*******************\r\n";
            // 
            // labelObject
            // 
            this.labelObject.AutoSize = true;
            this.labelObject.Location = new System.Drawing.Point(153, 249);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new System.Drawing.Size(113, 17);
            this.labelObject.TabIndex = 24;
            this.labelObject.Text = "*********************\r\n";
            this.labelObject.Click += new System.EventHandler(this.labelObject_Click);
            // 
            // labelIDAntenna
            // 
            this.labelIDAntenna.AutoSize = true;
            this.labelIDAntenna.Location = new System.Drawing.Point(104, 223);
            this.labelIDAntenna.Name = "labelIDAntenna";
            this.labelIDAntenna.Size = new System.Drawing.Size(268, 17);
            this.labelIDAntenna.TabIndex = 25;
            this.labelIDAntenna.Text = "****************************************************";
            // 
            // labelIDObject
            // 
            this.labelIDObject.AutoSize = true;
            this.labelIDObject.Location = new System.Drawing.Point(93, 276);
            this.labelIDObject.Name = "labelIDObject";
            this.labelIDObject.Size = new System.Drawing.Size(278, 17);
            this.labelIDObject.TabIndex = 26;
            this.labelIDObject.Text = "******************************************************";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 303);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 17);
            this.label11.TabIndex = 27;
            this.label11.Text = "Messages Log :";
            // 
            // WebSocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 623);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.labelIDObject);
            this.Controls.Add(this.labelIDAntenna);
            this.Controls.Add(this.labelObject);
            this.Controls.Add(this.labelAntenna);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.listTags);
            this.Controls.Add(this.listAntennas);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tagObject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tagAntenna);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serverUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messages);
            this.Name = "WebSocketClient";
            this.Text = "Simple WebSocketClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox messages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tagAntenna;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tagObject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listAntennas;
        private System.Windows.Forms.ListBox listTags;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Label labelAntenna;
        private System.Windows.Forms.Label labelObject;
        private System.Windows.Forms.Label labelIDAntenna;
        private System.Windows.Forms.Label labelIDObject;
        private System.Windows.Forms.Label label11;
    }
}

