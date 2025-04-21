namespace Core.Parsing.Ast;

public class BinaryExpressionNode(ExpressionNode left, string op, ExpressionNode right) : ExpressionNode
{
    public ExpressionNode Left { get; set; } = left;
    public string Operator { get; } = op;
    public ExpressionNode Right { get; set; } = right;
}   