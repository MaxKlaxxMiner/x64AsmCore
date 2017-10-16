using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// Dekompiler für 64-Bit Code
  /// </summary>
  public static unsafe class DeAsm64
  {
    /// <summary>
    /// dekodiert eine einzelne Assembler Anweisung (64-Bit Modus)
    /// </summary>
    /// <param name="code">Byte-Array, wo sich der Code befindet</param>
    /// <param name="codeOffset">Startposition innerhalb der Arrays</param>
    /// <returns></returns>
    public static AsmResult Decode(byte[] code, uint codeOffset)
    {
      if (code == null || code.Length == 0) throw new ArgumentNullException("code");
      if (codeOffset >= code.Length) throw new ArgumentOutOfRangeException("codeOffset");
      if (code.Length - codeOffset < 16) Array.Resize(ref code, code.Length + 16);

      fixed (byte* codeP = &code[codeOffset])
      {
        ulong opcodeLo = *(ulong*)codeP;
        ulong opcodeHi = *(ulong*)(codeP + sizeof(ulong));
        ulong insCode = Decode(codeP);
        return new AsmResult(opcodeLo, opcodeHi, insCode);
      }
    }

    /// <summary>
    /// Klasse, um sich den aktuellen Dekodierungstatus zu merken
    /// </summary>
    sealed class DecodeStatus
    {
      /// <summary>
      /// gibt an, ob die gesamte Instruktion als ungültig erkannt wurde
      /// </summary>
      public bool invalid;
      /// <summary>
      /// merkt sich die aktuell erkannte Instruktion
      /// </summary>
      public Ins.Instruction ins;
    }

    /// <summary>
    /// dekodiert eine allgemeine Instruktion
    /// </summary>
    /// <param name="code">Zeiger auf die aktuell zu lesende Bytefolge</param>
    /// <param name="maxRemain">maximale Anzahl der Bytes, welche noch verarbeitet werden dürfen</param>
    /// <param name="status">aktueller Status der Dekodierung</param>
    /// <returns>Anzahl der Bytes, welche vearbeitet wurden</returns>
    static int DecodeInternal(byte* code, int maxRemain, DecodeStatus status)
    {
      switch (*code)
      {
        case 0x06: status.invalid = true; return 1;
        case 0xcc: status.ins = Ins.Instruction.Int3; return 1;
        default: throw new NotImplementedException("todo");
      }
    }

    /// <summary>
    /// dekodiert intern einen Assembler-Befehl
    /// </summary>
    /// <param name="code">Zeiger auf den Opcode, welcher dekodiert werden soll</param>
    /// <returns>fertig dekodierte Instruktion</returns>
    public static ulong Decode(byte* code)
    {
      var status = new DecodeStatus();
      int length = DecodeInternal(code, (int)Ins.MaxLength, status);

      if (status.invalid || status.ins == Ins.Instruction.Invalid)
      {
        return Ins.SetInstruction(Ins.SetLength(0, 1), Ins.Instruction.Invalid); // ungültige Codefolge erkannt
      }

      ulong insCodes = Ins.SetLength(0, (uint)length); // Länge der Instruktion setzen

      insCodes = Ins.SetInstruction(insCodes, status.ins); // Instruktion selbst setzen

      return insCodes; // fertig kodierten Instruktions-Wert zurück geben
    }
  }
}
