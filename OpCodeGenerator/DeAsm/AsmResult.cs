#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Local
#endregion

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// vollständige Assembler Instruktion
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
    /// merkt sich die eingelesene Instruktion mit weiteren Informationen
    /// </summary>
    readonly ulong insCodes;

    #region # // --- Properties ---
    /// <summary>
    /// gibt die Länge der gesamten Instruktion in Bytes zurück (1 - 15)
    /// </summary>
    public uint Length { get { return (uint)Ins.GetLength(insCodes); } }
    /// <summary>
    /// gibt die Länge der Instruktion ohne Konstante/Offset zurück (1 - 15)
    /// </summary>
    public uint LengthWithoutConst
    {
      get
      {
        throw new NotImplementedException();
      }
    }
    #endregion

    #region # // --- Masken ---
    static readonly ulong[] MaskLo =
    {
      0x0000000000000000, 0x00000000000000ff, 0x000000000000ffff, 0x0000000000ffffff,
      0x00000000ffffffff, 0x000000ffffffffff, 0x0000ffffffffffff, 0x00ffffffffffffff,
      0xffffffffffffffff, 0xffffffffffffffff, 0xffffffffffffffff, 0xffffffffffffffff,
      0xffffffffffffffff, 0xffffffffffffffff, 0xffffffffffffffff, 0xffffffffffffffff
    };
    static readonly ulong[] MaskHi =
    {
      0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000,
      0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000,
      0x0000000000000000, 0x00000000000000ff, 0x000000000000ffff, 0x0000000000ffffff,
      0x00000000ffffffff, 0x000000ffffffffff, 0x0000ffffffffffff, 0x00ffffffffffffff
    };
    #endregion

    /// <summary>
    /// direkter Konstruktor
    /// </summary>
    /// <param name="opcodeLo">die ersten 8 OpCodes (0-7)</param>
    /// <param name="opcodeHi">die nächsten 8 OpCodes (8-15)</param>
    /// <param name="insCodes">die eingelesene Instruktion</param>
    public AsmResult(ulong opcodeLo, ulong opcodeHi, ulong insCodes)
    {
      var len = Ins.GetLength(insCodes);
      Debug.Assert(len >= Ins.MinLength && len <= Ins.MaxLength);
      this.opcodeLo = opcodeLo & MaskLo[len];
      this.opcodeHi = opcodeHi & MaskHi[len];
      this.insCodes = insCodes;
    }
  }
}
