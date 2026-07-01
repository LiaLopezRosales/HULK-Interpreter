using System.Globalization;

public class Parser
{
    private TokenStream tokenstream;
    private List<Token> tokens;
    private string sourceCode;

    public List<Error> errors;

    public Parser(List<Token> tokens_expression, string sourceCode = "")
    {
        tokenstream = new TokenStream(tokens_expression);
        tokens = tokens_expression;
        this.sourceCode = sourceCode;
        errors = new List<Error>();
    }

    private void AddError(Error.TypeError type, Error.ErrorCode code, string msg)
    {
        var err = new Error(type, code, msg);
        if (tokenstream.Position() < tokens.Count)
        {
            int idx = tokens[tokenstream.Position()].SourceIndex;
            if (idx >= 0)
                (err.Line, err.Col) = Tokenizer.PosFromIndex(sourceCode, idx);
        }
        errors.Add(err);
    }

    public Expression Parse()
    {
        List<Expression> exprs = new List<Expression>();
        exprs.Add(ParseExpression());
        while (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == ";")
        {
            tokenstream.MoveForward(1);
            if (tokenstream.Position() < tokens.Count &&
                tokens[tokenstream.Position()].Tipo != Token.Type.EOL)
                exprs.Add(ParseExpression());
        }
        return exprs.Count == 1 ? exprs[0] : new BlockExpression(exprs);
    }

    public List<Error> Syntactic_Errors()
    {
        return errors;
    }

