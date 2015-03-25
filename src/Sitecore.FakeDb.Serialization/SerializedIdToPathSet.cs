namespace Sitecore.FakeDb.Serialization
{
  using System.Collections.Generic;
  using Sitecore.Data;

  internal class SerializedIdToPathSet
  {
    internal Dictionary<ID, string> Paths { get; set; }

    internal Stack<string> FilePaths { get; set; }

    internal SerializedIdToPathSet()
    {
      this.Paths = new Dictionary<ID, string>();
      this.FilePaths = new Stack<string>();
    }
  }
}