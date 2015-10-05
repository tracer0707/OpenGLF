using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class BlockUpdate : BlockControl
    {
        public BlockUpdate(Blueprint owner) : base(owner)
        {
            
        }

        public override void update()
        {
            doNext();
        }
    }
}
