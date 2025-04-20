using Core.Parsing.Ast;
using Core.Tokenization;

namespace Core.CodeGeneration;

public class CSharpCodeGenerator
{
    public string Generate(ExpressionNode node)
    {
        return Visit(node);
    }

    private string Visit(ExpressionNode node)
    {
        return node switch
        {
            BinaryExpressionNode binary => VisitBinary(binary),
            LiteralNode literal => VisitLiteral(literal),
            _ => throw new NotSupportedException($"Node type {node.GetType().Name} is not supported")
        };
    }

    private string VisitBinary(BinaryExpressionNode node)
    {
        if (node.Left == null || node.Right == null)
        {
            throw new InvalidOperationException("Invalid binary expression: missing operands");
        }

        return $"{Visit(node.Left)} {node.Operator} {Visit(node.Right)}";
    }

    private string VisitLiteral(LiteralNode node)
    {
        return node.Type switch
        {
            TokenType.String => $"\"{EscapeString(node.Value)}\"",
            TokenType.Number => node.Value,
            TokenType.Identifier => node.Value,
            TokenType.Boolean => node.Value.ToLower(),
            TokenType.Null => "null",
            _ => throw new NotSupportedException($"Literal type {node.Type} is not supported")
        };
    }

    private string EscapeString(string value)
    {
        return value.Replace("\"", "\\\"")
            .Replace("\\", "\\\\")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}