namespace Core.Parsing.Ast;

public class ParenthesizedExpressionNode(ExpressionNode expression) : ExpressionNode
{
    public ExpressionNode Expression { get; } = expression;
}