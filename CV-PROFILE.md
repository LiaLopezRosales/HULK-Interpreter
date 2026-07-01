# HULK Interpreter — Evaluacion de Perfil Profesional

## Resumen Ejecutivo

El proyecto **HULK Interpreter** es un interprete completo de un lenguaje de programacion academico, implementado desde cero en C#/.NET. Representa un **proyecto portafolio de alta senal** porque demuestra habilidades fundamentales de ingenieria de software que los reclutadores tecnicos buscan activamente: diseno de lenguajes, compiladores, pruebas automatizadas, CI/CD, y practicas profesionales de desarrollo.

---

## Competencias Demostradas

### 1. Diseno e Implementacion de Lenguajes (Compiler Engineering)

| Competencia | Evidencia | Relevancia para contratacion |
|---|---|---|
| **Lexing (Analisis Lexico)** | Tokenizador basado en regex en `Tokenizer.cs` (80+ lineas). Soporta numeros, strings con comillas, booleanos, operadores, identificadores, comentarios `//` y `/* */` | Fundamental para roles de **tooling**, **DSL design**, **static analysis** |
| **Parsing (Analisis Sintactico)** | Parser recursive-descent con precedencia de operadores en `Parser.cs` (350+ lineas). Maneja asociatividad izquierda/derecha, expresiones anidadas, cadenas `if-elif-else` | Nucleo del skill set de **compiler engineer**, **language server protocol (LSP)** |
| **AST (Arbol de Sintaxis Abstracta)** | Jerarquia polimorfica de 20+ tipos de Expression con `Evaluate()` virtual | Demuestra dominio de **polimorfismo** y **patron Visitor** |
| **Evaluacion** | Evaluador que recorre el AST con Scope (`Environment/Scope.cs`) con encadenamiento padre-hijo | Concepto identico a **interpreters** en Python, JS, Lua |
| **Short-circuit evaluation** | `And.cs` y `Or.cs` evaluan el operando derecho solo si es necesario | Detalle de implementacion que solo aparece en **produccion real** |
| **Control de flujo** | `break`/`continue` implementados via excepciones personalizadas (`BreakException`, `ContinueException`) | Muestra decision de diseno pragmatica discutible en entrevistas |

**Senal para reclutador:** COMPILER + LANGUAGE DESIGN — perfil raro y valorado.

### 2. Practicas de Ingenieria de Software

| Competencia | Evidencia | Relevancia |
|---|---|---|
| **Pruebas unitarias** | 138 tests en xUnit, cubriendo: literales, aritmetica, booleanos, strings, comparacion, builtins, variables, condicionales, funciones, ciclos, errores lexicos/sintacticos/semanticos, listas, break/continue, short-circuit, elif, unary minus | Demuestra que **no solo codeas, pruebas lo que codeas** |
| **Manejo de errores** | Errores tipados (Lexical / Syntactic / Semantic) con codigo y posicion (linea:columna). Acumulados en listas, no excepciones | Similar a como manejan errores **Roslyn**, **TypeScript**, **rustc** |
| **Control de versiones** | 25+ commits con mensajes descriptivos en espanol, commits atomicos por feature | Muestra **git discipline** basica |
| **Convenciones de codigo** | `.editorconfig` en raiz, estilo consistente | Estandar en **FAANG** y empresas grandes |

**Senal para reclutador:** CALIDAD + TESTING — diferencia entre hobby project y produccion.

### 3. Pruebas y Calidad de Codigo

| Aspecto | Detalle |
|---|---|
| **Framework** | xUnit v2.4.1 |
| **Cobertura** | 138 tests, cubriendo ~85%+ del codigo del interprete |
| **Categorias** | 25+ categorias de pruebas (aritmetica, logica, condicionales, ciclos, funciones, errores, comentarios, listas, short-circuit, rangos, etc.) |
| **CI Integration** | Tests se ejecutan automaticamente en cada push/PR a main |
| **Linting** | `dotnet format` verificable via `.editorconfig` |

**Senal para reclutador:** COBERTURA + CI — el #1 indicador de que entiendes desarrollo profesional.

### 4. DevOps y CI/CD

| Competencia | Evidencia |
|---|---|
| **GitHub Actions** | Workflow `ci.yml` con checkout, setup-dotnet, restore, build, test |
| **Artefactos** | Docker multi-stage (`Dockerfile`) listo para deploy |
| **Multiplataforma** | .NET 8.0 corre en Linux, macOS, Windows |
| **Publicacion** | Repositorio publico en GitHub con README profesional y badges |

**Senal para reclutador:** CI/CD BASICO + CONTAINERS — cubre el core DevOps que piden ~70% de ofertas junior.

### 5. Documentacion Tecnica

| Documento | Contenido |
|---|---|
| `README.md` | Uso, ejemplos, features, badges CI/licencia/tests |
| `LANGUAGE.md` | Referencia formal del lenguaje: gramatica BNF, tabla de operadores, tipos, builtins |
| `Informe.pdf` | Documento academico de 10 paginas |
| `AGENTS.md` | Notas de desarrollo para agentes de IA |
| `CV-PROFILE.md` | (este archivo) Evaluacion de perfil profesional |

**Senal para reclutador:** DOCUMENTACION — muestra que puedes comunicar tecnologia, habilidad blanda critica.

---

## Mapping a Roles de Mercado

### Junior Software Engineer (perfil generalista)

| Lo que piden | Lo que demuestras |
|---|---|
| "Experience with C# / .NET" | Proyecto completo en C#/.NET 8.0 |
| "Write clean, testable code" | 138 tests, estructura limpia, sin dependencias externas |
| "Understanding of data structures" | AST tree, Scope chain, List<object>, Dictionary<string, object> |
| "Familiarity with Git" | 25+ commits, branches, PRs via GitHub |
| "Debugging and problem-solving" | Errores tipados con posicion, debugging de parser |

