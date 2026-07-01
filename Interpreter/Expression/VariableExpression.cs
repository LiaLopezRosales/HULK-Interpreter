public class VariableExpression : Expression
{
    public string Name { get; }

    public VariableExpression(string name)
    {
        Name = name;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        while (scope != null)
        {
            if (scope.Variables.ContainsKey(Name))
                return scope.Variables[Name];
            scope = scope.Parent;
        }
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "variable"));
        return null!;
    }
}
