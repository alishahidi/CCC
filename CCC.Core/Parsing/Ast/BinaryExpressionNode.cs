namespace Core.Parsing.Ast;

public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode Left { get; set; }
    public string Operator { get; }
    public ExpressionNode Right { get; set; }
    public BinaryExpressionNode(ExpressionNode left, string op, ExpressionNode right)
        => (Left, Operator, Right) = (left, op, right);
}   