using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockHub : BlockFunction
    {
        public Connector c_out2 = null;

        public BlockHub(Blueprint owner) : base(owner)
        {
            c_out2 = new Connector(this, ConnectorType.Output);
            c_out2.name = "Out 2";
            c_out2.color = Color.red;
            c_out2.value = new ConnectorDummy();
        }

        public override void doFunction()
        {
            doNext(c_out);
            doNext(c_out2);
        }
    }
}
