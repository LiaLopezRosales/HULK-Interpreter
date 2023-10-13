using System.Globalization;
public class Equal_Minor:Binary
{
    public Equal_Minor()
    {}
    public override ExpressionType Type { get => Type=ExpressionType.Bool; set => Type=ExpressionType.Bool; }

    public override object? Value { get => base.Value; set => base.Value = value; }
    public override void Evaluate()
    {
        Right!.Evaluate();
        Left!.Evaluate();
         Value = Convert.ToDouble(Right.Value!,CultureInfo.InvariantCulture) <= Convert.ToDouble(Left.Value!,CultureInfo.InvariantCulture);
    }
    public override string ToString()
    {
        if (Value==null)
        {
            return String.Format("({0}<={1})",Left,Right);
        }
        return Value.ToString()!;
    }
}