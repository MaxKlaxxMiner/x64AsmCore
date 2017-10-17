namespace OpCodeGenerator.DeAsm
{
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
    Int3,
    /// <summary>
    /// ADD = Addition
    /// </summary>
    Add
  }
}
