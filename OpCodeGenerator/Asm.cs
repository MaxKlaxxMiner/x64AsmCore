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
      "rax", "rcx", "rdx", "rbx",
      "rsp", "rbp", "rsi", "rdi",
      "r8",  "r9",  "r10", "r11",
      "r12", "r13", "r14", "r15",
      "",    "",    "",    "",
      "",    "rip", "",    ""
    };

    /// <summary>
    /// Namen der 32-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR32 =
    {
      "eax", "ecx", "edx", "ebx",
      "esp", "ebp", "esi", "edi",
      "",    "",    "",    "",
      "",    "eip", "",    ""
    };

    /// <summary>
    /// Namen der 16-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR16 =
    {
      "ax", "cx", "dx", "bx",
      "sp", "bp", "si", "di",
      "",   "",   "",   "",
      "",   "ip", "",   ""
    };

    /// <summary>
    /// Namen der 8-Bit Basis-Register
    /// </summary>
    public static readonly string[] RegistersR8 =
    {
      "al", "cl", "dl", "bl",
      "ah", "ch", "dh", "bh"
    };

    /// <summary>
    /// Namen der Assembler-Befehle
    /// </summary>
    public static readonly string[] Instructions = { "add", "or" };

    /// <summary>
    /// Namen der Assembler-Befehle, welche mit 0x0f beginnen
    /// </summary>
    public static readonly string[] InstructionsF = 
    {
      "sldt", "str",
      "lldt", "ltr",
      "verr", "verw"
    };

    /// <summary>
    /// Bezeichnung der Größe des Elementes, welches im Speicher verarbeitet werden soll
    /// </summary>
    public static readonly string[] MemType = { " byte ptr", " word ptr", " dword ptr", " qword ptr" };
  }
}
