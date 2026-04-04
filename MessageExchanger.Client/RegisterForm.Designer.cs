namespace MessageExchanger.Client
{
    partial class RegisterForm
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
            btn_register = new Button();
            lnklbl_go_login = new LinkLabel();
            btn_see_confirm_password = new Button();
            btn_see_password = new Button();
            txtbx_username = new TextBox();
            txtbx_firstname = new TextBox();
            txtbx_lastname = new TextBox();
            txtbx_password = new TextBox();
            txtbx_confirm_password = new TextBox();
            lbl_register = new Label();
            lbl_username = new Label();
            lbl_firstname = new Label();
            lbl_lastname = new Label();
            lbl_password = new Label();
            lbl_confirm_password = new Label();
            SuspendLayout();
            // 
            // btn_register
            // 
            btn_register.Location = new Point(230, 277);
            btn_register.Name = "btn_register";
            btn_register.Size = new Size(75, 23);
            btn_register.TabIndex = 0;
            btn_register.Text = "Registar";
            btn_register.UseVisualStyleBackColor = true;
            // 
            // lnklbl_go_login
            // 
            lnklbl_go_login.AutoSize = true;
            lnklbl_go_login.Location = new Point(12, 281);
            lnklbl_go_login.Name = "lnklbl_go_login";
            lnklbl_go_login.Size = new Size(84, 15);
            lnklbl_go_login.TabIndex = 1;
            lnklbl_go_login.TabStop = true;
            lnklbl_go_login.Text = "Já tenho conta";
            // 
            // btn_see_confirm_password
            // 
            btn_see_confirm_password.Location = new Point(269, 234);
            btn_see_confirm_password.Name = "btn_see_confirm_password";
            btn_see_confirm_password.Size = new Size(36, 23);
            btn_see_confirm_password.TabIndex = 2;
            btn_see_confirm_password.Text = "Ver";
            btn_see_confirm_password.UseVisualStyleBackColor = true;
            // 
            // btn_see_password
            // 
            btn_see_password.Location = new Point(269, 183);
            btn_see_password.Name = "btn_see_password";
            btn_see_password.Size = new Size(36, 23);
            btn_see_password.TabIndex = 3;
            btn_see_password.Text = "Ver";
            btn_see_password.UseVisualStyleBackColor = true;
            // 
            // txtbx_username
            // 
            txtbx_username.Location = new Point(12, 76);
            txtbx_username.Name = "txtbx_username";
            txtbx_username.Size = new Size(293, 23);
            txtbx_username.TabIndex = 4;
            // 
            // txtbx_firstname
            // 
            txtbx_firstname.Location = new Point(12, 128);
            txtbx_firstname.Name = "txtbx_firstname";
            txtbx_firstname.Size = new Size(130, 23);
            txtbx_firstname.TabIndex = 5;
            // 
            // txtbx_lastname
            // 
            txtbx_lastname.Location = new Point(175, 128);
            txtbx_lastname.Name = "txtbx_lastname";
            txtbx_lastname.Size = new Size(130, 23);
            txtbx_lastname.TabIndex = 6;
            // 
            // txtbx_password
            // 
            txtbx_password.Location = new Point(12, 184);
            txtbx_password.Name = "txtbx_password";
            txtbx_password.Size = new Size(251, 23);
            txtbx_password.TabIndex = 7;
            // 
            // txtbx_confirm_password
            // 
            txtbx_confirm_password.Location = new Point(12, 234);
            txtbx_confirm_password.Name = "txtbx_confirm_password";
            txtbx_confirm_password.Size = new Size(251, 23);
            txtbx_confirm_password.TabIndex = 8;
            // 
            // lbl_register
            // 
            lbl_register.AutoSize = true;
            lbl_register.Font = new Font("Segoe UI", 20F);
            lbl_register.Location = new Point(102, 9);
            lbl_register.Name = "lbl_register";
            lbl_register.Size = new Size(112, 37);
            lbl_register.TabIndex = 9;
            lbl_register.Text = "Register";
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new Point(12, 58);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(62, 15);
            lbl_username.TabIndex = 10;
            lbl_username.Text = "UserName";
            // 
            // lbl_firstname
            // 
            lbl_firstname.AutoSize = true;
            lbl_firstname.Location = new Point(12, 110);
            lbl_firstname.Name = "lbl_firstname";
            lbl_firstname.Size = new Size(88, 15);
            lbl_firstname.TabIndex = 11;
            lbl_firstname.Text = "Primeiro Nome";
            // 
            // lbl_lastname
            // 
            lbl_lastname.AutoSize = true;
            lbl_lastname.Location = new Point(176, 110);
            lbl_lastname.Name = "lbl_lastname";
            lbl_lastname.Size = new Size(79, 15);
            lbl_lastname.TabIndex = 12;
            lbl_lastname.Text = "Último Nome";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new Point(12, 166);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new Size(57, 15);
            lbl_password.TabIndex = 13;
            lbl_password.Text = "Password";
            // 
            // lbl_confirm_password
            // 
            lbl_confirm_password.AutoSize = true;
            lbl_confirm_password.Location = new Point(12, 216);
            lbl_confirm_password.Name = "lbl_confirm_password";
            lbl_confirm_password.Size = new Size(114, 15);
            lbl_confirm_password.TabIndex = 14;
            lbl_confirm_password.Text = "Confirmar Password";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(317, 314);
            Controls.Add(lbl_confirm_password);
            Controls.Add(lbl_password);
            Controls.Add(lbl_lastname);
            Controls.Add(lbl_firstname);
            Controls.Add(lbl_username);
            Controls.Add(lbl_register);
            Controls.Add(txtbx_confirm_password);
            Controls.Add(txtbx_password);
            Controls.Add(txtbx_lastname);
            Controls.Add(txtbx_firstname);
            Controls.Add(txtbx_username);
            Controls.Add(btn_see_password);
            Controls.Add(btn_see_confirm_password);
            Controls.Add(lnklbl_go_login);
            Controls.Add(btn_register);
            Name = "RegisterForm";
            Text = "RegisterForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_register;
        private LinkLabel lnklbl_go_login;
        private Button btn_see_confirm_password;
        private Button btn_see_password;
        private TextBox txtbx_username;
        private TextBox txtbx_firstname;
        private TextBox txtbx_lastname;
        private TextBox txtbx_password;
        private TextBox txtbx_confirm_password;
        private Label lbl_register;
        private Label lbl_username;
        private Label lbl_firstname;
        private Label lbl_lastname;
        private Label lbl_password;
        private Label lbl_confirm_password;
    }
}