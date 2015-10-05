using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace OpenGLF
{
    public class Assets
    {
        internal static int _assets = 0;
        public static AssetList items = new AssetList();

        public static Asset find(string name)
        {
            foreach (Asset a in items)
                if (a.name == name)
                    return a;

            return null;
        }

        public static Asset find(string name, Type type)
        {
            foreach (Asset a in items)
                if (a.name == name && a.GetType() == type)
                    return a;

            return null;
        }

        public static Asset find(int id)
        {
            foreach (Asset a in items)
                if (a.RES_ID == id)
                    return a;

            return null;
        }

        public static Asset find(int id, Type type)
        {
            foreach (Asset a in items)
                if (a.RES_ID == id && a.GetType() ==  type)
                    return a;

            return null;
        }

        public static Asset find(Type type)
        {
            foreach (Asset a in items)
                if (a.GetType() == type)
                    return a;

            return null;
        }

        internal static System.Collections.Specialized.StringCollection compileScripts(List<string> dependencies)
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
            {
              {"CompilerVersion", "v4.0"}
            };

            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters { GenerateInMemory = true };

            compilerParams.IncludeDebugInformation = false;

            compilerParams.ReferencedAssemblies.Add("System.Core.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParams.ReferencedAssemblies.Add("System.Drawing.Design.dll");
            compilerParams.ReferencedAssemblies.Add("System.Design.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.dll");
            compilerParams.ReferencedAssemblies.Add("System.Runtime.Serialization.dll");
            compilerParams.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            compilerParams.ReferencedAssemblies.Add("OpenTK.dll");
            compilerParams.ReferencedAssemblies.Add("irrKlang.NET4.dll");
            compilerParams.ReferencedAssemblies.Add("FarseerPhysics.dll");
            compilerParams.ReferencedAssemblies.Add("OpenGLF.dll");

            foreach (string d in dependencies)
            {
                compilerParams.ReferencedAssemblies.Add(d);
            }

            int count = 0;

            foreach (Asset asset in Assets.items)
            {
                if (asset is Script)
                {
                    count += 1;
                }
            }

            string[] sources = new string[count];

            count = 0;

            foreach (Asset asset in Assets.items)
            {
                if (asset is Script)
                {
                    sources[count] = ((Script)asset).code;
                    count += 1;
                }
            }

            CompilerResults results;

            results = provider.CompileAssemblyFromSource(compilerParams, sources);

            if (!results.Errors.HasErrors)
            {
                Assembly dll = results.CompiledAssembly;

                if (Engine.scene != null)
                {
                    foreach (GameObject obj in Engine.scene.objects)
                    {
                        foreach (Component cmp in obj.components)
                        {
                            if (cmp is Behavior)
                            {
                                Behavior beh = (Behavior)cmp;
                                
                                Type behtype = typeof(Behavior);

                                if (beh.script != null)
                                {
                                    //Behavior instance;
                                    Type type = dll.GetType(beh.script.name);

                                    try
                                    {
                                        beh.instance = (Behavior)Activator.CreateInstance(type);
                                        beh.instance.gameObject = obj;

                                        IEnumerable<FieldInfo> fields = type.GetFields();

                                        //beh.fields.Clear();

                                        foreach (FieldInfo field in fields)
                                        {
                                            if ((beh.fields.ContainsKey(field.Name) == false) || (beh.fields.ContainsKey(field.Name) == true && beh.fields[field.Name].GetType() != field.FieldType))
                                            {
                                                switch (field.FieldType.Name)
                                                {
                                                    case "GameObject":
                                                        beh.fields[field.Name] = Engine.scene.objects[0];
                                                        break;
                                                    case "Material":
                                                        beh.fields[field.Name] = (Material)Assets.find(typeof(Material));
                                                        break;
                                                    case "Color":
                                                        beh.fields[field.Name] = Color.white;
                                                        break;
                                                    case "Single":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Float":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Int32":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Byte":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "String":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Boolean":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Vector":
                                                        beh.fields[field.Name] = field.GetValue(beh.instance);
                                                        break;
                                                    case "Prefab":
                                                        Prefab p = new Prefab();
                                                        p.name = "";
                                                        beh.fields[field.Name] = p;
                                                        break;
                                                }
                                            }
                                        }
                                        
                                        foreach (FieldInfo field in fields)
                                        {
                                            if (beh.fields.ContainsKey(field.Name) == false)
                                            {
                                                beh.fields.Remove(field.Name);
                                                //break;
                                            }
                                        }

                                        foreach (KeyValuePair<string, object> field in beh.fields)
                                        {
                                            if (field.Key == "enabled")
                                                beh.fields.Remove("enabled");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        beh.fields.Clear();
                                        beh.script = null;
                                        MessageBox.Show(e.Message);
                                    }
                                }
                                else
                                {
                                    beh.fields.Clear();
                                }
                            }
                        }
                    }
                }
                dll = null;
            }
            else
            {
                foreach (string s in results.Output)
                    MessageBox.Show(s);
            }

            return results.Output;
        }
    }
}
