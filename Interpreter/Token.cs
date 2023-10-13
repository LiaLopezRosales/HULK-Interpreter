public  class Token
{
    public Type Tipo{get; set;}
    public string Value {get; set;}

    public enum Type{sin,cos,sqrt,exp,rand,log,PI,E,print, sum,substraction,multiplication,division,power, keyword, conditional, symbol,left_bracket,right_bracket, concatenate, Or,And,minor,major,equal_minor,equal_major,equal,diferent, boolean, identifier, text , number,not_id, EOL}
    
    public Token(Type tipo,string value)
    {
      this.Tipo = tipo;
      this.Value=value;
    }

    public override string ToString() => string.Format("{0} [{1}]",Tipo,Value);
    
        
    

    
    // public abstract string Form();
    // // public abstract double Value();
    // public abstract int GetPrecedence();
}
