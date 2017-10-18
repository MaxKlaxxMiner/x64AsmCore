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
  public sealed class RefParam
  {
    public readonly int? nr;
    public readonly string group;
    public readonly string type;
    public readonly string address;
    public readonly bool? displayed;
    public readonly bool? depend;
    public readonly string a;
    public readonly string t;

    public RefParam(XElement xml)
    {
      nr = xml.ParseInt("nr");
      group = xml.GetValue("group");
      type = xml.GetValue("type");
      address = xml.GetValue("address");
      displayed = xml.ParseYesNo("displayed");
      depend = xml.ParseYesNo("depend");
      a = xml.GetValue("a");
      t = xml.GetValue("t");
    }

    public override string ToString()
    {
      string res = "";

      if (nr != null) res += ", nr: " + nr;
      if (group != null) res += ", group: " + group;
      if (type != null) res += ", type: " + type;
      if (address != null) res += ", address: " + address;
      if (displayed != null) res += ", displayed: " + displayed;
      if (depend != null) res += ", depend: " + depend;
      if (a != null) res += ", a: " + a;
      if (t != null) res += ", t: " + t;

      return res.TrimStart(',', ' ');
    }
  }
}
