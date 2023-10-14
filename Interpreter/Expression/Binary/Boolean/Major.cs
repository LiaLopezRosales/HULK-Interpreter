using System.Globalization;
public class Major:Binary
{
    public Major()
    {}
    public override ExpressionType Type { get => Type=ExpressionType.Bool; set => Type=ExpressionType.Bool; }

    public override object? Value { get => base.Value; set => base.Value = value; }
    public override void Evaluate(object left,object right)
    {
         Value = Convert.ToDouble(left,CultureInfo.InvariantCulture) > Convert.ToDouble(right,CultureInfo.InvariantCulture);
    }
    public override string ToString()
    {
        if (Value==null)
        {
            return String.Format("({0}<{1})",Left,Right);
        }
        return Value.ToString()!;
    }
}