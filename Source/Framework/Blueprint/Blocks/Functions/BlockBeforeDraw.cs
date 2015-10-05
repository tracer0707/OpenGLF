using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class BlockBeforeDraw : BlockControl
    {
        public BlockBeforeDraw(Blueprint owner) : base(owner)
        {
            
        }

        public override void beforeDraw()
        {
            doNext();
        }
    }
}
