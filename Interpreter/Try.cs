/*
    Este es el proyecto destinado a la 6ta clase práctica de programación. 
    La orientación de cada ejercicio puede encontrarla en el pdf adjunto.

    A la hora de entregar la cp, renombre el archivo Solution.cs con el formato:    <------ OJO aquí
    CP3-Nombre_Apellido1_Apellido2-grupo.cs
    Sólo debe entregar ese archivo renombrado, no el proyecto completo.
*/


string codigo = "let 44x = 55.43 - 20 in (if(44x<=60) print(44x @ \"es la edad de Eliza\") else print(44x+10))))) dias y noches\"";
//string codigo = "5+3*6;";
//string patron = @"\b(print|sin|cos|log|PI|E|let|in|function|if|else|true|false|[\w']+|\S)\b";
 
 Token[] possibletokens = Tokenizer.Tokens(codigo);
 
for (int j = 0; j < possibletokens.Length; j++)
{
    Console.WriteLine(possibletokens[j].ToString());
}

