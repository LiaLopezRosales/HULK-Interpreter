using System.Globalization;

public abstract class BinaryExpression : Expression
{
    public Expression Left { get; }
    public Expression Right { get; }

    protected BinaryExpression(Expression left, Expression right)
    {
        Left = left;
        Right = right;
    }

    protected static bool AreNumbers(object left, object right, List<Error> errors)
    {
        if (left is double && right is double)
            return true;
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "numerical values"));
        return false;
    }

    protected static bool AreBooleans(object left, object right, List<Error> errors)
    {
        if (left is bool && right is bool)
            return true;
        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Expected, "boolean values"));
        return false;
    }
}
