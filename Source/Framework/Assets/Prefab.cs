using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(PrefabConverter))]
    public class Prefab : Asset
    {
        private GameObject _object;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public GameObject gameObject
        {
            get
            {
                return _object;
            }
            set
            {
                try
                {
                    GameObject obj = value.clone();
                    Engine.scene.objects.Remove(obj);
                    Engine.scene.objCount -= 1;
                    _object = obj;
                }
                catch
                {
                    _object = null;
                }
            }
        }

        public Prefab()
        {
            name = "New prefab";
        }
    }

    public class PrefabConverter : TypeConverter
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
                if (Assets.items[i] is Prefab)
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
            if (value is string)
            {
                List<Prefab> names = new List<Prefab>();
                for (int i = 0; i < Assets.items.Count; i++)
                {
                    if (Assets.items[i] is Prefab)
                        names.Add((Prefab)Assets.items[i]);
                }

                foreach (Prefab s in names)
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
