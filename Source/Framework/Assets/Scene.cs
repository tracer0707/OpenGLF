using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(SceneEditor))]
    public class Scene : Asset
    {
        internal int objCount = 0;
        public GameObjectList objects;
        public Camera mainCamera;
        public Dictionary<string, object> data;

        public Scene(){
            name = "New scene";
            objects = new GameObjectList();
            data = new Dictionary<string, object>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override string ToString()
        {
            return name;
        }

        public override void Loaded()
        {
            if (objects == null)
                objects = new GameObjectList();
            else
            {
                foreach (GameObject obj in objects)
                {
                    if (obj.ID > objCount)
                        objCount = obj.ID;
                }
            }

            foreach (GameObject o in objects)
            {
                foreach (Component c in o.components)
                {
                    c.Loaded();
                }
            }
        }

        [OnDeserialized()]
        private void onDeserialized(StreamingContext context)
        {
            
        }
    }

    public class SceneEditor : TypeConverter
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
                if (Assets.items[i] is Scene)
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
            List<Scene> names = new List<Scene>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Scene)
                    names.Add((Scene)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Scene s in names)
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
