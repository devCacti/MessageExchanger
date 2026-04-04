namespace MessageExchanger.Client
{
    partial class ChatForm
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
            btn_send = new Button();
            txtbx_message = new TextBox();
            flp_chat = new FlowLayoutPanel();
            lbl_username_receiver = new Label();
            btn_exit = new Button();
            SuspendLayout();
            // 
            // btn_send
            // 
            btn_send.Location = new Point(643, 416);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(75, 23);
            btn_send.TabIndex = 0;
            btn_send.Text = "Enviar";
            btn_send.UseVisualStyleBackColor = true;
            // 
            // txtbx_message
            // 
            txtbx_message.Location = new Point(12, 416);
            txtbx_message.Name = "txtbx_message";
            txtbx_message.Size = new Size(625, 23);
            txtbx_message.TabIndex = 1;
            // 
            // flp_chat
            // 
            flp_chat.AutoScroll = true;
            flp_chat.FlowDirection = FlowDirection.TopDown;
            flp_chat.Location = new Point(12, 42);
            flp_chat.Name = "flp_chat";
            flp_chat.Size = new Size(706, 368);
            flp_chat.TabIndex = 2;
            // 
            // lbl_username_receiver
            // 
            lbl_username_receiver.AutoSize = true;
            lbl_username_receiver.Font = new Font("Segoe UI", 16F);
            lbl_username_receiver.Location = new Point(12, 9);
            lbl_username_receiver.Name = "lbl_username_receiver";
            lbl_username_receiver.Size = new Size(98, 30);
            lbl_username_receiver.TabIndex = 3;
            lbl_username_receiver.Text = "No User!";
            // 
            // btn_exit
            // 
            btn_exit.Font = new Font("Segoe UI", 11F);
            btn_exit.Location = new Point(608, 9);
            btn_exit.Name = "btn_exit";
            btn_exit.Size = new Size(110, 27);
            btn_exit.TabIndex = 4;
            btn_exit.Text = "Sair do Chat";
            btn_exit.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(730, 450);
            Controls.Add(btn_exit);
            Controls.Add(lbl_username_receiver);
            Controls.Add(flp_chat);
            Controls.Add(txtbx_message);
            Controls.Add(btn_send);
            Name = "ChatForm";
            Text = "ChatForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_send;
        private TextBox txtbx_message;
        private FlowLayoutPanel flp_chat;
        private Label lbl_username_receiver;
        private Button btn_exit;
    }
}