public class Binary:Expression
{   
    public Expression? Left{get;set;}
    public Expression? Right{get;set;}
    public Binary()
    {}

    public override bool ValidSemantic(Context context, Scope scope, List<Error> errors)
    {
        bool right=Right!.ValidSemantic(context,scope,errors);
        bool left=Left!.ValidSemantic(context,scope,errors);

        if (Right.Type==ExpressionType.Number&&Left.Type!=ExpressionType.Number)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type!=ExpressionType.Number&&Left.Type==ExpressionType.Number)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type==ExpressionType.Text&&Left.Type!=ExpressionType.Text)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type!=ExpressionType.Text&&Left.Type==ExpressionType.Text)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type==ExpressionType.Bool&&Left.Type!=ExpressionType.Bool)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type!=ExpressionType.Bool&&Left.Type==ExpressionType.Bool)
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"diferent types"));
            return false;
        }
        if (Right.Type==ExpressionType.Number&&Left.Type==ExpressionType.Number)
        {
            Type=ExpressionType.Number;
        }
        else if (Right.Type==ExpressionType.Text && Left.Type==ExpressionType.Text)
        {
            Type=ExpressionType.Text;
        }
        else if (Right.Type==ExpressionType.Bool && Left.Type==ExpressionType.Bool)
        {
            Type=ExpressionType.Bool;
        }
        return (right && left);
    }

    public override ExpressionType Type{get;set;}
    public override object? Value{get;set;}
    public override void Evaluate()
    {}



}