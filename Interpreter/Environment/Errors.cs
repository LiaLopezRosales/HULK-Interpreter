public class Error
{   //Clase donde se expresan y agrupan los detalles de un error encontrado
    public ErrorCode Code{get;set;}
    public string Argument{get;set;}
    public TypeError type{get;set;}
    public int Line{get;set;} = -1;
    public int Col{get;set;} = -1;
    public enum ErrorCode{None,Expected,Invalid,Unknown}
    public enum TypeError{Lexical_Error,Syntactic_Error,Semantic_Error}
    public Error(TypeError type,ErrorCode code,string argument)
    {
        this.type=type;
        this.Code =code;
        this.Argument=argument;
    }

    public override string ToString()
    {
        string pos = Line >= 0 ? $" (linea {Line}, col {Col})" : "";
        return String.Format("!{0}: {1} {2}{3}",type,Code,Argument,pos);
    }
}