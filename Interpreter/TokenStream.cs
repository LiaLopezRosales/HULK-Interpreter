using System.Collections;
public class TokenStream:IEnumerable<Token>
{
    public Token[] tokens{get;}
    int position;
    public TokenStream(Token[]tokens)
    {
       this.tokens=tokens;
       position=0;
    }

    public int Position()
    {
        return position;
    }

    public bool End()
    {
        if (position==tokens.Length-1)
        {
            return true;
        }
        else return false;
    }

    public void MoveForward(int i)
    {
        if ( position+i <= tokens.Length-1)
        {
            position+=i;
        }
        
    }

    public void MoveBackward(int i)
    {
        if ( position-i >= 0)
        {
            position-=i;
        }
    }
    public void MoveTo(int i)
    {
        if (i>=0&&i<tokens.Length)
        {
            position=i;
        }
        
    }

    public bool Next()
    {
        if (position<tokens.Length-1)
        {
            position++;
        }
        return position<tokens.Length;
    }
    
     public bool Next(Token.Type type)
    {
        if (position<tokens.Length-1&& LookAhead(1).Tipo==type)
        {
            position++;
            return true;
        }
        return false;
    }
    public bool Next(Token.Type type,int f)
    {
        if (position<f&& LookAhead(1).Tipo==type)
        {
            position++;
            return true;
        }
        return false;
    }
    public Token LookAhead(int k=0)
    {
        return tokens[position+k];
    }
    public bool CanLookAhead(int k=0)
    {
        return tokens.Length-position>k;
    }

    public bool Next(string value)
    {
        if (position<tokens.Length-1 && LookAhead(1).Value==value)
        {
            position++;
            return true;
        }
        return false;
    }
    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Length; i++)
        {
            yield return tokens[i];
        }
    }
     IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}