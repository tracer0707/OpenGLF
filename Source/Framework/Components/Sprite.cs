using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace OpenGLF
{
    [Serializable]
    public class Sprite : Component
    {
        internal int vId, tId;
        int _width, _height;
        int _frame;
        float speedCoef, maxSpeed, _speed;
        private bool _isplaying;
        internal Vector[] vertexBuf, normalsBuf, textureBuf;
        Vector _center;
        Vector _scale = new Vector(1, 1);
        Material _material;

        [Category("Transform")]
        public Vector center { get { return _center; } set { _center = value; vboUpdate(); } }

        [Category("Transform")]
        public int width { get { return _width; } set { _width = value; vboUpdate(); } }

        [Category("Transform")]
        public int height { get { return _height; } set { _height = value; vboUpdate(); } }

        [Category("Properties")]
        public Material material { get { return _material; } set { setMaterial(value); } }

        [Category("Animation")]
        public int frame { get { return _frame; } set { setFrame(value); } }

        [Browsable(false)]
        public bool isPlaying { get { return _isplaying; } }

        [Category("Animation")]
        public bool autoPlay { get; set; }

        [Category("Animation")]
        public float speed { get { return _speed; } set { _speed = value; } }

        [Category("Transform")]
        public Vector scale { get { return _scale; } set { _scale = value; } }

        public Sprite()
        {
            _material = null;

            _center = new Vector(0, 0);

            _width = 64;
            _height = 64;

            _speed = 1;

            if (autoPlay == true)
                play();

            genBuffers();
        }

        private void genBuffers()
        {
            vertexBuf = new Vector[4];
            normalsBuf = new Vector[4];
            textureBuf = new Vector[4];

            vertexBuf[0] = new Vector(0, 0);
            vertexBuf[1] = new Vector(width, 0);
            vertexBuf[2] = new Vector(width, height);
            vertexBuf[3] = new Vector(0, height);

            textureBuf[0] = new Vector(0, 0);
            textureBuf[1] = new Vector(1, 0);
            textureBuf[2] = new Vector(1, 1);
            textureBuf[3] = new Vector(0, 1);

            //Verts
            vId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
            GL.BufferData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf, BufferUsageHint.StreamDraw);
            GL.BufferSubData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Textures
            tId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * 2 * textureBuf.Length), textureBuf, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void deleteBuffers()
        {
            //GL.DeleteBuffer(vId);
            //GL.DeleteBuffer(tId);
        }

        internal override void draw(RenderingMode renderMode)
        {
            if (gameObject != null)
            {
                GL.Color4(1.0f, 0f, 1.0f, 1.0f);

                if (renderMode == RenderingMode.Select)
                {
                    GL.LoadName(gameObject.ID);
                }

                GL.PushMatrix();

                if (material != null && material.shader != null)
                {
                    material.shader.begin();

                    for (int i = 0; i < material.parameters.Count; i++)
                    {
                        if (material.parameters.ElementAt(i).Value != null)
                        {
                            switch (material.parameters.ElementAt(i).Value.GetType().Name)
                            {
                                case "Sampler2D":
                                    Sampler2D sampler = ((Sampler2D)(material.parameters.ElementAt(i).Value));
                                    if (sampler.animated == false)
                                    {
                                        if (sampler.texture != null)
                                            material.shader.pass(material.parameters.ElementAt(i).Key, sampler.texture);
                                        else
                                            material.shader.passNullTex(material.parameters.ElementAt(i).Key);
                                    }
                                    else
                                    {
                                        if (sampler.sequence != null)
                                        {
                                            if (sampler.sequence.frames.Count > 0)
                                            {
                                                if (_frame <= sampler.sequence.frames.Count && sampler.sequence.frames[_frame] != null)
                                                {
                                                    material.shader.pass(material.parameters.ElementAt(i).Key, sampler.sequence.frames[_frame]);
                                                    if (isPlaying == true)
                                                    {
                                                        maxSpeed = sampler.sequence.frames.Count;

                                                        if (speedCoef < maxSpeed)
                                                            speedCoef += speed;
                                                        else
                                                        {
                                                            animateSequence(sampler.sequence);
                                                            speedCoef = 0;
                                                        }
                                                    }
                                                }
                                                else
                                                    material.shader.passNullTex(material.parameters.ElementAt(i).Key);
                                            }
                                            else
                                                material.shader.passNullTex(material.parameters.ElementAt(i).Key);
                                        }
                                        else
                                            material.shader.passNullTex(material.parameters.ElementAt(i).Key);
                                    }
                                    break;

                                case "Vec4":
                                    material.shader.pass(material.parameters.ElementAt(i).Key, (Vec4)material.parameters.ElementAt(i).Value);
                                    break;
                                case "Float":
                                    material.shader.pass(material.parameters.ElementAt(i).Key, ((Float)(material.parameters.ElementAt(i).Value)).value);
                                    break;
                                case "Int":
                                    material.shader.pass(material.parameters.ElementAt(i).Key, ((Int)(material.parameters.ElementAt(i).Value)).value);
                                    break;
                                case "Int32":
                                    material.shader.pass(material.parameters.ElementAt(i).Key, (int)material.parameters.ElementAt(i).Value);
                                    break;
                                case "Single":
                                    material.shader.pass(material.parameters.ElementAt(i).Key, (float)material.parameters.ElementAt(i).Value);
                                    break;
                            }
                        }
                    }
                }

                GL.Translate(gameObject.position.x, gameObject.position.y, 0);
                GL.Rotate(gameObject.angle, 0, 0, 1);
                GL.Scale(_scale.x, _scale.y, 1);

                //Биндим текстурные координаты
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);

                //Биндим геометрию
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
                GL.VertexPointer(2, VertexPointerType.Float, 0, 0);

                //Рисуем
                GL.DrawArrays(PrimitiveType.Quads, 0, vertexBuf.Length);

                //Убираем текстуры
                if (material != null && material.shader != null)
                {
                    for (int i = 0; i < material.parameters.Count; i++)
                    {
                        if (material.parameters.ElementAt(i).Value != null)
                        {
                            switch (material.parameters.ElementAt(i).Value.GetType().Name)
                            {
                                case "Sampler2D":
                                    material.shader.passNullTex(material.parameters.ElementAt(i).Key);
                                    break;
                            }
                        }
                    }
                    material.shader.end();
                }

                GL.PopMatrix();

                //Отключаем все
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }
        }

        private void vboUpdate()
        {
            vertexBuf[0].x = -center.x; vertexBuf[0].y = -center.y;
            vertexBuf[1].x = width - center.x; vertexBuf[1].y = -center.y;
            vertexBuf[2].x = width - center.x; vertexBuf[2].y = height - center.y;
            vertexBuf[3].x = -center.x; vertexBuf[3].y = height - center.y;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
            GL.BufferSubData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
        }

        private void setMaterial(Material m)
        {
            _material = m;

            if (_material != null)
            {
                if (_material.shader != null)
                {
                    _material.shader.compile();
                }
            }
        }

        private void animateSequence(TextureSequence sequence)
        {
            if (_frame < sequence.frames.Count - 1)
                _frame += 1;
            else
                _frame = 0;
        }

        public void play()
        {
            _isplaying = true;
        }

        public void stop()
        {
            _isplaying = false;
        }

        void setFrame(int frm)
        {
            _frame = frm;
            draw(RenderingMode.Render);
        }

        public override Component clone()
        {
            Sprite spr = new Sprite();

            spr.center = center.clone();
            spr.width = width;
            spr.height = height;
            spr.material = material;
            spr.frame = frame;
            spr.speed = speed;
            spr.autoPlay = autoPlay;
            spr._isplaying = _isplaying;
            spr.enabled = enabled;

            if (isPlaying == true)
            {
                spr.play();
            }

            return spr;
        }

        ~Sprite()
        {
            deleteBuffers();
        }

        public override void Loaded()
        {
            if (material != null)
                material = (Material)Assets.find(material.RES_ID, typeof(Material));

            genBuffers();

            vboUpdate();

            if (autoPlay == true)
                play();
        }

        [OnDeserialized()]
        private void onDeserialized(StreamingContext context)
        {
            //_frame = 0;
            
        }
    }
}
