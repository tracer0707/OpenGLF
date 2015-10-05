using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Drawing;
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;
using OpenTK.Graphics.OpenGL;

namespace OpenGLF
{
    public class VerticesEditorWindow : Form
    {
        private ToolStrip toolStrip1;
        List<Vector> v;
        private ToolStripComboBox imgBox;
        int selected = -1;
        Vector camMemory = new Vector();
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator1;
        bool grid = true;
        bool drawGrid = true;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripTextBox toolStripTextBox3;
        private ToolStripLabel toolStripLabel4;
        private ToolStripTextBox toolStripTextBox4;
        Texture tex;
        private ToolStripButton toolStripButton3;
        private ToolStripSeparator toolStripSeparator3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem инструментыToolStripMenuItem;
        private ToolStripMenuItem вычислитьКоллайдерАвтоматическиToolStripMenuItem;
        private OpenTK.GLControl glControl1;

        bool loaded = false;
        GLCanvas canvas;

        public VerticesEditorWindow(List<Vector> verts)
        {
            v = verts;

            InitializeComponent();

           glControl1.MouseWheel += VerticesEditorWindow_MouseWheel;
        }

        private void Canvas_onDraw(RenderingMode mode)
        {
            float cx = 0;
            float cy = 0;
            float w = 0;
            float h = 0;

            float zoom = canvas.camera.z;

            try
            {
                cx = int.Parse(toolStripTextBox1.Text);
                cy = int.Parse(toolStripTextBox2.Text);
                w = int.Parse(toolStripTextBox3.Text);
                h = int.Parse(toolStripTextBox4.Text);
            }
            catch
            {
                cx = 0;
                cy = 0;
                w = 0;
                h = 0;
            }


            if (drawGrid == true)
            {
                for (int i = -100; i < 100; i++)
                {
                    Drawing.drawLine(new Vector(i * 16, -100 * 16), new Vector(i * 16, 100 * 16), 0.2f, Color.white);
                    Drawing.drawLine(new Vector(-100 * 16, i * 16), new Vector(16 * 100, i * 16), 0.2f, Color.white);
                }
            }
            
            if (tex != null)
                Drawing.drawTexture(tex, Vector.zero, new Vector(cx, cy), new Vector(1, 1), 0, (int)(w), (int)(h));

            if (v.Count > 2)
            {
                for (int i = 0; i < v.Count - 1; i++)
                {
                    Drawing.drawLine(new Vector(v[i].x, v[i].y), new Vector(v[i + 1].x, v[i + 1].y), 1, Color.green);
                }

                Drawing.drawLine(new Vector(v[v.Count - 1].x, v[v.Count - 1].y), new Vector(v[0].x, v[0].y), 1, Color.green);
            }

            for (int i = 0; i < v.Count; i++)
            {
                Drawing.drawPoint(new Vector(v[i].x, v[i].y), 8, Color.blue);
            }

            Drawing.drawPoint(new Vector(0, 0), 8, Color.yellow);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (loaded)
                draw();
        }

        void VerticesEditorWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            float m = e.Delta * SystemInformation.MouseWheelScrollLines / (120);
            canvas.camera.z += (-m / (64));

