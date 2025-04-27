namespace interpreter;

public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private bool hasError = false;

    public Scanner(string source)
    {
        this.source = source;
    }

    public List<Token> scanTokens()
    {
        while (!string.IsNullOrEmpty(source) && !IsAtEnd())
        {
            start = current;
            scanToken();
        }
        
        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    private void scanToken()
    {
        char c = advance();

        switch (c)
        {
            case '(':
                addToken(TokenType.LEFT_PAREN);
                break;
            case ')':
                addToken(TokenType.RIGHT_PAREN);
                break;
            case '{':
                addToken(TokenType.LEFT_BRACE);
                break;
            case '}':
                addToken(TokenType.RIGHT_BRACE);
                break;
            case ',':
                addToken(TokenType.COMMA);
                break;
            case '.':
                addToken(TokenType.DOT);
                break;
            case '-':
                addToken(TokenType.MINUS);
                break;
            case '+':
                addToken(TokenType.PLUS);
                break;
            case ';':
                addToken(TokenType.SEMICOLON);
                break;
            case '/':
                addToken(TokenType.SLASH);
                break;
            case '*':
                addToken(TokenType.STAR);
                break;
            case '=':
                //check the next character
                if (source.Length > current && peekNextChar() == '=')
                {
                    advance();
                    addToken(TokenType.EQUAL_EQUAL);
                }
                else
                {
                    addToken(TokenType.EQUAL);
                }
                break;
            case '!':
                if (source.Length > current && peekNextChar() == '=')
                {
                    advance();
                    addToken(TokenType.BANG_EQUAL);
                }
                else
                {
                    addToken(TokenType.BANG);
                }
                break;
            case '<':
                if (source.Length > current && peekNextChar() == '=')
                {
                    advance();
                    addToken(TokenType.LESS_EQUAL);
                }
                else
                {
                    addToken(TokenType.LESS);
                }
                break;
            case '>':
                if (source.Length > current && peekNextChar() == '=')
                {
                    advance();
                    addToken(TokenType.GREATER_EQUAL);
                }
                else
                {
                    addToken(TokenType.GREATER);
                }
                break;
            default:
                Console.Error.WriteLine("[line " + line +"]" + " Error: Unexpected character: " + c);
                hasError = true;
                break;
        }
    }

    private char advance()
    {
        return source.ToCharArray()[current++];
    }

    private void addToken(TokenType type)
    {
        addToken(type, null);
    }

    private void addToken(TokenType type, string? literal)
    {
        string text = source.Substring(start, current-start);
        tokens.Add(new Token(type, text, literal, line));
    }
    
    private char peekNextChar()
    {
        return source.ToCharArray()[current];
    }

    public bool HasError()
    {
        return hasError;
    }
}