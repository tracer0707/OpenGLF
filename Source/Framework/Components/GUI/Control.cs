using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public abstract class Control : Component
    {
        Vector _center;
        Vector _scale = new Vector(1, 1);
        bool _autoscale = true;

        [Category("Transform")]
        public Vector scale { get { return _scale; } set { _scale = value; } }

        [Category("Transform")]
        public Vector center { get { return _center; } set { _center = value; } }

        [Category("Properties")]
        public bool autoScale { get { return _autoscale; } set { _autoscale = value; } }

        internal void guiUpdate()
        {
            if (autoScale)
            {
                if (Camera.main != null)
                {
                    scale = new Vector(Camera.main.z, Camera.main.z);
                }
            }
        }
    }
}
