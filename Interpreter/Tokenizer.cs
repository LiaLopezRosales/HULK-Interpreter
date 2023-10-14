using System.Text.RegularExpressions;
using System.Globalization;
public class Tokenizer
{

    public static List<Token> Tokens(string code)
    {
        
        List<Error> lexererrors = new List<Error>();
        string patronNumeroNegativo = @"-?\d+(\.\d+)?";
        string patronTexto = "\".*?\"";
        string quotes ="\"";
        string patronPalabras = @"\+|\-|\*|(\<\=)|(\>\=)|(\=\=)|(\=\>)|(\|\|)|(\&\&)|\/|\^|\@|\,|\(|\)|\{|\}|\<|\>|\=|\;|\:";
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
        
        if (lexererrors.Count>0)
        {
            foreach (var error in lexererrors)
            {
                Console.WriteLine(error.ToString());
            }
        }
        return possibletokens;
    }

    //Agregar para sacar tokens sqrt,exp,rand
    public static Token IdentifyType(string possibletoken,List<Error> errors)
    {
        Token token=new Token(Token.Type.not_id,possibletoken);
        if (possibletoken == "print" )
        {
            token = new Token(Token.Type.print,possibletoken);
            //return token;
        }
        else if (possibletoken == "sqrt" )
        {
            token = new Token(Token.Type.sqrt,possibletoken);
            //return token;
        }
        else if (possibletoken == "exp" )
        {
            token = new Token(Token.Type.exp,possibletoken);
            //return token;
        }
        else if (possibletoken == "rand" )
        {
            token = new Token(Token.Type.rand,possibletoken);
            //return token;
        }
        else if (possibletoken == "sin" )
        {
            token = new Token(Token.Type.sin,possibletoken);
            //return token;
        }
        else if (possibletoken == "cos" )
        {
            token = new Token(Token.Type.cos,possibletoken);
            //return token;
        }
        else if (possibletoken == "log" )
        {
            token = new Token(Token.Type.log,possibletoken);
            //return token;
        }
        else if (possibletoken == "PI" )
        {
            token = new Token(Token.Type.PI,possibletoken);
            //return token;
        }
        else if (possibletoken == "E" )
        {
            token = new Token(Token.Type.E,possibletoken);
            //return token;
        }
        else if (possibletoken=="+")
        {
            token = new Token(Token.Type.sum,possibletoken);
            //return token;
        }
        else if (possibletoken=="-")
        {
            token = new Token(Token.Type.substraction,possibletoken);
            //return token;
        }
        else if (possibletoken=="*")
        {
            token = new Token(Token.Type.multiplication,possibletoken);
            //return token;
        }
        else if (possibletoken=="/")
        {
            token = new Token(Token.Type.division,possibletoken);
            //return token;
        }
        else if (possibletoken=="^")
        {
            token = new Token(Token.Type.power,possibletoken);
            //return token;
        }      
        else if (possibletoken == "let" || possibletoken == "in" || possibletoken == "function")
        {
            token = new Token(Token.Type.keyword, possibletoken);
            //return token;
        }
        else if (possibletoken == "if" || possibletoken == "else")
        {
            token = new Token(Token.Type.conditional, possibletoken);
            //return token;
        }
        else if (possibletoken == "," ||  possibletoken == ";" || possibletoken == ":" || possibletoken == "=>" || possibletoken == "=")
        {
            token = new Token(Token.Type.symbol, possibletoken);
            //return token;
        }
        else if (possibletoken=="(")
        {
            token = new Token(Token.Type.left_bracket,possibletoken);
        }
        else if (possibletoken==")")
        {
            token = new Token(Token.Type.right_bracket,possibletoken);
        }
        else if (possibletoken == "@")
        {
            token = new Token(Token.Type.concatenate, possibletoken);
            //return token;
        }
        else if (possibletoken == "==" )
        {
            token = new Token(Token.Type.equal, possibletoken);
            //return token;
        }
        else if (possibletoken == "<" )
        {
            token = new Token(Token.Type.minor, possibletoken);
            //return token;
        }
        else if (possibletoken == ">" )
        {
            token = new Token(Token.Type.major, possibletoken);
            //return token;
        }
        else if (possibletoken == "<=" )
        {
            token = new Token(Token.Type.equal_minor, possibletoken);
            //return token;
        }
        else if (possibletoken == ">=" )
        {
            token = new Token(Token.Type.equal_major, possibletoken);
            //return token;
        }
        else if (possibletoken == "|" )
        {
            token = new Token(Token.Type.Or, possibletoken);
            //return token;
        }
        else if (possibletoken == "&" )
        {
            token = new Token(Token.Type.And, possibletoken);
            //return token;
        }
        else if (possibletoken == "!=" )
        {
            token = new Token(Token.Type.diferent, possibletoken);
            //return token;
        }
        
        else if (possibletoken == "true" || possibletoken == "false")
        {
            token = new Token(Token.Type.boolean, possibletoken);
            //return token;
        }
        else if (double.TryParse(possibletoken, out double result))
        {
            
            //Number token = new Number(Token.Type.number, Convert.ToDouble(possibletoken, CultureInfo.InvariantCulture));
            token = new Token(Token.Type.number,possibletoken);
            //return token;
        }
        else if (possibletoken.StartsWith("\"") && possibletoken.EndsWith("\"")&&possibletoken!="\"")
        {
            token = new Token(Token.Type.text, possibletoken);
            //return token;
        }
        else if(possibletoken=="\"")
        {
          //Console.WriteLine("over here");
          errors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Expected,"\""));
        }
        else
        {
            if (char.IsDigit(possibletoken[0]))
            {
                errors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Invalid,"token, must start with letters"));
                token = new Token(Token.Type.not_id,possibletoken);
                //return token;
            }
            else
            {
            token = new Token(Token.Type.identifier, possibletoken);
            //return token;
            }
        }
        return token;

    }

}
