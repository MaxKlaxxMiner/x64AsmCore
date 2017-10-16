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
// ReSharper disable MemberCanBePrivate.Global
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

    /// <summary>
    /// gibt eine bestimmte Anzahl von Bytes in hexadezimale Schreibweise zurück und endet mit einem Bindestrich (z.B. "00 04 f4 - ")
    /// </summary>
    /// <param name="bytes">Bytes, welche ausgelesen werden sollen</param>
    /// <param name="lastByte">zeig auf das letzte Byte in der Kette, welche ausgelesen werden sollen</param>
    /// <param name="constBytes">optionale zusätzliche Bytes für eine direkte Konstante</param>
    /// <returns>fertige Zeichenkette</returns>
    static string StrB(byte[] bytes, int lastByte, int constBytes = 0)
    {
      return string.Join(" ", bytes.Take(lastByte + 1).Select(b => b.ToString("x2")).Concat(Enumerable.Range(0, constBytes).Select(i => "xx"))) + " - ";
    }

    /// <summary>
    /// langsame Dekodierungs-Variante mit voller Schreibweise inkl. Opcodes
    /// </summary>
    /// <returns>fertige dekodierte Zeile</returns>
    public string FullDecodeSlow()
    {
      uint len = Length;
      if (len >= 8) throw new NotImplementedException();

      return StrB(BitConverter.GetBytes(opcodeLo), (int)len - 1) + Ins.GetInstructionName(insCodes).ToLower();
    }

    /// <summary>
    /// gibt den Inhalt als lesbare Zeichenfolge zurück
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return FullDecodeSlow();
    }
  }
}
