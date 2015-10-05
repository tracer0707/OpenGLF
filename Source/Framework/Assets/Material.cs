using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(MaterialEditor))]
    public class Material : Asset
    {
        internal Shader _shader;

        public Shader shader { get { return _shader; } set { setShader(value); } }

        [Editor(typeof(DictionaryEditor), typeof(UITypeEditor))]
        public Dictionary<string, object> parameters { get; set; }

        public Material()
        {
            name = "Material";
            parameters = new Dictionary<string, object>();
        }

        private void addParam(string key, object obj)
        {
            switch (obj.GetType().Name)
            {
                case "Vec4":
                    parameters[key] = new Vec4();
                    break;
                case "Sampler2D":
                    parameters[key] = new Sampler2D();
                    break;
                case "Float":
                    parameters[key] = new Float();
                    break;
                case "Int":
                    parameters[key] = new Int();
                    break;
            }
        }

        void setShader(Shader sh)
        {
            _shader = sh;

            if (_shader != null)
            {
                _shader.fullRecompile();
            }
            else
                parameters.Clear();
        }

        public void updateParameters()
        {
            if (_shader != null)
            {
                Dictionary<string, object> uniforms = _shader.uniforms;

                if (parameters == null) parameters = new Dictionary<string, object>();

                for (int i = 0; i < uniforms.Count; i++)
                {
                    KeyValuePair<string, object> val = uniforms.ElementAt(i);
                    if (parameters.ContainsKey(val.Key) == false)
                        addParam(val.Key, val.Value);
                }

                for (int i = 0; i < parameters.Count; i++)
                {
                    KeyValuePair<string, object> val = parameters.ElementAt(i);
                    if (uniforms.ContainsKey(val.Key) == false)
                        parameters.Remove(val.Key);
                }
            }
            else
                parameters.Clear();
        }

        public override string ToString()
        {
            return name;
        }

        public override void Loaded()
        {
            if (shader != null)
                shader = (Shader)Assets.find(shader.RES_ID);

            for (int i = 0; i < parameters.Count; i++)
            {
                KeyValuePair<string, object> val = parameters.ElementAt(i);

                switch (val.Value.GetType().Name)
                {
                    case "Sampler2D":
                        Sampler2D v = (Sampler2D)val.Value;
                        try
                        {
                            if (v._texture_id > -1)
                                v.texture = (Texture)Assets.find(v._texture_id, typeof(Texture));
                            if (v._sequence_id > -1)
                                v.sequence = (TextureSequence)Assets.find(v._sequence_id, typeof(TextureSequence));
                        }
                        catch { }
                        break;
                }
            }
        }

        [OnDeserializedAttribute()]
        internal void onDeserialized(StreamingContext context)
        {
            //if (shader != null)
            //{
                //shader.compile();
            //}
        }
    }

    public class MaterialEditor : TypeConverter
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
                if (Assets.items[i] is Material)
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
            List<Material> names = new List<Material>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Material)
                    names.Add((Material)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Material s in names)
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
