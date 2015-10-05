using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenGLF
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {

        }

        public static string show(string caption, string text, string def, Form own)
        {
            InputBox box = new InputBox();
            box.Text = caption;
            box.label1.Text = text;
            box.textBox1.Text = def;
            box.Owner = own;
            box.ShowDialog();
            return box.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
