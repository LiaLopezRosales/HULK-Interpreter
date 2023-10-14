public class Ternary:Expression
{
    public Node? Condition{get;set;}
    public Node? If_True{get;set;}
    public Node? If_False{get;set;}

    public Ternary()
    {}

    // public override bool ValidSemantic(Context context, Scope scope, List<Error> errors)
    // {
    //     bool condition = Condition!.ValidSemantic(context,scope,errors);
    //     bool if_true = If_True!.ValidSemantic(context,scope,errors);
    //     bool if_false=If_False!.ValidSemantic(context,scope,errors);
    //     if (Condition!.Type!=ExpressionType.Bool)
    //     {
    //         errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"return of boolean value"));
    //     }
    //     return condition && if_true && if_false;
    // }
     
     public override ExpressionType Type{get=>Type=ExpressionType.Conditional;set=>Type=ExpressionType.Conditional;}
    public override object? Value{get;set;}
    public override void Evaluate(object condition,object If,object Else)
    {
        if((bool)condition)
        {
            Value=If;
        }
        else
        {
            Value=Else;
        }
    }
    public override void Evaluate(object left, object right)
    {
        throw new NotImplementedException();
  
}
}