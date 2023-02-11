namespace AtisMosGateway
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.TextBox();
            this.lblNumReq = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_xpr_ip = new System.Windows.Forms.Label();
            this.label_evs_ip = new System.Windows.Forms.Label();
            this.checkBox_xpr_lower = new System.Windows.Forms.CheckBox();
            this.checkBox_xpr_upper = new System.Windows.Forms.CheckBox();
            this.checkBox_evs_upper = new System.Windows.Forms.CheckBox();
            this.checkBox_evs_lower = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Web Thread";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(13, 98);
            this.logBox.Margin = new System.Windows.Forms.Padding(4);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(1357, 572);
            this.logBox.TabIndex = 1;
            // 
            // lblNumReq
            // 
            this.lblNumReq.AutoSize = true;
            this.lblNumReq.Location = new System.Drawing.Point(121, 31);
            this.lblNumReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNumReq.Name = "lblNumReq";
            this.lblNumReq.Size = new System.Drawing.Size(149, 17);
            this.lblNumReq.TabIndex = 2;
            this.lblNumReq.Text = "Number of requests: 0";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(922, 28);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(242, 53);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(411, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "XPRESSION MOS gw:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(411, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "EVS MOS gw:";
            // 
            // label_xpr_ip
            // 
            this.label_xpr_ip.AutoSize = true;
            this.label_xpr_ip.Location = new System.Drawing.Point(565, 13);
            this.label_xpr_ip.Name = "label_xpr_ip";
            this.label_xpr_ip.Size = new System.Drawing.Size(46, 17);
            this.label_xpr_ip.TabIndex = 6;
            this.label_xpr_ip.Text = "xpr_ip";
            // 
            // label_evs_ip
            // 
            this.label_evs_ip.AutoSize = true;
            this.label_evs_ip.Location = new System.Drawing.Point(565, 51);
            this.label_evs_ip.Name = "label_evs_ip";
            this.label_evs_ip.Size = new System.Drawing.Size(49, 17);
            this.label_evs_ip.TabIndex = 7;
            this.label_evs_ip.Text = "evs_ip";
            // 
            // checkBox_xpr_lower
            // 
            this.checkBox_xpr_lower.AutoSize = true;
            this.checkBox_xpr_lower.Enabled = false;
            this.checkBox_xpr_lower.Location = new System.Drawing.Point(448, 33);
            this.checkBox_xpr_lower.Name = "checkBox_xpr_lower";
            this.checkBox_xpr_lower.Size = new System.Drawing.Size(63, 21);
            this.checkBox_xpr_lower.TabIndex = 8;
            this.checkBox_xpr_lower.Text = "lower";
            this.checkBox_xpr_lower.UseVisualStyleBackColor = true;
            // 
            // checkBox_xpr_upper
            // 
            this.checkBox_xpr_upper.AutoSize = true;
            this.checkBox_xpr_upper.Enabled = false;
            this.checkBox_xpr_upper.Location = new System.Drawing.Point(518, 33);
            this.checkBox_xpr_upper.Name = "checkBox_xpr_upper";
            this.checkBox_xpr_upper.Size = new System.Drawing.Size(67, 21);
            this.checkBox_xpr_upper.TabIndex = 9;
            this.checkBox_xpr_upper.Text = "upper";
            this.checkBox_xpr_upper.UseVisualStyleBackColor = true;
            // 
            // checkBox_evs_upper
            // 
            this.checkBox_evs_upper.AutoSize = true;
            this.checkBox_evs_upper.Enabled = false;
            this.checkBox_evs_upper.Location = new System.Drawing.Point(518, 71);
            this.checkBox_evs_upper.Name = "checkBox_evs_upper";
            this.checkBox_evs_upper.Size = new System.Drawing.Size(67, 21);
            this.checkBox_evs_upper.TabIndex = 11;
            this.checkBox_evs_upper.Text = "upper";
            this.checkBox_evs_upper.UseVisualStyleBackColor = true;
            // 
            // checkBox_evs_lower
            // 
            this.checkBox_evs_lower.AutoSize = true;
            this.checkBox_evs_lower.Enabled = false;
            this.checkBox_evs_lower.Location = new System.Drawing.Point(448, 71);
            this.checkBox_evs_lower.Name = "checkBox_evs_lower";
            this.checkBox_evs_lower.Size = new System.Drawing.Size(63, 21);
            this.checkBox_evs_lower.TabIndex = 10;
            this.checkBox_evs_lower.Text = "lower";
            this.checkBox_evs_lower.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1688, 683);
            this.Controls.Add(this.checkBox_evs_upper);
            this.Controls.Add(this.checkBox_evs_lower);
            this.Controls.Add(this.checkBox_xpr_upper);
            this.Controls.Add(this.checkBox_xpr_lower);
            this.Controls.Add(this.label_evs_ip);
            this.Controls.Add(this.label_xpr_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblNumReq);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "ATIS MOS GW - Newsroom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label lblNumReq;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_xpr_ip;
        private System.Windows.Forms.Label label_evs_ip;
        private System.Windows.Forms.CheckBox checkBox_xpr_lower;
        private System.Windows.Forms.CheckBox checkBox_xpr_upper;
        private System.Windows.Forms.CheckBox checkBox_evs_upper;
        private System.Windows.Forms.CheckBox checkBox_evs_lower;
    }
}

