using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parsing.Ast;
using Core.Tokenization;

namespace Core.Parsing;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _currentTokenIndex;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokenIndex = 0;
    }

    public ExpressionNode Parse()
    {
        var expression = ParseExpression();
        if (HasMoreTokens())
        {
            throw new InvalidOperationException("Unexpected tokens at end of expression");
        }
        return expression;
    }

    private ExpressionNode ParseExpression(int minPrecedence = 0)
    {
        var left = ParsePrimary();

        while (HasMoreTokens())
        {
            var token = PeekToken();
            if (token.Type != TokenType.Operator) break;

            var precedence = GetPrecedence(token.Value);
            if (precedence < minPrecedence) break;

            ConsumeToken();
            var right = ParseExpression(precedence + 1);
            left = new BinaryExpressionNode(left, token.Value, right);
        }

        return left;
    }

    private ExpressionNode ParsePrimary()
    {
        var token = PeekToken();

        // Handle negation operator
        if (token.Type == TokenType.Operator && token.Value == "!")
        {
            ConsumeToken();
            var operand = ParsePrimary();
            return new UnaryExpressionNode("!", operand);
        }

        if (token.Type == TokenType.LeftParenthesis)
        {
            ConsumeToken();
            var expression = ParseExpression();
            if (PeekToken().Type != TokenType.RightParenthesis)
            {
                throw new InvalidOperationException("Expected closing parenthesis");
            }
            ConsumeToken();
            return new ParenthesizedExpressionNode(expression);
        }

        if (IsLiteral(token) || token.Type == TokenType.Identifier)
        {
            ConsumeToken();
            return new LiteralNode(token.Type, token.Value);
        }

        throw new InvalidOperationException($"Unexpected token: {token.Type} '{token.Value}'");
    }
    
    private ExpressionNode CreateOperatorNode(Token token)
    {
        return new BinaryExpressionNode(null, token.Value, null);
    }

    private static readonly HashSet<string> ValidOperators = new()
    {
        "==", "!=", "<", ">", "<=", ">=", "&&", "||", 
        "+", "-", "*", "/", "!"  // Add mathematical operators
    };

    private int GetPrecedence(string op)
    {
        if (!ValidOperators.Contains(op))
        {
            var suggestion = op == "=" ? " Did you mean '=='?" : "";
            throw new InvalidOperationException(
                $"Unsupported operator '{op}'. Supported operators are: {string.Join(", ", ValidOperators)}.{suggestion}");
        }

        return op switch
        {
            "||" => 1,
            "&&" => 2,
            "==" or "!=" => 3,
            "<" or ">" or "<=" or ">=" => 4,
            "+" or "-" => 5,
            "*" or "/" => 6,
            _ => throw new InvalidOperationException($"Unknown operator: {op}")
        };
    }

    private bool IsLiteral(Token token)
    {
        return token.Type == TokenType.Number ||
               token.Type == TokenType.String ||
               token.Type == TokenType.Boolean ||
               token.Type == TokenType.Null;
    }

    private bool HasMoreTokens()
    {
        return _currentTokenIndex < _tokens.Count;
    }

    private Token PeekToken()
    {
        if (!HasMoreTokens())
        {
            throw new InvalidOperationException("Unexpected end of input");
        }

        return _tokens[_currentTokenIndex];
    }

    private void ConsumeToken()
    {
        _currentTokenIndex++;
    }
}