using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockFunction : Block
    {
        internal Connector c_in = null;
        internal Connector c_out = null;

        public BlockFunction(Blueprint owner) : base(owner)
        {
            c_in = new Connector(this, ConnectorType.Input);
            c_in.name = "In";
            c_in.color = Color.red;
            c_in.value = new ConnectorDummy();

            c_out = new Connector(this, ConnectorType.Output);
            c_out.name = "Out";
            c_out.color = Color.red;
            c_out.value = new ConnectorDummy();
        }
    }
}
