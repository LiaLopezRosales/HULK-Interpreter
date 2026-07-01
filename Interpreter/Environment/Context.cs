public class Context
{
    public List<Function> Available_Functions { get; set; }
    public Dictionary<string, Func<double, double>> Trig_functions { get; }
    public Dictionary<string, Func<double>> Math_value { get; }
    public Dictionary<string, Func<double, double, double>> Log { get; }

    public Context()
    {
        Available_Functions = new List<Function>();

        Trig_functions = new Dictionary<string, Func<double, double>>
        {
            ["sin"] = Sin,
            ["cos"] = Cos,
            ["sqrt"] = Math.Sqrt,
            ["exp"] = Math.Exp
        };

        Math_value = new Dictionary<string, Func<double>>
        {
            ["PI"] = () => Math.PI,
            ["E"] = () => Math.E,
            ["rand"] = () => Random.Shared.NextDouble()
        };

        Log = new Dictionary<string, Func<double, double, double>>
        {
            ["log"] = (b, v) => Math.Log(v, b)
        };
    }

    private static double Cos(double argument)
    {
        if (Math.Abs(Math.Cos(argument)) < 0.0000001) return 0;
        if (1 - Math.Cos(argument) < 0.0000001) return 1;
        if (1 + Math.Cos(argument) < 0.0000001) return -1;
        return Math.Cos(argument);
    }

    private static double Sin(double argument)
    {
        if (Math.Abs(Math.Sin(argument)) < 0.0000001) return 0;
        if (1 - Math.Sin(argument) < 0.0000001) return 1;
        if (1 + Math.Sin(argument) < 0.0000001) return -1;
        return Math.Sin(argument);
    }
}
