
AST_Evaluator Evaluator = new AST_Evaluator();
string code = "start/";
Tokenizer lexer = new Tokenizer();
while (true)
{
    Console.Write(">");
    code = Console.ReadLine()!;
    if (code == "")
    {
        Console.WriteLine("Closing Hulk_Interpreter");
        break;
    }
    List<Token> possibletokens = lexer.Tokens(code);
    List<Error> Lexicon = lexer.Lexic_Errors();
    if (Lexicon.Count > 0)
    {
        Console.WriteLine("Invalid Hulk expression,please correct the following lexical errors");
        foreach (var error in Lexicon)
        {
            Console.WriteLine(error.ToString());
        }
        lexer.lexererrors.Clear();
        continue;
    }
    else
    {
        Parser parse = new Parser(possibletokens);
        Node AST = parse.Parse();
        List<Error> Syntactic = parse.Syntactic_Errors();
        if (Syntactic.Count > 0)
        {
            Console.WriteLine("Invalid Hulk expression,please correct the following syntax errors");
            foreach (var error in Syntactic)
            {
                Console.WriteLine(error.ToString());
            }
            parse.errors.Clear();
            continue;
        }
        else
        {
            Evaluator.Tree_Reader(AST);
            object result = Evaluator.StartEvaluation(AST);
            List<Error> Semantic = Evaluator.Semanti_Errors();
            if (Semantic.Count > 0)
            {
                Console.WriteLine("Invalid Hulk expression,please correct the following semantic errors");
                foreach (var error in Semantic)
                {
                    Console.WriteLine(error.ToString());
                }
                if (AST.Type==Node.NodeType.Fuction)
                {
                    break;
                }
                Evaluator.Semantic_Errors.Clear();
                continue;
            }
            else
            {
                Console.WriteLine(result);
            }
        }
    }
}
static void SubNodes(Node node, int i)
{
    if (node.Branches.Count > 0)
    {
        foreach (var subnode in node.Branches)
        {
            Console.WriteLine($"{subnode.Type} {i} type");
            Console.WriteLine($"{subnode.NodeExpression} {i} expression");
            
            SubNodes(subnode, i + 1);
        }
    }
    else return;
}


