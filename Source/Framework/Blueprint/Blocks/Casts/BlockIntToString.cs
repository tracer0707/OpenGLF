using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockIntToString : BlockFunction
    {
        Connector c_int = null;
        Connector c_out_string = null;

        public BlockIntToString(Blueprint owner) : base(owner)
        {
            c_int = new Connector(this, ConnectorType.Input);
            c_int.name = "Int";
            c_int.value = 0;

            c_out_string = new Connector(this, ConnectorType.Output);
            c_out_string.value = "0";
            c_out_string.name = "Converted string";
        }

        public override void doFunction()
        {
            try {
                int _int = 0;

                if (c_int.link != null)
                    _int = (int)c_int.link.value;

                c_out_string.value = _int.ToString();

                doNext();
            }
            catch { }
        }
    }
}
