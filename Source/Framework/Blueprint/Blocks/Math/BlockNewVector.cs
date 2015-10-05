using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockNewVector : BlockFunction
    {
        Connector _a = null;
        Connector _b = null;
        Connector _vec = null;

        public BlockNewVector(Blueprint owner) : base(owner)
        {
            _a = new Connector(this, ConnectorType.Input);
            _a.name = "A";
            _a.value = 0;

            _b = new Connector(this, ConnectorType.Input);
            _b.name = "B";
            _b.value = 0;

            _vec = new Connector(this, ConnectorType.Output);
            _vec.name = "New Vector";
            _vec.value = Vector.zero;
        }

        public override void doFunction()
        {
            float a = 0;
            float b = 0;

            if (_a.link != null)
            {
                switch (_a.link.value.GetType().Name)
                {
                    case "Int32":
                        a = (int)_a.link.value;
                        break;
                    case "Float":
                        a = (float)_a.link.value;
                        break;
                    default:
                        a = (float)_a.link.value;
                        break;
                }
            }
            if (_b.link != null)
            {
                switch (_b.link.value.GetType().Name)
                {
                    case "Int32":
                        b = (int)_b.link.value;
                        break;
                    case "Float":
                        b = (float)_b.link.value;
                        break;
                    default:
                        b = (float)_b.link.value;
                        break;
                }
            }

            _vec.value = new Vector(a, b);

            doNext();
        }
    }
}
