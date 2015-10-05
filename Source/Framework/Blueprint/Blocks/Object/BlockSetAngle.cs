using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockSetAngle : BlockFunction
    {
        Connector c_gameObject = null;
        Connector c_angle = null;

        public BlockSetAngle(Blueprint owner) : base(owner)
        {
            c_gameObject = new Connector(this, ConnectorType.Input);
            c_gameObject.name = "GameObject";
            GameObject obj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(obj);
            c_gameObject.value = obj;

            c_angle = new Connector(this, ConnectorType.Input);
            c_angle.name = "Angle";
            c_angle.value = 0;
        }

        public override void doFunction()
        {
            GameObject obj = null;
            float a = 0;

            if (c_gameObject.link != null)
            {
                obj = GameObject.find(((GameObject)c_gameObject.link.value).ID);
            }

            if (c_angle.link != null)
            {
                switch (c_angle.link.value.GetType().Name)
                {
                    case "Int32":
                        a = (int)c_angle.link.value;
                        break;
                    case "Float":
                        a = (float)c_angle.link.value;
                        break;
                    default:
                        a = (float)c_angle.link.value;
                        break;
                }
            }

            if (obj != null)
            {
                obj.angle = a;
            }

            doNext();
        }
    }
}
