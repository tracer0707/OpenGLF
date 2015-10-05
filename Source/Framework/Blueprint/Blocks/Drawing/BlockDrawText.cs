using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace OpenGLF
{
    [Serializable]
    public class BlockDrawText : BlockFunction
    {
        Connector c_text = null;
        Connector c_position = null;
        Connector c_center = null;
        Connector c_scale = null;
        Connector c_angle = null;
        Connector c_width = null;
        Connector c_height = null;
        Connector c_color = null;
        Connector c_font = null;

        public BlockDrawText(Blueprint owner) : base(owner)
        {
            secondColor = Color.amethyst;
            mainColor = Color.canonicalAubergine;

            c_text = new Connector(this, ConnectorType.Input);
            c_text.name = "Text";
            c_text.value = "";

            c_position = new Connector(this, ConnectorType.Input);
            c_position.name = "Position";
            c_position.value = new Vector();

            c_center = new Connector(this, ConnectorType.Input);
            c_center.name = "Center";
            c_center.value = new Vector();

            c_scale = new Connector(this, ConnectorType.Input);
            c_scale.name = "Scale";
            c_scale.value = new Vector(1, 1);

            c_angle = new Connector(this, ConnectorType.Input);
            c_angle.name = "Angle";
            c_angle.value = 0;

            c_width = new Connector(this, ConnectorType.Input);
            c_width.name = "Width";
            c_width.value = 100;

            c_height = new Connector(this, ConnectorType.Input);
            c_height.name = "Height";
            c_height.value = 30;

            c_color = new Connector(this, ConnectorType.Input);
            c_color.name = "Color";
            c_color.value = Color.black;

            c_font = new Connector(this, ConnectorType.Input);
            c_font.name = "Font";
            c_font.value = Assets.find(typeof(Font));

            if (c_font.value == null)
            {
                try
                {
                    c_font.value = new OpenGLF.Font("Data/Fonts/OpenSans.ttf");
                }
                catch { }
            }

            width = 200;
        }

        public override void doFunction()
        {
            try {
                Vector pos = (Vector)c_position.value;
                Vector center = (Vector)c_center.value;
                Vector scale = (Vector)c_scale.value;
                int angle = (int)c_angle.value;
                int width = (int)c_width.value;
                int height = (int)c_height.value;
                Color color = (Color)c_color.value;
                Font font = (Font)c_font.value;
                string text = (string)c_text.value;

                if (c_text.link != null)
                    text = (string)c_text.link.value;

                if (c_position.link != null)
                    pos = (Vector)c_position.link.value;

                if (c_center.link != null)
                    center = (Vector)c_center.link.value;

                if (c_scale.link != null)
                    pos = (Vector)c_scale.link.value;

                if (c_angle.link != null)
                {
                    if (c_angle.link.value.GetType() == typeof(float))
                        angle = (int)Math.Round((float)c_angle.link.value);
                    else if(c_angle.link.value.GetType() == typeof(int))
                        angle = (int)c_angle.link.value;
                }

                if (c_width.link != null)
                    width = (int)c_width.link.value;

                if (c_height.link != null)
                    height = (int)c_height.link.value;

                if (c_color.link != null)
                    color = (Color)c_color.link.value;

                if (c_font.link != null)
                    font = (Font)c_font.link.value;

                Drawing.drawText(pos, center, scale, angle, width, height, text, color, 14, font);

                doNext();
            }
            catch { }
        }
    }
}
