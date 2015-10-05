using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace OpenGLF
{
    public class Drawing
    {
        static BezierCurveCubic bezier = new BezierCurveCubic();

        public static void drawCircle(Vector pos, int radius, bool fill, float weight, Color color)
        {
            GL.Color4(1.0f / 255.0f * (float)color.r, 1.0f / 255.0f * (float)color.g, 1.0f / 255.0f * (float)color.b, 1.0f / 255.0f * (float)color.a);

            GL.LineWidth((float)weight);

            if (fill == true)
                GL.Begin(PrimitiveType.TriangleFan);
            else
                GL.Begin(PrimitiveType.LineLoop);

            for (int i = 0; i < 50; i++)
            {
                GL.Vertex2(
                        pos.x + (radius * Math.Cos(i * 2.0f * Math.PI / 50.0f)),
                        pos.y + (radius * Math.Sin(i * 2.0f * Math.PI / 50.0f))
                );
            }

            GL.End();
        }

        public static void drawPoint(Vector pos, float weight, Color color)
        {
            GL.Color4(1.0f / 255.0f * (float)color.r, 1.0f / 255.0f * (float)color.g, 1.0f / 255.0f * (float)color.b, 1.0f / 255.0f * (float)color.a);

            GL.PointSize((float)weight);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(pos.x, pos.y);
            GL.End();
        }

        public static void drawLine(Vector start, Vector end, float weight, Color color)
        {
            GL.Color4(1.0f / 255.0f * (float)color.r, 1.0f / 255.0f * (float)color.g, 1.0f / 255.0f * (float)color.b, 1.0f / 255.0f * (float)color.a);

            GL.LineWidth((float)weight);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(start.x, start.y);
            GL.Vertex2(end.x, end.y);
            GL.End();
        }

        public static void drawQuad(Vector start, float width, float height, float weight, bool fill, Color color)
        {
            GL.Color4(1.0f / 255.0f * (float)color.r, 1.0f / 255.0f * (float)color.g, 1.0f / 255.0f * (float)color.b, 1.0f / 255.0f * (float)color.a);

            GL.LineWidth((float)weight);

            if (fill)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(start.x, start.y);
                GL.Vertex2(start.x + width, start.y);
                GL.Vertex2(start.x + width, start.y + height);
                GL.Vertex2(start.x, start.y + height);
            }
            else
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(start.x, start.y);
                GL.Vertex2(start.x + width, start.y);
                GL.Vertex2(start.x + width, start.y);
                GL.Vertex2(start.x + width, start.y + height);
                GL.Vertex2(start.x + width, start.y + height);
                GL.Vertex2(start.x, start.y + height);
                GL.Vertex2(start.x, start.y + height);
                GL.Vertex2(start.x, start.y);
            }

            GL.End();
        }

        public static void drawQuadRounded(Vector start, float width, float height, float weight, int cornerRadius, bool fill, Color color)
        {
                drawCorner(start + new Vector(cornerRadius, cornerRadius), cornerRadius, weight, 12, 0, fill, color);
                drawCorner(start + new Vector(width, 0) + new Vector(-cornerRadius, cornerRadius), cornerRadius, weight, 12, 90, fill, color);
                drawCorner(start + new Vector(0, height) + new Vector(cornerRadius, -cornerRadius), cornerRadius, weight, 12, -90, fill, color);
                drawCorner(start + new Vector(width, height) + new Vector(-cornerRadius, -cornerRadius), cornerRadius, weight, 12, 180, fill, color);

            if (fill)
            {
                drawQuad(start + new Vector(cornerRadius, cornerRadius), width - cornerRadius * 2, height - cornerRadius * 2, weight, fill, color);

                drawQuad(start + new Vector(cornerRadius, 0), width - cornerRadius * 2, cornerRadius, width, fill, color);
                drawQuad(start + new Vector(cornerRadius, height - cornerRadius), width - cornerRadius * 2, cornerRadius, width, fill, color);
                drawQuad(start + new Vector(0, cornerRadius), cornerRadius, height - cornerRadius * 2, width, fill, color);
                drawQuad(start + new Vector(width - cornerRadius, cornerRadius), cornerRadius, height - cornerRadius * 2, width, fill, color);
            }
            else
            {
                drawLine(start + new Vector(cornerRadius, 0), start + new Vector(width - cornerRadius, 0), weight, color);
                drawLine(start + new Vector(cornerRadius, height), start + new Vector(width - cornerRadius, height), weight, color);
                drawLine(start + new Vector(0, cornerRadius), start + new Vector(0, height - cornerRadius), weight, color);
                drawLine(start + new Vector(width, cornerRadius), start + new Vector(width, height - cornerRadius), weight, color);
            }
        }

        public static void drawCorner(Vector start, float width, float weight, int steps, float angle, bool fill, Color color)
        {
            GL.Color4(1.0f / 255.0f * (float)color.r, 1.0f / 255.0f * (float)color.g, 1.0f / 255.0f * (float)color.b, 1.0f / 255.0f * (float)color.a);

            double sc = width;

            double I1x = 0;
            double I1y = 0;

            double delta = ((Math.PI / 2) / steps);
            double lastX = I1x - sc;
            double lastY = I1y;

            GL.LineWidth(weight);
            GL.PointSize(weight);

            GL.PushMatrix();
            GL.Translate(start.x, start.y, 0);
            GL.Rotate(angle, 0, 0, 1);

            if (fill)
                GL.Begin(PrimitiveType.Triangles);
            else
                GL.Begin(PrimitiveType.LineLoop);

            for (double w = delta, i = 1; i <= steps; w += delta, ++i)
            {
                double x = -Math.Cos(w);
                double y = -Math.Sin(w);
                double x1 = I1x + (x * sc);
                double y1 = I1y + (y * sc);

                GL.Vertex2(lastX, lastY);
                GL.Vertex2(x1, y1);
                GL.Vertex2(I1x, I1y);

                lastX = x1;
                lastY = y1;
            }

            GL.End();
            GL.PopMatrix();
        }

        public static void drawBezierCurve(Vector start, Vector end, Vector startOffset, Vector endOffset, float weight, Color color)
        {
            // setup anchor points and control points
            bezier.StartAnchor = new Vector2(start.x, start.y);
            bezier.EndAnchor = new Vector2(end.x, end.y);

            bezier.FirstControlPoint = new Vector2(start.x, start.y) + new Vector2(startOffset.x, startOffset.y);
            bezier.SecondControlPoint = new Vector2(end.x, end.y) - new Vector2(endOffset.x, endOffset.y);

            Vector2 oldPoint = bezier.StartAnchor;
            Vector2 point = new Vector2();

            // calculate 100 points on the curve
            for (float t = 0.0f; t <= 1.0f; t += 0.01f)
            {
                point = bezier.CalculatePoint(t);
                drawLine(new Vector(point.X, point.Y), new Vector(oldPoint.X, oldPoint.Y), weight, color);
                // draw with opengl a line from oldPoint to point
                oldPoint = point;
            }

            // calculate length of curve with a precision of 0.01f (100 steps)
            //float length = bezier.CalculateLength(0.01f);
        }

        public static void drawText(Vector position, Vector center, Vector scale, int angle, int width, int height, string text, Color color, int size, Font font)
        {
            if (font != null)
            {
                font.renderString(position, center, scale, angle, width, height, text, color, size);
            }
        }

        public static void drawText(string text, Vector position, int width, int height, Color color)
        {
            drawText(position, Vector.zero, new Vector(1, 1), 0, width, height, text, color, 14, (Font)Assets.find(typeof(Font)));
        }

        public static void drawTexture(Texture texture, Vector position, Vector center, Vector scale, int angle, int width, int height)
        {
            Vector[] vertexBuf = new Vector[4];
            Vector[] normalsBuf = new Vector[4];
            Vector[] textureBuf = new Vector[4];

            vertexBuf[0].x = -center.x; vertexBuf[0].y = -center.y;
            vertexBuf[1].x = width - center.x; vertexBuf[1].y = -center.y;
            vertexBuf[2].x = width - center.x; vertexBuf[2].y = height - center.y;
            vertexBuf[3].x = -center.x; vertexBuf[3].y = height - center.y;

            textureBuf[0] = new Vector(0, 0);
            textureBuf[1] = new Vector(1, 0);
            textureBuf[2] = new Vector(1, 1);
            textureBuf[3] = new Vector(0, 1);

            //Verts
            int vId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vId);
            GL.BufferData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf, BufferUsageHint.StreamDraw);
            GL.BufferSubData<Vector>(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(Marshal.SizeOf(new Vector()) * 2 * vertexBuf.Length), vertexBuf);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Textures
            int tId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tId);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * 2 * textureBuf.Length), textureBuf, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.PushMatrix();

            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            Texture tex = new Texture();
            Bitmap bitmap = new Bitmap(texture.bitmap, width, height);
            tex.LoadFromBitmap(bitmap);

            Engine.shaders.guiShader.begin();
            Engine.shaders.guiShader.pass("texture", tex);
            Engine.shaders.guiShader.pass("color", new Vec4(1, 1, 1, 1));

            GL.Translate(position.x, position.y, 0);
            GL.Rotate(angle, 0, 0, 1);

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

            Engine.shaders.guiShader.passNullTex("texture");
            Engine.shaders.guiShader.end();

            GL.DeleteTexture(tex.ID);

            bitmap.Dispose();
            tex.Dispose();

            GL.PopMatrix();

            //Отключаем все
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);

            GL.DeleteBuffer(vId);
            GL.DeleteBuffer(tId);
        }
    }
}
