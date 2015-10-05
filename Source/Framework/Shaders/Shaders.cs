using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    public class Shaders
    {
        public Shader defaultShader { get; protected set; }
        public Shader guiShader { get; protected set; }

        public Shaders()
        {
            defaultShader = new Shader();
            defaultShader.compile();

            guiShader = new Shader();

            guiShader.vertexProgram =
                        "void main(void)" +
                        "{" +
                            "gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;" +
                            "gl_Position = ftransform();" +
                        "}";

            guiShader.fragmentProgram =
                        "uniform sampler2D texture;" +
                        "uniform vec4 color;" +
                        "void main(void)" +
                        "{" +
                            "vec4 col = texture2D(texture, gl_TexCoord[0].st);" +
                            "gl_FragColor = col * color;" +
                        "}";

            guiShader.compile();
        }
    }
}
