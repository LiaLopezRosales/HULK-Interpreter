public class LiteralExpression : Expression
{
    public object Value { get; }

    public LiteralExpression(object value)
    {
        Value = value;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        return Value;
    }
}
