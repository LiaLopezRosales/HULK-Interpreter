public class AssignmentExpression : Expression
{
    public string VariableName { get; }
    public Expression Value { get; }

    public AssignmentExpression(string variableName, Expression value)
    {
        VariableName = variableName;
        Value = value;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var val = Value.Evaluate(scope, context, errors);
        var current = scope;
        while (current != null)
        {
            if (current.Variables.ContainsKey(VariableName))
            {
                current.Variables[VariableName] = val;
                return val;
            }
            current = current.Parent;
        }
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, $"variable '{VariableName}' not declared"));
        return null!;
    }
}
