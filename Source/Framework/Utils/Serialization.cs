using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace OpenGLF
{
    public class Serialization
    {
        public static void serialize(string fn, object target)
        {
            Stream fs = File.Open(fn, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            
            try
            {
                
                formatter.Serialize(fs, target);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                formatter = null;
            }
        }

        public static void serialize(Stream fs, object target)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                formatter.Serialize(fs, target);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                formatter = null;
            }
        }

        public static object deserialize(string fn)
        {
            object ret = null;

            Stream fs = File.Open(fn, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                ret = formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                formatter = null;
            }

            return ret;
        }

        public static object deserialize(Stream fs)
        {
            object ret = null;

            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                ret = formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                formatter = null;
            }

            return ret;
        }
    }
}
