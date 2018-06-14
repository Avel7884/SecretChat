using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecretChat
{
    public class LoginForm : Form
    {
        public LoginForm(IMessanger messanger, IInteracter interacter)
        {
            var label = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 30),
                Text = "Write a link"    
            };
            var box = new TextBox
            {
                Location = new Point(0, label.Bottom),
                Size = label.Size
            };
            var button = new Button
            {
                AutoSizeMode = AutoSizeMode.GrowOnly,
                Anchor = AnchorStyles.Bottom,
                Dock = DockStyle.Bottom,
                Text = "Log in!"
            };
            Controls.Add(label);
            Controls.Add(box);
            Controls.Add(button);

            new Action(messanger.LogIn).BeginInvoke(null, null);
            
            button.Click += (sender, args) =>
            {
                interacter.WriteLine(box.Text);
                new DialogForm(messanger, interacter).Run(this);
            };
        }
    }
}