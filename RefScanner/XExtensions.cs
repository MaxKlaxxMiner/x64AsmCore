using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefScanner
{
  public static class XExtensions
  {
    static string GetValue(this XElement xel, string name)
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

    public static int ParseHex(this XElement xel, string name, int alternate)
    {
      int result;
      return int.TryParse(xel.GetValue(name), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) ? result : alternate;
    }
  }
}
