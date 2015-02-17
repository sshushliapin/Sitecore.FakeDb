using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace Sitecore.FakeDb.Serialization
{
    internal class SerializedIdToPathSet
    {
        internal Dictionary<ID, string> Paths { get; set; }
        internal Stack<string> FilePaths { get; set; }

        internal SerializedIdToPathSet()
        {
            Paths = new Dictionary<ID, string>();
            FilePaths = new Stack<string>();
        }
    }
}
