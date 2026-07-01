# HULK Interpreter

[![CI](https://github.com/LiaLopezRosales/HULK-Interpreter/actions/workflows/ci.yml/badge.svg)](https://github.com/LiaLopezRosales/HULK-Interpreter/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Tests](https://img.shields.io/badge/Tests-138-passing-2ea44f)]()

Interprete del lenguaje HULK, implementado como parte del proyecto final de la Facultad de Matematicas de la Universidad de La Habana.

## Requisitos

- .NET SDK 8.0+

## Uso rapido

```bash
# REPL interactivo
dotnet run --project Interpreter

# Ejecutar archivo .hulk
dotnet run --project Interpreter -- programa.hulk

# Ayuda
dotnet run --project Interpreter -- --help
```

## Opciones CLI

| Flag | Descripcion |
|---|---|
| `--help`, `-h` | Muestra ayuda |
| `--version`, `-v` | Muestra la version |
| `--ast` | Muestra el AST despues de evaluar |
| `--tokens` | Muestra los tokens generados |

## Ejemplos

```hulk
let x = 42 in print(x * 2);
```

```hulk
function factorial(n) => if (n <= 1) 1 else n * factorial(n - 1);
factorial(10);
```

```hulk
for (x in range(0, 10, 2))
{
    if (x == 4) continue;
    print(x);
}
```

## Features

- **Variables:** `let x = 42 in x`
- **Asignacion destructiva:** `x := x + 1`
- **Funciones:** `function f(x, y) => x + y`
- **Recursion:** soporte completo para funciones recursivas y mutuamente recursivas
- **Condicionales:** `if` / `elif` / `else`
- **Ciclos:** `while`, `for (x in range(...))`
- **Control de flujo:** `break`, `continue`
- **Listas:** `[1, 2, 3]`, `range(0, 10, 2)`
- **Operadores:** aritmeticos (`+`, `-`, `*`, `/`, `%`, `^`), comparacion, booleanos (`&`, `|`, `!`), concatenacion (`@`, `@@`)
- **Builtins:** `sin`, `cos`, `sqrt`, `exp`, `log`, `rand`, `print`, `PI`, `E`
- **Comentarios:** `// linea` y `/* bloque */`
- **Short-circuit:** evaluacion perezosa en `&` y `|`

## Tests

138 tests con xUnit:

```bash
dotnet test Interpreter.Tests/
```

## Docker

```bash
docker build -t hulk-interpreter .
docker run -it hulk-interpreter
```

## Documentacion

- [Referencia del lenguaje](LANGUAGE.md)
- [Informe academico](Informe.pdf)

## Licencia

MIT — ver [LICENSE](LICENSE).
