public abstract class Node
{
    public Node()
    {

    }

    public abstract bool ValidSemantic(Context context,Scope scope,List<Error> errors);
}