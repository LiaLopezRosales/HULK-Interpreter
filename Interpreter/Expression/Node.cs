public class Node
{
    public NodeType Type{get;set;}
    public object? NodeExpression{get;set;}
    public List<Node>Branches;
    public Node()
    {
      Type=NodeType.Indefined;
      Branches=new();
    }

    // public bool ValidSemantic(Context context,Scope scope,List<Error> errors)
    // {
    //   return 
    // }
    public enum NodeType{Assignations,VarName,Assigment,Let_exp,Print,Conditional,IF,Else,FucName,Declared_FucName,Declared_Fuc,ParName,Negation,Var,parameters,Fuction,Concat,And,Or,Minor,Major,Equal_Minor,Equal_Major,Equal,Diferent,Sum,Sub,Mul,Div,Pow,No,Number,True,False,Text,Cos,Sin,Log,Sqrt,Exp,Rand,PI,E,Indefined};

}