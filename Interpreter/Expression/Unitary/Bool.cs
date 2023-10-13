public class Bool: Unitary
{
    public override object? Value { get; set; }
    public Bool(bool value)
    {
       this.Value=value;
    }
    public override ExpressionType Type 
    { get => ExpressionType.Bool; 
     set{}
    }
}