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
                // tokenstream.MoveForward(1);
                name=tokenstream.tokens[tokenstream.Position()].Value;
            }
            if (tokenstream.LookAhead(1).Value!="(")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"("));
                temp_errors+=1;
            }
            else 
            {
                //tokenstream.MoveForward(1);
                int temp_position=tokenstream.Position();
            tokenstream.MoveTo(End_of_Expression(tokenstream.Position(),tokenstream));
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
                        //tokenstream.MoveForward(1);
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
            //else tokenstream.MoveForward(1);

            if (temp_errors==0)
            {   //Implementar metodo que chequea sintaxis relativa del argumento(no comprueba la existencia de funciones)
                if (Valid_FunctionArgument(tokenstream.Position()+1,tokenstream.tokens.Length-3))
                {
                    Token[] function_argument=tokenstream.tokens[tokenstream.Position()..(tokenstream.tokens.Length-3)];
                    context.AddFunction(name,function_argument);
                }

            }
               
        }
        else ParseExpression(errors,context,result,tokenstream);

    }
    

   private Expression? ParseLet_In(TokenStream stream1,List<Error>errors)
   {
     if (stream1.LookAhead(1).Tipo!=Token.Type.identifier)
     {
        errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"some identifier"));
     }
     
     if (stream1.LookAhead(1).Value!="=")
     {
        errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"some identifier"));
     }
     
     if (stream1.LookAhead(1).Value=="(")
     {
        int end = End_of_Expression(stream1.Position(),stream1);
        Assig = ParseExpression(new TokenStream(stream1.tokens,stream1.Position(),end));
     }
     else
     
   }
    public int End_of_Expression(int i,TokenStream tokenstream)
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

    private Expression? ParseNumber(TokenStream tokenstream)
    {
        if (!tokenstream.Next(Token.Type.number))return null;
        
        return new Number(double.Parse(tokenstream.LookAhead().Value));
               
    }
    private Expression? ParseText(TokenStream tokenstream)
    {
        if (!tokenstream.Next(Token.Type.text))return null;
        
        return new Text(tokenstream.LookAhead().Value);
               
    }
    private Expression? ParseBool(TokenStream tokenstream)
    {
        if (!tokenstream.Next(Token.Type.boolean))return null;
        
        return new Bool(Convert.ToBoolean(tokenstream.LookAhead().Value));
               
    }
    public Expression? ParseMathExpression(TokenStream tokenstream)
    {
        return ParseMathExpressionLv1(tokenstream);
    }
    private Expression? ParseMathExpressionLv1(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseMathExpressionLv2(tokenstream); 
         Expression? exp = ParseMathExpressionLv1_(newLeft,tokenstream); 
         return exp; 
     } 
  
     private Expression? ParseMathExpressionLv1_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseMathExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParseAdd(left,tokenstream); 
         if(exp != null) return exp; 
  
         exp = ParseSub(left,tokenstream); 
         if(exp != null) return exp; 
  
         return left; 
     } 
  
     private Expression? ParseMathExpressionLv2(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseMathExpressionLv3(tokenstream); 
         return ParseMathExpressionLv2(tokenstream); 
     } 
  
     private Expression? ParseMathExpressionLv2_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseMathExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParseMul(left,tokenstream); 
         if(exp != null) return exp; 
  
         exp = ParseDiv(left,tokenstream); 
         if(exp != null) return exp; 
  
         return left; 
     } 
     private Expression? ParseMathExpressionLv3(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseExpressionLv4(tokenstream); 
         return ParseMathExpressionLv3_(newLeft,tokenstream); 
     } 
     private Expression? ParseMathExpressionLv3_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseMathExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParsePower(left,tokenstream); 
         if(exp != null) return exp; 
  
         return left; 
     } 
  
     private Expression? ParseExpressionLv4(TokenStream tokenstream) 
     { 
         Expression? exp = ParseNumber(tokenstream); 
         if(exp != null) return exp; 
  
         exp = ParseText(tokenstream); 
         if(exp != null) return exp; 

         exp = ParseBool(tokenstream); 
         if(exp != null) return exp; 
  
         return null; 
     } 

     private Expression? ParseAdd(Expression? left,TokenStream tokenstream) 
     { 
         Sum sum = new Sum(); 
  
         if(left == null || !tokenstream.Next(Token.Type.sum)) return null; 
  
         sum.Left = left; 
  
         Expression? right = ParseMathExpressionLv2(tokenstream); 
         if(right == null) 
         { 
             tokenstream.MoveBackward(2); 
             return null; 
         } 
         sum.Right = right; 
  
         return ParseMathExpressionLv1_(sum,tokenstream); 
     } 

     private Expression? ParseSub(Expression? left,TokenStream tokenstream)
     {
        Substraction sub =new Substraction();
        if (left==null || !tokenstream.Next(Token.Type.substraction))return null;
        
        sub.Left=left;
        Expression? right =ParseMathExpressionLv2(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        sub.Right=right;
            return ParseMathExpressionLv1_(sub,tokenstream);
     }
     private Expression? ParseMul(Expression? left,TokenStream tokenstream)
     {
        Multiplication mul =new Multiplication();
        if (left==null || !tokenstream.Next(Token.Type.multiplication))return null;
        
        mul.Left=left;
        Expression? right =ParseMathExpressionLv3(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        mul.Right=right;
            return ParseMathExpressionLv2_(mul,tokenstream);
     }
     private Expression? ParseDiv(Expression? left,TokenStream tokenstream)
     {
        Division div =new Division();
        if (left==null || !tokenstream.Next(Token.Type.division))return null;
        
        div.Left=left;
        Expression? right =ParseMathExpressionLv3(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        div.Right=right;
            return ParseMathExpressionLv2_(div,tokenstream);
     }
     private Expression? ParsePower(Expression? left,TokenStream tokenstream)
     {
        Power pow =new Power();
        if (left==null || !tokenstream.Next(Token.Type.power))return null;
        
        pow.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        pow.Right=right;
            return ParseMathExpressionLv3_(pow,tokenstream);
     }
    public bool Valid_FunctionArgument(int from,int until)
    {
        throw new NotImplementedException();
    }
    
    public Expression? ParseBoolExpression(TokenStream tokenstream)
    {
        return ParseBoolExpressionLv1(tokenstream);
    }
    private Expression? ParseBoolExpressionLv1(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseBoolExpressionLv2(tokenstream); 
         Expression? exp = ParseBoolExpressionLv1_(newLeft,tokenstream); 
         return exp; 
     } 
  
     private Expression? ParseBoolExpressionLv1_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseBoolExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParseOr(left,tokenstream); 
         if(exp != null) return exp; 
  
         exp = ParseAnd(left,tokenstream); 
         if(exp != null) return exp; 
  
         return left; 
     } 
  
     private Expression? ParseBoolExpressionLv2(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseExpressionLv4(tokenstream); 
         return ParseBoolExpressionLv2(tokenstream); 
     } 
  
     private Expression? ParseBoolExpressionLv2_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParseMinor(left,tokenstream); 
         if(exp != null) return exp; 
  
         exp = ParseMajor(left,tokenstream); 
         if(exp != null) return exp; 

         exp = ParseEqual_Minor(left,tokenstream); 
         if(exp != null) return exp;

         exp = ParseEqual_Major(left,tokenstream); 
         if(exp != null) return exp;

         exp = ParseEqual(left,tokenstream); 
         if(exp != null) return exp;

         exp = ParseDiferent(left,tokenstream); 
         if(exp != null) return exp;

         return left; 
     } 

      private Expression? ParseOr(Expression? left,TokenStream tokenstream) 
     { 
         Or or = new Or(); 
  
         if(left == null || !tokenstream.Next(Token.Type.Or)) return null; 
  
         or.Left = left; 
  
         Expression? right = ParseBoolExpressionLv2(tokenstream); 
         if(right == null) 
         { 
             tokenstream.MoveBackward(2); 
             return null; 
         } 
         or.Right = right; 
  
         return ParseBoolExpressionLv1_(or,tokenstream); 
     } 

     private Expression? ParseAnd(Expression? left,TokenStream tokenstream) 
     { 
         And and = new And(); 
  
         if(left == null || !tokenstream.Next(Token.Type.And)) return null; 
  
         and.Left = left; 
  
         Expression? right = ParseBoolExpressionLv2(tokenstream); 
         if(right == null) 
         { 
             tokenstream.MoveBackward(2); 
             return null; 
         } 
         and.Right = right; 
  
         return ParseBoolExpressionLv1_(and,tokenstream); 
     } 

     private Expression? ParseMinor(Expression? left,TokenStream tokenstream)
     {
        Minor min =new Minor();
        if (left==null || !tokenstream.Next(Token.Type.minor))return null;
        
        min.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        min.Right=right;
            return ParseBoolExpressionLv1_(min,tokenstream);
     }

     private Expression? ParseMajor(Expression? left,TokenStream tokenstream)
     {
        Major maj =new Major();
        if (left==null || !tokenstream.Next(Token.Type.major))return null;
        
        maj.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        maj.Right=right;
            return ParseBoolExpressionLv1_(maj,tokenstream);
     }
     private Expression? ParseEqual_Minor(Expression? left,TokenStream tokenstream)
     {
        Equal_Minor emin =new Equal_Minor();
        if (left==null || !tokenstream.Next(Token.Type.equal_minor))return null;
        
        emin.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        emin.Right=right;
            return ParseBoolExpressionLv1_(emin,tokenstream);
     }
     private Expression? ParseEqual_Major(Expression? left,TokenStream tokenstream)
     {
        Equal_Major emaj =new Equal_Major();
        if (left==null || !tokenstream.Next(Token.Type.equal_major))return null;
        
        emaj.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        emaj.Right=right;
            return ParseBoolExpressionLv1_(emaj,tokenstream);
     }
     private Expression? ParseEqual(Expression? left,TokenStream tokenstream)
     {
        Equal eq =new Equal();
        if (left==null || !tokenstream.Next(Token.Type.equal))return null;
        
        eq.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        eq.Right=right;
            return ParseBoolExpressionLv1_(eq,tokenstream);
     }
     private Expression? ParseDiferent(Expression? left,TokenStream tokenstream)
     {
        Diferent dif =new Diferent();
        if (left==null || !tokenstream.Next(Token.Type.diferent))return null;
        
        dif.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        dif.Right=right;
            return ParseBoolExpressionLv1_(dif,tokenstream);
     }

     private Expression? ParseTextExpression(TokenStream tokenstream)
     {
        return ParseTextExpressionLv1(tokenstream);
     }

     private Expression? ParseTextExpressionLv1(TokenStream tokenstream) 
     { 
         Expression? newLeft = ParseExpressionLv4(tokenstream); 
         Expression? exp = ParseTextExpressionLv1_(newLeft,tokenstream); 
         return exp; 
     } 
  
     private Expression? ParseTextExpressionLv1_(Expression? left,TokenStream tokenstream) 
     { 
        if (tokenstream.Next(Token.Type.left_bracket))
        {
            int end = End_of_Expression(tokenstream.Position(),tokenstream);
            left = ParseExpression(new TokenStream(tokenstream.tokens,tokenstream.Position(),end));
            if (left==null) return null;
            if (!tokenstream.Next(Token.Type.right_bracket))return null;
        }
         Expression? exp = ParseConcatenate(left,tokenstream); 
         if(exp != null) return exp;  
  
         return left; 
     } 

     private Expression? ParseConcatenate(Expression? left,TokenStream tokenstream)
     {
        Concatenation con =new Concatenation();
        if (left==null || !tokenstream.Next(Token.Type.concatenate))return null;
        
        con.Left=left;
        Expression? right =ParseExpressionLv4(tokenstream);
        if(right==null)
        {
            tokenstream.MoveBackward(2);
            return null;
        }
        con.Right=right;
            return ParseBoolExpressionLv1_(con,tokenstream);
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