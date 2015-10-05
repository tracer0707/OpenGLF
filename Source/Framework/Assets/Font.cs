using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Text;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(FontConverter))]
    public class Font : Asset
    {
        byte[] _bytes;
        [NonSerialized]
        System.Drawing.Font font;

        [NonSerialized]
        PrivateFontCollection pfc = new PrivateFontCollection();

        [Browsable(false)]
        public byte[] bytes { get { return _bytes; } }
        
        public Font()
        {
            name = "Font";
        }

        public Font(string path)
        {
            name = "Font";

            LoadFromFile(path);
        }

        public bool LoadFromFile(string path)
        {
            try {
                _bytes = File.ReadAllBytes(path);
                FontFamily fontFamily = LoadFontFamily(path);
                font = new System.Drawing.Font(fontFamily, 14);

                return true;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки внешнего ресурса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки внешнего ресурса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool LoadFromByteArray(byte[] bytes)
        {
            try
            {
                _bytes = bytes;
                FontFamily fontFamily = LoadFontFamily(bytes);

                font = new System.Drawing.Font(fontFamily, 14, FontStyle.Regular);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки внешнего ресурса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public FontFamily LoadFontFamily(string fileName)
        {
            pfc = new PrivateFontCollection();
            pfc.AddFontFile(fileName);
            return pfc.Families[0];
        }

        // Load font family from stream
        public FontFamily LoadFontFamily(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return LoadFontFamily(buffer);
        }

        // load font family from byte array
        public FontFamily LoadFontFamily(byte[] buffer)
        {
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                pfc = new PrivateFontCollection();
                pfc.AddMemoryFont(ptr, buffer.Length);
                return pfc.Families[0];
            }
            finally
            {
                handle.Free();
            }
        }

        [OnDeserialized()]
        private void onDeserialized(StreamingContext context)
        {
            pfc = new PrivateFontCollection();
            LoadFromByteArray(bytes);
        }

        internal void renderString(Vector position, Vector center, Vector scale, int angle, int width, int height, string text, Color color, int size)
        {
            Vector[] vertexBuf = new Vector[4];
            Vector[] normalsBuf = new Vector[4];
            Vector[] textureBuf = new Vector[4];

            vertexBuf[0].x = -center.x; vertexBuf[0].y = -center.y;
            vertexBuf[1].x = width - center.x; vertexBuf[1].y = -center.y;
            vertexBuf[2].x = width - center.x; vertexBuf[2].y = height - center.y;
            vertexBuf[3].x = -center.x; vertexBuf[3].y = height - center.y;

            textureBuf[0] = new Vector(0, 0);
            textureBuf[1] = new Vector(1, 0);
            textureBuf[2] = new Vector(1, 1);
            textureBuf[3] = new Vector(0, 1);

            //Verts
            int vId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
            GL.BufferData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf, BufferUsageHint.StreamDraw);
            GL.BufferSubData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Textures
            int tId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * 2 * textureBuf.Length), textureBuf, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.PushMatrix();

            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            g.Clear(System.Drawing.Color.Transparent);

            font = new System.Drawing.Font(font.FontFamily, size, FontStyle.Regular);
            g.DrawString(text, font, new SolidBrush(System.Drawing.Color.FromArgb(color.a, color.r, color.g, color.b)), new PointF(0, 0));

            if (bitmap != null)
            {
                Texture tex = new Texture();
                tex.LoadFromBitmap(bitmap);

                Engine.shaders.guiShader.begin();
                Engine.shaders.guiShader.pass("texture", tex);
                Engine.shaders.guiShader.pass("color", new Vec4(1, 1, 1, 1));

                GL.Translate(position.x, position.y, 0);
                GL.Rotate(angle, 0, 0, 1);

                GL.Scale(scale.x, scale.y, 1);

                //Биндим текстурные координаты
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);

                //Биндим геометрию
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
                GL.VertexPointer(2, VertexPointerType.Float, 0, 0);

                //Рисуем
                GL.DrawArrays(PrimitiveType.Quads, 0, vertexBuf.Length);

                Engine.shaders.guiShader.passNullTex("texture");
                Engine.shaders.guiShader.end();

                GL.DeleteTexture(tex.ID);

                bitmap.Dispose();
                g.Dispose();
                tex.Dispose();
            }

            GL.PopMatrix();

            if (bitmap != null)
            {
                //Отключаем все
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }

            GL.DeleteBuffer(vId);
            GL.DeleteBuffer(tId);

        }
    }

    public class FontConverter : TypeConverter
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
                if (Assets.items[i] is Font)
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
            List<Font> names = new List<Font>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Font)
                    names.Add((Font)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Font s in names)
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
