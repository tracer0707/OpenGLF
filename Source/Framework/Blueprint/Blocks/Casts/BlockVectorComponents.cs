using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockVectorComponents : BlockFunction
    {
        Connector c_vector = null;
        Connector c_x = null;
        Connector c_y = null;

        public BlockVectorComponents(Blueprint owner) : base(owner)
        {
            c_vector = new Connector(this, ConnectorType.Input);
            c_vector.name = "Vector";
            c_vector.value = new Vector();

            c_x = new Connector(this, ConnectorType.Output);
            c_x.name = "X";
            c_x.value = 0;

            c_y = new Connector(this, ConnectorType.Output);
            c_y.name = "Y";
            c_y.value = 0;
        }

        public override void doFunction()
        {
            try {
                Vector vec = (Vector)c_vector.value;

                if (c_vector.link != null)
                {
                    vec = (Vector)c_vector.link.value;
                }
                
                c_x.value = vec.x;
                c_y.value = vec.y;
            }
            catch { }

            doNext();
        }
    }
}
