public class FunctionCall : Expression
{
    public string Name { get; }
    public List<Expression> Arguments { get; }

    public FunctionCall(string name, List<Expression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        Function? func = null;
        foreach (var f in context.Available_Functions)
        {
            if (f.Name == Name)
            {
                func = f;
                break;
            }
        }

        if (func == null)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "name, function has not been declared"));
            return null!;
        }

        if (func.Parameters.Count != Arguments.Count)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, $"{func.Parameters.Count} parameters but received {Arguments.Count}"));
            return null!;
        }

        var newScope = new Scope { Parent = scope };
        for (int i = 0; i < func.Parameters.Count; i++)
        {
            var argValue = Arguments[i].Evaluate(scope, context, errors);
            newScope.Variables[func.Parameters[i]] = argValue;
        }

        return func.Code.Evaluate(newScope, context, errors);
    }
}
