namespace SatelliteServer
{
    partial class Window
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
            this.groupBoxOrientation = new System.Windows.Forms.GroupBox();
            this.tbYaw = new System.Windows.Forms.TextBox();
            this.tbPitch = new System.Windows.Forms.TextBox();
            this.tbRoll = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxServos = new System.Windows.Forms.GroupBox();
            this.stabilizeCb = new System.Windows.Forms.CheckBox();
            this.yawTrackBar = new System.Windows.Forms.TrackBar();
            this.pitchTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.captureBn = new System.Windows.Forms.Button();
            this.ipLabel = new System.Windows.Forms.Label();
            this.groupBoxOrientation.SuspendLayout();
            this.groupBoxServos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yawTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxOrientation
            // 
            this.groupBoxOrientation.Controls.Add(this.tbYaw);
            this.groupBoxOrientation.Controls.Add(this.tbPitch);
            this.groupBoxOrientation.Controls.Add(this.tbRoll);
            this.groupBoxOrientation.Controls.Add(this.label3);
            this.groupBoxOrientation.Controls.Add(this.label2);
            this.groupBoxOrientation.Controls.Add(this.label1);
            this.groupBoxOrientation.Location = new System.Drawing.Point(9, 10);
            this.groupBoxOrientation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxOrientation.Name = "groupBoxOrientation";
            this.groupBoxOrientation.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxOrientation.Size = new System.Drawing.Size(135, 97);
            this.groupBoxOrientation.TabIndex = 0;
            this.groupBoxOrientation.TabStop = false;
            this.groupBoxOrientation.Text = "Orientation";
            // 
            // tbYaw
            // 
            this.tbYaw.Location = new System.Drawing.Point(56, 66);
            this.tbYaw.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbYaw.Name = "tbYaw";
            this.tbYaw.Size = new System.Drawing.Size(76, 20);
            this.tbYaw.TabIndex = 5;
            // 
            // tbPitch
            // 
            this.tbPitch.Location = new System.Drawing.Point(56, 41);
            this.tbPitch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbPitch.Name = "tbPitch";
            this.tbPitch.Size = new System.Drawing.Size(76, 20);
            this.tbPitch.TabIndex = 4;
            // 
            // tbRoll
            // 
            this.tbRoll.Location = new System.Drawing.Point(56, 18);
            this.tbRoll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbRoll.Name = "tbRoll";
            this.tbRoll.Size = new System.Drawing.Size(76, 20);
            this.tbRoll.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Yaw";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Pitch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Roll";
            // 
            // groupBoxServos
            // 
            this.groupBoxServos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxServos.Controls.Add(this.stabilizeCb);
            this.groupBoxServos.Controls.Add(this.yawTrackBar);
            this.groupBoxServos.Controls.Add(this.pitchTrackBar);
            this.groupBoxServos.Controls.Add(this.label4);
            this.groupBoxServos.Controls.Add(this.label5);
            this.groupBoxServos.Location = new System.Drawing.Point(148, 10);
            this.groupBoxServos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxServos.Name = "groupBoxServos";
            this.groupBoxServos.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxServos.Size = new System.Drawing.Size(368, 97);
            this.groupBoxServos.TabIndex = 1;
            this.groupBoxServos.TabStop = false;
            this.groupBoxServos.Text = "Servos";
            // 
            // stabilizeCb
            // 
            this.stabilizeCb.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.stabilizeCb.AutoSize = true;
            this.stabilizeCb.Location = new System.Drawing.Point(298, 75);
            this.stabilizeCb.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.stabilizeCb.Name = "stabilizeCb";
            this.stabilizeCb.Size = new System.Drawing.Size(65, 17);
            this.stabilizeCb.TabIndex = 5;
            this.stabilizeCb.Text = "Stabilize";
            this.stabilizeCb.UseVisualStyleBackColor = true;
            this.stabilizeCb.CheckedChanged += new System.EventHandler(this.stabilizeCb_CheckedChanged);
            // 
            // yawTrackBar
            // 
            this.yawTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yawTrackBar.AutoSize = false;
            this.yawTrackBar.LargeChange = 100;
            this.yawTrackBar.Location = new System.Drawing.Point(38, 41);
            this.yawTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.yawTrackBar.Maximum = 8000;
            this.yawTrackBar.Minimum = 4000;
            this.yawTrackBar.Name = "yawTrackBar";
            this.yawTrackBar.Size = new System.Drawing.Size(325, 29);
            this.yawTrackBar.SmallChange = 10;
            this.yawTrackBar.TabIndex = 4;
            this.yawTrackBar.TickFrequency = 100;
            this.yawTrackBar.Value = 6000;
            // 
            // pitchTrackBar
            // 
            this.pitchTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchTrackBar.AutoSize = false;
            this.pitchTrackBar.LargeChange = 100;
            this.pitchTrackBar.Location = new System.Drawing.Point(39, 12);
            this.pitchTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pitchTrackBar.Maximum = 8000;
            this.pitchTrackBar.Minimum = 4000;
            this.pitchTrackBar.Name = "pitchTrackBar";
            this.pitchTrackBar.Size = new System.Drawing.Size(324, 29);
            this.pitchTrackBar.SmallChange = 10;
            this.pitchTrackBar.TabIndex = 3;
            this.pitchTrackBar.TickFrequency = 100;
            this.pitchTrackBar.Value = 6000;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 44);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Yaw";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Pitch";
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(9, 111);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(508, 353);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // captureBn
            // 
            this.captureBn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.captureBn.Location = new System.Drawing.Point(432, 470);
            this.captureBn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.captureBn.Name = "captureBn";
            this.captureBn.Size = new System.Drawing.Size(84, 24);
            this.captureBn.TabIndex = 3;
            this.captureBn.Text = "Capture";
            this.captureBn.UseVisualStyleBackColor = true;
            this.captureBn.Click += new System.EventHandler(this.captureBn_Click);
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(10, 470);
            this.ipLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(61, 13);
            this.ipLabel.TabIndex = 4;
            this.ipLabel.Text = "IP Address:";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 503);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.captureBn);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.groupBoxServos);
            this.Controls.Add(this.groupBoxOrientation);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Window";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.Load += new System.EventHandler(this.Window_Load);
            this.groupBoxOrientation.ResumeLayout(false);
            this.groupBoxOrientation.PerformLayout();
            this.groupBoxServos.ResumeLayout(false);
            this.groupBoxServos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yawTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOrientation;
        private System.Windows.Forms.TextBox tbYaw;
        private System.Windows.Forms.TextBox tbPitch;
        private System.Windows.Forms.TextBox tbRoll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxServos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar yawTrackBar;
        private System.Windows.Forms.TrackBar pitchTrackBar;
        private System.Windows.Forms.CheckBox stabilizeCb;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button captureBn;
        private System.Windows.Forms.Label ipLabel;

    }
}

