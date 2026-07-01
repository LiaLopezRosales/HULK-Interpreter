public  class Token
{
    public Type Tipo{get; set;}
    public string Value {get; set;}
    public int SourceIndex {get; set;} = -1;

    public enum Type{sin,cos,sqrt,exp,rand,log,PI,E,print, sum,substraction,multiplication,division,module,power, keyword, conditional, symbol,left_bracket,right_bracket, not, concatenate, Or,And,minor,major,equal_minor,equal_major,equal,diferent,assign, boolean, identifier, text , number,not_id, EOL}
    
    public Token(Type tipo,string value)
    {
      this.Tipo = tipo;
      this.Value=value;
    }

    public (int Line, int Col) GetPosition(string source)
    {
        int line = 1, col = 1;
        for (int i = 0; i < SourceIndex && i < source.Length; i++)
        {
            if (source[i] == '\n') { line++; col = 1; }
            else col++;
        }
        return (line, col);
    }

    public override string ToString() => string.Format("{0} [{1}]",Tipo,Value);
    
}
