using Core.CodeGeneration;
using Core.Parsing;
using Core.Tokenization;

namespace Core;

public class ExpressionCompiler
{
    public string CompileToCSharp(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException("Expression cannot be null or empty", nameof(expression));
        }

        try
        {
            // Step 1: Tokenization
            var tokenizer = new StringTokenizer(expression);
            var tokens = tokenizer.Tokenize();

            // Step 2: Parsing
            var parser = new Parser(tokens);
            var ast = parser.Parse();

            // Step 3: Code Generation
            var codeGenerator = new CSharpCodeGenerator();
            return codeGenerator.Generate(ast);
        }
        catch (Exception ex)
        {
            throw new ExpressionCompilationException("Failed to compile expression", ex);
        }
    }
}