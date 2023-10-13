public class Text: Unitary
{
    public override object? Value { get; set; }
    public Text(string value)
    {
       this.Value=value;
    }
    public override ExpressionType Type 
    { get => ExpressionType.Text; 
     set{}
    }
}