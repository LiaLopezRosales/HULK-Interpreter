public abstract class Expression
{
    public abstract object Evaluate(Scope scope, Context context, List<Error> errors);
}