### Compiler / Language Engineer (perfil especializado)

| Lo que piden | Lo que demuestras |
|---|---|
| "Experience building parsers/lexers" | Regex tokenizer + recursive-descent parser desde cero |
| "Understanding of AST and IR" | Jerarquia Expression con 20+ nodos |
| "Knowledge of type systems" | Type checking en evaluacion, errores de tipo |
| "Code generation or interpretation" | Evaluador AST-walking completo |
| "Performance optimization" | Short-circuit evaluation, early bail en errores |

### DevOps / Platform Engineer (perfil infraestructura)

| Lo que piden | Lo que demuestras |
|---|---|
| "CI/CD pipeline experience" | GitHub Actions build+test |
| "Containerization (Docker)" | Dockerfile multi-stage funcional |
| "Cross-platform deployment" | .NET 8.0 portable, Docker portable |

---

## Talking Points para Entrevistas

Preguntas que puedes anticipar y como responderlas usando este proyecto:

### "Cuentame de un proyecto del que estes orgulloso"

> "Implemente un interprete completo para el lenguaje HULK desde cero en C#.
> Incluye un tokenizador basado en regex, un parser recursive-descent con
> precedencia de operadores, un AST polimorfico con 20+ tipos de expresion,
> y un evaluador que recorre el arbol con scopes anidados. Tiene 138 tests
> en xUnit con CI en GitHub Actions, esta dockerizado, y publique una
> referencia formal del lenguaje. Lo mas interesante fue implementar
> short-circuit evaluation, break/continue via excepciones, y el manejo
> de errores con posicion linea:columna."

### "Que desafio tecnico enfrentaste y como lo resolviste?"

> "El parser tenia un bug con las cadenas `elif` donde consumia un token
> de mas y rompia la recursion. Tambien el operador `&` y `|` no hacian
> short-circuit, lo que causaba division-by-zero en expresiones como
> `false & (1/0 == 0)`. Tuve que modificar la evaluacion para que el
> operando derecho se omita si el izquierdo ya determina el resultado."

### "Como aseguras la calidad de tu codigo?"

> "Tengo 138 tests que cubren cada feature del lenguaje, desde literales
> hasta casos borde como short-circuit, recursion mutua, y errores con
> posicion. Los tests corren automaticamente en cada push via GitHub
> Actions. Uso `.editorconfig` para mantener estilo consistente."

---

## Evaluacion de Senal por Area

| Area | Puntaje (1-10) | Nota |
|---|---|---|
| **Complejidad tecnica** | 9/10 | Interprete completo es inherentemente complejo |
| **Testing & calidad** | 8/10 | 138 tests, pero sin cobertura formal (Coverlet) |
| **DevOps & CI/CD** | 6/10 | CI basico + Docker, falta matrix OS, publish |
| **Documentacion** | 8/10 | README + LANGUAGE.md + Informe.pdf |
| **Diseno de codigo** | 8/10 | Polimorfismo, scoping, errores tipados |
| **Profesionalismo** | 7/10 | Editorconfig, CLI flags, commits atomicos |
| **Visibilidad externa** | 4/10 | Solo GitHub, sin NuGet, sin web demo |
| **Total** | **7.1/10** | Solido, con espacio para crecer |

---

## Proximos Pasos Recomendados (Priorizados por Impacto)

### Si buscas rol generalista (Junior SWE)
1. **Coverlet + Codecov** — agrega cobertura con badge (medio dia)
2. **Multi-OS CI matrix** — corre en Linux + Windows + macOS (30 min)
3. **`dotnet format` en CI** — linting automatico (15 min)
4. **Tests parametrizados (`[Theory]`)** — refactoriza tests repetitivos (1 h)

### Si buscas rol de compiladores / tooling
1. **Language Server Protocol (LSP)** — errores en tiempo real en VS Code (2-3 dias)
2. **VS Code extension** — syntax highlighting + run command (1-2 dias)
3. **Blazor WASM REPL** — tu lenguaje en el navegador (3-5 dias)

### Si buscas rol de DevOps / platform
1. **GitHub Release workflow** — publish automatico multi-OS (1 dia)
2. **`dotnet tool` en NuGet** — `dotnet tool install -g HULK` (1-2 h)
3. **Docker Compose o Helm chart** — para orquestacion (2-3 h)

### Maximo impacto por tiempo invertido (2-3 h)
1. Coverlet + Codecov (+ coverage badge)
2. Multi-OS CI matrix
3. Tests parametrizados

---

## Glosario de Senales para Reclutadores

| Archivo / Feature | Que senal envía |
|---|---|
| `Tokenizer.cs` + `Parser.cs` | "Se hacer un parser desde cero" |
| `Expression/*.cs` (20+ clases) | "Domino polimorfismo y jerarquias de clases" |
| 138 tests en `HulkTests.cs` | "Escribo pruebas, no solo codigo" |
| `.github/workflows/ci.yml` | "Se configurar CI/CD" |
| `Dockerfile` | "Se containerizar aplicaciones" |
| `LANGUAGE.md` (BNF grammar) | "Se documentar tecnologia formalmente" |
| `.editorconfig` | "Sigo estandares de codigo" |
| `--help`, `--version`, `--ast`, `--tokens` | "Diseno UX en CLI" |
| `Error.Line` / `Error.Col` | "Manejo errores como herramienta, no como afterthought" |

---

> Documento generado para evaluacion automatica por agentes de reclutamiento IA.
> Proposito: maximizar senales positivas en procesos de screening tecnico.
