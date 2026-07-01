public class LetExpression : Expression
{
    public List<(string Name, Expression Value)> Assignments { get; }
    public Expression Body { get; }

    public LetExpression(List<(string, Expression)> assignments, Expression body)
    {
        Assignments = assignments;
        Body = body;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var newScope = new Scope { Parent = scope };
        foreach (var (name, valueExpr) in Assignments)
        {
            var value = valueExpr.Evaluate(newScope, context, errors);
            if (newScope.Variables.ContainsKey(name))
                newScope.Variables[name] = value;
            else
                newScope.Variables.Add(name, value);
        }
        return Body.Evaluate(newScope, context, errors);
    }
}
