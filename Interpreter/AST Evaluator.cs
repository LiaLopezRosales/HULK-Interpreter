using System.Globalization;
public class AST_Evaluator
{
    private Node AST{get;set;}
    private Scope scope{get;set;}
    private Context context{get;set;}
    private List<Scope>? currentcontext{get;set;}
    private List<Error> Semantic_Errors{get;set;}
     public AST_Evaluator()
     {
        context=new Context();
        scope = new Scope();
        Semantic_Errors=new List<Error>();
        AST=new Node();
     }

     public void Tree_Reader(Node root)
     {
       AST=root;
       currentcontext=new List<Scope>(){new Scope()};
     }

     public object StartEvaluation(Node node)
     {
      if (node.Type==Node.NodeType.Print)
      {
         object print=GeneralEvaluation(node.Branches[0]);
         if (Semantic_Errors.Count>0)
         {
            Console.WriteLine(print);
         }
         return print;
         
      }
      else
      { 
         object result=GeneralEvaluation(node);
         if (Semantic_Errors.Count>0)
         {
            Console.WriteLine(result);
         }
         return result;
      }
     }
      public List<Error> Semanti_Errors()
    {
        return Semantic_Errors;
    }

     public object GeneralEvaluation(Node node)
     {
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
         div.Evaluate(left,right);
         return div.Value!;
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
      else if (node.Type==Node.NodeType.Var)
      {
         return currentcontext![currentcontext.Count-1].Variables[node.NodeExpression!.ToString()!];
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
            Console.WriteLine("tttt");
            eq.Evaluate(left,right);
         return eq.Value!;
         }
         else if (left is bool && right is bool)
         {
            Console.WriteLine("trrr");
            eq.Evaluate(left,right);
            return eq.Value!;
         }
         else if (left is double && right is double)
         {
            Console.WriteLine("trrr");
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
            Console.WriteLine("tttt");
            df.Evaluate(left,right);
         return df.Value!;
         }
         else if (left is bool && right is bool)
         {
            Console.WriteLine("trrr");
            df.Evaluate(left,right);
            return df.Value!;
         }
         else if (left is double && right is double)
         {
            Console.WriteLine("trrr");
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
         if (!(left is string)||!(right is string))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"string values"));
         }
         con.Evaluate(left,right);
         return con.Value!;
      }
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
      else if (node.Type==Node.NodeType.Print)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Print["print"](arg);
      }
      else if (node.Type==Node.NodeType.Sin)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         return context.Trig_functions["sin"](Convert.ToDouble(arg,CultureInfo.InvariantCulture));
      }
      else if (node.Type==Node.NodeType.Cos)
      {
         object arg = GeneralEvaluation(node.Branches[0]);
         // Console.WriteLine(arg.ToString());
         // Console.WriteLine((double)arg);
         Console.WriteLine(context.Trig_functions["cos"](Convert.ToDouble(arg,CultureInfo.InvariantCulture)));
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
      else if (node.Type==Node.NodeType.Conditional)
      {
         object condition=GeneralEvaluation(node.Branches[0]);
         if (!(condition is bool))
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,"bool return value"));
         }
         object if_part=GeneralEvaluation(node.Branches[1]);
         object else_part=GeneralEvaluation(node.Branches[2]);
         Ternary Conditional =new Ternary();
         Conditional.Evaluate(condition,if_part,else_part);
         return Conditional.Value!;
      }
      else if (node.Type==Node.NodeType.Fuction)
      {
         Dictionary<string,object> Parameters = new Dictionary<string, object>();
         Node param=node.Branches[1];
         string par_name="";
         for (int i = 0; i < param.Branches.Count; i++)
         {
            par_name=(string)param.Branches[i].NodeExpression!;
            Parameters.Add(par_name,"");
         }
         Fuction func=new Fuction(node.Branches[0].NodeExpression!.ToString()!,node.Branches[2],Parameters);
         bool exist=false;
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
      else if (node.Type==Node.NodeType.Declared_Fuc)
      {
         string dfunc_name = node.Branches[0].NodeExpression!.ToString()!;
         Node func_parameters=node.Branches[1];
         bool exist=false;
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
             if (context.Available_Functions[i].Name==dfunc_name)
             {
               if (context.Available_Functions[i].Functions_Arguments.Count==func_parameters.Branches.Count)
               {
                  foreach (var p_name in currentcontext[currentcontext.Count-2].Variables.Keys)
                  {
                     currentcontext[currentcontext.Count-1].Variables.Add(p_name,currentcontext[currentcontext.Count-2].Variables[p_name]);
                  }
                  int param_number=0;
                  foreach (var p_name in context.Available_Functions[i].Functions_Arguments.Keys)
                  {
                     context.Available_Functions[i].Functions_Arguments[p_name]=func_parameters.Branches[param_number].NodeExpression!;
                     if (currentcontext[currentcontext.Count-1].Variables.ContainsKey(p_name))
                     {//Check this
                        //Console.WriteLine($"line 409 {func_parameters.Branches[param_number].NodeExpression!}");
                        currentcontext[currentcontext.Count-1].Variables[p_name]=GeneralEvaluation((Node)func_parameters.Branches[param_number].NodeExpression!);
                        //Console.WriteLine($"line 410 {currentcontext[currentcontext.Count-1].Variables[p_name]}");
                        param_number++;
                     }
                     else
                     {
                        //Console.WriteLine($"line 416 {func_parameters.Branches[param_number].NodeExpression}");
                        object par_value=GeneralEvaluation((Node)func_parameters.Branches[param_number].NodeExpression!);
                        currentcontext[currentcontext.Count-1].Variables.Add(p_name,par_value);
                        //Console.WriteLine("returned value");
                        //Console.WriteLine($"line 420 {par_value}");
                        param_number++;
                     }
                     //Console.WriteLine($"line 423 {param_number}");
                  }
                  index=i;
                  //Console.WriteLine($"line 426 {index}");
               }
               else
               {
                  Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Expected,$"{context.Available_Functions[i].Functions_Arguments.Count} parameters but received {func_parameters.Branches.Count}"));
               }
               //Console.WriteLine($"line 432 ");
             }
             //Console.WriteLine("line 434");
           }
           //Console.WriteLine("line 436");
         }
         else
         {
            Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Invalid,"name,function has not been declared"));
            index=-1;
         }
         //Console.WriteLine("line 443");
         object value =GeneralEvaluation(context.Available_Functions[index].Code);
         currentcontext!.Remove(currentcontext[currentcontext.Count-1]);
         //Console.WriteLine($"{value}");
         return value;
         
      }
      else if (node.Type==Node.NodeType.Assignations)
      {
        Scope var_of_let=scope.Child();
        foreach (var name in currentcontext![currentcontext.Count-1].Variables.Keys)
        {
          var_of_let.Variables.Add(name,currentcontext![currentcontext.Count-1].Variables[name]);
        }
        foreach (Node branch in node.Branches)
        {
         string name=branch.Branches[0].NodeExpression!.ToString()!;
         object value=GeneralEvaluation(branch.Branches[1]);
         //Check later if a function with te same name creates conflict
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
         GeneralEvaluation(node.Branches[0]);
         object result=GeneralEvaluation(node.Branches[1]);
         currentcontext!.Remove(currentcontext[currentcontext.Count-1]);
         return result;
      }
      else
      {
         Semantic_Errors.Add(new Error(Error.TypeError.Semantic_Error,Error.ErrorCode.Unknown,"operation required"));
      }
      
      
     return "end";
     }

}