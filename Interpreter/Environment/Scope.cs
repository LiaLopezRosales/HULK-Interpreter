public class Scope
{
    public Scope? Parent{get;set;}
    //public List<Scope>? Children{get;set;}
    public Dictionary<string,object> Variables{get;set;}

    public Scope()
    {
        Variables=new Dictionary<string, object>();
        
        this.Parent=null;
        //this.Children=new List<Scope>();
    }

    public Scope Child()
    {
        Scope child = new Scope();
        child.Parent=this;
        //child.Parent.Children!.Add(child);
        return child;
    }
}