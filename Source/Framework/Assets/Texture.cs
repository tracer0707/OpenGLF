using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(TextureConverter))]
    public class Texture : Asset
    {
        int _id;
        byte[] _bytes;

        [NonSerialized]
        Bitmap _bitmap;

        [NonSerialized]
        private string _filename;

        [Browsable(false)]
        public Bitmap bitmap { get { return _bitmap; } internal set { _bitmap = value; } }

        [Browsable(false)]
        public byte[] bytes { get { return _bytes; } }

        [Browsable(false)]
        public int ID { get { return _id; } }
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string file { get { return _filename; } set { LoadFromFile(value); } }

        public Texture()
        {
            name = "Texture";
            _id = 0;
        }

        public Texture(string fname)
        {
            name = "Texture";
            _id = 0;

            LoadFromFile(fname);
        }

        public Texture(byte[] bytes)
        {
            name = "Texture";
            _id = 0;

            LoadFromByteArray(bytes);
        }

        public Texture(Bitmap bitmap)
        {
            name = "Texture";
            _id = 0;

            LoadFromBitmap(bitmap);
        }

        public void LoadFromFile(string filename)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                if (File.Exists(filename))
                {
                    _filename = filename;

                    GL.GenTextures(1, out _id);

                    GL.BindTexture(TextureTarget.Texture2D, _id);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    Bitmap bmp = new Bitmap(filename);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        _bytes = ms.ToArray();
                        ms.Dispose();
                    }

                    bitmap = bmp;

                    BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                    bmp.UnlockBits(bmp_data);
                }
            }
        }

        public void LoadFromBitmap(Bitmap bmp)
        {
            _filename = file;

            GL.GenTextures(1, out _id);

            GL.BindTexture(TextureTarget.Texture2D, _id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                _bytes = ms.ToArray();
                ms.Dispose();
            }

            bitmap = bmp;

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
        }

        public void LoadFromByteArray(byte[] fbytes)
        {
            if (fbytes != null)
            {
                _bytes = fbytes;
                GL.GenTextures(1, out _id);

                GL.BindTexture(TextureTarget.Texture2D, _id);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                MemoryStream ms = new MemoryStream();
                ms.Write(fbytes, 0, fbytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                Bitmap bmp = new Bitmap(ms);
                ms.Dispose();

                bitmap = bmp;

                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
        }

        public void LoadFromData(IntPtr data, int width, int height)
        {
            if (data != null)
            {
                GL.GenTextures(1, out _id);

                GL.BindTexture(TextureTarget.Texture2D, _id);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data);
            }
        }

        ~Texture()
        {
            try
            {
                //GL.DeleteTexture(_id);
            }
            catch { }
        }

        public override string ToString()
        {
            return name;
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            LoadFromByteArray(bytes);
        }
    }

    public class TextureConverter : TypeConverter
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
                if (Assets.items[i] is Texture)
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
            List<Texture> names = new List<Texture>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Texture)
                    names.Add((Texture)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Texture s in names)
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
