# HULK Interpreter — AGENTS.md

## Build & run

```bash
dotnet build Interpreter/HULK-Interpreter.csproj
dotnet run --project Interpreter/HULK-Interpreter.csproj
dotnet watch run --project Interpreter/HULK-Interpreter.csproj   # auto-restart on change
```

`.NET SDK 6.0+` required (project targets `net6.0`, assembly name `HULK-Interpreter`). Shell wrapper: `bash Interpreter/Hulk-Interpreter.sh`.

## Project structure

- **`Interpreter/HULK-Interpreter.csproj`** — single .NET console app, `Exe` output, no test projects, no dependencies.
- **`Interpreter/Program.cs`** — REPL entrypoint. Reads stdin, tokenizes → parses → evaluates, prints result/errors.
- **`Lexer/Tokenizer.cs`** — regex-based tokenizer; `TokenStream` wraps token list with position helpers.
- **`Parser.cs`** — recursive-descent parser (operator precedence), builds polymorphic `Expression` tree.
- **`AST Evaluator.cs`** — thin wrapper; delegates evaluation to each `Expression` subclass via `Evaluate(Scope, Context, List<Error>)`.
- **`Expression/`** — polymorphic AST hierarchy: `Expression` (abstract) → `BinaryExpression` / `LiteralExpression` / `VariableExpression` / `UnaryExpression` / `ConditionalExpression` / `LetExpression` / `FunctionDefinition` / `FunctionCall` / `BuiltinCall` / `PrintExpression` / `BlockExpression` / `WhileExpression` / `AssignmentExpression`. Each subclass implements its own `Evaluate()`.
- **`Environment/`** — `Scope` (parent-linked chain), `Function` (name + params + body expression), `Context` (built-ins: sin, cos, sqrt, etc.), `Errors` (typed error with code).

## Code conventions

- **Spanish** — comments, variable names, error messages are all in Spanish.
- **InvariantCulture** — number parsing uses `CultureInfo.InvariantCulture` everywhere; decimal separator is always `.`.
- **Error handling** — errors accumulated in lists and checked between phases (not exceptions).
- **No LINQ, no generics** in evaluator — heavy use of `object` and casting.
- **`Random.Shared`** used instead of `new Random()` for randomness.

## Also note

- `.gitignore` present for .NET; `bin/` and `obj/` no longer tracked.
- MIT License at `/LICENSE`.
- `Interpreter/.vscode/` removed (root `.vscode/` is the active one).
