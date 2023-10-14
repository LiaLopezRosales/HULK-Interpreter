public abstract class Node
{
    public NodeType Type{get;set;}
    public object? NodeExpression{get;set;}
    public List<Node>Branch;
    public Node()
    {
      Branch=new();
    }

    public abstract bool ValidSemantic(Context context,Scope scope,List<Error> errors);
    public enum NodeType{}
}