using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
// ReSharper disable UnusedMember.Global

namespace OpCodeGenerator.DeAsm
{
  /// <summary>
  /// Klasse zum lesen und bearbeiten von kodierten Instruktionen
  /// </summary>
  public static partial class Ins
  {
    /// <summary>
    /// 1 Bit (0 - 1) - Parameter-Typ des Ziel-Parameters
    /// </summary>
    const int InsParam1Type = 1;
    /// <summary>
    /// 3 Bits (0 - 7) - Parameter-Daten des Ziel-Parameters
    /// </summary>
    const int InsParam1Data = 3;
    /// <summary>
    /// 1 Bit (0 - 1) - Parameter-Typ des Quell-Parameters
    /// </summary>
    const int InsParam2Type = 1;
    /// <summary>
    /// 3 Bits (0 - 7) - Parameter-Daten des Quell-Parameters
    /// </summary>
    const int InsParam2Data = 3;


    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsParam1TypeOfs = InsInstructionOfs + InsInstruction;
    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsParam1DataOfs = InsParam1TypeOfs + InsParam1Type;
    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsParam2TypeOfs = InsParam1DataOfs + InsParam1Data;
    /// <summary>
    /// berechnetes Bit-Offset
    /// </summary>
    const int InsParam2DataOfs = InsParam2TypeOfs + InsParam2Type;

    /// <summary>
    /// generiert alle Namen der Parameter-Größen
    /// </summary>
    /// <returns>fertiges Array mit allen Parameter-Größen</returns>
    static string[] GenParamSizeNames()
    {
      var dict = new Dictionary<ParamType, string>
      {
        { ParamType.None, "" },
        { ParamType.Reg8, "BYTE PTR" }
      };

      int max = dict.Max(d => (int)d.Key);
      var result = new string[max + 1];
      for (int i = 0; i < result.Length; i++) dict.TryGetValue((ParamType)i, out result[i]);
      return result;
    }

    /// <summary>
    /// merkt sich die Namen aller Instruktionen
    /// </summary>
    static readonly string[] ParamSizeNames = GenParamSizeNames();

    /// <summary>
    /// gibt den Typ des ersten gesetzten Parameters zurück (Ziel-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Parameter-Typ</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ParamType GetParam1Type(ulong insCodes)
    {
      return (ParamType)((insCodes >> InsParam1TypeOfs) & (1ul << InsParam1Type) - 1);
    }

    /// <summary>
    /// gibt die zugehörigen Daten des ersten Parameters zurück (Ziel-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Parameter-Daten</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetParam1Data(ulong insCodes)
    {
      return (uint)((insCodes >> InsParam1DataOfs) & (1ul << InsParam1Data) - 1);
    }

    /// <summary>
    /// gibt den Typ des zweiten gesetzten Parameters zurück (Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Parameter-Typ</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ParamType GetParam2Type(ulong insCodes)
    {
      return (ParamType)((insCodes >> InsParam2TypeOfs) & (1ul << InsParam2Type) - 1);
    }

    /// <summary>
    /// gibt die zugehörigen Daten des zweiten Parameters zurück (Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <returns>Parameter-Daten</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetParam2Data(ulong insCodes)
    {
      return (uint)((insCodes >> InsParam2DataOfs) & (1ul << InsParam2Data) - 1);
    }

    /// <summary>
    /// gibt den Namen einer Parametergröße zurück (z.B.: "BYTE PTR" oder null, wenn nicht bekannt)
    /// </summary>
    /// <param name="paramType">Parametergröße</param>
    /// <returns>Name der Parametergröße</returns>
    public static string GetName(ParamType paramType)
    {
      int index = (int)paramType;
      if ((uint)index >= ParamSizeNames.Length) return null;
      return ParamSizeNames[index];
    }

    /// <summary>
    /// setzt den Typ des ersten Parameters (Ziel-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="paramType">Parameter-Typ, welcher gesetzt werden soll</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParam1Type(ulong insCodes, ParamType paramType)
    {
      Debug.Assert(paramType.ToString() != ((int)paramType).ToString());
      return insCodes & ~((1ul << InsParam1Type) - 1 << InsParam1TypeOfs) | (ulong)paramType << InsParam1TypeOfs;
    }

    /// <summary>
    /// setzt die Daten des ersten Parameters (Ziel-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="paramData">Parameter-Daten, welche gesetzt werden sollen</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParam1Data(ulong insCodes, uint paramData)
    {
      Debug.Assert(paramData <= InsParam1Data);
      return insCodes & ~((1ul << InsParam1Data) - 1 << InsParam1DataOfs) | (ulong)paramData << InsParam1DataOfs;
    }

    /// <summary>
    /// setzt den Typ des ersten Parameters (Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="paramType">Parameter-Typ, welcher gesetzt werden soll</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParam2Type(ulong insCodes, ParamType paramType)
    {
      Debug.Assert(paramType.ToString() != ((int)paramType).ToString());
      return insCodes & ~((1ul << InsParam2Type) - 1 << InsParam2TypeOfs) | (ulong)paramType << InsParam2TypeOfs;
    }

    /// <summary>
    /// setzt die Daten des zweiten Parameters (Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="paramData">Parameter-Daten, welche gesetzt werden sollen</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParam2Data(ulong insCodes, uint paramData)
    {
      Debug.Assert(paramData <= InsParam2Data);
      return insCodes & ~((1ul << InsParam2Data) - 1 << InsParam2DataOfs) | (ulong)paramData << InsParam2DataOfs;
    }

    /// <summary>
    /// setzt die Typen von beiden Parametern gleichzeitig (Ziel- und Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="param1Type">Parameter-Typ des ersten Parameters (Ziel-Parameter)</param>
    /// <param name="param2Type">Parameter-Typ des zweiten Parameters (Quell-Parameters)</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParamTypes(ulong insCodes, ParamType param1Type, ParamType param2Type)
    {
      Debug.Assert(param1Type.ToString() != ((int)param1Type).ToString());
      Debug.Assert(param2Type.ToString() != ((int)param2Type).ToString());
      Debug.Assert(param1Type != ParamType.None || param2Type == ParamType.None);
      return insCodes & (~((1ul << InsParam1Type) - 1 << InsParam1TypeOfs) & ~((1ul << InsParam2Type) - 1 << InsParam2TypeOfs))
                      | (ulong)param1Type << InsParam1TypeOfs | (ulong)param2Type << InsParam2TypeOfs;
    }

    /// <summary>
    /// setzt die Daten von beiden Parametern gleichzeitig (Ziel- und Quell-Parameter)
    /// </summary>
    /// <param name="insCodes">Kodierte Instruktion</param>
    /// <param name="param1Data">Parameter-Daten des ersten Parameters (Ziel-Parameter)</param>
    /// <param name="param2Data">Parameter-Daten des zweiten Parameters (Quell-Parameters)</param>
    /// <returns>fertige Instruktion</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SetParamData(ulong insCodes, uint param1Data, uint param2Data)
    {
      Debug.Assert(param1Data < 1u << InsParam1Data);
      Debug.Assert(param2Data < 1u << InsParam2Data);
      return insCodes & (~((1ul << InsParam1Data) - 1 << InsParam1DataOfs) & ~((1ul << InsParam2Data) - 1 << InsParam2DataOfs))
                      | (ulong)param1Data << InsParam1DataOfs | (ulong)param2Data << InsParam2DataOfs;
    }
  }
}
