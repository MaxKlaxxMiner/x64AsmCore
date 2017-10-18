#region # using *.*
// ReSharper disable once RedundantUsingDirective
using System.Linq;
using System.Xml.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global
#endregion

namespace RefScanner
{
  public sealed class RefOneByte
  {
    public readonly RefPriOpcd[] priOpcd;

    public RefOneByte(XElement xml)
    {
      priOpcd = xml.Descendants("pri_opcd").Select(x => new RefPriOpcd(x)).ToArray();
    }

    public override string ToString()
    {
      return "priOpcd: RefPriOpcd[" + priOpcd.Length + "]";
    }
  }
}
