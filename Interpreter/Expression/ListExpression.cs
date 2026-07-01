using System.Globalization;

public class ListExpression : Expression
{
    public List<Expression> Elements { get; }

    public ListExpression(List<Expression> elements)
    {
        Elements = elements;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        List<object> result = new List<object>();
        foreach (var elem in Elements)
            result.Add(elem.Evaluate(scope, context, errors));
        return result;
    }
}
