using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockColor : BlockVariable
    {
        Connector _output;
        Color _color = Color.black;

        [Category("Данные")]
        public Color value_color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                secondColor = value;
                text = _color.ToString();
                _output.value = _color;
            }
        }

        public BlockColor(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "Color";
            value_color = Color.black;
            _output.value = value_color;

            width = 115;
        }
    }
}
