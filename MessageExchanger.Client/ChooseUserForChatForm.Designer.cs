namespace MessageExchanger.Client
{
    partial class ChooseUserForChatForm
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
            btn_continue = new Button();
            btn_back = new Button();
            txtbx_username = new TextBox();
            SuspendLayout();
            // 
            // btn_continue
            // 
            btn_continue.Location = new Point(121, 41);
            btn_continue.Name = "btn_continue";
            btn_continue.Size = new Size(209, 23);
            btn_continue.TabIndex = 0;
            btn_continue.Text = "Continuar";
            btn_continue.UseVisualStyleBackColor = true;
            // 
            // btn_back
            // 
            btn_back.Location = new Point(12, 41);
            btn_back.Name = "btn_back";
            btn_back.Size = new Size(103, 23);
            btn_back.TabIndex = 1;
            btn_back.Text = "Voltar";
            btn_back.UseVisualStyleBackColor = true;
            // 
            // txtbx_username
            // 
            txtbx_username.Location = new Point(12, 12);
            txtbx_username.Name = "txtbx_username";
            txtbx_username.Size = new Size(318, 23);
            txtbx_username.TabIndex = 2;
            // 
            // ChooseUserForChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 76);
            Controls.Add(txtbx_username);
            Controls.Add(btn_back);
            Controls.Add(btn_continue);
            Name = "ChooseUserForChatForm";
            Text = "Choose User";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_continue;
        private Button btn_back;
        private TextBox txtbx_username;
    }
}