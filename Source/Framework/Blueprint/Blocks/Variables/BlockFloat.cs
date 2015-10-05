using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockFloat : BlockVariable
    {
        Connector _output;
        float _int = 0;

        [Category("Данные")]
        public float value_int
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

        public BlockFloat(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "Float";
            value_int = 0;
            _output.value = 0;

            width = 100;
            secondColor = Color.airForceBlue;
        }
    }
}
