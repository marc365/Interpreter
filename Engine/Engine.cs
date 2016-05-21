/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 ___________________________________________________ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HiSystems.Interpreter
{

    public class Engine
    {


        private class OperatorAndPrecedence
        {
            public Operator Operation;
            public int Precedence;
        }

        #region Translated Tokens

        private abstract class TranslatedToken
        {
        }

        private class ConstructToken : TranslatedToken
        {
            private IConstruct value;

            public ConstructToken(IConstruct value)
            {
                this.value = value;
            }

            public IConstruct Value
            {
                get
                {
                    return this.value;
                }
            }

            public override string ToString()
            {
                return this.value.ToString();
            }
        }

        private class OperatorToken : TranslatedToken
        {
            private Operator operation;

            public OperatorToken(Operator operation)
            {
                if (operation == null)
                    throw new ArgumentNullException();

                this.operation = operation;
            }

            public Operator Value
            {
                get
                {
                    return this.operation;
                }
            }

            public override string ToString()
            {
                return this.operation.Token.ToString();
            }
        }

        private class LeftParenthesisToken : TranslatedToken
        {
            public override string ToString()
            {
                return "(";
            }
        }

        private class RightParenthesisToken : TranslatedToken
        {
            public override string ToString()
            {
                return ")";
            }
        }

        #endregion

        private class ReservedWord
        {
            public string Word;
            public IConstruct Construct;
        }

        private static OperatorAndPrecedence[] allOperators = new []
        {                   
            new OperatorAndPrecedence() { Operation = new MultiplyOperator(), Precedence = 6 },
            new OperatorAndPrecedence() { Operation = new DivideOperator(), Precedence = 6 },
            new OperatorAndPrecedence() { Operation = new ModulusOperator(), Precedence = 6 },
            new OperatorAndPrecedence() { Operation = new AddOperator(), Precedence = 5 },
            new OperatorAndPrecedence() { Operation = new SubtractOperator(), Precedence = 5 },
            new OperatorAndPrecedence() { Operation = new LessThanOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new LessThanOrEqualToOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new GreaterThanOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new GreaterThanOrEqualToOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new EqualToOperator(), Precedence = 3 },
            new OperatorAndPrecedence() { Operation = new NotEqualToOperator(), Precedence = 3 },
            new OperatorAndPrecedence() { Operation = new AndOperator(), Precedence = 2 },
            new OperatorAndPrecedence() { Operation = new OrOperator(), Precedence = 1 },
            new OperatorAndPrecedence() { Operation = new EqualsOperator(), Precedence = 0 },
        };

        private static ReservedWord[] reservedWords = new[] 
        {
            new ReservedWord() { Word = "true", Construct = new Boolean(true) },
            new ReservedWord() { Word = "false", Construct = new Boolean(false) }
        };

        private static int parenthesisPrecendence;

        static Engine() 
        {
            // Parentheses have higher precedence than all operations
            parenthesisPrecendence = allOperators
                .Select(item => item.Precedence)
                .Max() + 1;
        }

        public Expression Parse(string expression)
        {
            return new ExpressionParsed(expression, ParseToConstruct(expression));
        }

        private IConstruct ParseToConstruct(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return new Error(string.Empty);
            return GetConstructFromTokens(Tokenizer.Parse(expression));
        }

        private IConstruct GetConstructFromTokens(List<Token> tokens)
        {
            // Translate the tokens to meaningful tokens such as a variables, functions and operators
            // Unknown or unexpected tokens will cause an exception to be thrown
            var translatedTokens = TranslateTokens(tokens);

            // If there is only one item in the expression (i.e. a function or number and no operations)
            if (translatedTokens.Count == 1)
            {
                if (!(translatedTokens[0] is ConstructToken))
                {
                    return new Error("Construct error");
                }

                return ((ConstructToken)translatedTokens[0]).Value;
            }
            else
            {
                // Converts the tokens to the initial flat tree structure. 
                // The tree structure is flat (one level) and each Expression node is returned in the list.
                var expressions = TranslateTokensToOperations(translatedTokens);

                if (expressions.Count == 0 )
                    return new Error("Expression error");

                // Using the parentheses from the translated tokens determine the expression ordering
                SetExpressionPrecedenceFromParentheses(expressions.GetEnumerator(), translatedTokens.GetEnumerator(), depth: 0);

                // Set the ordering precedence based on the 
                SetExpressionPrecedenceFromOperators(expressions.GetEnumerator(), translatedTokens.GetEnumerator());

                // Enumerate through the ordered nodes and branch tree appropriately
                return TranslateToTreeUsingPrecedence(expressions);
            }
        }
 
        private List<TranslatedToken> TranslateTokens(List<Token> tokens)
        {
            var translatedTokens = new List<TranslatedToken>();
            var tokensEnum = new PeekableEnumerator<Token>(tokens);

            bool isInitial = true;

            while (tokensEnum.MoveNext())
            {
                var token = tokensEnum.Current;

                switch (token.Type)
                {
                    case TokenType.Number:
                        translatedTokens.Add(new ConstructToken(Number.Parse(token.Value)));
                        break;
                    case TokenType.Identifier:
                        var operationForTokenIdentifier = allOperators
                            .Select(item => item.Operation)
                            .SingleOrDefault(item => item.Token.Equals(token.Value));

                        if (operationForTokenIdentifier != null)
                            translatedTokens.Add(new OperatorToken(operationForTokenIdentifier));
                        else
                            translatedTokens.Add(new ConstructToken(TranslateIdentifierToken(tokensEnum, isInitial)));
                        break;
                    case TokenType.LeftParenthesis:
                        translatedTokens.Add(new LeftParenthesisToken());
                        break;
                    case TokenType.RightParenthesis:
                        translatedTokens.Add(new RightParenthesisToken());
                        break;
                    case TokenType.Text:
                        translatedTokens.Add(new ConstructToken(new Text(token.Value)));
                        break;
                    case TokenType.DateTime:
                        try
                        {
                            translatedTokens.Add(new ConstructToken(new DateTime(System.DateTime.Parse(token.Value))));
                        }
                        catch
                        {
                            translatedTokens.Add(new ConstructToken(new Text(token.Value)));
                        }
                        break;
                    case TokenType.Other:
                        var operationForToken = allOperators
                            .Select(item => item.Operation)
                            .SingleOrDefault(item => item.Token.Equals(token.Value));

                        if (operationForToken != null)
                            translatedTokens.Add(new OperatorToken(operationForToken));
                        else
                            Output.Text(token.Value + " in an unknown operation");

                        break;
                    case TokenType.Comma:
                        break;
                    default:
                        throw new NotImplementedException();
                }

                isInitial = false;
            }

            if (translatedTokens.Count == 0)
            {
                Output.Text(string.Format("Token Error: {0}", tokensEnum.Current));
            }
            return translatedTokens;
        }

        private static List<Operation> TranslateTokensToOperations(List<TranslatedToken> tokens)
        {
            var expectingOperation = false;
            var expressions = new List<Operation>();
            var currentExpression = new Operation();

            foreach (var token in tokens)
            {
                if (token is ConstructToken) // function, variable or number
                {
                    //added to parse normal text as well
                    //if a I'm a second ConstructToken then don't error
                    if (expectingOperation)
                    {
                        expectingOperation = false;
                    }
                    else
                    {
                        expectingOperation = true; // on next iteration an operator is expected
                    }
                }
                else if (token is OperatorToken)
                {
                    if (!expectingOperation)
                        Output.Text("Expecting value not an operation; " + token.ToString());

                    expectingOperation = false; // on next iteration an operator is not expected
                }

                if (token is ConstructToken) // function, variable or number
                {
                    if (currentExpression.LeftValue == null)
                        currentExpression.LeftValue = ((ConstructToken)token).Value;
                    else if (currentExpression.RightValue == null)
                    {
                        currentExpression.RightValue = ((ConstructToken)token).Value;
                        expressions.Add(currentExpression);
                        currentExpression = new Operation();
                        currentExpression.LeftValue = ((ConstructToken)token).Value;
                    }
                }
                else if (token is OperatorToken)
                {
                    currentExpression.Operator = ((OperatorToken)token).Value;
                }
            }

            if (expressions.Count == 0) Output.Text(string.Format("Expression error: {0}", string.Join(", ", tokens)));
            return expressions;
        }

        private static void SetExpressionPrecedenceFromParentheses(IEnumerator<Operation> expressions, IEnumerator<TranslatedToken> translatedTokens, int depth)
        {
            while (translatedTokens.MoveNext())
            {
                var token = translatedTokens.Current;

                if (token is LeftParenthesisToken)
                {
                    SetExpressionPrecedenceFromParentheses(expressions, translatedTokens, depth + 1);
                }
                else if (token is OperatorToken)
                {
                    expressions.MoveNext();
                    // find the associated expression that is using the operator object instance
                    var expression = expressions.Current;

                    if (expression == null)
                    {
                        break;
                    }
                    else
                    {
                        if (expression.PrecedenceIsSet)
                            Output.Text("Expression precedence should not be set");

                        // Set the precedence explicitly considering this is the first pass: 'expression.PrecedenceSet == false'
                        expression.Precedence = depth * parenthesisPrecendence;
                    }
                }
                else if (token is RightParenthesisToken)
                {
                    break;
                }
            }

            if (depth > 0 && !(translatedTokens.Current is RightParenthesisToken))
                Output.Text("Missing ending right parenthesis token");
        }

        private static void SetExpressionPrecedenceFromOperators(IEnumerator<Operation> expressions, IEnumerator<TranslatedToken> translatedTokens)
        {
            while (translatedTokens.MoveNext())
            {
                var token = translatedTokens.Current;

                if (token is OperatorToken)
                {
                    expressions.MoveNext();
                    // find the associated expression that is using the operator object instance
                    var expression = expressions.Current;

                    if (expression != null)
                    {
                        expression.Precedence += allOperators.Where(item => item.Operation == ((OperatorToken)token).Value).Single().Precedence;
                    }
                }
            }
        }

        private static Operation TranslateToTreeUsingPrecedence(List<Operation> expressions)
        {
            var expressionsOrdered = expressions
                .Select((item, index) => new { Expression = item, LeftToRightIndex = index })
                .OrderByDescending(item => item.Expression.Precedence)
                .ThenBy(item => item.LeftToRightIndex) // for expressions with the same precedence order from left to right
                .Select(item => item.Expression) // remove the LeftToRightIndex now that it has been ordered correctly
                .GetEnumerator();

            while (expressionsOrdered.MoveNext() && expressions.Count > 1)
            {
                var orderedExpression = expressionsOrdered.Current;
                var orderedExpressionIndex = expressions.IndexOf(orderedExpression);

                // If there is an expression before this expression in the normal left to right index
                // then get it to point to this expression rather than the Value node that is shared with another expression.
                // Effectively, the orderedExpression is "pushed" to the bottom of the tree
                if (orderedExpressionIndex > 0)
                {
                    var previousExpression = expressions[orderedExpressionIndex - 1];
                    previousExpression.RightValue = orderedExpression;
                }

                // If there is an expression after this expression in the normal left to right index
                // then get it to point to this expression rather than the Value node that is shared with another expression.
                // Effectively, the orderedExpression is "pushed" to the bottom of the tree
                if (orderedExpressionIndex < expressions.Count - 1)
                {
                    var nextExpression = expressions[orderedExpressionIndex + 1];
                    nextExpression.LeftValue = orderedExpression;
                }

                // this expression has been pushed to the bottom of the tree (for an upside down tree)
                expressions.Remove(orderedExpression);
            }

            if (expressions.Count == 0)
            {
                return new Operation();
            }

            return expressions[0];
        }

        private IConstruct[] GetFunctionArguments(PeekableEnumerator<Token> tokensEnum)
        {
            var arguments = new List<IConstruct>();
            var functionName = tokensEnum.Current.Value;

            if (!(tokensEnum.MoveNext() && tokensEnum.Current.Type == TokenType.LeftParenthesis))
                Output.Text(String.Format("{0} arguments; first token should be '(' not '{1}'", functionName, tokensEnum.Current.Value));
            else if (tokensEnum.Current.Type == TokenType.LeftParenthesis && tokensEnum.CanPeek && tokensEnum.Peek.Type == TokenType.RightParenthesis)
                // No arguments were specified - empty parentheses were specified
                tokensEnum.MoveNext(); // consume the left parenthesis token and point it to the right parenthesis token - i.e. the end of the function
            else
            {
                bool reachedEndOfArguments = false;

                while (!reachedEndOfArguments)
                {
                    arguments.Add(GetConstructFromTokens(GetFunctionArgumentTokens(functionName, tokensEnum)));

                    // tokensEnum.Current will be the last token processed by GetFunctionArgumentTokens()
                    if (tokensEnum.Current == null)
                    {
                        return new List<IConstruct>().ToArray();
                    }
                    else if (tokensEnum.Current.Type == TokenType.RightParenthesis)
                        reachedEndOfArguments = true;
                }
            }

            return arguments.ToArray();
        }

        private string[] GetCommandArguments(PeekableEnumerator<Token> tokensEnum)
        {
            var arguments = new List<string>();
            var functionName = tokensEnum.Current.Value;

            while (tokensEnum.MoveNext())
            {
                arguments.Add(tokensEnum.Current.Value);
            }

            return arguments.ToArray();
        }

        private static List<Token> GetFunctionArgumentTokens(string functionName, PeekableEnumerator<Token> tokensEnum)
        {
            var argumentTokens = new List<Token> ();

            int functionDepth = 0;
            bool reachedEndOfArgument = false;

            while (!reachedEndOfArgument && tokensEnum.MoveNext()) 
            {
                var token = tokensEnum.Current;

                // found the argument's terminating comma or right parenthesis
                if (functionDepth == 0 && (token.Type == TokenType.Comma || token.Type == TokenType.RightParenthesis))
                    reachedEndOfArgument = true;
                else
                {
                    argumentTokens.Add(token);

                    if (token.Type == TokenType.LeftParenthesis)
                        functionDepth++;
                    else if (token.Type == TokenType.RightParenthesis)
                        functionDepth--;
                }
            }

            if (argumentTokens.Count == 0)
            {
                Output.Text(String.Format("{0} has an empty argument", functionName));
            }
            else if (!reachedEndOfArgument)
            {
                Output.Text(String.Format("{0} is missing a terminating argument character; ',' or ')'", functionName));
            }
            return argumentTokens;
        }

        private IConstruct TranslateIdentifierToken(PeekableEnumerator<Token> tokensEnum, bool isInitial)
        {
            var identifierToken = tokensEnum.Current;

            var reservedWordForToken = reservedWords.SingleOrDefault(reserverWord => identifierToken == reserverWord.Word);
            var commandForToken = Program.Commands.SingleOrDefault(aCommand => identifierToken == aCommand.Name);

            if (reservedWordForToken != null)
            {
                return reservedWordForToken.Construct;
            }
            else if (commandForToken != null && isInitial)
            {
                return new CommandOperation(commandForToken, GetCommandArguments(tokensEnum));
            }
            else
            {
                if (tokensEnum.CanPeek && tokensEnum.Peek.Type == TokenType.LeftParenthesis)
                {
                    var functionForToken = Program.Functions.SingleOrDefault(aFunction => identifierToken == aFunction.Name);

                    if (functionForToken == null)
                        return new Error(String.Format("Function '{0}' is undefined", identifierToken));
                    else
                        return new FunctionOperation(functionForToken, GetFunctionArguments(tokensEnum));
                }
                else
                {
                    // ensure there is only one Variable instance for the same variable name
                    var variable = Program.Variables.SingleOrDefault(aVariable => identifierToken == aVariable.Name);

                    if (variable == null)
                    {
                       // return null;
                        var newVariable = new Variable(tokensEnum.Current.Value);
                        Program.Variables.Add(newVariable);
                        return newVariable;
                    }
                    else
                        return variable;
                }
            }
        }
    }
}