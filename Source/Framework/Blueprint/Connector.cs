using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace OpenGLF
{
    [Serializable]
    public enum ConnectorType { Input, Output }

    [Serializable]
    public class Connector
    {
        public int ID { get; set; } = 0;
        public Vector position { get; set; } = Vector.zero;
        public Color color { get; set; } = Color.white;
        public Block block { get; }
        public ConnectorType type { get; }

        public Connector link { get; set; }

        public string name { get; set; } = "Connector";
        public object value { get; set; }

        public int width = 10;
        public int height = 10;

        internal static int connectors = 0;

        public Connector(Block b, ConnectorType t)
        {
            block = b;
            type = t;

            ID = block.ID + 513 + block.blueprint._cids;
            block.blueprint._cids += 1;

            if (type == ConnectorType.Input)
                block.input.Add(this);

            else if (type == ConnectorType.Output)
                block.output.Add(this);
        }

        public void draw(RenderingMode mode)
        {
            if (mode == RenderingMode.Select)
            {
                GL.LoadName(ID);
            }

            Drawing.drawQuad(block.position + position, width, height, 1, true, color);
        }

        public void draw(RenderingMode mode, Vector pos)
        {
            if (mode == RenderingMode.Select)
            {
                GL.LoadName(ID);
            }

            position = block.position + pos;
            Drawing.drawQuad(block.position + pos, width, height, 1, true, color);
        }

        
    }
}
