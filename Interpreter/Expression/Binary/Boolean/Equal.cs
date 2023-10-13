using System.Globalization;
public class Equal:Binary
{
    public Equal()
    {}
    public override ExpressionType Type { get => Type=ExpressionType.Bool; set => Type=ExpressionType.Bool; }

    public override object? Value { get => base.Value; set => base.Value = value; }
    public override void Evaluate()
    {
        Right!.Evaluate();
        Left!.Evaluate();
        if (Right.Type==ExpressionType.Number&&Left.Type==ExpressionType.Number)
        {
            Value = Convert.ToDouble(Right.Value!,CultureInfo.InvariantCulture) == Convert.ToDouble(Left.Value!,CultureInfo.InvariantCulture);
        }
        if(Right.Type==ExpressionType.Text&&Left.Type==ExpressionType.Text)
        {
            Value = Right.Value! == Left.Value!;
        }
         
    }
    public override string ToString()
    {
        if (Value==null)
        {
            return String.Format("({0}=={1})",Left,Right);
        }
        return Value.ToString()!;
    }
}