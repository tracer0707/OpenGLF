using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class Label : Control
    {
        string _text = "Label";
        [NonSerialized]
        Font _font;
        internal int _fid;
        OpenGLF.Color _color = Color.black;
        int _w = 100;
        int _h = 32;
        Vector[] vertexBuf, normalsBuf, textureBuf;
        int vId, tId;

        public string text { get { return _text; } set { _text = value; } }
        public Font font { get { return _font; } set { _font = value; _fid = _font.RES_ID; } }
        public OpenGLF.Color color { get { return _color; } set { _color = value; } }
        public int width { get { return _w; } set { _w = value; } }
        public int height { get { return _h; } set { _h = value; } }
        public int fontSize { get; set; } = 18;

        public Label()
        {
            try
            {
                font = (Font)Assets.find(typeof(Font));
            }
            catch
            {
                try
                {
                    font = new Font("Data/Fonts/OpenSans.ttf");
                }
                catch
                {
                    MessageBox.Show("Шрифты не найдены! Добавьте шрифт чтобы использовать этот компонент", "Ошиба", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            color = new Color(color.r, color.g, color.b, 255);
        }

        private void genBuffers()
        {
            vertexBuf = new Vector[4];
            normalsBuf = new Vector[4];
            textureBuf = new Vector[4];

            vertexBuf[0].x = -center.x; vertexBuf[0].y = -center.y;
            vertexBuf[1].x = width - center.x; vertexBuf[1].y = -center.y;
            vertexBuf[2].x = width - center.x; vertexBuf[2].y = height - center.y;
            vertexBuf[3].x = -center.x; vertexBuf[3].y = height - center.y;

            textureBuf[0] = new Vector(0, 0);
            textureBuf[1] = new Vector(1, 0);
            textureBuf[2] = new Vector(1, 1);
            textureBuf[3] = new Vector(0, 1);

            //Verts
            vId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
            GL.BufferData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf, BufferUsageHint.StreamDraw);
            GL.BufferSubData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Textures
            tId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * 2 * textureBuf.Length), textureBuf, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void deleteBuffers()
        {
            GL.DeleteBuffer(vId);
            GL.DeleteBuffer(tId);
        }

        internal override void draw(OpenTK.Graphics.OpenGL.RenderingMode mode)
        {
            guiUpdate();

            if (mode == RenderingMode.Select)
            {
                GL.LoadName(gameObject.ID);
            }

            Drawing.drawText(gameObject.position, center, scale, 0, width, height, text, color, fontSize, font);

            if (Engine.debugGUI)
            {
                Drawing.drawQuad(gameObject.position, width * scale.x, height * scale.y, 1, false, Color.yellow);
            }
        }

        public override Component clone()
        {
            Label label = new Label();

            label.autoScale = autoScale;
            label.center = center.clone();
            label.color = new Color(color.r, color.g, color.b, color.a);
            label.enabled = enabled;
            label.font = font;
            label.height = height;
            label.scale = scale.clone();
            label.text = text;
            label.width = width;
            label.fontSize = fontSize;

            return label;
        }

        public override void Loaded()
        {
            try
            {
                font = (Font)Assets.find(_fid, typeof(Font));
            }
            catch
            {
                try
                {
                    font = new Font("Data/Fonts/OpenSans.ttf");
                }
                catch
                {
                    Console.WriteLine("Шрифты не найдены! Добавьте шрифт чтобы использовать этот компонент");
                }
            }
        }
    }
}
