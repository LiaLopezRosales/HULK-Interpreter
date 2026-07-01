# HULK Interpreter

Interprete del lenguaje HULK (Universidad de Matematicas de La Habana).

## Requisitos

- .NET SDK 8.0+

## Uso

```bash
# REPL interactivo
dotnet run --project Interpreter

# Ejecutar archivo .hulk
dotnet run --project Interpreter -- archivo.hulk

# Mostrar AST despues de evaluar
dotnet run --project Interpreter -- --ast

# Mostrar tokens
dotnet run --project Interpreter -- --tokens

# Combinar flags
dotnet run --project Interpreter -- --ast --tokens archivo.hulk
```

## Tests

```bash
dotnet test Interpreter.Tests/
```

## Features

- Variables: `let x = 42 in x`
- Funciones: `function f(x) => x * 2`
- Condicionales: `if (x > 0) "positivo" else "negativo"`
- Ciclos while: `while (x > 0) { print(x); x := x - 1; }`
- Asignacion destructiva: `x := x + 1`
- Builtins: `sin()`, `cos()`, `sqrt()`, `exp()`, `log()`, `rand()`, `PI`, `E`, `print()`, `range()`
- Operadores: aritmeticos, comparacion, booleanos, concatenacion (`@`, `@@`)
- Comentarios con `//` (via tokenizer)
