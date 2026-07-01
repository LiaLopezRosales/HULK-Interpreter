using System.Text.RegularExpressions;
using System.Globalization;
public class Tokenizer
{
    //Lista que recoge los errores encontrados en la etapa léxica
    public List<Error> lexererrors;

    public Tokenizer()
    {
        lexererrors = new List<Error>();
    }
    //Método que genera la lista de tokens que se utiliza en las siguientes etapas
    public List<Token> Tokens(string code)
    {
        //Se utilizan expresiones regulares para separar en posibles tokenes
        //Identifica como número negativo si el símbolo '-' está justo antes del número sin espacios entre ellos

        // Eliminar comentarios de bloque (/* ... */)
        code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
        // Eliminar comentarios de una linea (// ...)
        code = Regex.Replace(code, @"//.*", "");

        string patronNumeroNegativo = @"-?\d+(\.\d+)?";
        string patronTexto = "\".*?\"";
        string quotes ="\"";
        string patronPalabras = @"\+|\-|\*|\%|(\<\=)|(\>\=)|(\=\=)|(\!\=)|(\=\>)|(\:\=)|(\|)|(\&)|\/|\^|(\!)|(\@\@)|\@|\,|\(|\)|\{|\}|\[|\]|\<|\>|\=|\;|\:";
        string patronIdentificador = @"\b\w*[a-zA-Z]\w*\b";
        string patron = $"{patronTexto}|{quotes}|{patronIdentificador}|{patronNumeroNegativo}|{patronPalabras}";
        MatchCollection matches = Regex.Matches(code, patron);
        List<Token> possibletokens = new List<Token>();
        //Cada coincidencia obtenida se identifica y se añade su token correspondiente
        foreach (Match match in matches)
        {
            Token temporal = IdentifyType(match.Value,lexererrors);
            temporal.SourceIndex = match.Index;
            possibletokens.Add(temporal);
        }
        // Helper para agregar error lexico con posicion
        void AddLexError(int index, string msg)
        {
            var err = new Error(Error.TypeError.Lexical_Error, Error.ErrorCode.Invalid, msg);
            (err.Line, err.Col) = PosFromIndex(code, index);
            lexererrors.Add(err);
        }

        // verificar si hay caracteres no reconocidos entre matches
        int lastEnd = 0;
        foreach (Match m in matches)
        {
            if (m.Index > lastEnd)
            {
                string junk = code.Substring(lastEnd, m.Index - lastEnd).Trim();
                if (junk.Length > 0)
                    AddLexError(lastEnd, $"token no reconocido: '{junk}'");
            }
            lastEnd = m.Index + m.Length;
        }
        // saldo final
        string trailing = code.Substring(lastEnd).Trim();
        if (trailing.Length > 0)
            AddLexError(lastEnd, $"token no reconocido: '{trailing}'");

        possibletokens.Add(new Token(Token.Type.EOL, "EOL"));

        if (possibletokens.Count <= 2)
        {
            lexererrors.Add(new Error(Error.TypeError.Lexical_Error, Error.ErrorCode.Invalid, "expression"));
        }

        return possibletokens;
    }

    public static (int Line, int Col) PosFromIndex(string source, int index)
    {
        int line = 1, col = 1;
        for (int i = 0; i < index && i < source.Length; i++)
        {
            if (source[i] == '\n') { line++; col = 1; }
            else col++;
        }
        return (line, col);
    }

    public List<Error> Lexic_Errors()
    {
        return lexererrors;
    }
    
    //Método que dado un string identifica que tipo de token sería y devuelve este
    public static Token IdentifyType(string possibletoken,List<Error> errors)
    {
        //Si token nunca cambia su valor es un token inválido
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
        else if (possibletoken=="%")
        {
            token = new Token(Token.Type.module,possibletoken);
            
        }
        else if (possibletoken=="^")
        {
            token = new Token(Token.Type.power,possibletoken);
    
        }      
        else if (possibletoken == "let" || possibletoken == "in" || possibletoken == "function" || possibletoken == "while" || possibletoken == "for" || possibletoken == "range" || possibletoken == "break" || possibletoken == "continue")
        {
            token = new Token(Token.Type.keyword, possibletoken);
            
        }
        else if (possibletoken == "if" || possibletoken == "else" || possibletoken == "elif")
        {
            token = new Token(Token.Type.conditional, possibletoken);
            
        }
        else if (possibletoken == "!=" )
        {
            token = new Token(Token.Type.diferent, possibletoken);
            
        }
        else if (possibletoken == "," ||  possibletoken == ";" || possibletoken == ":" || possibletoken == "=>" || possibletoken == "=" || possibletoken == "{" || possibletoken == "}" || possibletoken == "[" || possibletoken == "]")
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
else if (possibletoken == "@@")
            {
                token = new Token(Token.Type.concatenate, possibletoken);
            }
            else if (possibletoken == "@")
            {
                token = new Token(Token.Type.concatenate, possibletoken);
                
            }
        else if (possibletoken == ":=")
            {
                token = new Token(Token.Type.assign, possibletoken);
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
        //Se trata de convertir el string en número, si la conversión es exitosa se genera el token
        else if (double.TryParse(possibletoken, out double result))
        {
            token = new Token(Token.Type.number,possibletoken);
            
        }
        //Se comprueba si el string está contenido entre comillas
        else if (possibletoken.StartsWith("\"") && possibletoken.EndsWith("\"")&&possibletoken!="\"")
        {
            token = new Token(Token.Type.text, possibletoken);
            
        }
        //Si el string es comilla sola se genera un error
        else if(possibletoken=="\"")
        {
          
          errors.Add(new Error(Error.TypeError.Lexical_Error,Error.ErrorCode.Expected,"\""));
        }
        else
        {
        //En este punto el token solo puede ser un identificador así que se comprueba la validez del nombre 
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
