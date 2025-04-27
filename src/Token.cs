namespace interpreter;

public class Token
{
    public readonly TokenType Type;
    public readonly string Lexeme;
    public readonly string? Literal;
    //Stores the location of the token. To be used for error handling
    public readonly int Line;

    public Token(TokenType type, string lexeme, string? literal, int line)
    {
        this.Type = type;
        this.Lexeme = lexeme;
        this.Literal = literal;
        this.Line = line;
    }

    public override string ToString()
    {
        return Type + " " + Lexeme + " " + (Literal ?? "null");
    }
}