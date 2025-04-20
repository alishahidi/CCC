namespace Core;

public class ExpressionCompilationException : Exception
{
    public ExpressionCompilationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}