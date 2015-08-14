namespace Sitecore.FakeDb.Tests
{
  using System;

  internal static class StringExtensions
  {
    public static void ToConsoleOut(this string str)
    {
      Console.Out.WriteLine("str = {0}", str);
    }
  }
}