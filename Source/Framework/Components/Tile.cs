using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;

namespace OpenGLF
{
    public class Tile : Sprite
    {
        [Browsable(false)]
        public Texture texture { get; set; } = null;

        public Tile()
        {
            material = new Material();
            material.shader = Engine.shaders.defaultShader;
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
                    material.shader.pass("diffuse", texture);
                    material.shader.pass("colorkey", new Vec4(1, 1, 1, 1));
                }

                GL.Translate(gameObject.position.x, gameObject.position.y, 0);
                GL.Rotate(gameObject.angle, 0, 0, 1);
                GL.Scale(scale.x, scale.y, 1);

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

                material.shader.passNullTex("diffuse");

                material.shader.end();

                GL.PopMatrix();

                //Отключаем все
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }
        }
    }
}
