using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    [Serializable]
    public class BlockGameObject : BlockVariable
    {
        Connector _output;
        GameObject _value = null;

        public GameObject value_gameObject
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;

                if (_value != null)
                {
                    text = _value.name;
                    _output.value = _value;
                }
            }
        }

        public BlockGameObject(Blueprint owner) : base(owner)
        {
            _output = new Connector(this, ConnectorType.Output);
            _output.name = "GameObject";
            GameObject obj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(obj);
            _output.value = obj;

            width = 120;

            if (blueprint.owner != null)
            {
                value_gameObject = blueprint.owner.gameObject;
            }
        }
    }
}
