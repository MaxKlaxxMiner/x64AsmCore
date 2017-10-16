using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// Klasse zum lesen und bearbeiten von kodierten Instruktionen
  /// </summary>
  public static partial class Ins
  {
    /// <summary>
    /// 1 Bit (0 - 1) - eigentliche Instruktion
    /// </summary>
    const int InsInstruction = 1;

    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsInstructionOfs = InsLengthOfs + InsLength;

    /// <summary>
    /// Enum aller möglichen Instruktionen
    /// </summary>
    public enum Instruction
    {
      /// <summary>
      /// ??? - Invalid Instruction
      /// </summary>
      Invalid = 0,
      /// <summary>
      /// INT3 = Call Interrupt 3 (Debug Break Point)
      /// </summary>
      Int3
    }

    /// <summary>
    /// generiert alle Namen der Instruktionen
    /// </summary>
    /// <returns>fertiges Array mit allen Instruktionen</returns>
    static string[] GenInstructionNames()
    {
      var dict = new Dictionary<Instruction, string>
      {
        { Instruction.Invalid, "???" },
        { Instruction.Int3, "INT3" },
      };

      int max = dict.Max(d => (int)d.Key);
      var result = new string[max + 1];
      for (int i = 0; i < result.Length; i++) dict.TryGetValue((Instruction)i, out result[i]);
      return result;
    }

    /// <summary>
    /// merkt sich die Namen aller Instruktionen
    /// </summary>
    static readonly string[] InstructionNames = GenInstructionNames();

    /// <summary>
    /// gibt die entsprechende Instruktion zurück
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>dekodierte Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Instruction GetInstruction(ulong insCodes)
    {
      return (Instruction)((insCodes >> InsInstructionOfs) & (1ul << InsInstruction) - 1);
    }

    /// <summary>
    /// gibt den Namen einer Instruktion zurück (oder "null", wenn nicht bekannt)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Name der Instruktion oder "null", wenn nicht bekannt</returns>
    public static string GetInstructionName(ulong insCodes)
    {
      return GetInstructionName(GetInstruction(insCodes));
    }

    /// <summary>
    /// gibt den Namen einer Instruktion zurück (oder "null", wenn nicht bekannt)
    /// </summary>
    /// <param name="instruction">Instruktion deren Name zurück gegeben werden soll</param>
    /// <returns>Name der Instruktion oder "null", wenn nicht bekannt</returns>
    public static string GetInstructionName(Instruction instruction)
    {
      int index = (int)instruction;
      if ((uint)index >= InstructionNames.Length) return null;
      return InstructionNames[index];
    }

    /// <summary>
    /// setzt die gewünschte Instruktion
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="instruction">Instruktion, welche gesetzt werden soll</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetInstruction(ulong insCodes, Instruction instruction)
    {
      Debug.Assert(instruction.ToString() != ((int)instruction).ToString());
      return insCodes & ~((1ul << InsInstruction) - 1 << InsInstructionOfs) | (ulong)instruction << InsInstructionOfs;
    }
  }
}
