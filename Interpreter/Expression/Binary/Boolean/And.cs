public class And : BinaryExpression
{
    public And(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);
        return AreBooleans(l, r, errors) ? (bool)l && (bool)r : false;
    }
}
