using Core.Tokenization;

namespace Core.Parsing.Ast;

public class LiteralNode : ExpressionNode
{
    public TokenType Type { get; }
    public string Value { get; }
    
    public LiteralNode(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}