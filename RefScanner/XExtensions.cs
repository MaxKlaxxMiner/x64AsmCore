using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

namespace RefScanner
{
  public static class XExtensions
  {
    public static string GetValue(this XElement xel, string name)
    {
      var attr = xel.Attribute(name);
      if (attr != null) return attr.Value;
      var el = xel.Element(name);
      if (el != null) return el.Value;
      return null;
    }

    public static byte ParseHex(this XElement xel, string name, byte alternate)
    {
      byte result;
      return byte.TryParse(xel.GetValue(name), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) ? result : alternate;
    }

    public static byte? ParseHex(this XElement xel, string name)
    {
      byte result;
      return byte.TryParse(xel.GetValue(name), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) ? result : (byte?)null;
    }

    public static int ParseHex(this XElement xel, string name, int alternate)
    {
      int result;
      return int.TryParse(xel.GetValue(name), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) ? result : alternate;
    }

    public static int? ParseInt(this XElement xel, string name)
    {
      int result;
      return int.TryParse(xel.GetValue(name), NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : (int?)null;
    }

    public static bool? ParseYesNo(this XElement xel, string name)
    {
      string val = xel.GetValue(name);
      if (val == null) return null;
      if (val == "yes") return true;
      if (val == "no") return false;
      Debug.Fail("yes/no ? \"" + val + "\"");
      return null;
    }
  }
}
