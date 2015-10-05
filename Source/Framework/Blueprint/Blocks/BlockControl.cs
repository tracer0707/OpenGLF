using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockControl : Block
    {
        Connector c_out = null;

        public BlockControl(Blueprint owner) : base(owner)
        {
            c_out = new Connector(this, ConnectorType.Output);
            c_out.name = "Out";
            c_out.color = Color.red;
            c_out.value = new ConnectorDummy();
        }

        public virtual void start() { }
        public virtual void update() { }
        public virtual void draw() { }
        public virtual void beforeDraw() { }
    }
}
