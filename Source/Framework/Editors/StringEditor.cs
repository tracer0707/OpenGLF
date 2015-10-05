using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Collections;

namespace OpenGLF
{
    public class StringEditorWindow : Form
    {
        public string Value;
        TextBox text;

        public StringEditorWindow(string str)
        {
            Value = str;
            Width = 415;
            Height = 375;
            this.Text = "Редактор строк";
            this.StartPosition = FormStartPosition.CenterParent;
            FormClosed += StringEditorWindow_FormClosed;
            //
            text = new TextBox();
            text.Parent = this;
            text.Multiline = true;
            text.Dock = DockStyle.Fill;
            text.Text = Value;
            text.ScrollBars = ScrollBars.Vertical;
            System.Drawing.Font font = new System.Drawing.Font("Verdana", 10);
            text.Font = font;
        }

        void StringEditorWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Value = text.Text;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StringEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "StringEditorWindow";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
    public class StringEditor : UITypeEditor
    {
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if ((context != null) && (provider != null))
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

                if (svc != null)
                {
                    using (StringEditorWindow ipfrm = new StringEditorWindow((string)value))
                    {
                        ipfrm.ShowDialog();
                        value = ipfrm.Value;
                    }
                }
            }

            return base.EditValue(context, provider, value);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(
          ITypeDescriptorContext context)
        {
            if (context != null)
                return System.Drawing.Design.UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }
    }
}
