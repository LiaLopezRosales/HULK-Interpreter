public class UnaryExpression : Expression
{
    public Expression Operand { get; }

    public UnaryExpression(Expression operand)
    {
        Operand = operand;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var value = Operand.Evaluate(scope, context, errors);
        if (value is bool b)
            return !b;
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "boolean value"));
        return false;
    }
}
