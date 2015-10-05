using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;

namespace OpenGLF
{
    public class Window : OpenTK.GameWindow
    {
        protected Engine engine;
        static Window self;
        public static bool showCursor { get { return self.CursorVisible; } set { self.CursorVisible = value; } }

        public Window()
            : base(800, 600)
        {
            self = this;
            showCursor = true;

            this.VSync = VSyncMode.Adaptive;
            engine = new Engine();

            try
            {
                engine.loadSeparatedAssets("Assets", true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            engine.resize(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            engine.start(true);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            engine.draw(RenderingMode.Render, true);
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            engine.resize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            int w = 0;
            int h = 0;
            float zoom = 1;
            float x = 0;
            float y = 0;
    
            if (Camera.main != null)
            {
                w = Screen.width / 2;
                h = Screen.height / 2;
                zoom = Camera.main.z;
                x = (float)Camera.main.gameObject.position.x;
                y = (float)Camera.main.gameObject.position.y;
            }
            Input._mousePos = new Vector(Mouse.X * zoom - w * zoom + x, Mouse.Y * zoom - h * zoom + y);

            Input._mouseInfinityPos = new Vector(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

            engine.update((float)e.Time, true);

            Thread.Sleep((int)e.Time);
        }

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            if (Input._wheel < e.Value)
                Input._wheelState = -1;
            else
                Input._wheelState = 1;

            Input._wheel = e.Value;
        }

        public static void setFullscreen(bool full)
        {
            if (full)
                self.WindowState = OpenTK.WindowState.Fullscreen;
            else
                self.WindowState = OpenTK.WindowState.Normal;
        }

        public static void close()
        {
            self.Close();
        }
    }
}
