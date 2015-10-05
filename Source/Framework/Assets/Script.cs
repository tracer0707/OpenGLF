using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(ScriptEditor))]
    public class Script : Asset
    {
        [Browsable(false)]
        public string code { get; set; }
        public Script()
        {
            name = "NewScript";

            code = "using System;" + Environment.NewLine +
                "using OpenGLF;" + Environment.NewLine + Environment.NewLine +
                "public class " + name + " : Behavior" + Environment.NewLine +
                "{" + Environment.NewLine +
                "    " + Environment.NewLine +
                "}" + Environment.NewLine;
        }
    }

    public class ScriptEditor : TypeConverter
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
                if (Assets.items[i] is Script)
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
            List<Script> names = new List<Script>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Script)
                    names.Add((Script)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Script s in names)
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
