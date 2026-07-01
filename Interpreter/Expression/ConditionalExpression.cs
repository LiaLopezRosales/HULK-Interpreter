public class ConditionalExpression : Expression
{
    public Expression Condition { get; }
    public Expression IfBranch { get; }
    public Expression? ElseBranch { get; }

    public ConditionalExpression(Expression condition, Expression ifBranch, Expression? elseBranch)
    {
        Condition = condition;
        IfBranch = ifBranch;
        ElseBranch = elseBranch;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        var cond = Condition.Evaluate(scope, context, errors);
        if (cond is bool b)
        {
            if (b)
                return IfBranch.Evaluate(scope, context, errors);
            if (ElseBranch != null)
                return ElseBranch.Evaluate(scope, context, errors);
            return null!;
        }
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "bool return value"));
        return null!;
    }
}
