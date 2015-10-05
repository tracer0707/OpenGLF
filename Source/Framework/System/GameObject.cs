using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.Serialization;
using System.Reflection;
using System.Dynamic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OpenGLF
{
    [Serializable]
    [TypeConverter(typeof(GameObjectConverter))]
    public class GameObject : IDisposable
    {
        bool disposed = false;
        Vector _position = Vector.zero;
        Vector _localPosition = Vector.zero;
        Vector _oldpos = Vector.zero;
        float _angle = 0;
        string _name = "";
        int _id = 0;
        GameObject _parent;
        GameObjectList _children = new GameObjectList();
        public Sprite sprite;
        public Camera camera;
        public Rigidbody rigidbody;

        [Category("Transform")]
        public Vector position 
        { 
            get 
            { 
                return _position; 
            } 
            set 
            {
                _oldpos = _position.clone();
                _position = value;
                
                List<GameObject> list = getChildren();

                if (list != null)
                {
                    foreach (GameObject child in list)
                    {
                        //if (child.rigidbody == null)
                        //{
                        Vector _pos = child.position - _oldpos;
                        child.localPosition = _pos;
                        //}
                    }
                }
            } 
        }

        [Category("Transform")]
        public Vector localPosition
        {
            get
            {
                if (parent != null)
                    return _position - parent._position;
                else
                    return _position;
            }
            set
            {
                if (parent != null)
                {
                    position = parent._position + value;
                    _localPosition = value;
                }
                else
                {
                    position = value;
                    _localPosition = value;
                }
            }
        }

        [Category("Transform")]
        public float angle {
            get
            {
                return _angle;
            }
            set
            {
                if (camera == null)
                {
                    List<GameObject> list = getAllChildren();

                    float _old_angle = _angle;
                    _angle = Mathf.roundDegrees(value);

                    float _new_angle = _angle - _old_angle;

                    for (int i = 0; i < list.Count; i++)
                    {
                        GameObject child = list[i];
                        if (child.camera == null)
                        {
                            child._position = Mathf.rotateAround(child.position, position, (float)Mathf.toRadians(_new_angle));
                            child._angle = child._angle + _new_angle;
                        }
                    }
                }
            }
        }

        [Category("Properties")]
        public string name {
            get 
            {
                return _name; 
            }
            set 
            {
                Regex regex = new Regex(@"\d+");
                string val = regex.Replace(value, "");

                if (Engine.scene != null)
                {
                    if (Engine.scene.objects.Contains(GameObject.find(value)))
                        _name = val + ID;
                    else
                        _name = value;
                }
                else
                {
                    _name = value;
                }

                if (String.IsNullOrEmpty(_name))
                    name = "Unnamed object";
            }
        }

        [Category("Properties")]
        public string tag { get; set; }

        [Browsable(false)]
        public GameObject parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public int ID { get { return _id; } }

        [Category("Components")]
        public ComponentList components { get; set; }

        [Category("Properties")]
        public int depth
        {
            get
            {
                if (Engine.scene != null)
                    return Engine.scene.objects.IndexOf(this);
                else return 0;
            }
            set
            {
                setDepth(value);
            }
        }

        void setDepth(int d)
        {
            try
            {
                if (Engine.scene.objects.IndexOf(this) > -1)
                {
                    if (d <= 1) d = 1;
                    if (d >= Engine.scene.objects.Count) d = Engine.scene.objects.Count - 1;

                    Engine.scene.objects.Remove(this);
                    Engine.scene.objects.Insert(d, this);
                }
            }
            catch
            {
                Console.WriteLine("Ошибка изменения глубины");
            }
        }

        public GameObject()
        {
            defaultConstructor();
        }

        public GameObject(Prefab prefab)
        {
            defaultConstructor();

            try
            {
                name = prefab.name;
                angle = prefab.gameObject.angle;

                for (int i = 0; i < prefab.gameObject.components.Count; i++)
                {
                    Component c = prefab.gameObject.components[i].clone();
                    components.Add(c);
                }
            }
            catch
            {
                Console.WriteLine("Ошибка копирования данных");
            }
        }

        void defaultConstructor()
        {
            _position = new Vector(0, 0);
            components = new ComponentList(this);

            try
            {
                Engine.scene.objects.Add(this);
                Engine.scene.objCount += 1;
                _id = Engine.scene.objCount;
            }
            catch
            {
                Console.WriteLine("Ошибка создания объекта. Сначала создайте сцену");
            }

            name = "Game Object";

            createInstances();
            start();
        }

        public static GameObject getRoot(GameObject obj)
        {
            GameObject root = null;

            if (obj.parent != null)
            {
                GameObject o = obj.parent;
                root = o;

                while (o != null)
                {
                    o = o.parent;
                    if (o != null)
                        root = o;
                }

                return root;
            }

            return null;
        }

        static GameObject getParent(GameObject obj)
        {
            return obj.parent;
        }

        public virtual GameObject clone()
        {
            GameObject obj = new GameObject();
            obj.name = name;
            obj.position = position.clone();
            obj.angle = angle;
            obj.tag = tag;
            obj.parent = parent;

            for (int i = 0; i < components.Count; i++)
            {
                Component c = components[i].clone();
                obj.components.Add(c);
            }

            setComponents();

            for (int i = 0; i < Engine.scene.objects.Count; i++)
            {
                if (Engine.scene.objects[i].parent == this)
                {
                    GameObject clone = Engine.scene.objects[i].clone();
                    clone.parent = obj;
                }
            }

            return obj;
        }

        public T getComponent<T>() where T : Component
        {
            Component c = null;

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T)
                {
                    c = components[i];
                    break;
                }
                else if (components[i] is Behavior)
                {
                    if (((Behavior)components[i]).instance != null)
                    {
                        if (((Behavior)components[i]).instance is T)
                        {
                            c = ((Behavior)components[i]).instance;
                            break;
                        }
                    }
                }
            }

            return (T)c;
        }

        internal void createInstances()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is Behavior)
                {
                    Behavior beh = (Behavior)components[i];
                    beh.createInstance(this);
                }
            }
        }

        internal void start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is Behavior)
                {
                    Behavior beh = (Behavior)components[i];

                    if (beh.enabled)
                    {
                        if (beh.isMethodExists("start"))
                            beh.call("start");
                    }
                }
                else if (components[i] is BlueprintBehavior)
                {
                    BlueprintBehavior beh = (BlueprintBehavior)components[i];

                    if (beh.blueprint != null && beh.enabled)
                    {
                        beh.blueprint.start();
                    }
                }
            }
        }

        internal void update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is Behavior)
                {
                    Behavior beh = (Behavior)components[i];

                    if (beh.enabled)
                    {
                        if (beh.isMethodExists("update"))
                            beh.call("update");
                    }
                }

                else if (components[i] is BlueprintBehavior)
                {
                    BlueprintBehavior beh = (BlueprintBehavior)components[i];

                    if (beh.blueprint != null && beh.enabled)
                    {
                        beh.blueprint.update();
                    }
                }
            }
        }

        internal void beforeDraw()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is Behavior)
                {
                    Behavior beh = (Behavior)components[i];

                    if (beh.enabled)
                    {
                        if (beh.isMethodExists("beforeDraw"))
                            beh.call("beforeDraw");
                    }
                }
                else if (components[i] is BlueprintBehavior)
                {
                    BlueprintBehavior beh = (BlueprintBehavior)components[i];

                    if (beh.blueprint != null && beh.enabled)
                    {
                        beh.blueprint.beforeDraw();
                    }
                }
            }
        }

        internal void afterDraw()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is Behavior)
                {
                    Behavior beh = (Behavior)components[i];

                    if (beh.enabled)
                    {
                        if (beh.isMethodExists("afterDraw"))
                            beh.call("afterDraw");
                    }
                }
                else if (components[i] is BlueprintBehavior)
                {
                    BlueprintBehavior beh = (BlueprintBehavior)components[i];

                    if (beh.blueprint != null && beh.enabled)
                    {
                        beh.blueprint.afterDraw();
                    }
                }
            }
        }

        public List<GameObject> getAllChildren()
        {
            List<GameObject> list = new List<GameObject>();
            _getAllChildren(list);
            return list;
        }

        public List<GameObject> getChildren()
        {
            if (Engine.scene != null)
            {
                List<GameObject> children = new List<GameObject>();

                for (int i = 0; i < Engine.scene.objects.Count; i++)
                {
                    GameObject obj = Engine.scene.objects[i];
                    if (obj._parent == this)
                        children.Add(obj);
                }

                return children;
            }
            else return null;
        }

        void _getAllChildren(List<GameObject> children)
        {
            if (Engine.scene != null)
            {
                for (int i = 0; i < Engine.scene.objects.Count; i++)
                {
                    GameObject obj = Engine.scene.objects[i];
                    if (obj._parent == this)
                    {
                        children.Add(obj);
                        obj._getAllChildren(children);
                    }
                }
            }
        }

        public static GameObject find(int id)
        {
            GameObject o = null;

            if (Engine.scene != null)
            {
                for (int i = 0; i < Engine.scene.objects.Count; i++)
                {
                    if (Engine.scene.objects[i] != null)
                    {
                        if (Engine.scene.objects[i].ID == id)
                            o = Engine.scene.objects[i];
                    }
                }
            }

            return o;
        }

        public static GameObject find(string id)
        {
            GameObject o = null;

            if (Engine.scene != null)
            {
                for (int i = 0; i < Engine.scene.objects.Count; i++)
                {
                    if (Engine.scene.objects[i] != null)
                    {
                        if (Engine.scene.objects[i].name == id)
                            o = Engine.scene.objects[i];
                    }
                }
            }

            return o;
        }

        public static GameObject[] findWithTag(string tag)
        {
            GameObject[] o = null;

            if (Engine.scene != null)
            {
                o = Engine.scene.objects.FindAll(obj => obj.tag == tag).ToArray();
            }

            return o;
        }

        ~GameObject(){
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Engine.scene != null)
                    {
                        if (Engine.scene.objects.Contains(this))
                            Engine.scene.objects.Remove(this);
                    }

                    List<GameObject> children = getChildren();

                    foreach (GameObject obj in children)
                    {
                        obj.Dispose();
                    }
                    
                }

                disposed = true;
            }
        }

        public override string ToString()
        {
            return name;
        }

        internal void setComponents()
        {
            sprite = getComponent<Sprite>();
            camera = getComponent<Camera>();
            rigidbody = getComponent<Rigidbody>();
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            setComponents();
        }
    }

    public class GameObjectConverter : TypeConverter
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

            for (int i = 0; i < Engine.scene.objects.Count; i++)
            {
                names.Add(Engine.scene.objects[i].name);
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
                foreach (GameObject s in Engine.scene.objects)
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
