using System.Globalization;

bool showAst = false;
bool showTokens = false;
string? filePath = null;

foreach (string arg in args)
{
    if (arg == "--ast") showAst = true;
    else if (arg == "--tokens") showTokens = true;
    else if (!arg.StartsWith("--")) filePath = arg;
}

AST_Evaluator evaluator = new AST_Evaluator();
Tokenizer lexer = new Tokenizer();

void WriteColored(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

void WriteLineColored(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}

string ReadMultiline()
{
    string input = Console.ReadLine() ?? "";
    while (input.EndsWith("\\"))
    {
        input = input[..^1] + "\n" + (Console.ReadLine() ?? "");
    }
    return input;
}

bool ProcessCode(string code)
{
    List<Token> possibletokens = lexer.Tokens(code);
    List<Error> lexicon = lexer.Lexic_Errors();

    if (showTokens)
    {
        WriteLineColored("── Tokens ──────────────────────", ConsoleColor.DarkGray);
        foreach (var t in possibletokens)
            Console.WriteLine($"  {t}");
    }

    if (lexicon.Count > 0)
    {
        WriteLineColored("Errores lexicos:", ConsoleColor.Red);
        foreach (var error in lexicon)
            Console.WriteLine($"  {error}");
        lexer.lexererrors.Clear();
        return false;
    }

    Parser parse = new Parser(possibletokens, code);
    Expression ast = parse.Parse();

    if (showAst)
    {
        WriteLineColored("── AST ──────────────────────────", ConsoleColor.DarkGray);
        PrintAst(ast, "");
    }

    List<Error> syntactic = parse.Syntactic_Errors();
    if (syntactic.Count > 0)
    {
        WriteLineColored("Errores sintacticos:", ConsoleColor.Red);
        foreach (var error in syntactic)
            Console.WriteLine($"  {error}");
        parse.errors.Clear();
        return false;
    }

    object result = evaluator.Evaluate(ast);

    if (evaluator.Semantic_Errors.Count > 0)
    {
        WriteLineColored("Errores semanticos:", ConsoleColor.Red);
        foreach (var error in evaluator.Semantic_Errors)
            Console.WriteLine($"  {error}");
        evaluator.Semantic_Errors.Clear();
        return false;
    }

    WriteColored("=> ", ConsoleColor.Green);
    Console.WriteLine(result);
    return true;
}

void PrintAst(Expression expr, string indent)
{
    string nodeName = expr.GetType().Name;
    Console.Write(indent + nodeName);
    if (expr is LiteralExpression lit) Console.Write(" = " + lit.Value);
    else if (expr is VariableExpression varE) Console.Write(" '" + varE.Name + "'");
    else if (expr is FunctionDefinition fd) Console.Write(" '" + fd.Name + "'");
    else if (expr is FunctionCall fc) Console.Write(" '" + fc.Name + "'");
    else if (expr is BuiltinCall bc) Console.Write(" '" + bc.FunctionName + "'");
    else if (expr is AssignmentExpression ae) Console.Write(" '" + ae.VariableName + "'");
    Console.WriteLine();
    foreach (var child in GetChildren(expr))
        PrintAst(child, indent + "  ");
}

List<Expression> GetChildren(Expression expr)
{
    var children = new List<Expression>();
    if (expr is BinaryExpression bin) { children.Add(bin.Left); children.Add(bin.Right); }
    else if (expr is UnaryExpression un) children.Add(un.Operand);
    else if (expr is ConditionalExpression cond) { children.Add(cond.Condition); children.Add(cond.IfBranch); if (cond.ElseBranch != null) children.Add(cond.ElseBranch); }
    else if (expr is LetExpression let) { foreach (var (_, v) in let.Assignments) children.Add(v); children.Add(let.Body); }
    else if (expr is FunctionDefinition fd) children.Add(fd.Body);
    else if (expr is FunctionCall fc) children.AddRange(fc.Arguments);
    else if (expr is BuiltinCall bc) children.AddRange(bc.Arguments);
    else if (expr is PrintExpression pr) children.Add(pr.Argument);
    else if (expr is WhileExpression wh) { children.Add(wh.Condition); children.Add(wh.Body); }
    else if (expr is BlockExpression bl) children.AddRange(bl.Expressions);
    else if (expr is AssignmentExpression ae) children.Add(ae.Value);
    else if (expr is ListExpression li) children.AddRange(li.Elements);
    return children;
}

// ─── File mode ──────────────────────────────────

if (filePath != null)
{
    if (!File.Exists(filePath))
    {
        WriteLineColored($"Error: archivo '{filePath}' no encontrado", ConsoleColor.Red);
        return;
    }
    string content = File.ReadAllText(filePath);
    if (content.Length > 0)
        ProcessCode(content);
    return;
}

// ─── REPL ───────────────────────────────────────

WriteLineColored("HULK Interpreter", ConsoleColor.Cyan);
WriteLineColored("Escribe una expresion o presiona Enter para salir.", ConsoleColor.DarkGray);

while (true)
{
    Console.Write("> ");
    string code = ReadMultiline();
    if (code == "")
    {
        WriteLineColored("Cerrando HULK Interpreter", ConsoleColor.Cyan);
        break;
    }

    ProcessCode(code);
}
