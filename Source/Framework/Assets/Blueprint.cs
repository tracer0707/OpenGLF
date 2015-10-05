using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(BlueprintConverter))]
    public class Blueprint : Asset
    {
        internal int _ids = 0;
        internal int _cids = 0;

        public List<Block> blocks = new List<Block>();

        internal BlueprintBehavior owner = null;

        public Blueprint()
        {
            name = "New Blueprint";
        }

        public void start()
        {
            foreach (Block b in blocks)
            {
                if (b.GetType().BaseType.Name == "BlockControl")
                    ((BlockControl)b).start();
            }
        }

        public void update()
        {
            foreach (Block b in blocks)
            {
                if (b.GetType().BaseType.Name == "BlockControl")
                    ((BlockControl)b).update();
            }
        }

        public void beforeDraw()
        {
            foreach (Block b in blocks)
            {
                if (b.GetType().BaseType.Name == "BlockControl")
                    ((BlockControl)b).beforeDraw();
            }
        }

        public void afterDraw()
        {
            foreach (Block b in blocks)
            {
                if (b.GetType().BaseType.Name == "BlockControl")
                    ((BlockControl)b).draw();
            }
        }
    }

    public class BlueprintConverter : TypeConverter
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
                if (Assets.items[i] is Blueprint)
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
            List<Blueprint> names = new List<Blueprint>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Blueprint)
                    names.Add((Blueprint)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Blueprint s in names)
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
