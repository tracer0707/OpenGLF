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
    public class DictionaryEditorWindow : Form
    {
        public Dictionary<string, object> Value;
        TreeView list;
        PropertyGrid grid;
        Button ok;
        public DictionaryEditorWindow(Dictionary<string, object> dictionary)
        {
            Value = dictionary;
            Width = 415;
            Height = 375;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Text = "Настройки материала";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            list = new TreeView();
            list.Parent = this;
            list.Width = 150;
            list.Height = 300;
            list.Left = 0;
            list.Top = 0;
            list.AfterSelect += list_AfterSelect;
            //
            grid = new PropertyGrid();
            grid.Parent = this;
            grid.Width = 250;
            grid.Height = 300;
            grid.Left = 150;
            grid.Top = 0;
            grid.PropertySort = PropertySort.NoSort;
            grid.ToolbarVisible = false;
            //
            ok = new Button();
            ok.Parent = this;
            ok.Width = 100;
            ok.Height = 24;
            ok.Left = 290;
            ok.Top = 305;
            ok.Text = "OK";
            ok.Visible = true;
            ok.Click += ok_Click;
            //
            for (int i = 0; i < dictionary.Count; i++)
            {
                KeyValuePair<string, object> val = dictionary.ElementAt(i);
                TreeNode node = list.Nodes.Add(val.Key + " (" + val.Value.GetType().Name + ")");
                node.Tag = val.Key;
            }
        }

        void list_AfterSelect(object sender, TreeViewEventArgs e)
        {
            grid.SelectedObject = Value[(string)e.Node.Tag];
        }

        void ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DictionaryEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "DictionaryEditorWindow";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
    public class DictionaryEditor : UITypeEditor
    {
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if ((context != null) && (provider != null))
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

                if (svc != null)
                {
                    DictionaryEditorWindow ipfrm = new DictionaryEditorWindow((Dictionary<string, object>)value);
                    ipfrm.ShowDialog();
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