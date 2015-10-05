using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace OpenGLF
{
    [Serializable]
    public class AudioSource : Component
    {
        [NonSerialized]
        internal Audio _audioAsset;
        internal int _audio_id = -1;
        float _minDistance = 50.0f;
        float _maxDistance = 150.0f;
        float _volume = 1.0f;
        bool _loop = false;
        float _playbackSpeed = 1.0f;
        float _pan = 0.5f;

        [NonSerialized]
        ISound _audio;

        [NonSerialized]
        internal ISoundSource _source;

        public Audio audio 
        {
            get
            {
                return _audioAsset; 
            }
            set
            {
                _audioAsset = value;
                _source = Engine.sound.AddSoundSourceFromMemory(_audioAsset.data, _audioAsset.name);
                _audio_id = _audioAsset.RES_ID;
            } 
        }

        public bool playOnStart { get; set; }
        public float minDistance
        {
            get { return _minDistance; }
            set { _minDistance = value;  if (_audio != null) _audio.MinDistance = _minDistance; }
        }

        public float maxDistance
        {
            get { return _maxDistance; }
            set { _maxDistance = value; if (_audio != null) _audio.MaxDistance = _maxDistance; }
        }

        public float volume
        {
            get { return _volume; }
            set { _volume = value; if (_audio != null) _audio.Volume = _volume; }
        }

        public float playbackSpeed
        {
            get { return _playbackSpeed; }
            set { _playbackSpeed = value; if (_audio != null) _audio.PlaybackSpeed = _playbackSpeed; }
        }

        public float pan
        {
            get { return _pan; }
            set { _pan = value; if (_audio != null) _audio.Pan = _pan; }
        }

        public bool loop
        {
            get { return _loop; }
            set { _loop = value; if (_audio != null) _audio.Looped = _loop; }
        }

        [Browsable(false)]
        public bool isPaused
        {
            get { if (_audio != null) return _audio.Paused; else return false; }
        }

        [Browsable(false)]
        public bool isFinished
        {
            get { if (_audio != null) return _audio.Finished; else return true; }
        }

        internal override void start()
        {
            if (playOnStart)
                play();
        }

        internal override void update()
        {
            if (_audio != null)
                _audio.Position = new Vector3D((float)gameObject.position.x, (float)gameObject.position.y, 0);
        }

        public void play()
        {
            _audio = Engine.sound.Play3D(_source, (float)gameObject.position.x, (float)gameObject.position.y, 0, false, false, true);
            _audio.Looped = _loop;
            _audio.MaxDistance = _maxDistance;
            _audio.MinDistance = _minDistance;
            _audio.Pan = _pan;
            _audio.PlaybackSpeed = _playbackSpeed;
            _audio.Volume = _volume;
        }

        public void stop()
        {
            if (_audio != null)
                _audio.Stop();
        }

        public override Component clone()
        {
            AudioSource s = new AudioSource();

            s._audioAsset = _audioAsset;
            s._source = Engine.sound.AddSoundSourceFromMemory(_audioAsset.data, _audioAsset.name + "_cln");
            s._audio_id = _audio_id;
            s._loop = loop;
            s._maxDistance = maxDistance;
            s._minDistance = minDistance;
            s._pan = pan;
            s._playbackSpeed = playbackSpeed;
            s.playOnStart = playOnStart;
            s._volume = volume;

            return s;
        }

        internal override bool multiple()
        {
            return true;
        }

        public override void Loaded()
        {
            if (_audio_id > -1)
            {
                _audioAsset = (Audio)Assets.find(_audio_id);
                _source = Engine.sound.AddSoundSourceFromMemory(_audioAsset.data, _audioAsset.name + gameObject.ID);
            }
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            //_source = Engine.sound.AddSoundSourceFromMemory(_audioAsset.data, _audioAsset.name + gameObject.ID);
        }
    }
}
