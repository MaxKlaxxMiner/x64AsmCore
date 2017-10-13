namespace OpCodeGenerator
{
  /// <summary>
  /// statische Klasse mit Begriffen zu allen Assembler-Befehlen
  /// </summary>
  public static class Asm
  {
    /// <summary>
    /// Namen der 64-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR64 = { "rax", "rcx", "rdx", "rbx", "rsp", "rbp", "rsi", "rdi" };

    /// <summary>
    /// Namen der 8-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR8 = { "al" };

    /// <summary>
    /// Namen der Assembler-Befehle
    /// </summary>
    public static readonly string[] Instructions = { "add" };
  }
}
