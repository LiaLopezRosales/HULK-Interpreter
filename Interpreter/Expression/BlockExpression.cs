public class BlockExpression : Expression
{
    public List<Expression> Expressions { get; }

    public BlockExpression(List<Expression> expressions)
    {
        Expressions = expressions;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        object? last = null;
        foreach (var expr in Expressions)
            last = expr.Evaluate(scope, context, errors);
        return last!;
    }
}
