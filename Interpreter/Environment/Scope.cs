public class Scope
{   //Ámbito donde existen un grupo de variables
    public Scope? Parent{get;set;}
    public Dictionary<string,object> Variables{get;set;}

    public Scope()
    {
        Variables=new Dictionary<string, object>();
        
        this.Parent=null;
    }

    public Scope Child()
    {
        Scope child = new Scope();
        child.Parent=this;
        return child;
    }
}