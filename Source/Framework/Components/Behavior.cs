using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Drawing;
using System.Runtime.Serialization;
using System.Collections;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class Behavior : Component
    {
        Dictionary<string, object> _fields = new Dictionary<string, object>();
        internal Script _script = null;

        [NonSerialized]
        internal Behavior instance;

        public Script script 
        { 
            get 
            { 
                return _script; 
            } 
            set 
            {
                _script = value;
                Assets.compileScripts(Engine.dependencies);
            } 
        }

        [TypeConverter(typeof(FieldListConverter))]
        public Dictionary<string, object> fields { get { return _fields; } }

        public override Component clone()
        {
            Behavior beh = new Behavior();
            beh.script = script;
            return beh;
        }

        public override string ToString()
        {
            if (script != null)
                return script.name;
            else
                return GetType().Name;
        }

        internal override bool multiple()
        {
            return true;
        }

        internal bool createInstance(GameObject obj)
        {
            if (script != null)
            {
                Type type = Assembly.GetEntryAssembly().GetType(script.name);
                try
                {
                    instance = (Behavior)Activator.CreateInstance(type);
                    instance.gameObject = obj;

                    IEnumerable<FieldInfo> fl = type.GetFields();

                    foreach (FieldInfo field in fl)
                    {
                        if (fields.ContainsKey(field.Name))
                        {
                            field.SetValue(instance, fields[field.Name]);
                        }
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        internal void call(string method)
        {
            Type type = Assembly.GetEntryAssembly().GetType(script.name);
            try
            {
                type.GetMethod(method).Invoke(instance, null);
            }
            catch
            {
                throw new EntryPointNotFoundException();
            }
        }

        internal bool isMethodExists(string method)
        {
            if (Assembly.GetEntryAssembly() != null)
            {
                if (script != null)
                {
                    Type type = Assembly.GetEntryAssembly().GetType(script.name);
                    if (type != null)
                        return type.GetMethod(method) != null;
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public void findRefs()
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    KeyValuePair<string, object> val = fields.ElementAt(i);

                    if (val.Value != null)
                    {
                        string s = val.Key;
                        object o = val.Value;

                        if (o is Asset)
                            fields[s] = Assets.find(((Asset)val.Value).RES_ID, val.Value.GetType());
                        if (o is GameObject)
                            fields[s] = GameObject.find(((GameObject)val.Value).ID);
                    }
                }
            }
        }

        public override void Loaded()
        {
            if (script != null)
                _script = (Script)Assets.find(_script.RES_ID, typeof(Script));

            findRefs();
        }

        [OnDeserialized()]
        private void onDeserialized(StreamingContext context)
        {
            if (_fields == null)
                _fields = new Dictionary<string, object>();
        }
    }
}
