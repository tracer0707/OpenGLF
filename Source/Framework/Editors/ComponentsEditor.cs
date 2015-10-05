using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OpenGLF
{
    public class ComponentsEditorWindow : Form
    {
        public ComponentList value;
        private TreeView tree;
        private PropertyGrid grid;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem добавитьToolStripMenuItem;
        private ToolStripMenuItem графикаToolStripMenuItem;
        private ToolStripMenuItem спрайтToolStripMenuItem;
        private ToolStripMenuItem физикаToolStripMenuItem;
        private ToolStripMenuItem телоToolStripMenuItem;
        private ToolStripMenuItem камераToolStripMenuItem;
        private ToolStripMenuItem скриптToolStripMenuItem;
        private ToolStripMenuItem звукToolStripMenuItem;
        private ToolStripMenuItem источникЗвукаToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem удалитьToolStripMenuItem;
        private ToolStripMenuItem gUIToolStripMenuItem;
        private ToolStripMenuItem меткаToolStripMenuItem;
        private ToolStripMenuItem планToolStripMenuItem;
        private ToolStripMenuItem слушательToolStripMenuItem;

        public ComponentsEditorWindow(ComponentList list)
        {
            InitializeComponent();

            value = list;

            reload();
        }

        void ComponentsEditorWindow_Activated(object sender, EventArgs e)
        {
            Assets.compileScripts(Engine.dependencies);
            grid.SelectedObject = grid.SelectedObject;
        }

        void ComponentsEditorWindow_Load(object sender, EventArgs e)
        {
            Assets.compileScripts(Engine.dependencies);
            grid.SelectedObject = grid.SelectedObject;
        }

        void grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            grid.SelectedObject = grid.SelectedObject;
        }

        void beh_Click(object sender, EventArgs e)
        {
            value.Add(new Behavior());
            reload();
        }

        void cam_Click(object sender, EventArgs e)
        {
            value.Add(new Camera());
            reload();
        }

        void spr_Click(object sender, EventArgs e)
        {
            value.Add(new Sprite());
            reload();
        }

        void bod_Click(object sender, EventArgs e)
        {
            value.Add(new Rigidbody());
            reload();
        }

        void aul_Click(object sender, EventArgs e)
        {
            value.Add(new AudioListener());
            reload();
        }

        void aus_Click(object sender, EventArgs e)
        {
            value.Add(new AudioSource());
            reload();
        }

        void remove_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null)
            {
                if (tree.SelectedNode.Tag is Camera)
                    if (Camera.main == (Camera)tree.SelectedNode.Tag)
                        Camera.main = null;

                value.Remove((Component)tree.SelectedNode.Tag);
                reload();
            }

            grid.SelectedObject = null;
        }

        void reload()
        {
            tree.Nodes.Clear();

            for (int i = 0; i < value.Count; i++)
            {
                string name = value[i].ToString();
                if (value[i] is Camera) name = "Camera";
                if (value[i] is Behavior)
                {
                    if (((Behavior)value[i]).script != null)
                        name = ((Behavior)value[i]).script.name;
                }
                if (value[i] is BlueprintBehavior)
                {
                    if (((BlueprintBehavior)value[i]).blueprint != null)
                        name = ((BlueprintBehavior)value[i]).blueprint.name;
                }

                TreeNode node = tree.Nodes.Add(name);
                node.Tag = value[i];
            }
        }

        void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            grid.SelectedObject = e.Node.Tag;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.графикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.спрайтToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.физикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.телоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.звукToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.источникЗвукаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.слушательToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.меткаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.камераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.скриптToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.планToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.ContextMenuStrip = this.contextMenuStrip1;
            this.tree.Location = new System.Drawing.Point(12, 27);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(179, 318);
            this.tree.TabIndex = 0;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.remove_Click);
            // 
            // grid
            // 
            this.grid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.grid.Location = new System.Drawing.Point(197, 27);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(284, 318);
            this.grid.TabIndex = 1;
            this.grid.ToolbarVisible = false;
            this.grid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.grid_PropertyValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(493, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.графикаToolStripMenuItem,
            this.физикаToolStripMenuItem,
            this.звукToolStripMenuItem,
            this.gUIToolStripMenuItem,
            this.камераToolStripMenuItem,
            this.скриптToolStripMenuItem,
            this.планToolStripMenuItem});
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            // 
            // графикаToolStripMenuItem
            // 
            this.графикаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.спрайтToolStripMenuItem});
            this.графикаToolStripMenuItem.Name = "графикаToolStripMenuItem";
            this.графикаToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.графикаToolStripMenuItem.Text = "Графика";
            // 
            // спрайтToolStripMenuItem
            // 
            this.спрайтToolStripMenuItem.Name = "спрайтToolStripMenuItem";
            this.спрайтToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.спрайтToolStripMenuItem.Text = "Спрайт";
            this.спрайтToolStripMenuItem.Click += new System.EventHandler(this.spr_Click);
            // 
            // физикаToolStripMenuItem
            // 
            this.физикаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.телоToolStripMenuItem});
            this.физикаToolStripMenuItem.Name = "физикаToolStripMenuItem";
            this.физикаToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.физикаToolStripMenuItem.Text = "Физика";
            // 
            // телоToolStripMenuItem
            // 
            this.телоToolStripMenuItem.Name = "телоToolStripMenuItem";
            this.телоToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.телоToolStripMenuItem.Text = "Тело";
            this.телоToolStripMenuItem.Click += new System.EventHandler(this.bod_Click);
            // 
            // звукToolStripMenuItem
            // 
            this.звукToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.источникЗвукаToolStripMenuItem,
            this.слушательToolStripMenuItem});
            this.звукToolStripMenuItem.Name = "звукToolStripMenuItem";
            this.звукToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.звукToolStripMenuItem.Text = "Звук";
            // 
            // источникЗвукаToolStripMenuItem
            // 
            this.источникЗвукаToolStripMenuItem.Name = "источникЗвукаToolStripMenuItem";
            this.источникЗвукаToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.источникЗвукаToolStripMenuItem.Text = "Источник звука";
            this.источникЗвукаToolStripMenuItem.Click += new System.EventHandler(this.aus_Click);
            // 
            // слушательToolStripMenuItem
            // 
            this.слушательToolStripMenuItem.Name = "слушательToolStripMenuItem";
            this.слушательToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.слушательToolStripMenuItem.Text = "Слушатель";
            this.слушательToolStripMenuItem.Click += new System.EventHandler(this.aul_Click);
            // 
            // gUIToolStripMenuItem
            // 
            this.gUIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.меткаToolStripMenuItem});
            this.gUIToolStripMenuItem.Name = "gUIToolStripMenuItem";
            this.gUIToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.gUIToolStripMenuItem.Text = "GUI";
            // 
            // меткаToolStripMenuItem
            // 
            this.меткаToolStripMenuItem.Name = "меткаToolStripMenuItem";
            this.меткаToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.меткаToolStripMenuItem.Text = "Метка";
            this.меткаToolStripMenuItem.Click += new System.EventHandler(this.меткаToolStripMenuItem_Click);
            // 
            // камераToolStripMenuItem
            // 
            this.камераToolStripMenuItem.Name = "камераToolStripMenuItem";
            this.камераToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.камераToolStripMenuItem.Text = "Камера";
            this.камераToolStripMenuItem.Click += new System.EventHandler(this.cam_Click);
            // 
            // скриптToolStripMenuItem
            // 
            this.скриптToolStripMenuItem.Name = "скриптToolStripMenuItem";
            this.скриптToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.скриптToolStripMenuItem.Text = "Скрипт";
            this.скриптToolStripMenuItem.Click += new System.EventHandler(this.beh_Click);
            // 
            // планToolStripMenuItem
            // 
            this.планToolStripMenuItem.Name = "планToolStripMenuItem";
            this.планToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.планToolStripMenuItem.Text = "План";
            this.планToolStripMenuItem.Click += new System.EventHandler(this.планToolStripMenuItem_Click);
            // 
            // ComponentsEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(493, 357);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComponentsEditorWindow";
            this.ShowInTaskbar = false;
            this.Text = "Редактор компонентов";
            this.Activated += new System.EventHandler(this.ComponentsEditorWindow_Activated);
            this.Load += new System.EventHandler(this.ComponentsEditorWindow_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void меткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            value.Add(new Label());
            reload();
        }

        private void планToolStripMenuItem_Click(object sender, EventArgs e)
        {
            value.Add(new BlueprintBehavior());
            reload();
        }
    }

    public class ComponentsEditor : UITypeEditor
    {
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if ((context != null) && (provider != null))
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

                if (svc != null)
                {
                    if (value is ComponentList)
                    {
                        ComponentsEditorWindow ipfrm = new ComponentsEditorWindow((ComponentList)value);
                        ipfrm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Окно компонентов для нескольких объектов сразу не доступно! Выберите один объект", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
