public class Scope
{
    public Scope? Parent{get;set;}
    public Dictionary<Token,Token[]> Variables{get;set;}

    public Scope()
    {
        Variables=new Dictionary<Token, Token[]>();
    }

    public Scope Child()
    {
        Scope child = new Scope();
        child.Parent=this;
        return child;
    }
}