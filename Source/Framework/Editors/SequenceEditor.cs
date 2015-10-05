using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace OpenGLF
{
    public class SequenceEditorWindow : Form
    {
        private ToolStrip toolStrip1;
        private Panel panel1;
        private FlowLayoutPanel panel;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private OpenFileDialog openFameDlg;
        public TextureList Value;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton5;
        private ToolStripSeparator toolStripSeparator2;
        PictureBox selectedImg = null;
        public SequenceEditorWindow(TextureList seq)
        {
            InitializeComponent();

            Value = seq;
            Width = 600;
            Height = 400;
            this.Text = "Редактор анимации";
            this.StartPosition = FormStartPosition.CenterParent;
            //
            for (int i = 0; i < seq.Count; i++)
            {
                PictureBox img = new PictureBox();
                img.Parent = panel;
                img.Width = 128;
                img.Height = 128;
                img.SizeMode = PictureBoxSizeMode.Zoom;
                img.Tag = seq[i];
                img.Click += img_Click;

                MemoryStream ms = new MemoryStream();
                ms.Write(seq[i].bytes, 0, seq[i].bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                Bitmap bmp = new Bitmap(ms);
                ms.Dispose();

                img.Image = bmp;
            }
        }

        void img_Click(object sender, EventArgs e)
        {
            if (selectedImg != null)
                selectedImg.BorderStyle = BorderStyle.None;

            selectedImg = (PictureBox)sender;
            selectedImg.BorderStyle = BorderStyle.FixedSingle;
        }

        void ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFameDlg.ShowDialog();
            string[] fname = openFameDlg.FileNames;

            for (int i = 0; i < fname.Length; i++)
            {
                if (File.Exists(fname[i]))
                {
                    Texture tex = new Texture();
                    tex.LoadFromFile(fname[i]);
                    Value.Add(tex);

                    PictureBox img = new PictureBox();
                    img.Parent = panel;
                    img.Width = 128;
                    img.Height = 128;
                    img.SizeMode = PictureBoxSizeMode.Zoom;
                    img.Tag = tex;
                    img.Click += img_Click;

                    MemoryStream ms = new MemoryStream();
                    ms.Write(tex.bytes, 0, tex.bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    Bitmap bmp = new Bitmap(ms);
                    ms.Dispose();

                    img.Image = bmp;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Value.Remove((Texture)selectedImg.Tag);
            selectedImg.Dispose();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (selectedImg != null)
            {
                int ind = Value.IndexOf((Texture)selectedImg.Tag);
                if (ind <= 0) ind = 1;
                Value.Remove((Texture)selectedImg.Tag);
                Value.Insert(ind - 1, (Texture)selectedImg.Tag);
                panel.Controls.SetChildIndex(selectedImg, ind - 1);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (selectedImg != null)
            {
                int ind = Value.IndexOf((Texture)selectedImg.Tag);
                if (ind >= Value.Count) ind = Value.Count-1;
                Value.Remove((Texture)selectedImg.Tag);
                Value.Insert(ind + 1, (Texture)selectedImg.Tag);
                panel.Controls.SetChildIndex(selectedImg, ind + 1);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SequenceEditorWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel = new System.Windows.Forms.FlowLayoutPanel();
            this.openFameDlg = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator1,
            this.toolStripButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(549, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Добавить кадр(ы)";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Удалить кадр";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Передвинуть кадр влево";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Передвинуть кадр вправо";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Сохранить кадр в файл";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(549, 331);
            this.panel1.TabIndex = 2;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(549, 331);
            this.panel.TabIndex = 1;
            // 
            // openFameDlg
            // 
            this.openFameDlg.Multiselect = true;
            // 
            // SequenceEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(549, 356);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SequenceEditorWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
    public class SequenceEditor : UITypeEditor
    {
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if ((context != null) && (provider != null))
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

                if (svc != null)
                {
                    SequenceEditorWindow ipfrm = new SequenceEditorWindow((TextureList)value);
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

    public class TextureSequenceEditor : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> names = new List<string>();
            names.Add("");

            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is TextureSequence)
                    names.Add(Assets.items[i].name);
            }

            return new StandardValuesCollection(names);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            List<TextureSequence> names = new List<TextureSequence>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is TextureSequence)
                    names.Add((TextureSequence)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (TextureSequence s in names)
                {
                    if (s.name == (string)value)
                    {
                        return s;
                    }
                }

                if (String.IsNullOrEmpty((string)value))
                    return null;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
