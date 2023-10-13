public class Let_In:Expression
{
    public string name{get;set;}
    public Expression? Assigment{get;set;}
    public Expression? Inside{get;set;}

    public Let_In(){}

    public override bool ValidSemantic(Context context, Scope scope, List<Error> errors)
    {
        bool assig = Assigment!.ValidSemantic(context,scope,errors);
        bool In = Inside!.ValidSemantic(context,scope,errors);
        return assig && In;
    }

    public override ExpressionType Type{get=>Type=ExpressionType.Let_In;set=>Type=ExpressionType.Let_In;}
    public override object? Value{get;set;}
    public override void Evaluate()
    {
        Assigment!.Evaluate();
        Expression? value = (Expression)Assigment.Value!;

    }

}