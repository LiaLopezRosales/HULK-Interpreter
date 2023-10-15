
//Basic if-else working,variable and functions evaluation not completed,I think basic let-in parse correct, need to implement something to find most external operator for aritmetic expression
AST_Evaluator Semantic=new AST_Evaluator();
//Revisar evaluacion de booleanos entre textos(!= siempre devuelve verdadero y == devuelve false)
string codigo = "if(4+5>8 - 6)print(3)else 10*2;";
//string codigo = "5+4*3 - 2+4/2 + -5*3;";
//string codigo = "5+4*3*2^(2+1) - 6 / 3;";
//string codigo = "5+3*6;";
//string patron = @"\b(print|sin|cos|log|PI|E|let|in|function|if|else|true|false|[\w']+|\S)\b";
 
 Tokenizer lexer =new Tokenizer();
 List<Token> possibletokens = lexer.Tokens(codigo);
 Console.WriteLine(Math.Cos(0));
foreach (var token in possibletokens)
{
    Console.WriteLine(token.ToString());
}
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
Console.WriteLine(AST.Type);
int i =0;
// foreach (var node in AST.Branches)
// {
//     Console.WriteLine(node.Type.ToString()+i);
//     foreach (var subnode in node.Branches)
//     {
//         Console.WriteLine(subnode.Type.ToString()+i);
//     }
//     i++;
// }
SubNodes(AST,0);
List<Error> Syntac = parse.Syntactic_Errors();
if(Syntac.Count>0)
{
    foreach (var error in Syntac)
    {
        Console.WriteLine(error.ToString());
    }
}
Semantic.Tree_Reader(AST);
Semantic.StartEvaluation(AST);
List<Error> Semant = Semantic.Semanti_Errors();
if(Semant.Count>0)
{
    foreach (var error in Semant)
    {
        Console.WriteLine(error.ToString());
    }
}
   
//Parser pars=new Parser(new TokenStream(possibletokens,0,possibletokens.Count-1));
//Expression ? v = pars.ParseMathExpression(pars.tokenstream);
// v.Evaluate();
// Console.WriteLine(v.Value);

 static void SubNodes(Node node,int i)
{
   if (node.Branches.Count>0)
   {
     foreach (var subnode in node.Branches)
     {
        Console.WriteLine($"{subnode.Type} {i}");
        Console.WriteLine($"{subnode.NodeExpression} {i}");
        SubNodes(subnode,i+1);
     }
   }
   else return;
}


  