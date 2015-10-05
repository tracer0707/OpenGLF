using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class BlockStart : BlockControl
    {
        public BlockStart(Blueprint owner) : base(owner)
        {
            
        }

        public override void start()
        {
            doNext();
        }
    }
}
