using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageExchanger.Client
{
    public partial class LoginForm : Form
    {
        // IMPORTANTE
        // O login form é o primeiro formulário, ou seja, este está sempre aberto, apenas é escondido quando necessário.

        // O login Form apenas tem a função initialize por isso não precisa de chavetas
        public LoginForm() => InitializeComponent();



        // Quando se clica na link label do registo, abre-se o formulário de registo, o LoginForm Desaparece, ou seja é "Hidden"
        private void lnklbl_go_register_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Esconde o login form
            Hide();

            // Criar Instância do formulário de registo
            RegisterForm registerForm = new RegisterForm();

            // Quando o formulário de registo é fechado, o login volta a aparecer, ou seja, é "Shown"
            // FormClosed é um evento que podemos usar para executar código
            registerForm.FormClosed += (s, args) => Show();

            // Mostrar o formulário de registo
            registerForm.Show();
        }

        // Quando se clica no botão de login, o formulário
        private void btn_login_Click(object sender, EventArgs e)
        {

        }
    }
}
