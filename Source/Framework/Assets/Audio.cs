using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(AudioEditor))]
    public class Audio : Asset
    {
        byte[] _data;

        [NonSerialized]
        string _filename;

        [Browsable(false)]
        public byte[] data { get { return _data; } }

        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string file { get { return _filename; } set { loadFromFile(value); } }

        public Audio()
        {
            name = "New audio";
        }

        public Audio(string filename)
        {
            name = "New audio";
            loadFromFile(filename);
        }

        void loadFromFile(string file)
        {
            _filename = file;
            _data = File.ReadAllBytes(file);
        }
    }

    public class AudioEditor : TypeConverter
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
                if (Assets.items[i] is Audio)
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
            List<Audio> names = new List<Audio>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Audio)
                    names.Add((Audio)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Audio s in names)
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
