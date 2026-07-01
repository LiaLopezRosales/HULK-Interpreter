using System.Globalization;

public class Module : BinaryExpression
{
    public Module(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);
        return AreNumbers(l, r, errors) ? Convert.ToDouble(l, CultureInfo.InvariantCulture) % Convert.ToDouble(r, CultureInfo.InvariantCulture) : 0.0;
    }
}
