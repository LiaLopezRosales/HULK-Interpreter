using System.Globalization;

public class ForExpression : Expression
{
    public string VariableName { get; }
    public Expression Iterable { get; }
    public Expression Body { get; }

    public ForExpression(string variableName, Expression iterable, Expression body)
    {
        VariableName = variableName;
        Iterable = iterable;
        Body = body;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        object iterable = Iterable.Evaluate(scope, context, errors);
        object? last = null;

        if (iterable is List<double> list)
        {
            foreach (double item in list)
            {
                Scope newScope = new Scope { Parent = scope };
                newScope.Variables[VariableName] = item;
                try { last = Body.Evaluate(newScope, context, errors); }
                catch (BreakException) { break; }
                catch (ContinueException) { continue; }
            }
        }
        else if (iterable is List<object> objList)
        {
            foreach (object item in objList)
            {
                Scope newScope = new Scope { Parent = scope };
                newScope.Variables[VariableName] = item;
                try { last = Body.Evaluate(newScope, context, errors); }
                catch (BreakException) { break; }
                catch (ContinueException) { continue; }
            }
        }
        else if (iterable is string s)
        {
            foreach (char ch in s)
            {
                Scope newScope = new Scope { Parent = scope };
                newScope.Variables[VariableName] = (double)ch;
                try { last = Body.Evaluate(newScope, context, errors); }
                catch (BreakException) { break; }
                catch (ContinueException) { continue; }
            }
        }
        else
        {
            errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "iterable expected"));
        }

        return last!;
    }
}
