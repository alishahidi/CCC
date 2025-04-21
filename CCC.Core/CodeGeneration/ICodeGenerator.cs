using Core.Parsing.Ast;

namespace Core.CodeGeneration;

public interface ICodeGenerator 
{
    /// <summary>
    /// Generates code from an expression AST node
    /// </summary>
    /// <param name="node">The root node of the expression AST</param>
    /// <returns>Generated code string</returns>
    string Generate(ExpressionNode node);
}