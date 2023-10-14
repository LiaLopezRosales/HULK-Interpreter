

string codigo = "55+4;";
//string codigo = "5+3*6;";
//string patron = @"\b(print|sin|cos|log|PI|E|let|in|function|if|else|true|false|[\w']+|\S)\b";
 
 List<Token> possibletokens = Tokenizer.Tokens(codigo);
 
// for (int j = 0; j < possibletokens.Length; j++)
// {
//     Console.WriteLine(possibletokens[j].ToString());
// }
//Parser pars=new Parser(new TokenStream(possibletokens,0,possibletokens.Count-1));
//Expression ? v = pars.ParseMathExpression(pars.tokenstream);
// v.Evaluate();
// Console.WriteLine(v.Value);
  