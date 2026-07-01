using System.Globalization;

public class Division : BinaryExpression
{
    public Division(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);
        if (!AreNumbers(l, r, errors))
            return 0.0;
        if (Convert.ToDouble(r, CultureInfo.InvariantCulture) == 0)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "operation, can't divide by zero"));
            return l;
        }
        return Convert.ToDouble(l, CultureInfo.InvariantCulture) / Convert.ToDouble(r, CultureInfo.InvariantCulture);
    }
}
