public class Ternary:Expression
{
    public Expression? Condition{get;set;}
    public Expression? If_True{get;set;}
    public Expression? If_False{get;set;}

    public Ternary()
    {}

    public override bool ValidSemantic(Context context, Scope scope, List<Error> errors)
    {
        bool condition = Condition!.ValidSemantic(context,scope,errors);
        bool if_true = If_True!.ValidSemantic(context,scope,errors);
        bool if_false=If_False!.ValidSemantic(context,scope,errors);
        if (Condition!.Type!=ExpressionType.Bool)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"return of boolean value"));
        }
        return condition && if_true && if_false;
    }
     
     public override ExpressionType Type{get=>Type=ExpressionType.Conditional;set=>Type=ExpressionType.Conditional;}
    public override object? Value{get;set;}
    public override void Evaluate()
    {
        bool condition;
        Condition!.Evaluate();
        condition=(bool)Condition.Value!;
        if(condition)
        {
            If_True!.Evaluate();
            Value=If_True.Value!;
        }
        else
        {
            If_False!.Evaluate();
            Value=If_False.Value!;
        }
    }
}