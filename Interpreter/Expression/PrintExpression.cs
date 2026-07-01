public class PrintExpression : Expression
{
    public Expression Argument { get; }

    public PrintExpression(Expression argument)
    {
        Argument = argument;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var arg = Argument.Evaluate(scope, context, errors);
        Console.WriteLine(arg?.ToString() ?? "");
        return arg!;
    }
}
