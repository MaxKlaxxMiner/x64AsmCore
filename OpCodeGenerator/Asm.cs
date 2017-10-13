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
    public static readonly string[] RegistersR64 = 
    {
      "rax", "rcx", "rdx", "rbx", "rsp", "rbp", "rsi", "rdi",
      "r8",  "r9",  "r10", "r11", "r12", "r13", "r14", "r15",
      "",    "",    "",    "",    "",    "rip", "",    ""
    };

    /// <summary>
    /// Namen der 8-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR8 = { "al", "cl", "dl", "bl" };

    /// <summary>
    /// Namen der Assembler-Befehle
    /// </summary>
    public static readonly string[] Instructions = { "add" };
  }
}
