using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable]
    public class BlueprintBehavior : Component
    {
        Blueprint _blueprint = null;

        public Blueprint blueprint
        {
            get { return _blueprint; }
            set { _blueprint = value; if (_blueprint != null) _blueprint.owner = this; }
        }

        internal override bool multiple()
        {
            return true;
        }

        public override Component clone()
        {
            BlueprintBehavior b = new BlueprintBehavior();
            b.blueprint = blueprint;
            b.enabled = enabled;

            return b;
        }

        public override void Loaded()
        {
            if (blueprint != null)
                blueprint = (Blueprint)Assets.find(blueprint.RES_ID, typeof(Blueprint));
        }
    }
}
