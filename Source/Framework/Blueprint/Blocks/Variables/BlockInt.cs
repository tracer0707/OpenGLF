using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockInt : BlockVariable
    {
        Connector _output;
        int _int = 0;

        [Category("Данные")]
        public int value_int
        {
            get
            {
                return _int;
            }
            set
            {
                _int = value;
                text = _int.ToString();
                _output.value = _int;
            }
        }

        public BlockInt(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "Int";
            value_int = 0;
            _output.value = 0;

            width = 100;
            secondColor = Color.airForceBlue;
        }
    }
}
