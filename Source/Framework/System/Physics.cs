using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    public static class Physics
    {
        public static Vector gravity { get
            {
                return new Vector(Engine.world.Gravity.X, Engine.world.Gravity.Y);
            }
            set
            {
                Engine.world.Gravity = new FarseerPhysics.Common.Vector2(value.x, value.y);
            }
        }
    }

}
