using System.Globalization;
public class AST_Evaluator
{
    private Node AST{get;set;}
    private Scope scope{get;set;}
    private Context context{get;set;}
    private List<Scope>? currentcontext{get;set;}
    public List<Error> Semantic_Errors{get;set;}
    //Se crean el contexto al que se irán agregando funciones,lista de errores encontrados y ámbitos de variables
     public AST_Evaluator()
     {
        context=new Context();
        scope = new Scope();
        Semantic_Errors=new List<Error>();
        AST=new Node();
     }
      
      //Se asigna el árbol a evaluar y se crea un scope padre para él
     public void Tree_Reader(Node root)
     {
       AST=root;
       currentcontext=new List<Scope>(){new Scope()};
     }
     //Método que inicia la evaluación
     public object StartEvaluation(Node node)
     {
      if (node.Type==Node.NodeType.Print)
      {
         object print=GeneralEvaluation(node.Branches[0]);
         return print;
      }
      else
      { 
         object result=GeneralEvaluation(node);
         return result;
      }
     }
      public List<Error> Semanti_Errors()
    {
        return Semantic_Errors;
    }

     public object GeneralEvaluation(Node node)
     {
      //Si el nodo es un valor numérico,de texto o booleano retorna dicho valor
      if (node.Type==Node.NodeType.Text)
      {
         return node.NodeExpression!;
      }
      else if (node.Type==Node.NodeType.Number)
      {
         return node.NodeExpression!;
      }
      else if (node.Type==Node.NodeType.False)
      {
         return false;
      }
      else if (node.Type==Node.NodeType.True)
      {
         return true;
      }
   //En la evaluación de operaciones se asigna las partes derecha e izquierda y se comprueba si con esas expresiones ya evaluadas es válida su ejecución
      else if (node.Type==Node.NodeType.Sum)
      {
         Sum sum =new Sum();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         sum.Evaluate(left,right);
         return sum.Value!;
      }
      else if (node.Type==Node.NodeType.Sub)
      {
         Substraction sub =new Substraction();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         sub.Evaluate(left,right);
         return sub.Value!;
      }
      else if (node.Type==Node.NodeType.Mul)
      {
         Multiplication mul =new Multiplication();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         mul.Evaluate(left,right);
         return mul.Value!;
      }
      else if (node.Type==Node.NodeType.Div)
      {
         Division div =new Division();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         if (Convert.ToDouble(right,CultureInfo.InvariantCulture)==0)
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"operation,can't divide by zero"));
            return left;
         }
         div.Evaluate(left,right);
         return div.Value!;
      }
      else if (node.Type==Node.NodeType.Mod)
      {
         Module mod =new Module();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         mod.Evaluate(left,right);
         return mod.Value!;
      }
      else if (node.Type==Node.NodeType.Pow)
      {
         Power pow =new Power();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         pow.Evaluate(left,right);
         return pow.Value!;
      }
      else if (node.Type==Node.NodeType.Negation)
      {
        object negation=GeneralEvaluation(node.Branches[0]);
        if (!(negation is bool))
        {
          Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected," boolean vale"));
        }
        else return !(bool)negation;
      }
      //Si el scope actual no contiene a la variable se lanza un error en caso contrario se devuelve el valor guardado
      else if (node.Type==Node.NodeType.Var)
      {
         if (!currentcontext![currentcontext.Count-1].Variables.ContainsKey(node.NodeExpression!.ToString()!))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"variable"));
         }
         else return currentcontext![currentcontext.Count-1].Variables[node.NodeExpression!.ToString()!];
      }
      else if (node.Type==Node.NodeType.Declared_FucName)
      {
         return node.NodeExpression!;
      }
      else if (node.Type==Node.NodeType.Minor)
      {
         Minor min =new Minor();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         min.Evaluate(left,right);
         return min.Value!;
      }
      else if (node.Type==Node.NodeType.Major)
      {
         Major maj =new Major();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         maj.Evaluate(left,right);
         return maj.Value!;
      }
      else if (node.Type==Node.NodeType.Equal_Major)
      {
         Equal_Major emaj =new Equal_Major();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         emaj.Evaluate(left,right);
         return emaj.Value!;
      }
      else if (node.Type==Node.NodeType.Equal_Minor)
      {
         Equal_Minor emin =new Equal_Minor();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is double)||!(right is double))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"numerical values"));
         }
         emin.Evaluate(left,right);
         return emin.Value!;
      }
      else if (node.Type==Node.NodeType.Equal)
      {
         Equal eq =new Equal();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (left is string && right is string)
         {
            
            eq.Evaluate(left,right);
         return eq.Value!;
         }
         else if (left is bool && right is bool)
         {
           
            eq.Evaluate(left,right);
            return eq.Value!;
         }
         else if (left is double && right is double)
         {
           
            eq.Evaluate(left,right);
            return eq.Value!;
         }
         else 
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"equal type of values"));
         }
         eq.Evaluate(left,right);
         return eq.Value!;
      }
      else if (node.Type==Node.NodeType.Diferent)
      {
         Diferent df =new Diferent();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (left is string && right is string)
         {
           
            df.Evaluate(left,right);
         return df.Value!;
         }
         else if (left is bool && right is bool)
         {
            
            df.Evaluate(left,right);
            return df.Value!;
         }
         else if (left is double && right is double)
         {
            
            df.Evaluate(left,right);
            return df.Value!;
         }
         else 
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"equal type of values"));
         }
         df.Evaluate(left,right);
         return df.Value!;
      }
      else if (node.Type==Node.NodeType.And)
      {
         And and =new And();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is bool)||!(right is bool))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"boolean values"));
         }
         and.Evaluate(left,right);
         return and.Value!;
      }else if (node.Type==Node.NodeType.Or)
      {
         Or or =new Or();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is bool)||!(right is bool))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"boolean values"));
         }
         or.Evaluate(left,right);
         return or.Value!;
      }
      else if (node.Type==Node.NodeType.Concat)
      {
         Concatenation con =new Concatenation();
         object left = GeneralEvaluation(node.Branches[0]);
         object right=GeneralEvaluation(node.Branches[1]);
         if (!(left is string || left is double || left is bool)||!(right is string || right is double || right is bool))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"valid values"));
         }
         con.Evaluate(left,right);
         return con.Value!;
      }
      //Si node es el un nombre se devuelve el valor guardado
      else if (node.Type==Node.NodeType.FucName)
      {
         return node.NodeExpression!;
      }
      else if (node.Type==Node.NodeType.ParName)
      {
         return node.NodeExpression!;
      }
      else if (node.Type==Node.NodeType.VarName)
      {
         return node.NodeExpression!;
      }
      //Se evalua el argumento y esto se envia a la función creada Print en el contexto
      else if (node.Type==Node.NodeType.Print)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Print["print"](arg);
      }
      //Se evaluan los argumentos y se accede a las funciones predeterminadas del contexto con estos
      else if (node.Type==Node.NodeType.Sin)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Trig_functions["sin"](Convert.ToDouble(arg,CultureInfo.InvariantCulture));
      }
      else if (node.Type==Node.NodeType.Cos)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Trig_functions["cos"]((double)arg);
      }
      else if (node.Type==Node.NodeType.Sqrt)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Trig_functions["sqrt"](Convert.ToDouble(arg,CultureInfo.InvariantCulture));
      }
      else if (node.Type==Node.NodeType.Exp)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Trig_functions["exp"](Convert.ToDouble(arg,CultureInfo.InvariantCulture));
      }
      else if (node.Type==Node.NodeType.Log)
      {
         object base_of = GeneralEvaluation(node.Branches[0]);
         object arg = GeneralEvaluation(node.Branches[1]);
         return context.Log["log"](Convert.ToDouble(base_of,CultureInfo.InvariantCulture),Convert.ToDouble(arg,CultureInfo.InvariantCulture));
      }
      else if (node.Type==Node.NodeType.Rand)
      {
         return context.Math_value["rand"]();
      }
      else if (node.Type==Node.NodeType.PI)
      {
         return context.Math_value["PI"]();
      }
      else if (node.Type==Node.NodeType.E)
      {
         return context.Math_value["E"]();
      }
      //Se evalua la condición,se comprueba que devuelva un bool, y en dependencia de ese bool se evalua if o else
      else if (node.Type==Node.NodeType.Conditional)
      {
         object condition=GeneralEvaluation(node.Branches[0]);
         if (!(condition is bool))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"bool return value"));
            return null!;
         }
         Ternary Conditional =new Ternary();
         if ((bool)condition)
         {
            object if_part=GeneralEvaluation(node.Branches[1]);
            Conditional.Evaluate(condition,if_part,-1);
         }
         else
         {
            object else_part=GeneralEvaluation(node.Branches[2]);
            Conditional.Evaluate(condition,-1,else_part);
         }
         
         return Conditional.Value!;
      }
      //Guarda la definición de la función en el contexto
      else if (node.Type==Node.NodeType.Fuction)
      {  //Se crea un diccionario para guardar los parámetros
         Dictionary<string,object> Parameters = new Dictionary<string, object>();
         Node param=node.Branches[1];
         string par_name="";
         //Se agregan todos los parámetros parseados
         for (int i = 0; i < param.Branches.Count; i++)
         {
            par_name=(string)param.Branches[i].NodeExpression!;
            Parameters.Add(par_name,"");
         }
         //Se crea un objeto función con el nombre,el cuerpo y los parámetros obtenidos
         Fuction func=new Fuction(node.Branches[0].NodeExpression!.ToString()!,node.Branches[2],Parameters);
         bool exist=false;
         //Se busca si ya existe una función con ese nombre,de ser así se lanza excepción
         foreach (var function in context.Available_Functions)
         {
            if (function.Name==par_name)
            {
               exist=true;
            }
         }
         if (exist)
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"function name, already exist a preexistent function with the same name"));
         }
         else context.Available_Functions.Add(func);
         return context.Available_Functions;
         
      }
      //Se evalua una llamada a una función supuestamente existente
      else if (node.Type==Node.NodeType.Declared_Fuc)
      {
         string dfunc_name = node.Branches[0].NodeExpression!.ToString()!;
         Node func_parameters=node.Branches[1];
         bool exist=false;
         //Se comprueba si la función fue declarada con anterioridad
         foreach (var function in context.Available_Functions)
         {
            if (function.Name==dfunc_name)
            {
               exist=true;
            }
         }
         int index=-1;
         if (exist)
         {
           Scope func_scope= scope.Child();
           currentcontext!.Add(func_scope);
           
           for (int i = 0; i < context.Available_Functions.Count; i++)
           {
            //Se accede a los detalles de la función guardada
             if (context.Available_Functions[i].Name==dfunc_name)
             {
               //Se comprueba si tienen la misma cantidad de argumentos
               if (context.Available_Functions[i].Functions_Arguments.Count==func_parameters.Branches.Count)
               {
                  //Se agregan los argumentos dados a el scope del cuerpo de la función
                  foreach (var p_name in currentcontext[currentcontext.Count-2].Variables.Keys)
                  {
                     currentcontext[currentcontext.Count-1].Variables.Add(p_name,currentcontext[currentcontext.Count-2].Variables[p_name]);
                  }
                  int param_number=0;
                  foreach (var p_name in context.Available_Functions[i].Functions_Arguments.Keys)
                  { 
                     //Se asignan los argumentos dados a su correspondiente en los declarados por la función
                     context.Available_Functions[i].Functions_Arguments[p_name]=func_parameters.Branches[param_number].NodeExpression!;
                     //Se evaluan estos argumentos
                     if (currentcontext[currentcontext.Count-1].Variables.ContainsKey(p_name))
                     {
                        
                        currentcontext[currentcontext.Count-1].Variables[p_name]=GeneralEvaluation((Node)func_parameters.Branches[param_number].NodeExpression!);
                        param_number++;
                     }
                     else
                     {
                        object par_value=GeneralEvaluation((Node)func_parameters.Branches[param_number].NodeExpression!);
                        currentcontext[currentcontext.Count-1].Variables.Add(p_name,par_value);
                        param_number++;
                     }
                   
                  }
                  index=i;
                  
               }
               else
               {
                  //Se lanza un error si el número de parámetros no coincide
                  Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,$"{context.Available_Functions[i].Functions_Arguments.Count} parameters but received {func_parameters.Branches.Count}"));
               }
              
             }
             
           }
          
         }
         else
         {
            //Se lanza error si se está llamando a una función que no existe
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"name,function has not been declared"));
            index=-1;
         }
         ;
         //Se evalua la función solicitada
         object value =GeneralEvaluation(context.Available_Functions[index].Code);
         //Se elimina el contexto creado para la función
         currentcontext!.Remove(currentcontext[currentcontext.Count-1]);
        
         return value;
         
      }
      else if (node.Type==Node.NodeType.Assignations)
      {
         //Se crea el contexto de las variables declaradas
        Scope var_of_let=scope.Child();
        foreach (var name in currentcontext![currentcontext.Count-1].Variables.Keys)
        {
         //Se agregan todas las variables existentes
          var_of_let.Variables.Add(name,currentcontext![currentcontext.Count-1].Variables[name]);
        }
        foreach (Node branch in node.Branches)
        {
         //Se procesa cada variable declarada
         string name=branch.Branches[0].NodeExpression!.ToString()!;
         object value=GeneralEvaluation(branch.Branches[1]);
         //Si ya existe el nombre de la variable se actualiza su valor
         if (var_of_let.Variables.ContainsKey(name))
         {
            var_of_let.Variables[name]=value;
         }
         else
         {
            var_of_let.Variables.Add(name,value);
         }
        }
        currentcontext.Add(var_of_let);
      
      }
      else if (node.Type==Node.NodeType.Let_exp)
      {
         //Se evalua la asignación y a continuación la parte in ya teniendo el nuevo scope
         GeneralEvaluation(node.Branches[0]);
         object result=GeneralEvaluation(node.Branches[1]);
         //Se elimina el scope existente en esta expresión
         currentcontext!.Remove(currentcontext[currentcontext.Count-1]);
         return result;
      }
      else
      {
         //Si no evalua como ninguno de los nodos anteriores no existe la operación
         Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Unknown,"operation required"));
      }
      
      
     return "end";
     }

}