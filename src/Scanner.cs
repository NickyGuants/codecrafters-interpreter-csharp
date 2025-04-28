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
        while (!string.IsNullOrWhiteSpace(source) && !IsAtEnd())
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
                if (source.Length > current && peekNextChar() == '/')
                {
                    while (!IsAtEnd() && peekNextChar() != '\n')
                    {
                        advance();
                    }
                }
                else
                {
                    addToken(TokenType.SLASH);
                }
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
            case '\t':
            case '\r':
            case ' ':
                break;
            case '\n':
                line++;
                break;
            case '"':
                handle_string();
                break;
            default:
                if (IsDigit(c))
                {
                    handle_digit();
                }
                else if(IsAlpha(c))
                {
                    handle_identifier();
                }
                else
                {
                    Console.Error.WriteLine("[line " + line +"]" + " Error: Unexpected character: " + c);
                    hasError = true;
                }
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

    private void addToken(TokenType type, object? literal)
    {
        string text = source.Substring(start, current-start);
        tokens.Add(new Token(type, text, literal, line));
    }
    
    private char peekNextChar()
    {
        if (IsAtEnd()) return '\0';
        return source.ToCharArray()[current];
    }
    
    private char peekNext() {
        if (current + 1 >= source.Length) return '\0';
        return source.ToCharArray()[current +1];
    }

    private void handle_string()
    {
        var startString = current;
        while (!IsAtEnd() && peekNextChar() != '"')
        {
            if (peekNextChar() == '\n')
            {
                line++;
            }
            advance();
        }
        
        if (IsAtEnd())
        {
            Console.Error.WriteLine("[line " + line +"]" + " Error: Unterminated string.");
            hasError = true;
            return;
        }

        advance();

        string value = source.Substring(startString, current-startString-1);
        
        addToken(TokenType.STRING, value);
    }

    private void handle_digit()
    {
        while (IsDigit(peekNextChar()) || peekNextChar() == '.')
        {
            advance();
            
            if (peekNextChar() == '.' && IsDigit(peekNext()))
            {
                advance();
            }

            while (IsDigit(peekNextChar()))
            {
                advance();
            }
        }

        var value = Double.Parse(source.Substring(start, current-start));
        
        addToken(TokenType.NUMBER, value);
    }

    private void handle_identifier()
    {
        while (IsAlphaNumeric(peekNextChar()))
        {
            advance();
        }

        addToken(TokenType.IDENTIFIER);
    }

    private bool IsDigit(char c)
    {
        return c is >= '0' and <= '9';
    }

    private bool IsAlpha(char c)
    {
        return c is >= 'a' and <= 'z' || c is >= 'A' and <= 'Z' || c == '_';
    }
    
    private bool IsAlphaNumeric(char c) {
        return IsAlpha(c) || IsDigit(c);
    }

    public bool HasError()
    {
        return hasError;
    }
}