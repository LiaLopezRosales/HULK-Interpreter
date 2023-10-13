public class Parser
{
    public TokenStream tokenstream {get;private set;}
    //public enum TypeExpression{Let_argument,In,Condition,If,Else,Function_Argument,Numerical,Boolean,Textual,Expression,Math,Bool,Conc}

    public Parser(TokenStream stream)
    {
        this.tokenstream=stream;
    }
    object result="";
    Context context=new Context();
    public object? ParseProgram(List<Error> errors)
    {
        
        if(!tokenstream.CanLookAhead(0))return result;
        if(tokenstream.tokens[0].Value=="function")
        {
            int temp_errors=0;
            string name="";
            if (tokenstream.LookAhead(1).Tipo!=Token.Type.identifier)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"id"));
                temp_errors+=1;
            }
            else 
            {
                tokenstream.MoveForward(1);
                name=tokenstream.tokens[tokenstream.Position()].Value;
            }
            if (tokenstream.LookAhead(1).Value!="(")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"("));
                temp_errors+=1;
            }
            else 
            {
                tokenstream.MoveForward(1);
                int temp_position=tokenstream.Position();
            tokenstream.MoveTo(End_of_Expression(tokenstream.Position()));
               if ((tokenstream.Position()-temp_position)!=1)
               {
                int end = tokenstream.Position();
                int argumentcount=1;
                 tokenstream.MoveTo(temp_position);
                 while (tokenstream.Position()<end)
                 {
                    if (tokenstream.LookAhead(1).Tipo!=Token.Type.identifier)
                    {
                        errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"argument"));
                    }
                    else
                    {
                        tokenstream.MoveForward(1);
                        context.Functions_Arguments.Add(name,new Dictionary<int, Token>());
                        context.Functions_Arguments[name].Add(argumentcount,tokenstream.tokens[tokenstream.Position()]);
                        argumentcount+=1;
                        if (tokenstream.Position()+1<end)
                        {
                            if (tokenstream.LookAhead(1).Value!=",")
                            {
                                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"',' symbol"));
                                break;
                            }
                        }
                        else break;
                    }
                 }
                 tokenstream.MoveTo(end);
                 
               }
            }
            if (tokenstream.LookAhead(1).Value!="=>")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'=>' symbol"));
                temp_errors+=1;
            }
            else tokenstream.MoveForward(1);

            if (temp_errors==0)
            {   //Implementar metodo que chequea sintaxis relativa del argumento(no comprueba la existencia de funciones)
                if (Valid_FunctionArgument(tokenstream.Position()+1,tokenstream.tokens.Length-3))
                {
                    Token[] function_argument=tokenstream.tokens[tokenstream.Position()..(tokenstream.tokens.Length-3)];
                    context.AddFunction(name,function_argument);
                }

            }
               
        }
        else ParseExpression(errors,context,result);

    }
    

    public int End_of_Expression(int i)
    {
        int end=i;
        int count=0;
        int original_position = tokenstream.Position();
        tokenstream.MoveTo(i);
        if (tokenstream.tokens[i].Value=="(")
        {
            count+=1;
        }
        while (count!=0)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Value=="(")
            {
                count+=1;
                
            }
            if (tokenstream.tokens[tokenstream.Position()].Value==")")
            {
                count-=0;
                end=tokenstream.Position();
            }
        }
        tokenstream.MoveTo(original_position);
        return end;
    }

    private Expression? ParseNumber(int f)
    {
        if (!tokenstream.Next(Token.Type.number,f))return null;
        
        return new Number(double.Parse(tokenstream.LookAhead().Value));
               
    }
    private Expression? ParseText(int f)
    {
        if (!tokenstream.Next(Token.Type.text,f))return null;
        
        return new Text(tokenstream.LookAhead().Value);
               
    }
    private Expression? ParseBool(int f)
    {
        if (!tokenstream.Next(Token.Type.boolean,f))return null;
        
        return new Bool(Convert.ToBoolean(tokenstream.LookAhead().Value));
               
    }
    
    private Expression? ParseMathExpressionLv1(int i,int f) 
     { 
         Expression? newLeft = ParseMathExpressionLv2(i,f); 
         Expression? exp = ParseMathExpressionLv1_(newLeft,i,f); 
         return exp; 
     } 
  
     private Expression? ParseMathExpressionLv1_(Expression? left,int i,int f) 
     { 
         Expression? exp = ParseAdd(left,i,f); 
         if(exp != null) return exp; 
  
         exp = ParseSub(left,i,f); 
         if(exp != null) return exp; 
  
         return left; 
     } 
  
     private Expression? ParseMathExpressionLv2(int i,int f) 
     { 
         Expression? newLeft = ParseMathExpressionLv3(i,f); 
         return ParseMathExpressionLv2_(newLeft,i,f); 
     } 
  
     private Expression? ParseMathExpressionLv2_(Expression? left,int i,int f) 
     { 
         Expression? exp = ParseMul(left,i,f); 
         if(exp != null) return exp; 
  
         exp = ParseDiv(left,i,f); 
         if(exp != null) return exp; 
  
         return left; 
     } 
     private Expression? ParseMathExpressionLv3(int i,int f) 
     { 
         Expression? newLeft = ParseExpressionLv4(f); 
         return ParseMathExpressionLv3_(newLeft,i,f); 
     } 
     private Expression? ParseMathExpressionLv3_(Expression? left,int i,int f) 
     { 
         Expression? exp = ParsePower(left,i,f); 
         if(exp != null) return exp; 
  
         return left; 
     } 

     //Hacer metodo que delimite tipo de expresiones y parsear el parentesis con estos metodos primero
    //  private Expression? ParseMathExpressionLv4()
    //  {
    //     if (tokenstream.tokens[tokenstream.Position()].Value!="(")
    //     {
    //         int i=
    //     }
    //  }
  
     private Expression? ParseExpressionLv4(int f) 
     { 
         Expression? exp = ParseNumber(f); 
         if(exp != null) return exp; 
  
         exp = ParseText(f); 
         if(exp != null) return exp; 

         exp = ParseBool(f); 
         if(exp != null) return exp; 
  
         return null; 
     } 

     private Expression? ParseAdd(Expression? left,int i,int f) 
     { 
         Sum sum = new Sum(); 
  
         if(left == null || !tokenstream.Next(Token.Type.sum)) return null; 
  
         sum.Left = left; 
  
         Expression? right = ParseMathExpressionLv2(i,f); 
         if(right == null) 
         { 
             tokenstream.MoveBack(2); 
             return null; 
         } 
         sum.Right = right; 
  
         return ParseMathExpressionLv1_(sum,i,f); 
     } 
    public bool Valid_FunctionArgument(int from,int until)
    {
        throw new NotImplementedException();
    }

    // public Dictionary<int,Tuple<TypeExpression,int>> ListofExpressions(Token[]tokens,int i,int j)
    // {
    //   int original_position =tokenstream.Position();
    //   tokenstream.MoveTo(i);
    //   Dictionary<int,Tuple<TypeExpression,int>> expressions = new Dictionary<int, Tuple<TypeExpression, int>>();
    //   while(tokenstream.CanLookAhead())
    //   {
    //     if (true)
    //     {
            
    //     }
    //   }
    // }

}