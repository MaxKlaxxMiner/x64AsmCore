#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Local
#endregion

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// Assembler Instruktion
  /// </summary>
  public struct AsmResult
  {
    /// <summary>
    /// merkt sich die ersten 8 OpCodes (0-7)
    /// </summary>
    readonly ulong opcodeLo;
    /// <summary>
    /// merkt sich die nächsten 8 OpCodes (8-15)
    /// </summary>
    readonly ulong opcodeHi;

    /// <summary>
    /// merkt sich die eingelesene Instruktion
    /// byte 0: Länge der Instruktion (1 - 15)
    /// </summary>
    readonly ulong insCodes;

    /// <summary>
    /// merkt sich die kodierten Parameter
    /// </summary>
    readonly ulong parCodes;

    #region # // --- Properties ---
    /// <summary>
    /// gibt die Länge der gesamten Instruktion in Bytes zurück (1 - 15)
    /// </summary>
    public int Length
    {
      get
      {
        return (byte)insCodes;
      }
    }
    #endregion

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="opcodeLo">die ersten 8 OpCodes (0-7)</param>
    /// <param name="opcodeHi">die nächsten 8 OpCodes (8-15)</param>
    /// <param name="insCodes">die eingelesene Instruktion</param>
    /// <param name="parCodes">die kodierten Parameter</param>
    public AsmResult(ulong opcodeLo, ulong opcodeHi, ulong insCodes, ulong parCodes)
    {
      this.opcodeLo = opcodeLo;
      this.opcodeHi = opcodeHi;
      this.insCodes = insCodes;
      this.parCodes = parCodes;

      switch ((byte)insCodes)
      {
        default: throw new Exception("invalid Instruciton-Length: " + (byte)insCodes + " (0x" + insCodes.ToString("x16") + ")");
      }
    }
  }
}
