using System.Globalization;

public class EqualMajor : BinaryExpression
{
    public EqualMajor(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);
        if (AreNumbers(l, r, errors))
            return Convert.ToDouble(l, CultureInfo.InvariantCulture) >= Convert.ToDouble(r, CultureInfo.InvariantCulture);
        return false;
    }
}
