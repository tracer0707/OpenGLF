using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockLookAt : BlockFunction
    {
        Connector c_point = null;
        Connector c_point2 = null;
        Connector c_out_angle = null;

        public BlockLookAt(Blueprint owner) : base(owner)
        {
            c_point = new Connector(this, ConnectorType.Input);
            c_point.name = "Point 1";
            c_point.value = new Vector();

            c_point2 = new Connector(this, ConnectorType.Input);
            c_point2.name = "Point 2";
            c_point2.value = new Vector();

            c_out_angle = new Connector(this, ConnectorType.Output);
            c_out_angle.name = "New angle";
            c_out_angle.value = 0;
        }

        public override void doFunction()
        {
            Vector pos = (Vector)c_point.value;
            Vector pos2 = (Vector)c_point2.value;

            if (c_point.link != null)
                pos = (Vector)c_point.link.value;

            if (c_point2.link != null)
                pos2 = (Vector)c_point2.link.value;

            c_out_angle.value = (float)Mathf.lookAt(pos, pos2);

            doNext();
        }
    }
}