    public Expression ParseExpression()
    {
        Expression left;

        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Tipo == Token.Type.print)
            return Print();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "let")
            return Let_In();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "if")
            return IF_ElSE();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "function")
            return Function();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "while")
            return While();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "for")
            return ForLoop();
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "break")
        {
            tokenstream.MoveForward(1);
            return new BreakExpression();
        }
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "continue")
        {
            tokenstream.MoveForward(1);
            return new ContinueExpression();
        }
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokens[tokenstream.Position()].Value == "{")
            return Block();

        left = ParseOP();

        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Tipo == Token.Type.assign)
        {
            if (left is VariableExpression varExpr)
            {
                tokenstream.MoveForward(1);
                Expression right = ParseExpression();
                return new AssignmentExpression(varExpr.Name, right);
            }
            else
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Invalid, "asignación inválida"));
            }
        }

        return left;
    }

    public Expression Print()
    {
        tokenstream.MoveForward(1);
        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression argument = ParseExpression();

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
        else
            tokenstream.MoveForward(1);

        return new PrintExpression(argument);
    }

    public Expression IF_ElSE()
    {
        tokenstream.MoveForward(1);

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression condition = ParseExpression();

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression ifPart = ParseExpression();

        Expression? elsePart = null;
        if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "else")
        {
            tokenstream.MoveForward(1);
            elsePart = ParseExpression();
        }
        else if (tokenstream.Position() < tokens.Count && tokens[tokenstream.Position()].Value == "elif")
        {
            elsePart = IF_ElSE();
        }

        return new ConditionalExpression(condition, ifPart, elsePart);
    }

    public Expression Function()
    {
        tokenstream.MoveForward(1);

        List<string> parameters = new List<string>();

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.identifier)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "function name"));
        else
            tokenstream.MoveForward(1);

        string name = tokenstream.tokens[tokenstream.Position() - 1].Value;

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol"));
        else
            tokenstream.MoveForward(1);

        while (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.identifier)
        {
            parameters.Add(tokenstream.tokens[tokenstream.Position()].Value);
            tokenstream.MoveForward(1);
            if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ",")
                tokenstream.MoveForward(1);
        }

        if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
        else
            tokenstream.MoveForward(1);

        // Check for expression block body { }
        if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokenstream.tokens[tokenstream.Position()].Value == "{")
        {
            Expression body = Block();
            return new FunctionDefinition(name, parameters, body);
        }

        if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value != "=>")
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'=>' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression bodyExpr = ParseExpression();
        return new FunctionDefinition(name, parameters, bodyExpr);
    }

    public Expression While()
    {
        tokenstream.MoveForward(1);

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression condition = ParseExpression();

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression body = ParseExpression();

        return new WhileExpression(condition, body);
    }

    public Expression ForLoop()
    {
        tokenstream.MoveForward(1);

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol"));
        else
            tokenstream.MoveForward(1);

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.identifier)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "variable identifier"));
        else
            tokenstream.MoveForward(1);

        string varName = tokenstream.tokens[tokenstream.Position() - 1].Value;

        if (tokenstream.tokens[tokenstream.Position()].Value != "in")
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'in' keyword"));
        else
            tokenstream.MoveForward(1);

        Expression iterable = ParseExpression();

        if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
        else
            tokenstream.MoveForward(1);

        Expression body = ParseExpression();

        return new ForExpression(varName, iterable, body);
    }

    public Expression Block()
    {
        tokenstream.MoveForward(1);
        List<Expression> expressions = new List<Expression>();

        while (tokenstream.Position() < tokens.Count &&
               tokens[tokenstream.Position()].Tipo != Token.Type.EOL &&
               !(tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokenstream.tokens[tokenstream.Position()].Value == "}"))
        {
            Expression e = ParseExpression();
            expressions.Add(e);
            if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ";")
                tokenstream.MoveForward(1);
        }

        if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "}")
            tokenstream.MoveForward(1);

        return new BlockExpression(expressions);
    }

    public Expression ListLiteral()
    {
        tokenstream.MoveForward(1);
        List<Expression> elements = new List<Expression>();

        if (tokenstream.Position() < tokens.Count &&
            !(tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokenstream.tokens[tokenstream.Position()].Value == "]"))
        {
            elements.Add(ParseExpression());
            while (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ",")
            {
                tokenstream.MoveForward(1);
                elements.Add(ParseExpression());
            }
        }

        if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "]")
            tokenstream.MoveForward(1);
        else
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "']' symbol"));

        return new ListExpression(elements);
    }

    public Expression Let_In()
    {
        tokenstream.MoveForward(1);
        List<(string, Expression)> assignments = new List<(string, Expression)>();

        bool first = true;
        while (tokenstream.Position() < tokens.Count)
        {
            if (!first)
                tokenstream.MoveForward(1);
            first = false;

            if (tokenstream.Position() >= tokens.Count ||
                (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.identifier))
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "variable identifier"));
                break;
            }

            string varName = tokenstream.tokens[tokenstream.Position()].Value;

            // Check if it's a function call, not a variable
            if (tokenstream.Position() + 1 < tokens.Count && tokenstream.tokens[tokenstream.Position() + 1].Tipo == Token.Type.left_bracket)
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "variable identifier"));
                break;
            }

            tokenstream.MoveForward(1);

            if (tokenstream.tokens[tokenstream.Position()].Value != "=")
            {
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'=' symbol"));
                break;
            }
            tokenstream.MoveForward(1);

            Expression value = ParseExpression();

            assignments.Add((varName, value));

            if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ",")
                continue;
            else
                break;
        }

        if (tokenstream.Position() >= tokens.Count || tokenstream.tokens[tokenstream.Position()].Value != "in")
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'in' keyword"));
        }
        else
            tokenstream.MoveForward(1);

        Expression body = ParseExpression();
        return new LetExpression(assignments, body);
    }

    public Expression Unit()
    {
        if (tokenstream.Position() >= tokenstream.tokens.Count)
        {
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "more tokens, end of expression"));
            return new LiteralExpression(0.0);
        }

        Token current = tokenstream.tokens[tokenstream.Position()];

        if (current.Tipo == Token.Type.left_bracket)
        {
            tokenstream.MoveForward(1);
            Expression subnode = ParseExpression();
            if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
            else
                tokenstream.MoveForward(1);
            return subnode;
        }

        if (current.Value == "!")
        {
            tokenstream.MoveForward(1);
            return new UnaryExpression(ParseExpression());
        }

        if (current.Tipo == Token.Type.substraction)
        {
            tokenstream.MoveForward(1);
            Expression operand = Unit();
            return new Subtraction(new LiteralExpression(0.0), operand);
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.number)
        {
            double value = Convert.ToDouble(tokenstream.tokens[tokenstream.Position()].Value, CultureInfo.InvariantCulture);
            tokenstream.MoveForward(1);
            return new LiteralExpression(value);
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.text)
        {
            string value = tokenstream.tokens[tokenstream.Position()].Value;
            tokenstream.MoveForward(1);
            return new LiteralExpression(value);
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.PI)
        {
            tokenstream.MoveForward(1);
            return new BuiltinCall("PI", new List<Expression>());
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.E)
        {
            tokenstream.MoveForward(1);
            return new BuiltinCall("E", new List<Expression>());
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.rand)
        {
            tokenstream.MoveForward(1);
            if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.left_bracket)
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "'(' symbol after rand function"));
            else
                tokenstream.MoveForward(1);

            if (tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol to close rand function"));
            else
                tokenstream.MoveForward(1);

            return new BuiltinCall("rand", new List<Expression>());
        }

        if (tokenstream.tokens[tokenstream.Position()].Value == "range")
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression start = ParseExpression();
            if (tokenstream.Position() >= tokenstream.tokens.Count || tokenstream.tokens[tokenstream.Position()].Value != ",")
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "',' symbol"));
            else
                tokenstream.MoveForward(1);
            Expression end = ParseExpression();

            List<Expression> rangeArgs = new List<Expression> { start, end };
            if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ",")
            {
                tokenstream.MoveForward(1);
                rangeArgs.Add(ParseExpression());
            }

            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("range", rangeArgs);
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.boolean)
        {
            bool value = tokenstream.tokens[tokenstream.Position()].Value == "true";
            tokenstream.MoveForward(1);
            return new LiteralExpression(value);
        }

        // Built-in functions with one argument
        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.sin)
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression arg = ParseOP();
            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("sin", new List<Expression> { arg });
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.cos)
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression arg = ParseOP();
            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("cos", new List<Expression> { arg });
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.sqrt)
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression arg = ParseOP();
            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("sqrt", new List<Expression> { arg });
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.exp)
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression arg = ParseOP();
            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("exp", new List<Expression> { arg });
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.log)
        {
            tokenstream.MoveForward(1);
            ExpectAndAdvance(Token.Type.left_bracket, "'(' symbol");
            Expression baseOfLog = ParseOP();
            if (tokenstream.tokens[tokenstream.Position()].Value != ",")
                errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "',' symbol"));
            else
                tokenstream.MoveForward(1);
            Expression number = ParseOP();
            ExpectAndAdvance(Token.Type.right_bracket, "')' symbol");
            return new BuiltinCall("log", new List<Expression> { baseOfLog, number });
        }

        if (tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.identifier)
        {
            string name = tokenstream.tokens[tokenstream.Position()].Value;

            if (tokenstream.Position() + 1 < tokens.Count && tokenstream.tokens[tokenstream.Position() + 1].Tipo == Token.Type.left_bracket)
            {
                tokenstream.MoveForward(1);
                tokenstream.MoveForward(1);

                List<Expression> args = new List<Expression>();
                if (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
                {
                    args.Add(ParseExpression());
                    while (tokenstream.Position() < tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == ",")
                    {
                        tokenstream.MoveForward(1);
                        args.Add(ParseExpression());
                    }
                }

                if (tokenstream.Position() >= tokens.Count || tokenstream.tokens[tokenstream.Position()].Tipo != Token.Type.right_bracket)
                    errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, "')' symbol"));
                else
                    tokenstream.MoveForward(1);

                return new FunctionCall(name, args);
            }

            tokenstream.MoveForward(1);
            return new VariableExpression(name);
        }

        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "let")
            return Let_In();

        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "while")
            return While();

        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "for")
            return ForLoop();

        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokenstream.tokens[tokenstream.Position()].Value == "{")
            return Block();

        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo == Token.Type.symbol && tokenstream.tokens[tokenstream.Position()].Value == "[")
            return ListLiteral();

        if (tokenstream.Position() >= tokenstream.tokens.Count || tokenstream.tokens[tokenstream.Position()] == null)
            return new LiteralExpression(0.0);

        errors.Add(new Error(Error.TypeError.Semantic_Error, Error.ErrorCode.Invalid, "expression"));
        return new LiteralExpression(0.0);
    }

    public Expression ParsePower()
    {
        Expression left = Unit();
        while (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Value == "^")
        {
            tokenstream.MoveForward(1);
            Expression right = ParsePower();
            left = new Power(left, right);
        }
        return left;
    }

    public Expression ParseMul_O_Div()
    {
        Expression left = ParsePower();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               (tokenstream.tokens[tokenstream.Position()].Value == "*" ||
                tokenstream.tokens[tokenstream.Position()].Value == "/" ||
                tokenstream.tokens[tokenstream.Position()].Value == "%"))
        {
            Token.Type op = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Expression right = ParsePower();

            left = op switch
            {
                Token.Type.multiplication => new Multiplication(left, right),
                Token.Type.module => new Module(left, right),
                _ => new Division(left, right)
            };
        }
        return left;
    }

    public Expression ParseSum_O_Sub()
    {
        Expression left = ParseMul_O_Div();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               (tokenstream.tokens[tokenstream.Position()].Value == "+" ||
                tokenstream.tokens[tokenstream.Position()].Value == "-"))
        {
            Token.Type op = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Expression right = ParseMul_O_Div();

            left = op == Token.Type.sum ? new Sum(left, right) : new Subtraction(left, right);
        }
        return left;
    }

    public Expression ParseComparation()
    {
        Expression left = ParseSum_O_Sub();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               (tokenstream.tokens[tokenstream.Position()].Value == "<" ||
                tokenstream.tokens[tokenstream.Position()].Value == ">" ||
                tokenstream.tokens[tokenstream.Position()].Value == ">=" ||
                tokenstream.tokens[tokenstream.Position()].Value == "<=" ||
                tokenstream.tokens[tokenstream.Position()].Value == "==" ||
                tokenstream.tokens[tokenstream.Position()].Value == "!="))
        {
            Token.Type op = tokenstream.tokens[tokenstream.Position()].Tipo;
            tokenstream.MoveForward(1);
            Expression right = ParseSum_O_Sub();

            left = op switch
            {
                Token.Type.minor => new Minor(left, right),
                Token.Type.major => new Major(left, right),
                Token.Type.equal_major => new EqualMajor(left, right),
                Token.Type.equal_minor => new EqualMinor(left, right),
                Token.Type.equal => new Equal(left, right),
                _ => new Different(left, right)
            };
        }
        return left;
    }

    public Expression ParseAnd()
    {
        Expression left = ParseComparation();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               tokenstream.tokens[tokenstream.Position()].Value == "&")
        {
            tokenstream.MoveForward(1);
            Expression right = ParseComparation();
            left = new And(left, right);
        }
        return left;
    }

    public Expression ParseOr()
    {
        Expression left = ParseAnd();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               tokenstream.tokens[tokenstream.Position()].Value == "|")
        {
            tokenstream.MoveForward(1);
            Expression right = ParseAnd();
            left = new Or(left, right);
        }
        return left;
    }

    public Expression ParseOP()
    {
        Expression left = ParseOr();
        while (tokenstream.Position() < tokenstream.tokens.Count &&
               (tokenstream.tokens[tokenstream.Position()].Value == "@" ||
                tokenstream.tokens[tokenstream.Position()].Value == "@@"))
        {
            string op = tokenstream.tokens[tokenstream.Position()].Value;
            tokenstream.MoveForward(1);
            Expression right = ParseOr();
            if (op == "@@")
                left = new Concatenation(new Concatenation(left, new LiteralExpression(" ")), right);
            else
                left = new Concatenation(left, right);
        }
        return left;
    }

    private void ExpectAndAdvance(Token.Type type, string errorMsg)
    {
        if (tokenstream.Position() < tokenstream.tokens.Count && tokenstream.tokens[tokenstream.Position()].Tipo == type)
            tokenstream.MoveForward(1);
        else
            errors.Add(new Error(Error.TypeError.Syntactic_Error, Error.ErrorCode.Expected, errorMsg));
    }
}
