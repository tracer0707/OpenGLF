using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class BlockVector : BlockVariable
    {
        Connector _output;
        Vector _vector = Vector.zero;

        [Category("Данные")]
        public Vector value_vector
        {
            get
            {
                return _vector;
            }
            set
            {
                _vector = value;
                text = _vector.ToString();
                _output.value = _vector;
            }
        }

        public BlockVector(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "Vector";
            value_vector = new Vector();
            _output.value = value_vector;

            secondColor = Color.alizarinCrimson;

            width = 100;
        }
    }
}
