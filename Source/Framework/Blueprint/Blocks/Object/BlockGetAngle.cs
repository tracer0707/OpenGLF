using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockGetAngle : BlockFunction
    {
        Connector c_gameObject = null;
        Connector c_angle = null;

        public BlockGetAngle(Blueprint owner) : base(owner)
        {
            c_gameObject = new Connector(this, ConnectorType.Input);
            c_gameObject.name = "GameObject";
            GameObject obj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(obj);
            c_gameObject.value = obj;

            c_angle = new Connector(this, ConnectorType.Output);
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

            if (obj != null)
            {
                c_angle.value = obj.angle;
            }

            doNext();
        }
    }
}
