namespace Core.Tokenization;

using System;
using System.Collections.Generic;
using System.Text;

public class StringTokenizer(string input)
{
    private int _position = 0;
    private readonly string _input = input?.Trim() ?? string.Empty;

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_position < _input.Length)
        {
            char current = _input[_position];

            if (char.IsWhiteSpace(current))
            {
                _position++;
                continue;
            }

            if (char.IsLetter(current) || current == '_')
            {
                tokens.Add(ReadIdentifier());
            }
            else if (char.IsDigit(current))
            {
                tokens.Add(ReadNumber());
            }
            else if (current == '"')
            {
                tokens.Add(ReadString());
            }
            else if (IsOperatorCharacter(current))
            {
                tokens.Add(ReadOperator());
            }
            else if (current == '(' || current == ')')
            {
                tokens.Add(ReadParenthesis());
            }
            else
            {
                throw new InvalidOperationException($"Unexpected character: '{current}' at position {_position}");
            }
        }

        return tokens;
    }

    private Token ReadIdentifier()
    {
        var start = _position;
        while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
        {
            _position++;
        }

        string value = _input.Substring(start, _position - start);

        // Check for boolean literals
        if (value == "true" || value == "false")
        {
            return new Token(TokenType.Boolean, value);
        }
        else if (value == "null")
        {
            return new Token(TokenType.Null, value);
        }

        return new Token(TokenType.Identifier, value);
    }

    private Token ReadNumber()
    {
        var start = _position;
        bool hasDecimal = false;

        while (_position < _input.Length)
        {
            char current = _input[_position];
            if (char.IsDigit(current))
            {
                _position++;
            }
            else if (current == '.' && !hasDecimal)
            {
                hasDecimal = true;
                _position++;
            }
            else
            {
                break;
            }
        }

        return new Token(TokenType.Number, _input.Substring(start, _position - start));
    }

    private Token ReadString()
    {
        var sb = new StringBuilder();
        _position++; // Skip opening quote

        while (_position < _input.Length && _input[_position] != '"')
        {
            if (_input[_position] == '\\' && _position + 1 < _input.Length)
            {
                // Handle escape sequences
                sb.Append(_input[_position + 1] switch
                {
                    '"' => '"',
                    '\\' => '\\',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    _ => throw new InvalidOperationException($"Invalid escape sequence: \\{_input[_position + 1]}")
                });
                _position += 2;
            }
            else
            {
                sb.Append(_input[_position++]);
            }
        }

        if (_position >= _input.Length || _input[_position] != '"')
        {
            throw new InvalidOperationException("Unterminated string literal");
        }

        _position++; // Skip closing quote
        return new Token(TokenType.String, sb.ToString());
    }

    private Token ReadOperator()
    {
        char current = _input[_position];
        char? next = _position + 1 < _input.Length ? _input[_position + 1] : null;

        // Handle multi-character operators first
        if (next.HasValue && IsMultiCharOperator(current, next.Value))
        {
            string op = new string(new[] { current, next.Value });
            _position += 2;
            return new Token(TokenType.Operator, op);
        }

        // Handle single-character operators
        if (IsOperatorCharacter(current))
        {
            _position++;
            return new Token(TokenType.Operator, current.ToString());
        }

        throw new InvalidOperationException($"Invalid operator sequence at position {_position}");
    }

    private Token ReadParenthesis()
    {
        char current = _input[_position++];
        return new Token(
            current == '(' ? TokenType.LeftParenthesis : TokenType.RightParenthesis,
            current.ToString()
        );
    }

    private static bool IsOperatorCharacter(char c)
    {
        return c == '>' || c == '<' || c == '=' || c == '!' || c == '&' || c == '|' || 
               c == '*' || c == '/' || c == '+' || c == '-';
    }
    
    private static bool IsMultiCharOperator(char first, char second)
    {
        return (first == '>' && second == '=') ||  // >=
               (first == '<' && second == '=') ||  // <=
               (first == '=' && second == '=') ||  // ==
               (first == '!' && second == '=') ||  // !=
               (first == '&' && second == '&') ||  // &&
               (first == '|' && second == '|');    // ||
    }
}