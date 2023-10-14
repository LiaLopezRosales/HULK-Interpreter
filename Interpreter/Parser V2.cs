using System.Globalization;
public class Parser
{
    List<Token> tokens;
    TokenStream tokenstream;
    Scope scope;

    List<Error>errors;
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
       if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
       {
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
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.identifier || (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier && tokenstream.tokens[tokenstream.Position()+1].Tipo==Token.Type.left_bracket))
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
             //Implement parse for operations

        } while (tokenstream.tokens[tokenstream.Position()].Value==",");
        if (tokenstream.tokens[tokenstream.Position()].Value!="in")
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"'in' keyword"));
        }
        tokenstream.MoveForward(1);
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
            //implement metod to parse whole expression
            Node subnode=ParseExpression();
            if (tokenstream.Position()>=tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
             tokenstream.MoveForward(1);
             return subnode;

        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.number)
        {
            //Check this one
            Number value = new Number(Convert.ToDouble(tokenstream.tokens[tokenstream.Position()].Value,CultureInfo.InvariantCulture));
            Node temp=new Node();
            temp.Type=Node.NodeType.Number;
            temp.NodeExpression=value.Value;
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.text)
        {
           Text value=new Text(tokenstream.tokens[tokenstream.Position()].Value);
           Node temp=new Node();
           temp.Type=Node.NodeType.Text;
           temp.NodeExpression=value.Value;
           return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.PI)
        {
            Node temp=new Node();
            temp.Type=Node.NodeType.PI;
            temp.NodeExpression="PI";
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.E)
        {
            Node temp=new Node();
            temp.Type=Node.NodeType.E;
            temp.NodeExpression="E";
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
            return temp;
        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.boolean)
        {
            if (tokenstream.tokens[tokenstream.Position()].Value=="false")
            {
               Bool value = new Bool(Convert.ToBoolean(tokenstream.tokens[tokenstream.Position()+1].Value));
               Node temp=new Node();
               temp.Type=Node.NodeType.False;
               temp.NodeExpression=value;
               return temp;
            }
            else
            {
                 Bool value = new Bool(Convert.ToBoolean(tokenstream.tokens[tokenstream.Position()+1].Value));
               Node temp=new Node();
               temp.Type=Node.NodeType.True;
               temp.NodeExpression=value;
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
            tokenstream.MoveForward(1);
            //Implement method to parse operations
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            tokenstream.MoveForward(1);
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
            tokenstream.MoveForward(1);
            //Implement method to parse operations
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            tokenstream.MoveForward(1);
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
            tokenstream.MoveForward(1);
            //Implement method to parse operations
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            tokenstream.MoveForward(1);
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
            tokenstream.MoveForward(1);
            //Implement method to parse operations
            Node value= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            tokenstream.MoveForward(1);
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
            tokenstream.MoveForward(1);
            //Implement method to parse operations
            Node base_of_log= ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Value!=",")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"',' symbol"));
            }
            Node number = ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.right_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Expected,"')' symbol"));
            }
            tokenstream.MoveForward(1);
            Node temp=new Node();
            temp.Type=Node.NodeType.Log;
            temp.Branches=new List<Node>{base_of_log,number};
            return temp;

        }
        else if (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier)
        {
            if (tokenstream.tokens[tokenstream.Position()+1].Tipo==Token.Type.left_bracket)
            {
                string name = tokenstream.tokens[tokenstream.Position()+1].Value;
                Node namedfunction=new Node();
                namedfunction.Type=Node.NodeType.Declared_FucName;
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
                    tokenstream.MoveForward(1);
                    Node newvalue = new Node();
                    newvalue.Type=Node.NodeType.Declared_Fuc;
                    newvalue.Branches=new List<Node>{namedfunction,parameters};
                    return newvalue;
            }
            object value =tokenstream.tokens[tokenstream.Position()+1].Value;
                Node final =new Node();
                final.Type=Node.NodeType.Var;
                final.NodeExpression=value;
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
        Node power =Unit();
        while (tokenstream.Position()<tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Value=="^")
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =Unit();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            power=new Node();
            power.Type=(Node.NodeType)whatkind;
            power.Branches=new List<Node>{power,right};
        }
        return power;
    }
    public Node ParseMul_O_Div()
    {
        Node prod =ParsePower();

        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="*"||tokenstream.tokens[tokenstream.Position()].Value=="/"))
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =ParsePower();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            prod=new Node();
            prod.Type=(Node.NodeType)whatkind;
            prod.Branches=new List<Node>{prod,right};
        }
        return prod;
    }
    public Node ParseSum_O_Sub()
    {
        Node su =ParseMul_O_Div();

        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="+"||tokenstream.tokens[tokenstream.Position()].Value=="-"))
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =ParseMul_O_Div();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            su=new Node();
            su.Type=(Node.NodeType)whatkind;
            su.Branches=new List<Node>{su,right};
        }
        return su;
    }
    public Node ParseComparation()
    {
        Node com =ParseSum_O_Sub();

        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="<"||tokenstream.tokens[tokenstream.Position()].Value==">"||tokenstream.tokens[tokenstream.Position()].Value==">="||tokenstream.tokens[tokenstream.Position()].Value=="<="||tokenstream.tokens[tokenstream.Position()].Value=="=="||tokenstream.tokens[tokenstream.Position()].Value=="!="))
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =ParseSum_O_Sub();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            com=new Node();
            com.Type=(Node.NodeType)whatkind;
            com.Branches=new List<Node>{com,right};
        }
        return com;
    }
    public Node ParseOr_O_And()
    {
        Node and_or =ParseComparation();

        while (tokenstream.Position()<tokenstream.tokens.Count&&(tokenstream.tokens[tokenstream.Position()].Value=="|"||tokenstream.tokens[tokenstream.Position()].Value=="&"))
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =ParseComparation();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            and_or=new Node();
            and_or.Type=(Node.NodeType)whatkind;
            and_or.Branches=new List<Node>{and_or,right};
        }
        return and_or;
    }
    public Node ParseOP()
    {
        Node exp =ParseOr_O_And();
        while (tokenstream.Position()<tokenstream.tokens.Count&&tokenstream.tokens[tokenstream.Position()].Value=="@")
        {
            Token.Type whatkind = tokenstream.tokens[tokenstream.Position()].Tipo;
            Node right =ParseOr_O_And();
            if (right.NodeExpression is Error)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error,Error.ErrorCode.Invalid,"expression"));
            }
            exp=new Node();
            exp.Type=(Node.NodeType)whatkind;
            exp.Branches=new List<Node>{exp,right};
        }
        return exp;
    }


}