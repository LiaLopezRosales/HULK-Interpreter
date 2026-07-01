public class FunctionDefinition : Expression
{
    public string Name { get; }
    public List<string> Parameters { get; }
    public Expression Body { get; }

    public FunctionDefinition(string name, List<string> parameters, Expression body)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        foreach (var f in context.Available_Functions)
            if (f.Name == Name)
            {
                errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "function name, already exists"));
                return null!;
            }

        context.Available_Functions.Add(new Function(Name, Body, Parameters));
        return $"Function '{Name}' added";
    }
}
