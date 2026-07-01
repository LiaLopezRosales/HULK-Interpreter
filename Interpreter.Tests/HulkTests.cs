using System.Globalization;
using Xunit;

namespace HULK_Tests;

public class HulkTests
{
    // ─── Helper ───────────────────────────────────────────────

    private static object Evaluate(string code)
    {
        var lexer = new Tokenizer();
        var tokens = lexer.Tokens(code);

        var lexicalErrors = lexer.Lexic_Errors();
        if (lexicalErrors.Count > 0)
            throw new Exception($"Lexical error: {string.Join("; ", lexicalErrors)}");

        var parser = new Parser(tokens);
        var ast = parser.Parse();

        var syntacticErrors = parser.Syntactic_Errors();
        if (syntacticErrors.Count > 0)
            throw new Exception($"Syntax error: {string.Join("; ", syntacticErrors)}");

        var evaluator = new AST_Evaluator();
        var result = evaluator.Evaluate(ast);

        if (evaluator.Semantic_Errors.Count > 0)
            throw new Exception($"Semantic error: {string.Join("; ", evaluator.Semantic_Errors)}");

        return result;
    }

    private static void AssertError(string code, string expectedErrorFragment)
    {
        var ex = Assert.Throws<Exception>(() => Evaluate(code));
        Assert.Contains(expectedErrorFragment, ex.Message, StringComparison.InvariantCultureIgnoreCase);
    }

    // ─── Literales ────────────────────────────────────────────

    [Fact] public void Literal_Number()     => Assert.Equal(42.0, Evaluate("42;"));
    [Fact] public void Literal_String()     => Assert.Equal("\"hello\"", Evaluate("\"hello\";"));
    [Fact] public void Literal_BoolTrue()   => Assert.Equal(true, Evaluate("true;"));
    [Fact] public void Literal_BoolFalse()  => Assert.Equal(false, Evaluate("false;"));
    [Fact] public void Literal_Decimal()    => Assert.Equal(3.14, Evaluate("3.14;"));
    [Fact] public void Literal_Negative()   => Assert.Equal(-5.0, Evaluate("-5;"));

    // ─── Aritmética ───────────────────────────────────────────

    [Fact] public void Arithmetic_Sum()            => Assert.Equal(5.0, Evaluate("2 + 3;"));
    [Fact] public void Arithmetic_Sub()            => Assert.Equal(1.0, Evaluate("3 - 2;"));
    [Fact] public void Arithmetic_Mul()            => Assert.Equal(12.0, Evaluate("3 * 4;"));
    [Fact] public void Arithmetic_Div()            => Assert.Equal(2.5, Evaluate("5 / 2;"));
    [Fact] public void Arithmetic_Pow()            => Assert.Equal(8.0, Evaluate("2 ^ 3;"));
    [Fact] public void Arithmetic_Mod()            => Assert.Equal(1.0, Evaluate("10 % 3;"));
    [Fact] public void Arithmetic_Precedence()     => Assert.Equal(14.0, Evaluate("2 + 3 * 4;"));
    [Fact] public void Arithmetic_Parens()         => Assert.Equal(20.0, Evaluate("(2 + 3) * 4;"));
    [Fact] public void Arithmetic_RightAssoc()     => Assert.Equal(512.0, Evaluate("2 ^ 3 ^ 2;"));
    [Fact] public void Arithmetic_NegativeExpr()   => Assert.Equal(2.0, Evaluate("5 + -3;"));
    [Fact] public void Arithmetic_Chained()        => Assert.Equal(10.0, Evaluate("1 + 2 + 3 + 4;"));
    [Fact] public void Arithmetic_Complex()        => Assert.Equal(42.0, Evaluate("(1 + 2 ^ 3) * 4 / (2 + 4) * 7 + 0;"));
    [Fact] public void Arithmetic_DivByZero()      => AssertError("5 / 0;", "divide by zero");

    // ─── Strings ──────────────────────────────────────────────

