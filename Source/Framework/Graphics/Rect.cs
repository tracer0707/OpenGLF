using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLF
{
    public struct Rect
    {
        private float __x;
        private float __y;
        private float __w;
        private float __h;

        public float x { get { return __x; } set { __x = value; } }
        public float y { get { return __y; } set { __y = value; } }
        public float width { get { return __w; } set { __w = value; } }
        public float height { get { return __h; } set { __h = value; } }

        public Rect(float _x, float _y, float _w, float _h)
        {
            __x = _x;
            __y = _y;
            __w = _w;
            __h = _h;
        }

        public Rect clone()
        {
            return (Rect)MemberwiseClone();
        }

        public override string ToString()
        {
            return "[" + __x + ", " + __y + ", " + __w + ", " + __h + "]";
        }
    }
}
