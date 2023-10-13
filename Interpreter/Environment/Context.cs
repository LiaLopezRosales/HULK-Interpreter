public class Context
{
    public Dictionary<string,Token[]> Functions{get;set;}
    public Dictionary<string,Dictionary<int,Token>> Functions_Arguments{get;set;}
    public Dictionary<string,Func<Number,double>> Trig_functions{get;}
    public Dictionary<string,Func<double>> Math_value{get;}
    public Dictionary<string,Func<double,double,double>> Log{get;}
    public Dictionary<string,Func<object,object>> Print{get;}

    public Context()
    {
        Functions=new Dictionary<string, Token[]>();
        Functions_Arguments=new Dictionary<string,Dictionary<int,Token>>();
        Trig_functions = new Dictionary<string, Func<Number,double>>();
        Trig_functions.Add("sin",(Number argument)=>Math.Sin((double)argument.Value!));
        Trig_functions.Add("cos",(Number argument)=>Math.Cos((double)argument.Value!));
        Trig_functions.Add("sqrt",(Number argument)=>Math.Sqrt((double)argument.Value!));
        Trig_functions.Add("exp",(Number argument)=>Math.Exp((double)argument.Value!));
        Math_value = new Dictionary<string, Func<double>>();
        Math_value.Add("PI",()=>Math.PI);
        Math_value.Add("E",()=>Math.E);
        Math_value.Add("rand",Rand);
        Log = new Dictionary<string, Func<double, double, double>>();
        Log.Add("log",(double Base,double argument)=> Math.Log(Base,argument));
        Print = new Dictionary<string, Func<object, object>>();
        //Agregar a Print funcion rand()
        object PrintReturn(object argument)
        {
            Console.WriteLine(argument.ToString());
            return argument;
        }
        Print.Add("print",PrintReturn);
        double Rand()
        {
            Random random = new Random();
            double number = random.NextDouble()*(1-0)+0;
            return number;
        }

    }

    public void AddFunction(string name,Token[] code)
    {
        Functions.Add(name,code);
    }
}