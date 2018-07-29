using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.FakeDb.Serialization
{
    /// <summary>
    /// Exception thrown if the deserialization logic encounters a duplicated content item ID.
    /// Ensures that the user gets a meaningful error message to help them debug their serialized content.
    /// </summary>
    public class DuplicateIdException : ArgumentException
    {
        public DuplicateIdException(ID id, string firstFile, string secondFile)
            : base(string.Format(
                "FakeDb.Serialization is unable to process this content tree.\n\nThe item id {0} defined by item file being processed \"{1}\" has already been used by the item in \"{2}\"",
                id, firstFile, secondFile))
        {
        }
    }
}