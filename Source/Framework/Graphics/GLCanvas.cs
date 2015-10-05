using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.Serialization;

namespace OpenGLF
{
    public class GLCanvas
    {
        int MAX_OBJECTS = 512;
        int[] sz;
        int[] viewport;
        bool nodraw = false;
        Camera _camera;

        public Color backColor { get; set; } = Color.gray;
        public Color contentColor { get; set; } = Color.white;
        public Camera camera { get { return _camera; } }
        public int width { get; set; }
        public int height { get; set; }
        public delegate void OnDraw(RenderingMode mode);
        public delegate void OnSecondDraw();
        public event OnDraw onDraw;
        public event OnSecondDraw onSecondDraw;

        public GLCanvas()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            viewport = new int[4];

            sz = new int[MAX_OBJECTS * 4];

            GL.Viewport(0, 0, width, height);

            createCamera();
        }

        void createCamera()
        {
            GameObject cameraObj = new GameObject();
            if (Engine.scene != null)
                Engine.scene.objects.Remove(cameraObj);
            _camera = new Camera();

            cameraObj.components.Add(camera);
        }

        public void draw(RenderingMode mode, bool callObjectDraw)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(1.0f / 255.0f * backColor.r, 1.0f / 255.0f * backColor.g, 1.0f / 255.0f * backColor.b, 1.0f / 255.0f * backColor.a);
            GL.ClearDepth(1.0f);

            if (nodraw == false)
            {
                //Set matrix
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();

                //Camera
                float xx = -(width / 2);
                float yy = -(height / 2);
                float ww = width + xx;
                float hh = height + yy;

                float zoom = camera.z;
                if (zoom < 0.01f) zoom = 0.01f;

                GL.Ortho(xx * zoom, ww * zoom, hh * zoom, yy * zoom, 0, 100);
                GL.Translate(-camera.gameObject.position.x, -camera.gameObject.position.y, 0);

                //Restore matrix
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
            }

            if (callObjectDraw)
            {
                //Draw event
                if (onDraw != null)
                    onDraw(mode);
            }

            if (nodraw == false)
            {
                if (onSecondDraw != null)
                    onSecondDraw();

                //Reset textures
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, -1);
            }
        }

        public int selectAt(int x, int y)
        {
            resize(width, height);

            GL.SelectBuffer(sz.Length, sz);

            GL.RenderMode(RenderingMode.Select);

            GL.InitNames();
            GL.PushName(0);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();

            GL.GetInteger(GetPName.Viewport, viewport);

            Glu.gluPickMatrix(x, y, 0.001f, 0.001f, viewport);

                float xx = -(viewport[2] / 2);
                float yy = -(viewport[3] / 2);
                float ww = viewport[2] + xx;
                float hh = viewport[3] + yy;

                float zoom = camera.z;
                if (zoom < 0.01f) zoom = 0.01f;

                GL.Ortho(xx * zoom, ww * zoom, yy * zoom, hh * zoom, 0, 100);
                GL.Translate(-camera.gameObject.position.x, -camera.gameObject.position.y, 0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            nodraw = true;
            draw(RenderingMode.Select, true);
            nodraw = false;

            GL.PopMatrix();
            GL.PopMatrix();

            GL.Flush();

            resize(viewport[2], viewport[3]);

            int hits = GL.RenderMode(RenderingMode.Render);

            uint closest = uint.MaxValue;
            int selectedId = -1;

            for (int i = 0; i < hits; i++)
            {
                uint distance = (uint)sz[i * 4 + 1];

                if (closest >= distance)
                {
                    closest = distance;
                    selectedId = (int)sz[i * 4 + 3];
                }
            }

            return selectedId;
        }

        public void resize(int w, int h)
        {
            width = w;
            height = h;

            GL.Viewport(0, 0, width, height);
        }
    }
}
