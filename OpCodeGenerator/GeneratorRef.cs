#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RefScanner;

// ReSharper disable ConvertToConstant.Local
#endregion

namespace OpCodeGenerator
{
  /// <summary>
  /// Klasse mit den Methoden zum generieren der Opcodes anhand der original x86reference.xml
  /// </summary>
  public static class GeneratorRef
  {
    /// <summary>
    /// generiert alle OpCodes
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    public static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[15];

      var oneByte = RefPriOpcd.ReadElements();

      yield break;
    }
  }
}
