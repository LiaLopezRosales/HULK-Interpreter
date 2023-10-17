public class Context
{
    public List<Fuction> Available_Functions{get;set;}
    //public Dictionary<string,Dictionary<int,Token>> Functions_Arguments{get;set;}
    public Dictionary<string,Func<double,double>> Trig_functions{get;}
    public Dictionary<string,Func<double>> Math_value{get;}
    public Dictionary<string,Func<double,double,double>> Log{get;}
    public Dictionary<string,Func<object,object>> Print{get;}

    public Context()
    {
        Available_Functions=new List<Fuction>();
        //Functions_Arguments=new Dictionary<string,Dictionary<int,Token>>();
        Trig_functions = new Dictionary<string, Func<double,double>>();
        Trig_functions.Add("sin",(double argument)=>Math.Sin(argument));
        Trig_functions.Add("cos",(double argument)=>Math.Cos(argument));
        Trig_functions.Add("sqrt",(double argument)=>Math.Sqrt(argument));
        Trig_functions.Add("exp",(double argument)=>Math.Exp(argument));
        Math_value = new Dictionary<string, Func<double>>();
        Math_value.Add("PI",()=>Math.PI);
        Math_value.Add("E",()=>Math.E);
        Math_value.Add("rand",Rand);
        Log = new Dictionary<string, Func<double, double, double>>();
        Log.Add("log",(double Base,double argument)=> Math.Log(argument,Base));
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

    // public void AddFunction(string name,Node code)
    // {
    //     Available_Functions.Add(name,code);
    // }
}