using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OpenGLF
{
    [Serializable]
    public class Asset : IDisposable
    {
        bool disposed = false;
        string _name;
        string _package;
        int _resid;

        [Browsable(false)]
        public string package { get { return _package; } set { _package = value; } }
        public string name { get { return _name; } set { setName(value); } }

        [Browsable(false)]
        public int RES_ID { get { return _resid; } }

        public Asset()
        {
            _resid = Assets._assets;
            Assets._assets += 1;

            Assets.items.Add(this);
        }

        void setName(string n)
        {
            Regex regex = new Regex(@"\d+");
            string val = regex.Replace(n, "");

            if (Assets.find(n) != null)
                _name = val + RES_ID.ToString();
            else
                _name = n;

            if (String.IsNullOrEmpty(_name))
                _name = "Empty asset";
        }

        ~Asset()
        {

        }

        public override string ToString()
        {
            return name;
        }

        public virtual void Loaded()
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {

            }


            disposed = true;
        }
    }
}
