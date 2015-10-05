using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockGetPosition : BlockFunction
    {
        Connector c_gameObject = null;
        Connector c_pos = null;

        public BlockGetPosition(Blueprint owner) : base(owner)
        {
            c_gameObject = new Connector(this, ConnectorType.Input);
            c_gameObject.name = "GameObject";
            GameObject obj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(obj);
            c_gameObject.value = obj;

            c_pos = new Connector(this, ConnectorType.Output);
            c_pos.name = "Position";
            c_pos.value = Vector.zero;
        }

        public override void doFunction()
        {
            GameObject obj = null;
            Vector p = Vector.zero;

            if (c_gameObject.link != null)
            {
                obj = GameObject.find(((GameObject)c_gameObject.link.value).ID);
            }

            if (obj != null)
            {
                c_pos.value = obj.position;
            }

            doNext();
        }
    }
}
