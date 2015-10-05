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
using System.Drawing.Design;
using System.Collections;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(TextureSequenceEditor))]
    public class TextureSequence : Asset
    {
        [NonSerialized]
        private string _filename;

        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string file { get { return _filename; } set { LoadFromFile(value); } }
        public TextureList frames { get; set; }

        public TextureSequence()
        {
            frames = new TextureList();
            name = "Texture Sequence";
        }

        public TextureSequence(string filename)
        {
            frames = new TextureList();
            name = "Texture Sequence";
            LoadFromFile(filename);
        }

        void LoadFromFile(string f){
            if (!String.IsNullOrEmpty(f))
            {
                if (File.Exists(f))
                {
                    _filename = f;
                    if (frames == null) frames = new TextureList();
                    frames.Clear();

                    Image gifImg = Image.FromFile(f);
                    FrameDimension dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);

                    int frameCount = gifImg.GetFrameCount(dimension);

                    for (int i = 0; i < frameCount; i++)
                    {
                        gifImg.SelectActiveFrame(dimension, i);
                        Bitmap frame = new Bitmap(gifImg);
                        Texture tex = new Texture();
                        tex.LoadFromBitmap(frame);
                        frames.Add(tex);
                    }
                }
            }
            else
            {
                frames = null;
            }
        }

        public override string ToString()
        {
            return name;
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {

        }
    }

    [Serializable]
    [Editor(typeof(SequenceEditor), typeof(UITypeEditor))]
    public class TextureList : List<Texture>{

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            for (int i = 0; i < Count; i++)
                this[i].LoadFromByteArray(this[i].bytes);
        }
    }
}