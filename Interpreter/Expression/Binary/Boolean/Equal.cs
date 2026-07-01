public class Equal : BinaryExpression
{
    public Equal(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);
        if (l.GetType() == r.GetType())
            return l.Equals(r);
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "equal type of values"));
        return false;
    }
}