    [Fact] public void String_Concat()            => Assert.Equal("\"a\"\"b\"", Evaluate("\"a\" @ \"b\";"));
    [Fact] public void String_ConcatNumber()      => Assert.Equal("\"n:\"42", Evaluate("\"n:\" @ 42;"));
    [Fact] public void String_ConcatBool()        => Assert.Equal("\"b:\"True", Evaluate("\"b:\" @ true;"));
    [Fact] public void String_Empty()             => Assert.Equal("\"\"", Evaluate("\"\";"));
    [Fact] public void String_ChainedConcat()     => Assert.Equal("\"a\"\" \"\"c\"", Evaluate("\"a\" @ \" \" @ \"c\";"));

    // ─── Booleanos ────────────────────────────────────────────

    [Fact] public void Bool_NotTrue()    => Assert.Equal(false, Evaluate("!true;"));
    [Fact] public void Bool_NotFalse()   => Assert.Equal(true, Evaluate("!false;"));
    [Fact] public void Bool_And()        => Assert.Equal(false, Evaluate("true & false;"));
    [Fact] public void Bool_AndTrue()    => Assert.Equal(true, Evaluate("true & true;"));
    [Fact] public void Bool_Or()         => Assert.Equal(true, Evaluate("true | false;"));
    [Fact] public void Bool_OrFalse()    => Assert.Equal(false, Evaluate("false | false;"));
    [Fact] public void Bool_Precedence() => Assert.Equal(true, Evaluate("true | false & false;"));

    // ─── Comparación ──────────────────────────────────────────

    [Fact] public void Compare_Less()        => Assert.Equal(true, Evaluate("2 < 3;"));
    [Fact] public void Compare_Greater()     => Assert.Equal(false, Evaluate("3 > 5;"));
    [Fact] public void Compare_LessEqual()   => Assert.Equal(true, Evaluate("2 <= 2;"));
    [Fact] public void Compare_GreaterEqual()=> Assert.Equal(true, Evaluate("5 >= 3;"));
    [Fact] public void Compare_EqualNum()    => Assert.Equal(true, Evaluate("5 == 5;"));
    [Fact] public void Compare_NotEqual()    => Assert.Equal(true, Evaluate("5 != 3;"));
    [Fact] public void Compare_EqualStr()    => Assert.Equal(true, Evaluate("\"a\" == \"a\";"));
    [Fact] public void Compare_EqualBool()   => Assert.Equal(true, Evaluate("true == true;"));
    [Fact] public void Compare_DiffTypes()   => AssertError("\"a\" == 5;", "equal type");

    // ─── Builtins ─────────────────────────────────────────────

    [Fact] public void Builtin_Sin()    => Assert.Equal(0.0, Evaluate("sin(0);"));
    [Fact] public void Builtin_Cos()    => Assert.Equal(1.0, Evaluate("cos(0);"));
    [Fact] public void Builtin_Sqrt()   => Assert.Equal(3.0, Evaluate("sqrt(9);"));
    [Fact] public void Builtin_Exp()    => Assert.Equal(1.0, Evaluate("exp(0);"));
    [Fact] public void Builtin_Log()    => Assert.Equal(2.0, Evaluate("log(10, 100);"));
    [Fact] public void Builtin_PI()     => Assert.Equal(Math.PI, Evaluate("PI;"));
    [Fact] public void Builtin_E()      => Assert.Equal(Math.E, Evaluate("E;"));
    [Fact] public void Builtin_Rand()   => { var r = (double)Evaluate("rand();"); Assert.InRange(r, 0.0, 1.0); }
    [Fact] public void Builtin_Print()  => Assert.Equal(42.0, Evaluate("print(42);"));
    [Fact] public void Builtin_PrintStr() => Assert.Equal("\"hi\"", Evaluate("print(\"hi\");"));

    // ─── Variables ────────────────────────────────────────────

    [Fact] public void Var_Simple()           => Assert.Equal(5.0, Evaluate("let x = 5 in x;"));
    [Fact] public void Var_Multiple()         => Assert.Equal(15.0, Evaluate("let x = 5, y = 10 in x + y;"));
    [Fact] public void Var_DependentInit()    => Assert.Equal(42.0, Evaluate("let a = 6, b = a * 7 in b;"));
    [Fact] public void Var_NestedScopes()     => Assert.Equal(20.0, Evaluate("let x = 10 in let x = 20 in x;"));
    [Fact] public void Var_OuterScope() => Assert.Equal(10.0, Evaluate("let x = 10 in { let x = 20 in 0; x; }"));
    [Fact] public void Var_InExpr()           => Assert.Equal(42.0, Evaluate("print(let b = 6 in b * 7);"));
    [Fact] public void Var_DeepNesting()      => Assert.Equal(9.0, Evaluate("let a = 1, b = 2 in let c = 3 in a + b + c + 3;"));

