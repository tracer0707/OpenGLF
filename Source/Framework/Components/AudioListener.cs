using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;

namespace OpenGLF
{
    [Serializable]
    public class AudioListener : Component
    {
        float _volume = 1;

        public float volume 
        {
            get
            {
                return _volume;
            } 
            set 
            { 
                _volume = value;
                if (Engine.sound != null)
                    Engine.sound.SoundVolume = _volume;
            } 
        }

        internal override void start()
        {
            Engine.sound.SoundVolume = _volume;
        }

        internal override void update()
        {
            Engine.sound.SetListenerPosition(new Vector3D((float)gameObject.position.x, (float)gameObject.position.y, 200), new Vector3D(0, 0, 1));
        }

        public override Component clone()
        {
            AudioListener l = new AudioListener();
            l.volume = _volume;

            return l;
        }
    }
}
