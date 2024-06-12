using System.Reflection;

namespace ASM.Application;

public static class AssemblyReference
{
    public static readonly Assembly Executing = Assembly.GetExecutingAssembly();
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
