using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class Float
    {
        public float value { get; set; }

        public Float()
        {
            value = 0.0f;
        }

        public Float(float f)
        {
            value = f;
        }
    }
}
