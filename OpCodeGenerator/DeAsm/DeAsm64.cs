using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public static AsmResult DeCodeAsm(byte[] code, uint codeOffset)
    {
      if (code == null || code.Length == 0) throw new ArgumentNullException("code");
      if (codeOffset >= code.Length) throw new ArgumentOutOfRangeException("codeOffset");
      if (code.Length - codeOffset < 16) Array.Resize(ref code, code.Length + 16);

      fixed (byte* codeP = &code[codeOffset])
      {
        ulong opcodeLo = *(ulong*)codeP;
        ulong opcodeHi = *(ulong*)(codeP + sizeof(ulong));
        ulong insCode = DeCodeAsm(codeP);
        return new AsmResult(opcodeLo, opcodeHi, insCode);
      }
    }

    /// <summary>
    /// dekodiert intern einen Assembler-Befehl
    /// </summary>
    /// <param name="code">Zeiger auf den Opcode, welcher dekodiert werden soll</param>
    /// <returns>fertig dekodierte Instruktion</returns>
    static ulong DeCodeAsm(byte* code)
    {
      ulong insCode = 0;

      throw new NotImplementedException("todo");

      return insCode;
    }
  }
}
