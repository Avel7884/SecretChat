using System.Drawing;
using System.Windows.Forms;

namespace SecretChat
{
    public class DialogForm : Form
    {
        public DialogForm(IMessanger messanger, IInteracter interacter)
        {
            var label = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 30),
                Text = interacter.ReadLine()
            };
            var box = new TextBox
            {
                Location = new Point(0, label.Bottom),
                Size = label.Size
            };
            var button = new Button
            {
                Location = new Point(0, box.Bottom),
                Size = label.Size,
                Text = "Increment!"
            };
            Controls.Add(label);
            Controls.Add(box);
            Controls.Add(button);
            Show();
            messanger.CreateChat();
            button.Click += (sender, args) => box.Text = (int.Parse(box.Text) + 1).ToString();
        }

        public void Run(Form previous) => previous.Hide();
    }
}