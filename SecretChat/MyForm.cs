using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace SecretChat
{
    public class LoginForm : Form
    {
        public LoginForm(IMessanger messanger)
        {
            var label = new Label
            {
                Text = "Enter a link",
                Dock = DockStyle.Fill
            };
            var box = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            var button = new Button
            {
                Text = "Log in!",
                Dock = DockStyle.Fill
            };
            Controls.Add(label);
            Controls.Add(box);
            Controls.Add(button);
            messanger.LogIn();

            button.Click += (sender, args) => { messanger.GetLoginLink(box.Text); };
        }
    }
}