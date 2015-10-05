using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Collections;
using System.Windows.Forms;

namespace OpenGLF
{

    [Serializable]
    [Editor(typeof(ComponentsEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ComponentListConverter))]
    public class ComponentList : CollectionBase, ICustomTypeDescriptor
    {
        internal GameObject gameObject { get; set; }

        public ComponentList(GameObject obj)
        {
            gameObject = obj;
        }

        public Component this[int index]
        {
            get
            {
                return (Component)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        public void AddRange(IEnumerable<Component> collection)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                bool exists = false;
                Component T = collection.ElementAt(i);

                for (int j = 0; j < Count; j++)
                {
                    if (T.GetType() == this[i].GetType())
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                {
                    T.gameObject = gameObject;
                    Add(T);
                }
                else if (T.multiple() == true)
                {
                    T.gameObject = gameObject;
                    this.List.Add(T);
                }
            }
            gameObject.setComponents();
        }
        public void Add(Component T)
        {
            bool exists = false;

            for (int i = 0; i < Count; i++)
            {
                if (T.GetType() == this[i].GetType())
                {
                    exists = true;
                    break;
                }
            }

            if (exists == false)
            {
                T.gameObject = gameObject;
                this.List.Add(T);
            }
            else if (T.multiple() == true)
            {
                T.gameObject = gameObject;
                this.List.Add(T);
            }

            gameObject.setComponents();
        }

        public void Remove(Component T)
        {
            T.gameObject = null;
            this.List.Remove(T);
        }

        public void RemoveAll(Predicate<Component> match)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].gameObject = null;
            }

            while (this.List.Count > 0)
            {
                this.List.RemoveAt(0);
            }
        }

        public new void RemoveAt(int index)
        {
            this[index].gameObject = null;
            base.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            int i = 0;
            while(i < count)
            {
                this[index + i].gameObject = null;
                this.List.Remove(this[index + i]);
                i+=1;
            }
        }

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            Dictionary<string, object> d = new Dictionary<string, object>();

            for (int i = 0; i < this.List.Count; i++)
            {
                string n = this.List[i].GetType().Name;

                switch (this.List[i].GetType().Name)
                {
                    case "Behavior":
                        Behavior b = (Behavior)this.List[i];
                        if (b.script != null)
                            n = b.script.name;
                        break;
                    case "BlueprintBehavior":
                        BlueprintBehavior bb = (BlueprintBehavior)this.List[i];
                        if (bb.blueprint != null)
                            n = bb.blueprint.name;
                        break;
                }

                int it = 0;

                foreach (string k in d.Keys)
                {
                    if (k.Equals(n) || k.Equals(n + " (" + it.ToString() + ")"))
                        it += 1;
                }
                if (it > 0)
                    n += " (" + it.ToString() + ")";

                d.Add(n, this.List[i]);

                CollectionPropertyDescriptor pd = new CollectionPropertyDescriptor(d, i);
                pds.Add(pd);
            }

            return pds;
        }

        public override string ToString()
        {
            return "Компоненты";
        }
    }

    class ComponentListConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {

            return base.ConvertFrom(context, culture, value);
        }
    }
}
