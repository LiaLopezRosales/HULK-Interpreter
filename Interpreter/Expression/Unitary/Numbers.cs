public class Number: Unitary
{
    public override object? Value { get; set; }
    public Number(double value)
    {
       this.Value=value;
    }
    public override ExpressionType Type 
    { get => ExpressionType.Number; 
     set{}
    }
}