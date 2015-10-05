using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockMouseGetPosition : BlockFunction
    {
        Connector _out = null;

        public BlockMouseGetPosition(Blueprint owner) : base(owner)
        {
            _out = new Connector(this, ConnectorType.Output);
            _out.name = "Mouse position";
            _out.value = Vector.zero;
        }

        public override void doFunction()
        {
            _out.value = Input.mousePosition;

            doNext();
        }
    }
}