            if (canvas.camera.z < 0.2f) canvas.camera.z = 0.2f;
            if (canvas.camera.z > 4.0f) canvas.camera.z = 4.0f;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerticesEditorWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.imgBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.инструментыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glControl1 = new OpenTK.GLControl();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripSeparator3,
            this.imgBox,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripLabel2,
            this.toolStripTextBox2,
            this.toolStripLabel3,
            this.toolStripTextBox3,
            this.toolStripLabel4,
            this.toolStripTextBox4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(669, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Checked = true;
            this.toolStripButton1.CheckOnClick = true;
            this.toolStripButton1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Привязка к сетке";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Checked = true;
            this.toolStripButton2.CheckOnClick = true;
            this.toolStripButton2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Сетка";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Рассчитать вершины из текстуры";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // imgBox
            // 
            this.imgBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(121, 25);
            this.imgBox.ToolTipText = "Текстура";
            this.imgBox.SelectedIndexChanged += new System.EventHandler(this.imgBox_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(17, 22);
            this.toolStripLabel1.Text = "X:";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(48, 25);
            this.toolStripTextBox1.Text = "0";
            this.toolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(17, 22);
            this.toolStripLabel2.Text = "Y:";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(48, 25);
            this.toolStripTextBox2.Text = "0";
            this.toolStripTextBox2.TextChanged += new System.EventHandler(this.toolStripTextBox2_TextChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(55, 22);
            this.toolStripLabel3.Text = "Ширина:";
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(48, 25);
            this.toolStripTextBox3.TextChanged += new System.EventHandler(this.toolStripTextBox3_TextChanged);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabel4.Text = "Высота:";
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(48, 25);
            this.toolStripTextBox4.TextChanged += new System.EventHandler(this.toolStripTextBox4_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.инструментыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(669, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // инструментыToolStripMenuItem
            // 
            this.инструментыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem});
            this.инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            this.инструментыToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.инструментыToolStripMenuItem.Text = "Инструменты";
            // 
            // вычислитьКоллайдерАвтоматическиToolStripMenuItem
            // 
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem.Name = "вычислитьКоллайдерАвтоматическиToolStripMenuItem";
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem.Text = "Создать автоматически";
            this.вычислитьКоллайдерАвтоматическиToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 49);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(669, 396);
            this.glControl1.TabIndex = 3;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // VerticesEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(669, 445);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VerticesEditorWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор вершин";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VerticesEditorWindow_FormClosing);
            this.Load += new System.EventHandler(this.VerticesEditorWindow_Load);
            this.Resize += new System.EventHandler(this.VerticesEditorWindow_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void draw()
        {
            glControl1.MakeCurrent();
            canvas.resize(glControl1.Width, glControl1.Height);
            canvas.draw(RenderingMode.Render, true);
            glControl1.SwapBuffers();
        }

        private void VerticesEditorWindow_Resize(object sender, EventArgs e)
        {
            if (loaded)
                canvas.resize(glControl1.Width, glControl1.Height);
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;

            float zoom = canvas.camera.z;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                camMemory.x = -(e.X * zoom + canvas.camera.gameObject.position.x);
                camMemory.y = -(e.Y * zoom + canvas.camera.gameObject.position.y);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right && ModifierKeys == System.Windows.Forms.Keys.Shift)
            {
                camMemory.x = -(e.X * zoom + canvas.camera.gameObject.position.x);
                camMemory.y = -(e.Y * zoom + canvas.camera.gameObject.position.y);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                float minx = canvas.camera.gameObject.position.x - glControl1.Width / 2 * zoom;
                float miny = canvas.camera.gameObject.position.y - glControl1.Height / 2 * zoom;

                bool sel = false;

                for (int i = 0; i < v.Count; i++)
                {
                    Vector pos = new Vector(e.X * zoom + minx, e.Y * zoom + miny);

                    if (Mathf.intersect(pos.x, pos.y, pos.x, pos.y, v[i].x - 4, v[i].y - 4, v[i].x + 4, v[i].y + 4))
                    {
                        selected = i;
                        sel = true;
                        break;
                    }
                }

                if (sel == false)
                {
                    v.Add(new Vector(e.X * zoom + minx, e.Y * zoom + miny));
                    selected = v.Count - 1;
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                float minx = canvas.camera.gameObject.position.x - glControl1.Width / 2 * zoom;
                float miny = canvas.camera.gameObject.position.y - glControl1.Height / 2 * zoom;
                Vector mousePos = new Vector(e.X * zoom, e.Y * zoom);
                for (int i = 0; i < v.Count; i++)
                {
                    Vector pos = new Vector(e.X * zoom + minx, e.Y * zoom + miny);

                    if (Mathf.intersect(pos.x, pos.y, pos.x, pos.y, v[i].x - 4, v[i].y - 4, v[i].x + 4, v[i].y + 4))
                    {
                        v.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            float zoom = canvas.camera.z;

            float minx = canvas.camera.gameObject.position.x - glControl1.Width / 2 * zoom;
            float miny = canvas.camera.gameObject.position.y - glControl1.Height / 2 * zoom;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                canvas.camera.gameObject.position = new Vector(-(e.X * zoom + camMemory.x), -(e.Y * zoom + camMemory.y));

            if (e.Button == System.Windows.Forms.MouseButtons.Right && ModifierKeys == System.Windows.Forms.Keys.Shift)
            {
                canvas.camera.gameObject.position = new Vector(-(e.X * zoom + camMemory.x), -(e.Y * zoom + camMemory.y));
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (selected > -1)
                {
                    if (grid == true)
                        v[selected] = new Vector((int)(e.X * zoom + minx) / 16 * 16, (int)(e.Y * zoom + miny) / 16 * 16);
                    else
                        v[selected] = new Vector((e.X * zoom + minx), (e.Y * zoom + miny));
                }
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            List<Vertices> _verts;
            Vertices vrts = new Vertices();

            for (int i = 0; i < v.Count; i++)
            {
                vrts.Add(new Vector2((float)v[i].x, (float)v[i].y));
            }

            try
            {
                 _verts = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(vrts, FarseerPhysics.Common.Decomposition.TriangulationAlgorithm.Bayazit);

                if (_verts.Count == 0)
                {
                    Vertices hull = Melkman.GetConvexHull(vrts);

                    v.Clear();

                    for (int i = 0; i < hull.Count; i++)
                    {
                        v.Add(new Vector(hull[i].X, hull[i].Y));
                    }
                }
            }
            catch
            {
                try
                {
                    Vertices hull = Melkman.GetConvexHull(vrts);

                    v.Clear();

                    for (int i = 0; i < hull.Count; i++)
                    {
                        v.Add(new Vector(hull[i].X, hull[i].Y));
                    }
                }
                catch
                {
                    v.Clear();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            grid = !grid;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            drawGrid = !drawGrid;
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {

        }

        private void VerticesEditorWindow_Load(object sender, EventArgs e)
        {
            imgBox.Items.Add("Нет текстуры");

            foreach (Asset asset in Assets.items)
            {
                if (asset is Texture)
                {
                    Texture t = (Texture)asset;
                    imgBox.Items.Add(t);
                }
            }

            imgBox.SelectedIndex = 0;

            searchTexture();
        }

        void searchTexture()
        {
            if (Engine.scene != null)
            {
                foreach (GameObject o in Engine.scene.objects)
                {
                    if (o.rigidbody != null)
                    {
                        if (o.rigidbody.vertices == v)
                        {
                            if (o.sprite != null)
                            {
                                if (o.sprite.material != null)
                                {
                                    foreach (string s in o.sprite.material.parameters.Keys)
                                    {
                                        if (o.sprite.material.parameters[s] is Sampler2D)
                                        {
                                            Sampler2D smp = (Sampler2D)o.sprite.material.parameters[s];
                                            if (smp.texture != null)
                                            {
                                                tex = smp.texture;
                                                toolStripTextBox3.Text = o.sprite.width.ToString();
                                                toolStripTextBox4.Text = o.sprite.height.ToString();
                                                toolStripTextBox1.Text = o.sprite.center.x.ToString();
                                                toolStripTextBox2.Text = o.sprite.center.y.ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void imgBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imgBox.SelectedItem is Texture)
            {
                tex = ((Texture)imgBox.SelectedItem);
                toolStripTextBox3.Text = tex.bitmap.Width.ToString();
                toolStripTextBox4.Text = tex.bitmap.Height.ToString();
            }
            else
            {
                tex = null;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (tex != null)
            {
                int cx = 0;
                int cy = 0;
                int w = 0;
                int h = 0;

                try
                {
                    cx = int.Parse(toolStripTextBox1.Text);
                    cy = int.Parse(toolStripTextBox2.Text);
                    w = int.Parse(toolStripTextBox3.Text);
                    h = int.Parse(toolStripTextBox4.Text);
                }
                catch
                {
                    cx = 0;
                    cy = 0;
                    w = tex.bitmap.Width;
                    h = tex.bitmap.Height;
                }

                int density = 20;
                
                try
                {
                    density = int.Parse(InputBox.show("Введите значение", "Плотность вершин", density.ToString(), this));
                }
                catch
                {
                    MessageBox.Show("Ошибка", "Следует ввести число!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Bitmap b = new Bitmap(tex.bitmap, w, h);

                List<Vector> points = new List<Vector>();
                System.Drawing.Color t = System.Drawing.Color.FromArgb(0, 0, 0, 0);

                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (b.GetPixel(j, i) != t)
                        {
                            if (j >= 2 && j <= w - 2)
                            {
                                if (b.GetPixel(j - 1, i) == t)
                                {
                                    points.Add(new Vector(j, i));
                                    break;
                                }
                            }
                        }
                    }

                    int d = density;
                    if (i + d > h)
                        d = h - i;

                    i += d;
                }

                for (int i = h-1; i > 0; i--)
                {
                    for (int j = w-1; j > 0; j--)
                    {
                        if (b.GetPixel(j, i) != t)
                        {
                            if (j >= 2 && j <= w - 2)
                            {
                                if (b.GetPixel(j + 1, i) == t)
                                {
                                    points.Add(new Vector(j, i));
                                    break;
                                }
                            }
                        }
                    }

                    int d = density;
                    if (i - d < 0)
                        d = h - (h - i);

                    i -= d;
                }

                v.Clear();

                if (points.Count >= 3)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        v.Add(new Vector(points[i].x - cx, points[i].y - cy));
                    }
                }
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;

            canvas = new GLCanvas();
            canvas.onDraw += Canvas_onDraw;

            Application.Idle += Application_Idle;
        }

        private void VerticesEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            loaded = false;
        }
    }

    public class VerticesEditor : UITypeEditor
    {
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if ((context != null) && (provider != null))
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

                if (svc != null)
                {
                    VerticesEditorWindow ipfrm = new VerticesEditorWindow((List<Vector>)value);
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
