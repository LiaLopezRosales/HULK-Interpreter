# HULK Language Reference

## Gramatica (BNF)

```bnf
<program>       ::= <expression-list>
<expression-list> ::= <expression> { ";" <expression> }

<expression>    ::= <let-expr>
                  | <if-expr>
                  | <while-expr>
                  | <for-expr>
                  | <function-def>
                  | <assignment>
                  | <block>
                  | <logical-or>

<let-expr>      ::= "let" <let-binding> { "," <let-binding> } "in" <expression>
<let-binding>   ::= <identifier> "=" <expression>

<if-expr>       ::= "if" "(" <expression> ")" <expression> { "elif" "(" <expression> ")" <expression> } [ "else" <expression> ]

<while-expr>    ::= "while" "(" <expression> ")" <expression>

<for-expr>      ::= "for" "(" <identifier> "in" <expression> ")" <expression>

<function-def>  ::= "function" <identifier> "(" [ <param-list> ] ")" "=>" <expression>
<param-list>    ::= <identifier> { "," <identifier> }

<assignment>    ::= <identifier> ":=" <expression>

<block>         ::= "{" <expression-list> "}"

<logical-or>    ::= <logical-and> { "|" <logical-and> }
<logical-and>   ::= <comparison> { "&" <comparison> }

<comparison>    ::= <addition> [ ("<" | ">" | "<=" | ">=" | "==" | "!=") <addition> ]

<addition>      ::= <term> { ("+" | "-" | "@" | "@@") <term> }
<term>          ::= <unary> { ("*" | "/" | "%") <unary> }
<unary>         ::= "!" <unary>
                  | "-" <unary>
                  | <power>
<power>         ::= <primary> [ "^" <unary> ]

<primary>       ::= <literal>
                  | <identifier>
                  | <function-call>
                  | <builtin-call>
                  | <list-literal>
                  | "(" <expression> ")"

<function-call> ::= <identifier> "(" [ <arg-list> ] ")"
<builtin-call>  ::= <builtin-name> "(" [ <arg-list> ] ")"
<builtin-name>  ::= "sin" | "cos" | "sqrt" | "exp" | "log" | "rand" | "print" | "range"
<arg-list>      ::= <expression> { "," <expression> }

<list-literal>  ::= "[" [ <expression> { "," <expression> } ] "]"

<literal>       ::= <number> | <string> | <boolean>
<number>        ::= <digit> { <digit> } [ "." <digit> { <digit> } ]
<string>        ::= "\"" <any-char> "\""
<boolean>       ::= "true" | "false"

<identifier>    ::= <letter> { <letter> | <digit> }
<letter>        ::= "a".."z" | "A".."Z" | "_"
<digit>         ::= "0".."9"
```

## Operadores (precedencia descendente)

| Nivel | Operadores | Asociatividad | Descripcion |
|---|---|---|---|
| 1 | `\|` | Izquierda | OR logico (short-circuit) |
| 2 | `&` | Izquierda | AND logico (short-circuit) |
| 3 | `<` `>` `<=` `>=` `==` `!=` | Izquierda | Comparacion |
| 4 | `@` `@@` | Izquierda | Concatenacion (con/sin espacios) |
| 5 | `+` `-` | Izquierda | Suma / Resta |
| 6 | `*` `/` `%` | Izquierda | Multiplicacion / Division / Modulo |
| 7 | `!` `-` (unario) | Derecha | NOT logico / Negacion |
| 8 | `^` | Derecha | Potencia |
| 9 | `f(...)` `[...]` | -- | Llamada a funcion / Lista literal |

## Tipos

| Tipo | Ejemplos | Notas |
|---|---|---|
| `Number` | `42`, `3.14`, `-1e5` | Punto flotante (`double`) |
| `String` | `"hola"`, `"42"` | Incluye comillas en el valor interno |
| `Boolean` | `true`, `false` | -- |
| `List<Number>` | `range(1,5)` | Resultado de `range()` |
| `List<object>` | `[1, "a", true]` | Lista literal heterogenea |
| `null` | -- | Resultado de ciclos sin iteraciones |

## Built-in functions

| Funcion | Argumentos | Retorna | Descripcion |
|---|---|---|---|
| `sin(x)` | 1 | `Number` | Seno (radianes) |
| `cos(x)` | 1 | `Number` | Coseno (radianes) |
| `sqrt(x)` | 1 | `Number` | Raiz cuadrada |
| `exp(x)` | 1 | `Number` | Exponencial e^x |
| `log(x)` | 1 | `Number` | Logaritmo natural |
| `rand()` | 0 | `Number` | Aleatorio en [0, 1) |
| `print(x)` | 1 | `String` | Imprime en consola y retorna el valor |
| `range(s, e)` | 2 | `List<Number>` | Numeros de s a e-1 |
| `range(s, e, step)` | 3 | `List<Number>` | Numeros de s a e-1 con paso `step` |

## Constantes

| Nombre | Valor |
|---|---|
| `PI` | 3.141592653589793 |
| `E` | 2.718281828459045 |

## Comentarios

- **Linea:** `// texto`
- **Bloque:** `/* texto */` (multi-linea)

## Palabras reservadas

```
let in if elif else while for function
true false sin cos sqrt exp log rand print range
PI E break continue
```
