public class Function
{
    public string Name { get; }
    public Expression Code { get; }
    public List<string> Parameters { get; }

    public Function(string name, Expression code, List<string> parameters)
    {
        Name = name;
        Code = code;
        Parameters = parameters;
    }
}
