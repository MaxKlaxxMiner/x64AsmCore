#region # using *.*
// ReSharper disable RedundantUsingDirective
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global
#endregion

namespace RefScanner
{
  public sealed class RefPriOpcd
  {
    public readonly byte value;
    public readonly int procStart;
    public readonly RefEntry[] entries;

    public RefPriOpcd(XElement xml)
    {
      value = xml.ParseHex("value", (byte)0);
      procStart = xml.ParseHex("proc_start", 0);
      entries = xml.Descendants("entry").Select(x => new RefEntry(x)).ToArray();
    }

    public override string ToString()
    {
      return (new { value = value.ToString("X2"), procStart, entries = "RefEntry[" + entries.Length + "]" }).ToString();
    }
  }
}
