using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public enum ExecutionType { Start, Update, BeforeDraw, AfterDraw }

    [Serializable]
    public abstract class Block
    {
        [Browsable(false)]
        public int ID { get; set; }
        [Browsable(false)]
        public Vector position { get; set; } = Vector.zero;
        [Browsable(false)]
        public string headerText { get; set; } = "New block";
        [Browsable(false)]
        public string text { get; set; }
        [Browsable(false)]
        public OpenGLF.Color mainColor { get; set; } = new OpenGLF.Color(16, 50, 114, 255);
        [Browsable(false)]
        public OpenGLF.Color secondColor { get; set; } = OpenGLF.Color.blue;
        [Browsable(false)]
        public OpenGLF.Color textColor { get; set; } = OpenGLF.Color.white;
        [Browsable(false)]
        public int width { get; set; } = 160;
        [Browsable(false)]
        public int height { get; set; } = 80;
        [Browsable(false)]
        public int headerHeight { get; set; } = 22;
        [Browsable(false)]
        public Font font { get; set; }

        [Browsable(false)]
        public List<Connector> input = new List<Connector>();
        [Browsable(false)]
        public List<Connector> output = new List<Connector>();

        [Browsable(false)]
        public Blueprint blueprint { get; }

        public object value = null;

        public Block(Blueprint owner)
        {
            ID = owner._ids;
            owner._ids += 1;
            owner.blocks.Add(this);

            blueprint = owner;

            font = (Font)Assets.find(typeof(Font));

            if (font == null)
            {
                try
                {
                    font = new Font("Data/Fonts/OpenSansB.ttf");
                }
                catch { }
            }

            textColor = new OpenGLF.Color(textColor.r, textColor.g, textColor.b, 255);
        }

        public void draw(RenderingMode mode)
        {
            if (mode == RenderingMode.Select)
            {
                GL.LoadName(ID);
            }

            //GL.PushMatrix();

            Drawing.drawQuadRounded(position, width, height, 4, 10, false, Color.lightGray);
            Drawing.drawQuadRounded(position, width, height, 1, 10, true, secondColor);
            Drawing.drawQuadRounded(position, width, headerHeight, 1, 10, true, mainColor);
            Drawing.drawQuad(position + new Vector(0, 10), width, headerHeight - 10, 1, true, mainColor);
            Drawing.drawText(position, Vector.zero , new Vector(1, 1), 0, width -10, headerHeight, headerText, textColor, 12, font);
            Drawing.drawText(position + new Vector(0, headerHeight), Vector.zero, new Vector(1, 1), 0, width - 10, height - headerHeight - 5, text, textColor, 12, font);

            for (int i = 0; i < input.Count; i++)
            {
                input[i].draw(mode, new Vector(10 + i * (input[i].width + 5), -input[i].height));
            }

            for (int i = 0; i < output.Count; i++)
            {
                output[i].draw(mode, new Vector(10 + i * (output[i].width + 5), height));
            }

            //GL.PopMatrix();
        }

        public void breakLinks()
        {
            foreach (Connector c in input)
            {
                if (c.link != null)
                {
                    c.link.link = null;
                    c.link = null;
                }
            }

            foreach (Connector c in output)
            {
                if (c.link != null)
                {
                    c.link.link = null;
                    c.link = null;
                }
            }
        }

        public void secondDraw() { }

        public virtual void doFunction() { }

        public void doNext()
        {
            //doFunction();

            foreach (Connector c in output)
            {
                if (c.link != null)
                { 
                    if (c.link.value is ConnectorDummy)
                    {
                        //c.link.block.doNext();
                        c.link.block.doFunction();
                    }
                }
            }
        }

        public void doNext(Connector c)
        {
            //doFunction();

            if (c.link != null)
            {
                if (c.link.value is ConnectorDummy)
                {
                    //c.link.block.doNext();
                    c.link.block.doFunction();
                }
            }
        }
    }
}
