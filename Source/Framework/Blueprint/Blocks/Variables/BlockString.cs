using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockString : BlockVariable
    {
        Connector _output;
        string _string = "";

        [Category("Данные")]
        public string value_text
        {
            get
            {
                return _string;
            }
            set
            {
                _string = value;
                text = _string;
                _output.value = _string;
            }
        }

        public BlockString(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "String";
            value_text = "";
            _output.value = "";

            secondColor = Color.oliveDrab;
        }
    }
}
