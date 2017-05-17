namespace BluetoothApplication
{
    partial class BluetoothForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.availableDevicesListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.rbClient = new System.Windows.Forms.RadioButton();
            this.communicationMonitorTextBox = new System.Windows.Forms.TextBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.bGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sendButton = new System.Windows.Forms.Button();
            this.connectedBluetoothDevices = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // availableDevicesListBox
            // 
            this.availableDevicesListBox.FormattingEnabled = true;
            this.availableDevicesListBox.ItemHeight = 16;
            this.availableDevicesListBox.Location = new System.Drawing.Point(629, 192);
            this.availableDevicesListBox.Name = "availableDevicesListBox";
            this.availableDevicesListBox.Size = new System.Drawing.Size(385, 148);
            this.availableDevicesListBox.TabIndex = 0;
            this.availableDevicesListBox.DoubleClick += new System.EventHandler(this.availableDevicesListBox_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbServer);
            this.groupBox1.Controls.Add(this.rbClient);
            this.groupBox1.Location = new System.Drawing.Point(629, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Mode";
            // 
            // rbServer
            // 
            this.rbServer.AutoSize = true;
            this.rbServer.Location = new System.Drawing.Point(12, 48);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(71, 21);
            this.rbServer.TabIndex = 3;
            this.rbServer.TabStop = true;
            this.rbServer.Text = "Server";
            this.rbServer.UseVisualStyleBackColor = true;
            // 
            // rbClient
            // 
            this.rbClient.AutoSize = true;
            this.rbClient.Location = new System.Drawing.Point(12, 21);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(64, 21);
            this.rbClient.TabIndex = 2;
            this.rbClient.TabStop = true;
            this.rbClient.Text = "Client";
            this.rbClient.UseVisualStyleBackColor = true;
            // 
            // communicationMonitorTextBox
            // 
            this.communicationMonitorTextBox.AcceptsReturn = true;
            this.communicationMonitorTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.communicationMonitorTextBox.Location = new System.Drawing.Point(3, 20);
            this.communicationMonitorTextBox.Multiline = true;
            this.communicationMonitorTextBox.Name = "communicationMonitorTextBox";
            this.communicationMonitorTextBox.ReadOnly = true;
            this.communicationMonitorTextBox.Size = new System.Drawing.Size(585, 460);
            this.communicationMonitorTextBox.TabIndex = 4;
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(3, 498);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(501, 22);
            this.tbText.TabIndex = 5;
            this.tbText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbText_KeyPress);
            // 
            // bGo
            // 
            this.bGo.Location = new System.Drawing.Point(818, 118);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(75, 23);
            this.bGo.TabIndex = 6;
            this.bGo.Text = "Go";
            this.bGo.UseVisualStyleBackColor = true;
            this.bGo.Click += new System.EventHandler(this.bGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Communication Monitor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(626, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Available Bluetooth Devices";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(510, 497);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 9;
            this.sendButton.Text = "send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // connectedBluetoothDevices
            // 
            this.connectedBluetoothDevices.FormattingEnabled = true;
            this.connectedBluetoothDevices.ItemHeight = 16;
            this.connectedBluetoothDevices.Location = new System.Drawing.Point(629, 387);
            this.connectedBluetoothDevices.Name = "connectedBluetoothDevices";
            this.connectedBluetoothDevices.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.connectedBluetoothDevices.Size = new System.Drawing.Size(385, 84);
            this.connectedBluetoothDevices.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(626, 367);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Connected Bluetooth Devices";
            // 
            // BluetoothForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 532);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connectedBluetoothDevices);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bGo);
            this.Controls.Add(this.communicationMonitorTextBox);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.availableDevicesListBox);
            this.Name = "BluetoothForm";
            this.Text = "Bluetooth";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox availableDevicesListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbServer;
        private System.Windows.Forms.RadioButton rbClient;
        private System.Windows.Forms.TextBox communicationMonitorTextBox;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button bGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.ListBox connectedBluetoothDevices;
        private System.Windows.Forms.Label label3;
    }
}

