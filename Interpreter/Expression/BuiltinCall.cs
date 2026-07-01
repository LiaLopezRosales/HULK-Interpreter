using System.Globalization;

public class BuiltinCall : Expression
{
    public string FunctionName { get; }
    public List<Expression> Arguments { get; }

    public BuiltinCall(string functionName, List<Expression> arguments)
    {
        FunctionName = functionName;
        Arguments = arguments;
    }

    public override object Evaluate(Scope scope, Context context, List<Error> errors)
    {
        switch (FunctionName)
        {
            case "sin":
            case "cos":
            case "sqrt":
            case "exp":
                return context.Trig_functions[FunctionName](Convert.ToDouble(Arguments[0].Evaluate(scope, context, errors), CultureInfo.InvariantCulture));

            case "log":
                return context.Log["log"](
                    Convert.ToDouble(Arguments[0].Evaluate(scope, context, errors), CultureInfo.InvariantCulture),
                    Convert.ToDouble(Arguments[1].Evaluate(scope, context, errors), CultureInfo.InvariantCulture));

            case "range":
                {
                    double start = Convert.ToDouble(Arguments[0].Evaluate(scope, context, errors), CultureInfo.InvariantCulture);
                    double end = Convert.ToDouble(Arguments[1].Evaluate(scope, context, errors), CultureInfo.InvariantCulture);
                    double step = Arguments.Count >= 3
                        ? Convert.ToDouble(Arguments[2].Evaluate(scope, context, errors), CultureInfo.InvariantCulture)
                        : 1.0;
                    if (step == 0)
                    {
                        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "range step cannot be zero"));
                        return new List<double>();
                    }
                    List<double> result = new List<double>();
                    if (step > 0)
                        for (double i = start; i < end; i += step)
                            result.Add(i);
                    else
                        for (double i = start; i > end; i += step)
                            result.Add(i);
                    return result;
                }

            case "rand":
                return context.Math_value["rand"]();

            case "PI":
                return context.Math_value["PI"]();

            case "E":
                return context.Math_value["E"]();

            default:
                errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, $"unknown builtin: {FunctionName}"));
                return null!;
        }
    }
}
