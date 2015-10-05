using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class Sampler2D
    {
        [NonSerialized]
        Texture _texture;
        TextureSequence _sequence;
        internal int _texture_id = -1;
        internal int _sequence_id = -1;

        public bool animated { get; set; }
        public Texture texture 
        {
            get 
            {
                return _texture; 
            } 
            set 
            {
                _texture = value;
                if (_texture != null)
                    _texture_id = _texture.RES_ID;
            } 
        }
        public TextureSequence sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
                if (_sequence != null)
                    _sequence_id = _sequence.RES_ID;
            }
        }

        public Sampler2D()
        {

        }

        public Sampler2D(Texture tex)
        {
            animated = false;
            texture = tex;
        }

        public Sampler2D(TextureSequence seq)
        {
            animated = true;
            sequence = seq;
        }
    }
}
