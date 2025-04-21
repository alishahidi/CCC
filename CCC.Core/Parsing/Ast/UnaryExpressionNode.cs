namespace Core.Parsing.Ast;

public class UnaryExpressionNode(string op, ExpressionNode operand) : ExpressionNode
{
    public string Operator { get; } = op;
    public ExpressionNode Operand { get; } = operand;
}