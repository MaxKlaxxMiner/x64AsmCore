using System;
using System.Linq;

namespace OpCodeGenerator
{
  public static partial class Generator
  {
    /// <summary>
    /// gibt eine bestimmte Anzahl von Bytes in hexadezimale Schreibweise zurück und endet mit einem Bindestrich (z.B. "00 04 f4 - ")
    /// </summary>
    /// <param name="bytes">Bytes, welche ausgelesen werden sollen</param>
    /// <param name="lastByte">zeig auf das letzte Byte in der Kette, welche ausgelesen werden sollen</param>
    /// <param name="constBytes">optionale zusätzliche Bytes für eine direkte Konstante</param>
    /// <returns>fertige Zeichenkette</returns>
    public static string StrB(byte[] bytes, int lastByte, int constBytes = 0)
    {
      return string.Join(" ", bytes.Take(lastByte + 1).Select(b => b.ToString("x2")).Concat(Enumerable.Range(0, constBytes).Select(i => "xx"))) + " - ";
    }

    /// <summary>
    /// gibt eine 64-Bit Adressierung zurück, mit Leerzeichen am Anfang (z.B. " [rsi]")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "rdx")</param>
    /// <returns>fertige Adressierung</returns>
    /// <param name="ext">optionale Zusatzinfos</param>
    static string R64Addr(int index, AddrExt ext = AddrExt.None)
    {
      int constBytes = 0;
      if (ext.HasFlag(AddrExt.C1)) { constBytes = 1; }
      if (ext.HasFlag(AddrExt.C4)) { constBytes = 4; }

      string t1 = Asm.RegistersR64[index];

      string t2 = constBytes > 0 ? "x" : "";

      string str = t1;
      if (str != "" && t2 != "") str += " + ";
      str += t2;
      if (str == "") str = "0";

      return " [" + str + "]";
    }

    /// <summary>
    /// Flags für Zusatzinformationen
    /// </summary>
    [Flags]
    enum AddrExt
    {
      /// <summary>
      /// keine Sonderinfos vorhanden
      /// </summary>
      None = 0,
      /// <summary>
      /// fügt eine Int8-Konstante hinzu (-128 bis 127)
      /// </summary>
      C1 = 1,
      /// <summary>
      /// fügt eine Int32-Konstante hinzu (-2147483648 bis 2147483647)
      /// </summary>
      C4 = 2,
      /// <summary>
      /// ersetzt den Register "RBP" an erster Stelle durch eine Int32-Konstante aus (-2147483648 bis 2147483647)
      /// </summary>
      Rbp1ToC4 = 4,
      /// <summary>
      /// entfernt den Register "RSP" an zweiter Stelle 
      /// </summary>
      Rsp2Skip = 8
    }

    /// <summary>
    /// gibt eine mehrteilige 64-Bit Adressierung zurück, mit Leerzeichen am Anfang (z.B. "[rsi + rcx * 4]")
    /// </summary>
    /// <param name="index1">Index auf den ersten Register (z.B. 6: "rsi")</param>
    /// <param name="index2">Index auf den zweiten Register (z.B. 1: "rcx")</param>
    /// <param name="mulShift2">Shiftwert für die Multiplikation des zweiten Registers (z.B. 2: "rcx * 4")</param>
    /// <param name="ext">optionale Zusatzinfos</param>
    /// <returns>fertige Adressierung</returns>
    static string R64Addr(int index1, int index2, int mulShift2, AddrExt ext = AddrExt.None)
    {
      int constBytes = 0;
      bool skip1 = false;
      bool skip2 = false;

      if (ext.HasFlag(AddrExt.C1)) { constBytes = 1; }
      if (ext.HasFlag(AddrExt.C4)) { constBytes = 4; }
      if (ext.HasFlag(AddrExt.Rbp1ToC4) && index1 == 5) { skip1 = true; constBytes = 4; }
      if (ext.HasFlag(AddrExt.Rsp2Skip) && index2 == 4) { skip2 = true; }

      string t1 = skip1 ? "" : Asm.RegistersR64[index1];

      string t2 = skip2 ? "" : Asm.RegistersR64[index2];
      if (mulShift2 != 0 && t2 != "") t2 += " * " + (1 << mulShift2);

      string t3 = constBytes > 0 ? "x" : "";

      string str = t1;
      if (str != "" && t2 != "") str += " + ";
      str += t2;
      if (str != "" && t3 != "") str += " + ";
      str += t3;
      if (str == "") str = "0";

      return " [" + str + "]";
    }

    /// <summary>
    /// gibt einen 8-Bit Register zurück mit Leerzeichen am Anfang (z.B. " al")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "dl")</param>
    /// <returns>fertiger Register</returns>
    static string R8(int index)
    {
      return " " + Asm.RegistersR8[index];
    }

    /// <summary>
    /// gibt einen 16-Bit Register zurück mit Leerzeichen am Anfang (z.B. " ax")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "dx")</param>
    /// <returns>fertiger Register</returns>
    static string R16(int index)
    {
      return " " + Asm.RegistersR16[index];
    }

    /// <summary>
    /// gibt einen 32-Bit Register zurück mit Leerzeichen am Anfang (z.B. " eax")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "edx")</param>
    /// <returns>fertiger Register</returns>
    static string R32(int index)
    {
      return " " + Asm.RegistersR32[index];
    }
  }
}
