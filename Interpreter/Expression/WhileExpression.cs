public class WhileExpression : Expression
{
    public Expression Condition { get; }
    public Expression Body { get; }

    public WhileExpression(Expression condition, Expression body)
    {
        Condition = condition;
        Body = body;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        object? last = null;
        while (true)
        {
            var cond = Condition.Evaluate(scope, context, errors);
            if (cond is bool b)
            {
                if (!b) break;
                last = Body.Evaluate(scope, context, errors);
            }
            else
            {
                errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "bool return value"));
                break;
            }
        }
        return last!;
    }
}
