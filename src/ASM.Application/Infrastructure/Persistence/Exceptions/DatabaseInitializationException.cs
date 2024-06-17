namespace ASM.Application.Infrastructure.Persistence.Exceptions;

public sealed class DatabaseInitializationException(string message, Exception exception)
    : Exception(message, exception);
