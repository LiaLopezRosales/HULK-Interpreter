public abstract class Expression:Node
{
    public Expression()
    {}
    public abstract ExpressionType Type {get;set;}

    public abstract object? Value{get;set;}

    public abstract void Evaluate();
    public abstract bool ValidSemantic(Context context,Scope scope,List<Error> errors);

    public enum ExpressionType{Bool,Number,Text,Other,Conditional,Let_In}
}