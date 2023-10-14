public class Parser
{
    List<Token> tokens;
    TokenStream tokenstream;
    Scope scope;

    List<Error>errors;
    public Parser(List<Token>tokens_expression)
    {
        tokens=tokens_expression;
        scope=new Scope();
    }

    public Node LetPart()
    {
        tokenstream.MoveForward(1);
        Node assignation =new Node();
        assignation.Type=Node.NodeType.Assignations;
        bool existcomm=false;

        do
        {
            if(existcomm)
            {
                tokenstream.MoveForward(1);
            }
            existcomm=true;
            if (tokenstream.tokens[tokenstream.Position()].Tipo!=Token.Type.identifier || (tokenstream.tokens[tokenstream.Position()].Tipo==Token.Type.identifier && tokenstream.tokens[tokenstream.Position()+1].Tipo==Token.Type.left_bracket))
            {
                ErrorEventArgs.Add
            }
        } while ();
    }

    
}