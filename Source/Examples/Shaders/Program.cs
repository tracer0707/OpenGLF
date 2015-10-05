using System;
using OpenGLF;
using OpenTK;
using System.IO;
using System.Windows.Forms;

namespace Shaders
{
    class Program
    {
        static void Main(string[] args)
        {
            MainWindow wnd = new MainWindow();
            wnd.Run();
        }

        class MainWindow : Window
        {
            GameObject bump_test;
            Font font;

            public MainWindow()
            {
                engine.afterDraw += Engine_afterDraw;
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                base.OnUpdateFrame(e);

                bump_test.sprite.material.parameters["light_p[0]"] = new Vec4(Input.mousePosition.x, Input.mousePosition.y, 150.0f, 1.0f);
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                
                Engine.scene = new Scene();

                font = new Font("Assets/OpenSans-Bold.ttf");

                bump_test = new GameObject();
                bump_test.components.Add(new Sprite());

                bump_test.sprite.width = 512;
                bump_test.sprite.height = 512;

                bump_test.sprite.material = new Material();

                Shader bump = new Shader();

                bump.name = "Bump shader";
                bump.vertexProgram = File.ReadAllText("Assets/vertex.glsl");
                bump.fragmentProgram = File.ReadAllText("Assets/fragment.glsl");
                bump.fullRecompile();

                bump_test.sprite.material.shader = bump;

                Texture tex = new Texture("Assets/tex.png");
                Texture nrm = new Texture("Assets/tex_nrm.png");
                Sampler2D smp_tex = new Sampler2D(tex);
                Sampler2D smp_nrm = new Sampler2D(nrm);

                bump_test.sprite.material.parameters["light_c"] = (int)1;

                bump_test.sprite.material.parameters["texture"] = smp_tex;
                bump_test.sprite.material.parameters["normal"] = smp_nrm;
                bump_test.sprite.material.parameters["color"] = new Vec4(1.0f, 1.0f, 1.0f, 1.0f);
                bump_test.sprite.material.parameters["ambient"] = new Vec4(0.0f, 0.0f, 0.0f, 1.0f);
                bump_test.sprite.material.parameters["light_p[0]"] = new Vec4(256.0f, 256.0f, 150.0f, 1.0f);
                bump_test.sprite.material.parameters["light_col[0]"] = new Vec4(1.0f, 1.0f, 1.0f, 1.0f);
                bump_test.sprite.material.parameters["light_int[0]"] = (float)0.3f;

                bump_test.sprite.material.parameters["amount"] = (float)1.0f;
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);
            }

            private void Engine_afterDraw()
            {
                Drawing.drawText(new Vector(20, 20), Vector.zero, new Vector(1, 1), 0, 250, 50, "Bump shader example", Color.yellow, 15, font);
            }
        }
    }
}
