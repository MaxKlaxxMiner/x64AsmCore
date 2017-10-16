#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable ConvertToConstant.Local
#endregion

namespace OpCodeGenerator
{
  /// <summary>
  /// Klasse mit den Methoden zum generieren der Opcodes
  /// </summary>
  public static class GeneratorV2
  {
    /// <summary>
    /// generiert alle OpCodes
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    public static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[15];

      yield break;
    }
  }
}
