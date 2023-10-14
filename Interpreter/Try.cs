

string codigo = "55+4*(66 - 4)/3^2;";
//string codigo = "5+3*6;";
//string patron = @"\b(print|sin|cos|log|PI|E|let|in|function|if|else|true|false|[\w']+|\S)\b";
 
 Tokenizer lexer =new Tokenizer();
 List<Token> possibletokens = lexer.Tokens(codigo);
 
// foreach (var token in possibletokens)
// {
//     Console.WriteLine(token.ToString());
// }
List<Error> Lexic = lexer.Lexic_Errors();
if(Lexic.Count>0)
{
    foreach (var error in Lexic)
    {
        Console.WriteLine(error.ToString());
    }
}
Parser parse = new Parser(possibletokens);
Node AST=parse.Parse();
   
//Parser pars=new Parser(new TokenStream(possibletokens,0,possibletokens.Count-1));
//Expression ? v = pars.ParseMathExpression(pars.tokenstream);
// v.Evaluate();
// Console.WriteLine(v.Value);
  