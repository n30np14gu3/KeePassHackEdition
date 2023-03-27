namespace KeePassHackEdition
{
    partial class ActivationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivationForm));
            this.pOfflineActivation = new System.Windows.Forms.Panel();
            this.tActivationResponse = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tActivationRequest = new System.Windows.Forms.TextBox();
            this.bActivateOnline = new System.Windows.Forms.RadioButton();
            this.bActivateOffline = new System.Windows.Forms.RadioButton();
            this.bActivate = new System.Windows.Forms.Button();
            this.bDev = new System.Windows.Forms.Button();
            this.pOfflineActivation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pOfflineActivation
            // 
            this.pOfflineActivation.Controls.Add(this.tActivationResponse);
            this.pOfflineActivation.Controls.Add(this.label2);
            this.pOfflineActivation.Controls.Add(this.label1);
            this.pOfflineActivation.Controls.Add(this.tActivationRequest);
            this.pOfflineActivation.Location = new System.Drawing.Point(12, 58);
            this.pOfflineActivation.Name = "pOfflineActivation";
            this.pOfflineActivation.Size = new System.Drawing.Size(533, 187);
            this.pOfflineActivation.TabIndex = 0;
            this.pOfflineActivation.Visible = false;
            // 
            // tActivationResponse
            // 
            this.tActivationResponse.Location = new System.Drawing.Point(6, 66);
            this.tActivationResponse.Name = "tActivationResponse";
            this.tActivationResponse.Size = new System.Drawing.Size(515, 116);
            this.tActivationResponse.TabIndex = 3;
            this.tActivationResponse.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Activation response:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Activation request:";
            // 
            // tActivationRequest
            // 
            this.tActivationRequest.Location = new System.Drawing.Point(104, 12);
            this.tActivationRequest.Name = "tActivationRequest";
            this.tActivationRequest.Size = new System.Drawing.Size(417, 20);
            this.tActivationRequest.TabIndex = 0;
            // 
            // bActivateOnline
            // 
            this.bActivateOnline.AutoSize = true;
            this.bActivateOnline.Location = new System.Drawing.Point(12, 12);
            this.bActivateOnline.Name = "bActivateOnline";
            this.bActivateOnline.Size = new System.Drawing.Size(95, 17);
            this.bActivateOnline.TabIndex = 1;
            this.bActivateOnline.TabStop = true;
            this.bActivateOnline.Text = "Activate online";
            this.bActivateOnline.UseVisualStyleBackColor = true;
            this.bActivateOnline.CheckedChanged += new System.EventHandler(this.bActivateOnline_CheckedChanged);
            // 
            // bActivateOffline
            // 
            this.bActivateOffline.AutoSize = true;
            this.bActivateOffline.Location = new System.Drawing.Point(12, 35);
            this.bActivateOffline.Name = "bActivateOffline";
            this.bActivateOffline.Size = new System.Drawing.Size(104, 17);
            this.bActivateOffline.TabIndex = 2;
            this.bActivateOffline.TabStop = true;
            this.bActivateOffline.Text = "Offline activation";
            this.bActivateOffline.UseVisualStyleBackColor = true;
            this.bActivateOffline.CheckedChanged += new System.EventHandler(this.bActivateOffline_CheckedChanged);
            // 
            // bActivate
            // 
            this.bActivate.Location = new System.Drawing.Point(12, 251);
            this.bActivate.Name = "bActivate";
            this.bActivate.Size = new System.Drawing.Size(533, 23);
            this.bActivate.TabIndex = 3;
            this.bActivate.Text = "activate";
            this.bActivate.UseVisualStyleBackColor = true;
            this.bActivate.Click += new System.EventHandler(this.bActivate_Click);
            // 
            // bDev
            // 
            this.bDev.Location = new System.Drawing.Point(435, 17);
            this.bDev.Name = "bDev";
            this.bDev.Size = new System.Drawing.Size(75, 23);
            this.bDev.TabIndex = 4;
            this.bDev.Text = "[DEV BTN]";
            this.bDev.UseVisualStyleBackColor = false;
            this.bDev.Click += new System.EventHandler(this.bDev_Click);
            // 
            // ActivationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 281);
            this.Controls.Add(this.bDev);
            this.Controls.Add(this.bActivate);
            this.Controls.Add(this.bActivateOffline);
            this.Controls.Add(this.bActivateOnline);
            this.Controls.Add(this.pOfflineActivation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationForm";
            this.ShowInTaskbar = false;
            this.Text = "Activation";
            this.pOfflineActivation.ResumeLayout(false);
            this.pOfflineActivation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pOfflineActivation;
        private System.Windows.Forms.RadioButton bActivateOnline;
        private System.Windows.Forms.RadioButton bActivateOffline;
        private System.Windows.Forms.Button bActivate;
        private System.Windows.Forms.TextBox tActivationRequest;
        private System.Windows.Forms.RichTextBox tActivationResponse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bDev;
    }
}