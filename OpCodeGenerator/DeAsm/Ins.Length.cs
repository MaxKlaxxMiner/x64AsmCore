using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// Klasse zum lesen und bearbeiten von kodierten Instruktionen
  /// </summary>
  public static partial class Ins
  {
    /// <summary>
    /// gibt die minimale Länge einer Instruktion an
    /// </summary>
    public const ulong MinLength = 1;
    /// <summary>
    /// gibt die maximale Länge einer Instruktion an
    /// </summary>
    public const ulong MaxLength = 15;

    /// <summary>
    /// 4 Bits (1 - 15) - Länge der gesamten Instruktion in Bytes
    /// </summary>
    const int InsLength = 4;

    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsLengthOfs = 0;

    /// <summary>
    /// gibt die Länge der Instruktion in Bytes zurück (1 - 15)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Länge der Instruktion in Bytes (1 - 15)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong GetLength(ulong insCodes)
    {
      return (insCodes >> InsLengthOfs) & (1ul << InsLength) - 1;
    }

    /// <summary>
    /// setzt die Länge der Instruktion in Bytes (1 - 15)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="length">neue Länge der Instruktion in Bytes</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetLength(ulong insCodes, ulong length)
    {
      Debug.Assert(length >= MinLength && length <= MaxLength);
      return insCodes & ~((1ul << InsLength) - 1 << InsLengthOfs) | length << InsLengthOfs;
    }
  }
}
