//Se crean los objetos encargados de generar la lista de tokens y evaluar el árbol correspondiente a esta
AST_Evaluator Evaluator = new AST_Evaluator();
string code = "start/";
Tokenizer lexer = new Tokenizer();
//Se crea un ciclo que acaba al recibir una expresión vacía
while (true)
{
    Console.Write(">");
    code = Console.ReadLine()!;
    if (code == "")
    {
        Console.WriteLine("Closing Hulk_Interpreter");
        break;
    }
    //Se recolecta la lista de tokens y se comprueba si se encontraron errores léxicos
    List<Token> possibletokens = lexer.Tokens(code);
    List<Error> Lexicon = lexer.Lexic_Errors();
    //Si existen errores léxicos se muestran y se avanza a la siguiente iteración en espera de una expresión válida
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
        //De no haber errores léxicos se avanza al análisis sintáctico y creación del árbol
        Parser parse = new Parser(possibletokens);
        Node AST = parse.Parse();
        //Se realiza un proceso similar al de los errores léxicos con los sintácticos
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
            //Si no se encuentran errores hasta el método se procede al último paso  evaluar la expresión
            Evaluator.Tree_Reader(AST);
            object result = Evaluator.StartEvaluation(AST);
            List<Error> Semantic = Evaluator.Semanti_Errors();
            //Mismo proceso con los errores semánticos
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
                //Se imprime el resultado de evaluar la expresión
                if (AST.Type==Node.NodeType.Fuction)
                {
                    Console.WriteLine("Función añadida");
                }
                else  Console.WriteLine(result);
               
            }
        }
    }
}
//Método auxiliar para visualizar el contenido del árbol
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


