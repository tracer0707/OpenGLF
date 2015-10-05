using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockMove : BlockFunction
    {
        Connector c_gameObject = null;
        Connector c_position = null;
        Connector c_out_position = null;

        public BlockMove(Blueprint owner) : base(owner)
        {
            c_gameObject = new Connector(this, ConnectorType.Input);
            c_gameObject.name = "GameObject";
            GameObject obj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(obj);
            c_gameObject.value = obj;

            c_position = new Connector(this, ConnectorType.Input);
            c_position.name = "Position";
            c_position.value = new Vector();

            c_out_position = new Connector(this, ConnectorType.Output);
            c_out_position.name = "New position";
            c_out_position.value = new Vector();
        }

        public override void doFunction()
        {
            GameObject obj = null;
            Vector pos = (Vector)c_position.value;

            if (c_gameObject.link != null)
            {
                obj = GameObject.find(((GameObject)c_gameObject.link.value).ID);
            }

            if (c_position.link != null)
                pos = (Vector)c_position.link.value;

            if (obj != null)
            {
                obj.position += pos;
                c_out_position.value = obj.position;
            }

            doNext();
        }
    }
}
