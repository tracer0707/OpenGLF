using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockGetKey : BlockFunction
    {
        public Keys key { get; set; } = Keys.A;

        Connector _true = null;
        Connector _false = null;

        public BlockGetKey(Blueprint owner) : base(owner)
        {
            _true = new Connector(this, ConnectorType.Output);
            _true.name = "True";
            _true.color = Color.red;
            _true.value = new ConnectorDummy();

            _false = new Connector(this, ConnectorType.Output);
            _false.name = "False";
            _false.color = Color.red;
            _false.value = new ConnectorDummy();

            secondColor = Color.canonicalAubergine;

            output.Remove(c_out);
        }

        public override void doFunction()
        {
            if (Input.getKeyDown(key))
                doNext(_true);
            else
                doNext(_false);
        }
    }
}