    // ─── Condicionales ────────────────────────────────────────

    [Fact] public void If_True()            => Assert.Equal(1.0, Evaluate("if (true) 1 else 2;"));
    [Fact] public void If_False()           => Assert.Equal(2.0, Evaluate("if (false) 1 else 2;"));
    [Fact] public void If_Comparison()      => Assert.Equal("\"yes\"", Evaluate("if (2 < 3) \"yes\" else \"no\";"));
    [Fact] public void If_Nested()          => Assert.Equal(3.0, Evaluate("if (true) if (false) 1 else 2 else 3;"));
    [Fact] public void If_LetInside()       => Assert.Equal(5.0, Evaluate("if (true) let x = 5 in x else 0;"));
    [Fact] public void If_ExprResult()      => Assert.Equal(42.0, Evaluate("print(if (true) 42 else 0);"));

    // ─── Funciones ────────────────────────────────────────────

    [Fact] public void Func_Simple()        => Assert.Equal(6.0, Evaluate("function f(x) => x + 1; f(5);"));
    [Fact] public void Func_MultiParam()    => Assert.Equal(7.0, Evaluate("function add(a, b) => a + b; add(3, 4);"));
    [Fact] public void Func_Recursive()     => Assert.Equal(120.0, Evaluate("function fact(n) => if (n == 0) 1 else n * fact(n - 1); fact(5);"));
    [Fact] public void Func_Chain()         => Assert.Equal(10.0, Evaluate("function id(x) => x; function add(a, b) => a + b; add(id(3), id(7));"));
    [Fact] public void Func_UseBuiltin()    => Assert.Equal(1.0, Evaluate("function f(x) => sin(x) / cos(x); f(0);"));
    [Fact] public void Func_MutualRecursion() => Assert.Equal(2.0, Evaluate("function isEven(n) => if (n == 0) true else isOdd(n - 1); function isOdd(n) => if (n == 0) false else isEven(n - 1); if (isEven(4)) 2 else 0;"));

    // ─── Combinaciones complejas ──────────────────────────────

    [Fact]
    public void Complex_LetWithIfAndFunc()
    {
        var result = Evaluate(@"
            function max(a, b) => if (a > b) a else b;
            let x = 10, y = 20 in max(x, y);
        ");
        Assert.Equal(20.0, result);
    }

    [Fact]
    public void Complex_DeepArithmetic()
    {
        var result = Evaluate("((1 + 2) * 3 - 4 / 2) ^ 2 + 5;");
        Assert.Equal(30.0, result);
    }

    [Fact]
    public void Complex_MixedTypes()
    {
        var result = Evaluate("let x = 42 in \"The answer is \" @ x;");
        Assert.Equal("\"The answer is \"42", result);
    }

    [Fact]
    public void Complex_Fibonacci()
    {
        var result = Evaluate(@"
            function fib(n) => if (n == 0 | n == 1) 1 else fib(n - 1) + fib(n - 2);
            fib(10);
        ");
        Assert.Equal(89.0, result);
    }

    [Fact]
    public void Complex_NestedBuiltins()
    {
        var result = Evaluate("sin(2 * PI) ^ 2 + cos(3 * PI / log(4, 64));");
        Assert.Equal(-1.0, Math.Round((double)result, 10));
    }

    // ─── Errores léxicos ──────────────────────────────────────

    [Fact] public void Error_MissingSemicolon() => AssertError("2 + 3", ";");
    [Fact] public void Error_InvalidToken()     => AssertError("2 $ 3;", "lexical");

    // ─── Errores sintácticos ──────────────────────────────────

    [Fact] public void Error_MissingParen()     => AssertError("print(42;", "')'");
    [Fact] public void Error_MissingThen()      => AssertError("if (true) 1;", "else");

    // ─── Errores semánticos ───────────────────────────────────

    [Fact] public void Error_UndeclaredVar()    => AssertError("x;", "variable");
    [Fact] public void Error_StringPlusNum()    => AssertError("\"a\" + 1;", "numerical");
}
