public class BreakException : System.Exception { }

public class BreakExpression : Expression
{
    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        throw new BreakException();
    }
}
