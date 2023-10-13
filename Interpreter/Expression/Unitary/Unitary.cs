public abstract class Unitary:Expression
{
    public Unitary()
    {}

    public override void Evaluate()
    {
        
    }

    public override string ToString()=>String.Format("{0}",Value);

    public override bool ValidSemantic(Context context, Scope scope, List<Error> errors)=>true;
    
        
    


    
    
}