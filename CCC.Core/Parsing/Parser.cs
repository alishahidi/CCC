using Core.Parsing.Ast;
using Core.Tokenization;

namespace Core.Parsing;

using System;
using System.Collections.Generic;
using System.Linq;

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
        var outputQueue = new Queue<ExpressionNode>();
        var operatorStack = new Stack<Token>();

        while (HasMoreTokens())
        {
            var token = PeekToken();

            if (IsLiteral(token))
            {
                outputQueue.Enqueue(new LiteralNode(token.Type, token.Value));
                ConsumeToken();
            }
            else if (token.Type == TokenType.Identifier)
            {
                outputQueue.Enqueue(new LiteralNode(token.Type, token.Value));
                ConsumeToken();
            }
            else if (token.Type == TokenType.Operator)
            {
                while (operatorStack.Count > 0 && 
                       operatorStack.Peek().Type == TokenType.Operator &&
                       GetPrecedence(operatorStack.Peek().Value) >= GetPrecedence(token.Value))
                {
                    outputQueue.Enqueue(CreateOperatorNode(operatorStack.Pop()));
                }
                operatorStack.Push(token);
                ConsumeToken();
            }
            else if (token.Type == TokenType.LeftParenthesis)
            {
                operatorStack.Push(token);
                ConsumeToken();
            }
            else if (token.Type == TokenType.RightParenthesis)
            {
                while (operatorStack.Count > 0 && operatorStack.Peek().Type != TokenType.LeftParenthesis)
                {
                    outputQueue.Enqueue(CreateOperatorNode(operatorStack.Pop()));
                }

                if (operatorStack.Count == 0)
                {
                    throw new InvalidOperationException("Mismatched parentheses");
                }

                operatorStack.Pop(); // Discard the left parenthesis
                ConsumeToken();
            }
        }

        // No more tokens to read, pop remaining operators
        while (operatorStack.Count > 0)
        {
            if (operatorStack.Peek().Type == TokenType.LeftParenthesis)
            {
                throw new InvalidOperationException("Mismatched parentheses");
            }
            outputQueue.Enqueue(CreateOperatorNode(operatorStack.Pop()));
        }

        // Build the AST from the output queue
        var astStack = new Stack<ExpressionNode>();
        while (outputQueue.Count > 0)
        {
            var node = outputQueue.Dequeue();
            if (node is BinaryExpressionNode binaryNode && binaryNode.Left == null)
            {
                // Operator node - pop operands
                if (astStack.Count < 2)
                {
                    throw new InvalidOperationException("Invalid expression");
                }
                var right = astStack.Pop();
                var left = astStack.Pop();
                binaryNode.Left = left;
                binaryNode.Right = right;
            }
            astStack.Push(node);
        }

        if (astStack.Count != 1)
        {
            throw new InvalidOperationException("Invalid expression");
        }

        return astStack.Pop();
    }

    private ExpressionNode CreateOperatorNode(Token token)
    {
        return new BinaryExpressionNode(null, token.Value, null);
    }

    private int GetPrecedence(string op)
    {
        return op switch
        {
            "||" => 1,
            "&&" => 2,
            "==" or "!=" => 3,
            "<" or ">" or "<=" or ">=" => 4,
            _ => 0
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