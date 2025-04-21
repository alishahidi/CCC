using Core.Parsing.Ast;
using Core.Tokenization;

namespace Core.CodeGeneration;

public class CSharpCodeGenerator : ICodeGenerator
{
    public string Generate(ExpressionNode node)
    {
        return Visit(node);
    }

    private string Visit(ExpressionNode node, int parentPrecedence = 0)
    {
        return node switch
        {
            BinaryExpressionNode binary => VisitBinary(binary, parentPrecedence),
            UnaryExpressionNode unary => VisitUnary(unary, parentPrecedence),
            LiteralNode literal => VisitLiteral(literal),
            ParenthesizedExpressionNode paren => $"({Visit(paren.Expression)})",
            _ => throw new NotSupportedException($"Node type {node.GetType().Name} is not supported")
        };
    }

    private string VisitUnary(UnaryExpressionNode node, int parentPrecedence)
    {
        var operand = Visit(node.Operand, GetPrecedence(node.Operator));
        return $"{node.Operator}{operand}";
    }

    private string VisitBinary(BinaryExpressionNode node, int parentPrecedence)
    {
        var currentPrecedence = GetPrecedence(node.Operator);
        var left = Visit(node.Left, currentPrecedence);
        var right = Visit(node.Right, currentPrecedence);
        var expression = $"{left} {node.Operator} {right}";

        return currentPrecedence < parentPrecedence ? $"({expression})" : expression;
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

    private int GetPrecedence(string op)
    {
        return op switch
        {
            "!" => 1,
            "||" => 2,
            "&&" => 3,
            "==" or "!=" => 4,
            "<" or ">" or "<=" or ">=" => 5,
            "+" or "-" => 6,  // Add these lines
            "*" or "/" => 7,
            _ => throw new InvalidOperationException($"Unknown operator: {op}")
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