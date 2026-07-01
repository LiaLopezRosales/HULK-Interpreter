public class VariableExpression : Expression
{
    public string Name { get; }

    public VariableExpression(string name)
    {
        Name = name;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        Scope? current = scope;
        while (current != null)
        {
            if (current.Variables.ContainsKey(Name))
                return current.Variables[Name];
            current = current.Parent;
        }
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "variable"));
        return null!;
    }
}
