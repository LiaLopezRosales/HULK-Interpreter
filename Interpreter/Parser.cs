using System.Globalization;
public class Parser
{
    List<Token> tokens;
    TokenStream tokenstream;
    Scope scope;

    public List<Error>errors;
    public Parser(List<Token>tokens_expression)
    {   
        tokenstream=new TokenStream(tokens_expression);
        tokens=tokens_expression;
        scope=new Scope();
        errors = new List<Error>();
    }

    public Node Parse()
    {
        Node AST=ParseExpression();
        return AST;
    }
    public List<Error> Syntactic_Errors()
    {
        return errors;
    }
    
    public Node ParseExpression()
    {
        if ((tokenstream.Position()<tokens.Count)&& tokens[tokenstream.Position()].Tipo==Token.Type.print)
        {
            return Print();
        }
        if ((tokenstream.Position()<tokens.Count)&& tokens[tokenstream.Position()].Value=="let")
        {
            return Let_In();
        }
        if ((tokenstream.Position()<tokens.Count)&& tokens[tokenstream.Position()].Value=="if")
        {
            return IF_ElSE();
        }
        if ((tokenstream.Position()<tokens.Count)&& tokens[tokenstream.Position()].Value=="function")
        {
            return Function();
        }
        return ParseOP();

    }
    public Node Print()
    {
       tokenstream.MoveForward(1);
       if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
       {
         errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
       }
       else tokenstream.MoveForward(1);
       Node argument=ParseExpression();
       Console.WriteLine(tokenstream.tokens[tokenstream.Position()].Value);
       if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
       {
        Console.WriteLine(tokenstream.tokens[tokenstream.Position()].Tipo);
         errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
       }
       else tokenstream.MoveForward(1);
       Node tem=new Node();
       tem.Type=Node.NodeType.Print;
       tem.Branches=new List<Node>{argument};
       return tem;
    }
    public Node IF_ElSE()
    {
        tokenstream.MoveForward(1);
        
       if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
       {
         errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
       }
       else tokenstream.MoveForward(1);
       Node argument=ParseOP();
       if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
       {
         errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
       }
       else tokenstream.MoveForward(1);
       Node if_part=ParseExpression();
       if (tokenstream.tokens[tokenstream.Position()].Value!="else")
       {
         errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'else' keyword after if expresion"));
       }
       else tokenstream.MoveForward(1);
       Node else_part=ParseExpression();
       Node conditional=new Node();
       conditional.Type=Node.NodeType.Conditional;
       conditional.Branches=new List<Node>{argument,if_part,else_part};
       return conditional;
    }
    public Node Function()
    {
        tokenstream.MoveForward(1);
        Node param =new Node();
        param.Type=Node.NodeType.parameters;
        if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.identifier)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"function name"));
        }
        else tokenstream.MoveForward(1);
        Node name =new Node();
        name.Type=Node.NodeType.FucName;
        name.NodeExpression=tokenstream.tokens[tokenstream.Position()-1].Value;
        if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
        }
        else tokenstream.MoveForward(1);
        while (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier)
        {
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.identifier)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"parameter name"));
        }
        else tokenstream.MoveForward(1);
        Node par_name=new Node();
        par_name.Type=Node.NodeType.ParName;
        par_name.NodeExpression=tokenstream.tokens[tokenstream.Position()-1].Value;
        param.Branches.Add(par_name);
        if (tokenstream.tokens[tokenstream.Position()].Value==",")
        {
            tokenstream.MoveForward(1);
        }
        }
        if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
        }
        else tokenstream.MoveForward(1);
        if (tokenstream.tokens[tokenstream.Position()].Value!="=>")
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'=>' symbol"));
        }
        else tokenstream.MoveForward(1);
        Node body=ParseExpression();
        if(body.NodeExpression is Error)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"function body"));
        }
        Node function = new Node();
        function.Type=Node.NodeType.Fuction;
        function.Branches=new List<Node>{name,param,body};
        return function;
    }
    
    public Node Let_In()
    {
        tokenstream.MoveForward(1);
        Node assignation =new Node();
        assignation.Type=Node.NodeType.Assignations;
        bool existcomm=false;

        do
        {
            if(existcomm)
            {
                tokenstream.MoveForward(1);
            }
            existcomm=true;
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.identifier || ((tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier && tokenstream.tokens[tokenstream.Position()+1].Tipo==Token.Type.left_bracket)))
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"variable identifier"));
            }
            else tokenstream.MoveForward(1);
             Node var_name=new Node();
             var_name.Type=Node.NodeType.VarName;
             var_name.NodeExpression=tokenstream.tokens[tokenstream.Position()-1].Value;
             if (tokenstream.tokens[tokenstream.Position()].Value!="=")
             {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'=' symbol"));
             }
             else tokenstream.MoveForward(1);
             Node value=ParseOP();
             if (value.NodeExpression is Error)
            {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"let-in expression"));
            }
            Node variab=new Node();
            variab.Type=Node.NodeType.Assigment;
            variab.Branches=new List<Node>{var_name,value};
            assignation.Branches.Add(variab);

        } while (tokenstream.tokens[tokenstream.Position()].Value==",");
        if (tokenstream.tokens[tokenstream.Position()].Value!="in")
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'in' keyword"));
        }
        else tokenstream.MoveForward(1);
        Node operators =ParseExpression();
        if (operators.NodeExpression is Error)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression let-in"));
        }
        Node var =new Node();
        var.Type=Node.NodeType.Let_exp;
        var.Branches=new List<Node>{assignation,operators};
        return var;
    }

    public Node Unit()
    {
        if (tokenstream.Position()>=tokenstream.tokens.Count)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"more tokens,end of expression"));
        }
        Token current = tokenstream.tokens[tokenstream.Position()];

        if (current.Tipo==Token.Type.left_bracket)
        {
            tokenstream.MoveForward(1);
            Node subnode=ParseExpression();
            if (tokenstream.Position()>=tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
             tokenstream.MoveForward(1);
             return subnode;

        }
        if (current.Value=="!")
        {
            tokenstream.MoveForward(1);
            Node value=ParseExpression();
            Node negation=new Node();
            negation.Type=Node.NodeType.Negation;
            negation.Branches=new List<Node>{value};
            return negation;
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.number)
        {
            double value = (Convert.ToDouble(tokenstream.tokens[tokenstream.Position()].Value,CultureInfo.InvariantCulture));
            Node temp=new Node();
            temp.Type=Node.NodeType.Number;
            temp.NodeExpression=value;
            tokenstream.MoveForward(1);
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.text)
        {
           string value=(tokenstream.tokens[tokenstream.Position()].Value);
           Node temp=new Node();
           temp.Type=Node.NodeType.Text;
           temp.NodeExpression=value;
           tokenstream.MoveForward(1);
           return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.PI)
        {
            Node temp=new Node();
            temp.Type=Node.NodeType.PI;
            temp.NodeExpression="PI";
            tokenstream.MoveForward(1);
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.E)
        {
            Node temp=new Node();
            temp.Type=Node.NodeType.E;
            temp.NodeExpression="E";
            tokenstream.MoveForward(1);
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.rand)
        {
            if (tokenstream.tokens[tokenstream.Position()+1].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol after rand function)"));
            }
            else tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()+1].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol to close rand function)"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Rand;
            temp.NodeExpression="rand";
            tokenstream.MoveForward(1);
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.boolean)
        {
            if (tokenstream.tokens[tokenstream.Position()].Value=="false")
            {
               bool value = (Convert.ToBoolean(tokenstream.tokens[tokenstream.Position()].Value));
               Node temp=new Node();
               temp.Type=Node.NodeType.False;
               temp.NodeExpression=value;
               tokenstream.MoveForward(1);
               return temp;
            }
            else
            {
                bool value = (Convert.ToBoolean(tokenstream.tokens[tokenstream.Position()].Value));
               Node temp=new Node();
               temp.Type=Node.NodeType.True;
               temp.NodeExpression=value;
               tokenstream.MoveForward(1);
               return temp;
            }
           
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.sin)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Sin;
            temp.Branches=new List<Node>{value};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.cos)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Cos;
            temp.Branches=new List<Node>{value};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.sqrt)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Sqrt;
            temp.Branches=new List<Node>{value};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.exp)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Exp;
            temp.Branches=new List<Node>{value};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.log)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'(' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node base_of_log= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Value!=",")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"',' symbol "));
            }
            else tokenstream.MoveForward(1);
            Node number = ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            else tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Log;
            temp.Branches=new List<Node>{base_of_log,number};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier)
        {
            if (tokenstream.tokens[tokenstream.Position()+1].Tipo==Token.Type.left_bracket)
            {
                string name = tokenstream.tokens[tokenstream.Position()].Value;
                tokenstream.MoveForward(1);
                Node namedfunction=new Node();
                namedfunction.Type=Node.NodeType.Declared_FucName;
                namedfunction.NodeExpression=name;
                tokenstream.MoveForward(1);
                Node parameters =new Node();
                parameters.Type=Node.NodeType.parameters;
                if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
                {
                    do
                    {
                        Node name_of_parm=new Node();
                        name_of_parm.Type=Node.NodeType.ParName;
                        object val = ParseOP();
                        name_of_parm.NodeExpression=val;
                        parameters.Branches.Add(name_of_parm);
                        if (tokenstream.tokens[tokenstream.Position()].Value==",")
                        {
                            tokenstream.MoveForward(1);
                        }
                    } while (tokenstream.tokens[tokenstream.Position()-1].Value==",");  
                }
                if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
                    {
                        errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
                    }
                    else tokenstream.MoveForward(1);
                    Node newvalue = new Node();
                    newvalue.Type=Node.NodeType.Declared_Fuc;
                    newvalue.Branches=new List<Node>{namedfunction,parameters};
                    return newvalue;
            }
            object value =tokenstream.tokens[tokenstream.Position()].Value;
                Node final =new Node();
                final.Type=Node.NodeType.Var;
                final.NodeExpression=value;
                tokenstream.MoveForward(1);
                return final;
        }
        else if (tokenstream.Position()<tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value=="let")
        {
            return Let_In();
        }
        else if(tokenstream.tokens[tokenstream.Position()]==null)
        {
            return new Node();
        }
        else
        {
            Node temp = new Node();
            temp.NodeExpression=new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"expression");
            return temp;
        }
       
    }

    public Node ParsePower()
    {
        Node left =Unit();
        Node pow=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Value=="^")
        {
            
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParsePower();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            
            pow.Type=Node.NodeType.Pow;
            pow.Branches=new List<Node>{left,right};
        }
        if (pow.Type!=Node.NodeType.Indefined)
        {
            return pow;
        }
        else return left;
    }
    public Node ParseMul_O_Div()
    {
        Node left =ParsePower();
        Node pro=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="*"||tokenstream.tokens[tokenstream.Position()].Value=="/"))
        {
            
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParseMul_O_Div();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            
            if (whatkind==Token.Type.multiplication)
            {
               pro.Type=Node.NodeType.Mul; 
            }
            else pro.Type=Node.NodeType.Div;
            
            pro.Branches=new List<Node>{left,right};
            Console.WriteLine(tokenstream.tokens[tokenstream.Position()].Value);

        }
        if (pro.Type!=Node.NodeType.Indefined)
        {
            return pro;
        }
        else return left;
    }
    public Node ParseSum_O_Sub()
    {
        Node left =ParseMul_O_Div();
        Node sus=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="+"||tokenstream.tokens[tokenstream.Position()].Value=="-"))
        {   
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParseSum_O_Sub();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            
            if (whatkind==Token.Type.sum)
            {
               sus.Type=Node.NodeType.Sum; 
               sus.NodeExpression="+";
            }
            else sus.Type=Node.NodeType.Sub;
            sus.NodeExpression="-";
            sus.Branches=new List<Node>{left,right};
        }
        if (sus.Type!=Node.NodeType.Indefined)
        {
            return sus;
        }
        else return left;
    }
    public Node ParseComparation()
    {
        Node left =ParseSum_O_Sub();
        Node com=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="<"||tokenstream.tokens[tokenstream.Position()].Value==">"||tokenstream.tokens[tokenstream.Position()].Value==">="||tokenstream.tokens[tokenstream.Position()].Value=="<="||tokenstream.tokens[tokenstream.Position()].Value=="=="||tokenstream.tokens[tokenstream.Position()].Value=="!="))
        {

            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParseSum_O_Sub();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            if (whatkind==Token.Type.minor)
            {
                com.Type=Node.NodeType.Minor;
            }
            else if (whatkind==Token.Type.major)
            {
                com.Type=Node.NodeType.Major;
            }
            else if (whatkind==Token.Type.equal_major)
            {
                com.Type=Node.NodeType.Equal_Major;
            }
            else if (whatkind==Token.Type.equal_minor)
            {
                com.Type=Node.NodeType.Equal_Minor;
            }
            else if (whatkind==Token.Type.equal)
            {
                com.Type=Node.NodeType.Equal;
            }
            else com.Type=Node.NodeType.Diferent;
            com.Branches=new List<Node>{left,right};
        }
        if (com.Type!=Node.NodeType.Indefined)
        {
            return com;
        }
        else return left;
    }
    public Node ParseOr_O_And()
    {
        Node left =ParseComparation();
        Node and_or=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="|"||tokenstream.tokens[tokenstream.Position()].Value=="&"))
        {

            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParseOr_O_And();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            
            if (whatkind==Token.Type.Or)
            {
                and_or.Type=Node.NodeType.Or;
            }
            else and_or.Type=Node.NodeType.And;
            and_or.Branches=new List<Node>{left,right};
        }
        if (and_or.Type!=Node.NodeType.Indefined)
        {
            return and_or;
        }
        else return left;
    }
    public Node ParseOP()
    { 
        Node left =ParseOr_O_And();
        Node exp=new Node();
        while (tokenstream.Position()<tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Value=="@")
        {   
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Node right =ParseOP();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            
            exp.Type=Node.NodeType.Concat;
            exp.Branches=new List<Node>{left,right};
        }
        if (exp.Type!=Node.NodeType.Indefined)
        {
            return exp;
        }
        else return left;
    }


}