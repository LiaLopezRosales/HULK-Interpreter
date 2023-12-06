public abstract class Unitary:Expression
{
    public Unitary()
    {}

    public override void Evaluate(object left, object right)
    {
        throw new NotImplementedException();
    }
    public override void Evaluate(object condition, object If, object Else)
    {
        throw new NotImplementedException();
    }

    public override string ToString()=>String.Format("{0}",Value);
    
}