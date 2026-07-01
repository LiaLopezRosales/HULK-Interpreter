public class And : BinaryExpression
{
    public And(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        if (l is not bool)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "boolean values"));
            return false;
        }
        if (!(bool)l) return false;
        var r = Right.Evaluate(scope, context, errors);
        if (r is not bool)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "boolean values"));
            return false;
        }
        return (bool)r;
    }
}
