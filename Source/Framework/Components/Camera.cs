using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable]
    public class Camera : Component
    {
        Color _backColor = Color.gray;
        float _z = 1.0f;

        public delegate void OnMainCameraChanged();
        public static event OnMainCameraChanged onMainCameraChanged;
        public Color backColor { get { return _backColor; } set { _backColor = value; } }
        public float z { get { return _z; } set { _z = value; if (_z < 0.01f) _z = 0.01f; } }

        [Browsable(false)]
        public Rect rect
        {
            get
            {
                return new Rect((gameObject.position.x - Screen.width / 2), (gameObject.position.y - Screen.height / 2), (gameObject.position.x + Screen.width / 2), (gameObject.position.y + Screen.height / 2));
            }
        }

        public static Camera main { 
            get 
            { 
                if (Engine.scene != null) 
                    return Engine.scene.mainCamera; 
                else return null; 
            } 

            set 
            { 
                if (Engine.scene != null) 
                { 
                    Engine.scene.mainCamera = value;
                    if (value != null)
                        if (onMainCameraChanged != null)
                            onMainCameraChanged();
                } 
            } 
        }

        public Camera()
        {
            
        }

        public override Component clone()
        {
            return new Camera();
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}
