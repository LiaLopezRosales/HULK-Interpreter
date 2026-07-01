public class ContinueException : System.Exception { }

public class ContinueExpression : Expression
{
    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        throw new ContinueException();
    }
}
