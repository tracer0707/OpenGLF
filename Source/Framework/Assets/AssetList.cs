using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace OpenGLF
{
    [Serializable]
    public class AssetList : List<Asset>
    {
        internal string name;
    }
}
