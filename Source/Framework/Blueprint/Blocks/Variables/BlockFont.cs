using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace OpenGLF
{
    [Serializable]
    public class BlockFont : BlockVariable
    {
        Connector _output;
        Font _font;

        [Category("Данные")]
        public Font value_font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                text = _font.name;
                _output.value = _font;
            }
        }

        public BlockFont(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "Font";

            try
            {
                value_font = (Font)Assets.find(typeof(Font));
            }
            catch
            {
                try
                {
                    value_font = new OpenGLF.Font("Data/Fonts/OpenSans.ttf");
                }
                catch { }
            }

            _output.value = value_font;
        }
    }
}
