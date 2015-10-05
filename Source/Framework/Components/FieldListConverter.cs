using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;

namespace OpenGLF
{
    public class FieldListConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return "(Коллекция)";
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return false;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            for (int i = 0; i < ((Dictionary<string, object>)value).Count; i++)
            {
                CollectionPropertyDescriptor pd = new CollectionPropertyDescriptor(((Dictionary<string, object>)value), i);
                pds.Add(pd);
            }

            return pds;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class CollectionPropertyDescriptor : PropertyDescriptor
    {
        private Dictionary<string, object> collection = null;
        private int index = -1;

        public CollectionPropertyDescriptor(Dictionary<string, object> coll,
                           int idx)
            : base("#" + idx.ToString(), null)
        {
            this.collection = coll;
            this.index = idx;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(null);
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return this.collection.GetType();
            }
        }

        public override string DisplayName
        {
            get
            {
                try
                {
                    return collection.ElementAt(index).Key.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public override string Description
        {
            get
            {
                return "";
            }
        }

        public override object GetValue(object component)
        {
            return this.collection.ElementAt(index).Value;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "#" + index.ToString(); }
        }

        public override Type PropertyType
        {
            get
            {
                if (this.collection.ElementAt(index).Value != null)
                    return this.collection.ElementAt(index).Value.GetType();
                else
                    return typeof(object);
            }
        }

        public override void ResetValue(object component) { }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            this.collection[this.collection.ElementAt(index).Key] = value;
        } 
    }
}