namespace Core;

public class ExpressionCompilationException(string message, Exception innerException)
    : Exception(message, innerException);