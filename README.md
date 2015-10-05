# OpenGLF
Powerful 2D OpenGL Framework for C#

Quick start:

```cs
using System;
using OpenGLF;
using OpenTK;
using System.IO;
using System.Windows.Forms;

namespace Test
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
            GameObject test;
            Font font;

            public MainWindow()
            {
                engine.afterDraw += Engine_afterDraw; //Drawing listener
            }
            
            // Drawing listener
            private void Engine_afterDraw()
            {
                //Draw some text
                Drawing.drawText(new Vector(20, 20), Vector.zero, new Vector(1, 1), 0, 250, 50, "Hello World!", Color.yellow, 15, font);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                base.OnUpdateFrame(e); // Do not remove that line

                //Your update code
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e); // Do not remove that line
                
                Engine.scene = new Scene(); //Create new scene

                font = new Font("Assets/OpenSans-Bold.ttf"); //Add a true type font from file

                test = new GameObject(); //Create new game object
                test.components.Add(new Sprite()); //Add a sprite to game object

                test.sprite.width = 512; // set sprite width
                test.sprite.height = 512; // set sprite height

                test.sprite.material = new Material(); //Add a material to sprite

                Texture tex = new Texture("Assets/tex.png"); // Create new texture from file
                Sampler2D smp_tex = new Sampler2D(tex); // Create new sampler for shader

     
                test.sprite.material.parameters["diffuse"] = smp_tex; // Pass texture to shader
                test.sprite.material.parameters["colorkey"] = new Vec4(1.0f, 1.0f, 1.0f, 1.0f); // pass texture color tint to shader
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);  // Do not remove that line
            }
        }
    }
}
```
