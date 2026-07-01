public class AST_Evaluator
{
    public List<Error> Semantic_Errors { get; set; }
    private Context context;
    private Scope globalScope;

    public AST_Evaluator()
    {
        context = new Context();
        globalScope = new Scope();
        Semantic_Errors = new List<Error>();
    }

    public object Evaluate(Expression expr)
    {
        Semantic_Errors.Clear();
        try
        {
            return expr.Evaluate(globalScope, context, Semantic_Errors);
        }
        catch (BreakException)
        {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "break fuera de un ciclo"));
            return null!;
        }
        catch (ContinueException)
        {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "continue fuera de un ciclo"));
            return null!;
        }
    }
}
