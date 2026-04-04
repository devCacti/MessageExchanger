namespace MessageExchanger.Client
{
    partial class LoginForm
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
            txtbx_username = new TextBox();
            txtbx_password = new TextBox();
            btn_login = new Button();
            btn_see_password = new Button();
            lbl_login = new Label();
            lnklbl_go_register = new LinkLabel();
            lbl_username = new Label();
            lbl_password = new Label();
            SuspendLayout();
            // 
            // txtbx_username
            // 
            txtbx_username.Location = new Point(12, 62);
            txtbx_username.Name = "txtbx_username";
            txtbx_username.Size = new Size(291, 23);
            txtbx_username.TabIndex = 0;
            // 
            // txtbx_password
            // 
            txtbx_password.Location = new Point(12, 118);
            txtbx_password.Name = "txtbx_password";
            txtbx_password.Size = new Size(252, 23);
            txtbx_password.TabIndex = 1;
            txtbx_password.UseSystemPasswordChar = true;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(193, 147);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(110, 23);
            btn_login.TabIndex = 2;
            btn_login.Text = "Log In";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btn_login_Click;
            // 
            // btn_see_password
            // 
            btn_see_password.Location = new Point(270, 118);
            btn_see_password.Name = "btn_see_password";
            btn_see_password.Size = new Size(33, 23);
            btn_see_password.TabIndex = 4;
            btn_see_password.Text = "Ver";
            btn_see_password.UseVisualStyleBackColor = true;
            // 
            // lbl_login
            // 
            lbl_login.AutoSize = true;
            lbl_login.Font = new Font("Segoe UI", 20F);
            lbl_login.Location = new Point(118, 9);
            lbl_login.Name = "lbl_login";
            lbl_login.Size = new Size(84, 37);
            lbl_login.TabIndex = 5;
            lbl_login.Text = "Login";
            // 
            // lnklbl_go_register
            // 
            lnklbl_go_register.AutoSize = true;
            lnklbl_go_register.Location = new Point(12, 151);
            lnklbl_go_register.Name = "lnklbl_go_register";
            lnklbl_go_register.Size = new Size(96, 15);
            lnklbl_go_register.TabIndex = 6;
            lnklbl_go_register.TabStop = true;
            lnklbl_go_register.Text = "Não tenho conta";
            lnklbl_go_register.LinkClicked += lnklbl_go_register_LinkClicked;
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new Point(12, 44);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(62, 15);
            lbl_username.TabIndex = 7;
            lbl_username.Text = "UserName";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new Point(12, 100);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new Size(57, 15);
            lbl_password.TabIndex = 8;
            lbl_password.Text = "Password";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(315, 182);
            Controls.Add(lbl_password);
            Controls.Add(lbl_username);
            Controls.Add(lnklbl_go_register);
            Controls.Add(lbl_login);
            Controls.Add(btn_see_password);
            Controls.Add(btn_login);
            Controls.Add(txtbx_password);
            Controls.Add(txtbx_username);
            Name = "LoginForm";
            Text = "LoginForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtbx_username;
        private TextBox txtbx_password;
        private Button btn_login;
        private Button btn_see_password;
        private Label lbl_login;
        private LinkLabel lnklbl_go_register;
        private Label lbl_username;
        private Label lbl_password;
    }
}