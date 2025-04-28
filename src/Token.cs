using System.Globalization;

namespace interpreter;

public class Token
{
    public readonly TokenType Type;
    public readonly string Lexeme;
    public readonly object? Literal;
    //Stores the location of the token. To be used for error handling
    public readonly int Line;

    public Token(TokenType type, string lexeme, object? literal, int line)
    {
        this.Type = type;
        this.Lexeme = lexeme;
        this.Literal = literal;
        this.Line = line;
    }

    public override string ToString()
    {
        string? literalStr;
        if (Literal == null)
        {
            literalStr = "null";
        }
        else if (Literal is double d)
        {
            // if it’s mathematically an integer, force one decimal place
            if (d % 1 == 0)
                literalStr = d.ToString("F1", CultureInfo.InvariantCulture);
            else
                literalStr = d.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            literalStr = Literal.ToString();
        }

        return $"{Type} {Lexeme} {literalStr}";
    }
}