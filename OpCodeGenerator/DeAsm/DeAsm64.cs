// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Local
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
    /// Struktur, um sich den aktuellen Dekodierungstatus zu merken
    /// </summary>
    struct DecodeStatus
    {
      /// <summary>
      /// merkt sich die aktuell erkannte Instruktion
      /// </summary>
      public Instruction ins;
      /// <summary>
      /// Typ des ersten Parameters (Ziel-Parameter)
      /// </summary>
      public ParamType p1;
      /// <summary>
      /// Typ des zweiten Parameters (Quell-Parameters)
      /// </summary>
      public ParamType p2;
      /// <summary>
      /// Daten zum ersten Parameter (Ziel-Parameter)
      /// </summary>
      public uint d1;
      /// <summary>
      /// Daten zum zweiten Parameter (Quell-Parameter)
      /// </summary>
      public uint d2;

      /// <summary>
      /// setzt die Instruktion
      /// </summary>
      /// <param name="instruction">Instruktion, welche gesetzt werden soll</param>
      public void S(Instruction instruction)
      {
        ins = instruction;
      }

      /// <summary>
      /// setzt die Instruktion und den ersten Standard-Parameter
      /// </summary>
      /// <param name="instruction">Instruktion, welche gesetzt werden soll</param>
      /// <param name="paramType">Parameter-Typ, welcher als erster Parameter gesetzt werden</param>
      public void S1(Instruction instruction, ParamType paramType)
      {
        ins = instruction;
        p1 = paramType;
      }

      /// <summary>
      /// setzt die Instruktion und beide Standard-Parameter
      /// </summary>
      /// <param name="instruction">Instruktion, welche gesetzt werden soll</param>
      /// <param name="paramType">Parameter-Typ, welcher bei beiden Parametern gesetzt werden soll</param>
      public void S2(Instruction instruction, ParamType paramType)
      {
        ins = instruction;
        p1 = paramType;
        p2 = paramType;
      }

      /// <summary>
      /// gibt den Inhalt als lesbare Zeichenfolge zurück
      /// </summary>
      /// <returns>lesbare Zeichenfolge</returns>
      public override string ToString()
      {
        return (new { ins, p1, p2 }).ToString();
      }
    }

    /// <summary>
    /// dekodiert eine allgemeine Instruktion
    /// </summary>
    /// <param name="c">Zeiger auf die aktuell zu lesende Bytefolge</param>
    /// <param name="cerr">Zeiger auf letztes Byte, welches nicht mehr gelesen werden darf</param>
    /// <param name="status">aktueller Status der Dekodierung</param>
    /// <returns>Zeiger auf das nachfolgende Byte</returns>
    static byte* DecodeInternal(byte* c, byte* cerr, ref DecodeStatus status)
    {
      byte code = *c++;
      if (c >= cerr) return cerr;
      switch (code)
      {
        case 0x00: status.S2(Instruction.Add, ParamType.Reg8); return DecodeDualParam(c, cerr, ref status);
        case 0x06: return cerr;
        case 0xcc: status.S(Instruction.Int3); return c;
        default: throw new NotImplementedException("todo: " + code.ToString("x2"));
      }
    }

    /// <summary>
    /// dekodiert zwei Parameter (Parameter-Größe muss bereits in <see cref="DecodeStatus.p1"/> gesetzt sein)
    /// </summary>
    /// <param name="c">Zeiger auf die aktuell zu lesende Bytefolge</param>
    /// <param name="cerr">Zeiger auf letztes Byte, welches nicht mehr gelesen werden darf</param>
    /// <param name="status">aktueller Status der Dekodierung</param>
    /// <param name="swap">gibt an, ob beide Parameter getauscht gelesen werden müssen</param>
    /// <returns>Zeiger auf das nachfolgende Byte</returns>
    static byte* DecodeDualParam(byte* c, byte* cerr, ref DecodeStatus status, bool swap = false)
    {
      Debug.Assert(status.p1 != ParamType.None);
      Debug.Assert(status.p2 != ParamType.None);
      byte code = *c++;
      if (c >= cerr) return cerr;
      if (code < 0xc0) throw new NotImplementedException("todo: " + code.ToString("x2"));
      status.d1 = (uint)code & 7;
      status.d2 = (uint)code >> 3 & 7;
      return c;
    }

    /// <summary>
    /// dekodiert intern einen Assembler-Befehl
    /// </summary>
    /// <param name="code">Zeiger auf den Opcode, welcher dekodiert werden soll</param>
    /// <returns>fertig dekodierte Instruktion</returns>
    public static ulong Decode(byte* code)
    {
      var status = new DecodeStatus();
      var cerr = code + Ins.MaxLength + 1;
      var newPos = DecodeInternal(code, cerr, ref status);

      if (status.ins == Instruction.Invalid || newPos >= cerr) // ungültige Codefolge erkannt?
      {
        return Ins.SetInstruction(Ins.SetLength(0, 1), Instruction.Invalid);
      }

      ulong insCodes = Ins.SetLength(0, (uint)(newPos - code)); // Länge der Instruktion setzen

      insCodes = Ins.SetInstruction(insCodes, status.ins); // Instruktion selbst setzen
      insCodes = Ins.SetParamTypes(insCodes, status.p1, status.p2); // beide Parameter-Typen setzen (sofern vorhanden)
      insCodes = Ins.SetParamData(insCodes, status.d1, status.d2); // die Daten der beiden Parameter-setzen (sofern vorhanden)

      return insCodes; // fertig kodierten Instruktions-Wert zurück geben
    }
  }
}
