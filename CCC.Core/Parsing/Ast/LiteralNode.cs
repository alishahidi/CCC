using Core.Tokenization;

namespace Core.Parsing.Ast;

public class LiteralNode(TokenType type, string value) : ExpressionNode
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;
}