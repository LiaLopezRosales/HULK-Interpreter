using System.Globalization;

public class Concatenation : BinaryExpression
{
    public Concatenation(Expression left, Expression right) : base(left, right) { }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var l = Left.Evaluate(scope, context, errors);
        var r = Right.Evaluate(scope, context, errors);

        string ls = Convert.ToString(l, CultureInfo.InvariantCulture) ?? "";
        string rs = Convert.ToString(r, CultureInfo.InvariantCulture) ?? "";

        // Strip surrounding quotes from string literal operands
        if (ls.Length >= 2 && ls[0] == '"' && ls[^1] == '"')
            ls = ls[1..^1];
        if (rs.Length >= 2 && rs[0] == '"' && rs[^1] == '"')
            rs = rs[1..^1];

        return $"\"{ls}{rs}\"";
    }
}
