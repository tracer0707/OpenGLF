using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class Canvas : Component
    {
        Rect _rect = new Rect(0, 0, 640, 480);

        public Rect rect { get { return _rect; } set { _rect = value; } }
    }
}
