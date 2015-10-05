using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class BlockSubtract : BlockFunction
    {
        Connector op1 = null;
        Connector op2 = null;

        Connector _result = null;

        public BlockSubtract(Blueprint owner) : base(owner)
        {
            op1 = new Connector(this, ConnectorType.Input);
            op1.name = "Operand A";
            op1.value = 0;

            op2 = new Connector(this, ConnectorType.Input);
            op2.name = "Operand B";
            op2.value = 0;

            _result = new Connector(this, ConnectorType.Output);
            _result.name = "Result (A - B)";
            _result.value = 0;
        }

        public override void doFunction()
        {
            float a = 0;
            float b = 0;
            
            if (op1.link != null)
            {
                switch (op1.link.value.GetType().Name)
                {
                    case "Int32":
                        a = (int)op1.link.value;
                        break;
                    case "Float":
                        a = (float)op1.link.value;
                        break;
                    default:
                        a = (float)op1.link.value;
                        break;
                }
            }

            if (op2.link != null)
            {
                switch (op2.link.value.GetType().Name)
                {
                    case "Int32":
                        b = (int)op2.link.value;
                        break;
                    case "Float":
                        b = (float)op2.link.value;
                        break;
                    default:
                        b = (float)op2.link.value;
                        break;
                }
            }

            _result.value = a - b;
            doNext();
        }
    }
}
