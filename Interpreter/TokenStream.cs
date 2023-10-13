using System.Collections;
public class TokenStream:IEnumerable<Token>
{
    public Token[] tokens{get;}
    int position;
    int start;
    int end;
    public TokenStream(Token[]tokens,int initial,int final)
    {
       this.tokens=tokens;
       start=initial;
       end=final;
       position=0;
    }

    public int Position()
    {
        return position;
    }

    public bool End()
    {
        if (position==end)
        {
            return true;
        }
        else return false;
    }

    public void MoveForward(int i)
    {
        if ( position+i <= end)
        {
            position+=i;
        }
        
    }

    public void MoveBackward(int i)
    {
        if ( position-i >= start)
        {
            position-=i;
        }
    }
    public void MoveTo(int i)
    {
        if (i>=start&&i<=end)
        {
            position=i;
        }
        
    }

    public bool Next()
    {
        if (position<end)
        {
            position++;
        }
        return position<end;
    }
    
     public bool Next(Token.Type type)
    {
        if (position<end&& LookAhead(1).Tipo==type)
        {
            position++;
            return true;
        }
        return false;
    }
    // public bool Next(Token.Type type,int f)
    // {
    //     if (position<f&& LookAhead(1).Tipo==type)
    //     {
    //         position++;
    //         return true;
    //     }
    //     return false;
    // }
    public Token LookAhead(int k=0)
    {
        return tokens[position+k];
    }
    public bool CanLookAhead(int k=0)
    {
        return end+1-position>k;
    }

    public bool Next(string value)
    {
        if (position<end && LookAhead(1).Value==value)
        {
            position++;
            return true;
        }
        return false;
    }
    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < end; i++)
        {
            yield return tokens[i];
        }
    }
     IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}