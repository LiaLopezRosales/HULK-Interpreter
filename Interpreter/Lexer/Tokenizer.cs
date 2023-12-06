using System.Text.RegularExpressions;
using System.Globalization;
public class Tokenizer
{
    public List<Error> lexererrors;

    public Tokenizer()
    {
        lexererrors = new List<Error>();
    }
    public List<Token> Tokens(string code)
    {
        
        string patronNumeroNegativo = @"-?\d+(\.\d+)?";
        string patronTexto = "\".*?\"";
        string quotes ="\"";
        string patronPalabras = @"\+|\-|\*|(\<\=)|(\>\=)|(\=\=)|(\!\=)|(\=\>)|(\|)|(\&)|\/|\^|(\!)|\@|\,|\(|\)|\{|\}|\<|\>|\=|\;|\:";
        string patronIdentificador = @"\b\w*[a-zA-Z]\w*\b";
        string patron = $"{patronTexto}|{quotes}|{patronIdentificador}|{patronNumeroNegativo}|{patronPalabras} ";
        MatchCollection matches = Regex.Matches(code, patron);
        List<Token> possibletokens = new List<Token>();
        foreach (Match match in matches)
        {
            Token temporal = IdentifyType(match.Value,lexererrors);
            possibletokens.Add(temporal);
        }
        possibletokens.Add(new Token(Token.Type.EOL, "EOL"));
        if (possibletokens.Count>2)
        {
            Token validexpression = possibletokens[possibletokens.Count-2];
            if (validexpression.Value!=";")
            {
                lexererrors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Expected,";"));
            }  

        }
        else
        {
            lexererrors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Invalid,"expression"));
        }

        return possibletokens;
    }

    public List<Error> Lexic_Errors()
    {
        return lexererrors;
    }

    public static Token IdentifyType(string possibletoken,List<Error> errors)
    {
        Token token=new Token(Token.Type.not_id,possibletoken);
        if (possibletoken == "print" )
        {
            token = new Token(Token.Type.print,possibletoken);
        }
        else if (possibletoken == "sqrt" )
        {
            token = new Token(Token.Type.sqrt,possibletoken);
            
        }
        else if (possibletoken == "exp" )
        {
            token = new Token(Token.Type.exp,possibletoken);
    
        }
        else if (possibletoken == "rand" )
        {
            token = new Token(Token.Type.rand,possibletoken);

        }
        else if (possibletoken == "sin" )
        {
            token = new Token(Token.Type.sin,possibletoken);

        }
        else if (possibletoken == "cos" )
        {
            token = new Token(Token.Type.cos,possibletoken);
        
        }
        else if (possibletoken == "log" )
        {
            token = new Token(Token.Type.log,possibletoken);
            
        }
        else if (possibletoken == "PI" )
        {
            token = new Token(Token.Type.PI,possibletoken);
        
        }
        else if (possibletoken == "E" )
        {
            token = new Token(Token.Type.E,possibletoken);
            
        }
        else if (possibletoken=="+")
        {
            token = new Token(Token.Type.sum,possibletoken);
            
        }
        else if (possibletoken=="-")
        {
            token = new Token(Token.Type.substraction,possibletoken);
            
        }
        else if (possibletoken=="*")
        {
            token = new Token(Token.Type.multiplication,possibletoken);
            
        }
        else if (possibletoken=="/")
        {
            token = new Token(Token.Type.division,possibletoken);
            
        }
        else if (possibletoken=="^")
        {
            token = new Token(Token.Type.power,possibletoken);
    
        }      
        else if (possibletoken == "let" || possibletoken == "in" || possibletoken == "function")
        {
            token = new Token(Token.Type.keyword, possibletoken);
            
        }
        else if (possibletoken == "if" || possibletoken == "else")
        {
            token = new Token(Token.Type.conditional, possibletoken);
            
        }
        else if (possibletoken == "!=" )
        {
            token = new Token(Token.Type.diferent, possibletoken);
            
        }
        else if (possibletoken == "," ||  possibletoken == ";" || possibletoken == ":" || possibletoken == "=>" || possibletoken == "=")
        {
            token = new Token(Token.Type.symbol, possibletoken);
            
        }
        else if (possibletoken=="(")
        {
            token = new Token(Token.Type.left_bracket,possibletoken);
        }
        else if (possibletoken==")")
        {
            token = new Token(Token.Type.right_bracket,possibletoken);
        }
        else if (possibletoken=="!")
        {
            token = new Token(Token.Type.not,possibletoken);
        }
        else if (possibletoken == "@")
        {
            token = new Token(Token.Type.concatenate, possibletoken);
            
        }
        else if (possibletoken == "==" )
        {
            token = new Token(Token.Type.equal, possibletoken);
            
        }
        else if (possibletoken == "<" )
        {
            token = new Token(Token.Type.minor, possibletoken);
            
        }
        else if (possibletoken == ">" )
        {
            token = new Token(Token.Type.major, possibletoken);
        
        }
        else if (possibletoken == "<=" )
        {
            token = new Token(Token.Type.equal_minor, possibletoken);
            
        }
        else if (possibletoken == ">=" )
        {
            token = new Token(Token.Type.equal_major, possibletoken);
            
        }
        else if (possibletoken == "|" )
        {
            token = new Token(Token.Type.Or, possibletoken);

        }
        else if (possibletoken == "&" )
        {
            token = new Token(Token.Type.And, possibletoken);
            
        } 
        else if (possibletoken == "true" || possibletoken == "false")
        {
            token = new Token(Token.Type.boolean, possibletoken);
            
        }
        else if (double.TryParse(possibletoken, out double result))
        {
            token = new Token(Token.Type.number,possibletoken);
            
        }
        else if (possibletoken.StartsWith("\"") && possibletoken.EndsWith("\"")&&possibletoken!="\"")
        {
            token = new Token(Token.Type.text, possibletoken);
            
        }
        else if(possibletoken=="\"")
        {
          
          errors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Expected,"\""));
        }
        else
        {
            if (char.IsDigit(possibletoken[0]))
            {
                errors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Invalid,"token, must start with letters"));
                token = new Token(Token.Type.not_id,possibletoken);
                
            }
            else
            {
            token = new Token(Token.Type.identifier, possibletoken);
            
            }
        }
        return token;

    }

}
