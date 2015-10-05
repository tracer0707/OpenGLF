using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Design;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable()]
    [TypeConverter(typeof(ShaderEditor))]
    public class Shader : Asset
    {
        [NonSerialized]
        int vertexShader, fragmentShader, program;

        [NonSerialized]
        bool compiled = false;

        string vert = 
		"void main(void)" +
		"{"+
			"gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;" +
			"gl_Position = ftransform();" +
		"}";

	    string frag =
		"uniform sampler2D diffuse;" +
        "uniform vec4 colorkey;" +
		"void main(void)" +
		"{" +
			"vec4 color = texture2D(diffuse, gl_TexCoord[0].st);" +
			"gl_FragColor = color * colorkey;" +
		"}";
        
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        public string vertexProgram { get { return vert; } set { vert = value; } }

        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        public string fragmentProgram { get { return frag; } set { frag = value; } }

        [NonSerialized]
        Dictionary<string, object> _uniforms;

        public Dictionary<string, object> uniforms { get { return _uniforms; } internal set { _uniforms = value; } }

        public Shader()
        {
            name = "Shader";
        }

        public void compile(){
            if (compiled == false)
            {
                compiled = true;

                uniforms = new Dictionary<string, object>();

                GL.LinkProgram(0);

                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);
                GL.DeleteProgram(program);

                vertexShader = GL.CreateShader(ShaderType.VertexShader);
                fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

                GL.ShaderSource(vertexShader, vert);
                GL.ShaderSource(fragmentShader, frag);

                GL.CompileShader(vertexShader);
                GL.CompileShader(fragmentShader);

                if (!String.IsNullOrEmpty(GL.GetShaderInfoLog(vertexShader)))
                    Console.WriteLine(GL.GetShaderInfoLog(vertexShader));

                if (!String.IsNullOrEmpty(GL.GetShaderInfoLog(fragmentShader)))
                    Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

                program = GL.CreateProgram();

                GL.AttachShader(program, vertexShader);
                GL.AttachShader(program, fragmentShader);

                GL.LinkProgram(program);

                if (!String.IsNullOrEmpty(GL.GetProgramInfoLog(program)))
                    Console.WriteLine(GL.GetProgramInfoLog(program));
                else
                    Console.WriteLine("Shader \"" + name + "\" successfully compiled");

                int total = 0;

                GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out total);
                for (int i = 0; i < total; i++)
                {
                    int size = 16;
                    int name_len = 16;

                    ActiveUniformType type = ActiveUniformType.Sampler2D;
                    StringBuilder name = new StringBuilder();

                    GL.GetActiveUniform(program, i, 16, out name_len, out size, out type, name);

                    switch (type)
                    {
                        case ActiveUniformType.Sampler2D:
                            if (!String.IsNullOrEmpty(name.ToString()))
                                uniforms[name.ToString()] = new Sampler2D();
                            break;
                        case ActiveUniformType.FloatVec4:
                            if (!String.IsNullOrEmpty(name.ToString()))
                                uniforms[name.ToString()] = new Vec4();
                            break;
                        case ActiveUniformType.Float:
                            if (!String.IsNullOrEmpty(name.ToString()))
                                uniforms[name.ToString()] = new Float();
                            break;
                        case ActiveUniformType.Int:
                            if (!String.IsNullOrEmpty(name.ToString()))
                                uniforms[name.ToString()] = new Int();
                            break;
                    }

                    name = null;
                    size = 0;
                    name_len = 0;
                }

                for (int i = 0; i < Assets.items.Count; i++)
                {
                    if (Assets.items[i] is Material)
                    {
                        Material m = (Material)Assets.items[i];
                        if (m.shader == this)
                        {
                            m.updateParameters();
                        }
                    }
                }
            }
        }

        public void fullRecompile()
        {
            compiled = false;
            compile();
        }

        public void begin()
        {
            GL.UseProgram(program);
        }

        public void end()
        {
            GL.UseProgram(0);
        }

        public void pass(string name, Texture tex)
        {
            int l = GL.GetUniformLocation(program, name);
            GL.ActiveTexture(TextureUnit.Texture0 + tex.ID);
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Uniform1(l, tex.ID);
            GL.ActiveTexture(TextureUnit.Texture0);
            l = 0;
        }

        public void passNullTex(string name)
        {
            int l = GL.GetUniformLocation(program, name);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Uniform1(l, 0);
            l = 0;
        }

        public void pass(string name, Vec4 vec)
        {
            int l = GL.GetUniformLocation(program, name);
            GL.Uniform4(l, vec.x, vec.y, vec.z, vec.w);
            l = 0;
        }

        public void pass(string name, float val)
        {
            int l = GL.GetUniformLocation(program, name);
            GL.Uniform1(l, val);
            l = 0;
        }

        public void pass(string name, int val)
        {
            int l = GL.GetUniformLocation(program, name);
            GL.Uniform1(l, val);
            l = 0;
        }

        public int getProgram()
        {
            return program;
        }

        public override string ToString()
        {
            return name;
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            //compile();
            //Console.WriteLine("Deserialized");
        }
    }

    public class ShaderEditor : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> names = new List<string>();
            names.Add("");

            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Shader)
                    names.Add(Assets.items[i].name);
            }

            return new StandardValuesCollection(names);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            List<Shader> names = new List<Shader>();
            for (int i = 0; i < Assets.items.Count; i++)
            {
                if (Assets.items[i] is Shader)
                    names.Add((Shader)Assets.items[i]);
            }

            if (value is string)
            {
                foreach (Shader s in names)
                {
                    if (s.name == (string)value)
                    {
                        return s;
                    }
                }

                if (String.IsNullOrEmpty((string)value))
                    return null;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}