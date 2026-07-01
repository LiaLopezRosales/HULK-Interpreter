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
        var result = expr.Evaluate(globalScope, context, Semantic_Errors);
        return result;
    }
}
