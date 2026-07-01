AST_Evaluator Evaluator = new AST_Evaluator();
Tokenizer lexer = new Tokenizer();

while (true)
{
    Console.Write(">");
    string code = Console.ReadLine()!;
    if (code == "")
    {
        Console.WriteLine("Closing Hulk_Interpreter");
        break;
    }

    List<Token> possibletokens = lexer.Tokens(code);
    List<Error> Lexicon = lexer.Lexic_Errors();

    if (Lexicon.Count > 0)
    {
        Console.WriteLine("Invalid Hulk expression, please correct the following lexical errors");
        foreach (var error in Lexicon)
            Console.WriteLine(error.ToString());
        lexer.lexererrors.Clear();
        continue;
    }

    Parser parse = new Parser(possibletokens);
    Expression AST = parse.Parse();

    List<Error> Syntactic = parse.Syntactic_Errors();
    if (Syntactic.Count > 0)
    {
        Console.WriteLine("Invalid Hulk expression, please correct the following syntax errors");
        foreach (var error in Syntactic)
            Console.WriteLine(error.ToString());
        parse.errors.Clear();
        continue;
    }

    object result = Evaluator.Evaluate(AST);

    if (Evaluator.Semantic_Errors.Count > 0)
    {
        Console.WriteLine("Invalid Hulk expression, please correct the following semantic errors");
        foreach (var error in Evaluator.Semantic_Errors)
            Console.WriteLine(error.ToString());
        Evaluator.Semantic_Errors.Clear();
        continue;
    }

    Console.WriteLine(result);
}
