﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extentions;

namespace Markdown
{
    public class TokenParser
    {
        public bool IsTokenCorrupted { get; set; }
        public string PartBeforeTokenStart { get; set; }
        public string PartAfterTokenEnd { get; set; }

        protected Func<string, bool> nestedTokenValidator;
        protected int corruptedOffset;

        public virtual Token ParseToken(List<string> text, int position)
        {
            return new Token(0, "", TokenType.Simple);
        }

        protected Token ParseToken(List<string> text, int position,
            StringBuilder tokenValue, TokenType type)
        {
            var parserOperator = new ParserOperator();
            if (text.Count == 0)
                return CreateEmptyToken(tokenValue, position, parserOperator);
            CollectToken(text, tokenValue, parserOperator);
            var value = tokenValue.ToString();
            if (CheckCorrectTokenValue(tokenValue, parserOperator, text.FirstOrDefault(), text.LastOrDefault()))
            {
                RecoverTokenValue(tokenValue, parserOperator);
                type = TokenType.Simple;
                value = tokenValue.ToString();
            }
            var nestedTokens = parserOperator.GetTokens();
            var token = new Token(position, value, type);
            token.SetNestedTokens(nestedTokens);
            return token;
        }

        protected virtual void CollectToken(List<string> text,
            StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            var offset = IsTokenCorrupted ? corruptedOffset : 0;
            foreach (var bigram in text.GetBigrams())
            {
                var part = bigram.Item1;
                if (nestedTokenValidator(part))
                {
                    if (part == "\\")
                        parserOperator.Position = offset;
                    if (isIntoToken)
                    {
                        parserOperator.Position = offset;
                        parserOperator.AddTokenPart(bigram);
                    }
                    isIntoToken = !isIntoToken;
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part);
                    offset += part.Length;
                }
                if (isIntoToken)
                    parserOperator.AddTokenPart(bigram);
            }
            parserOperator.Position = offset;
        }

        protected virtual void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, "__");
            value.Append("__");
        }

        private bool CheckCorrectDeclaration(string start, string end)
        {
            if (start == null && end == null) return true;
            return ParserOperator.IsCorrectEnd(end) &&
                ParserOperator.IsCorrectStart(start);
        }

        private bool IsTokenInPartWord(string start, string value)
        {
            return PartBeforeTokenStart != null &&
                CheckCorrectDeclaration(start, PartBeforeTokenStart) &&
                value.Contains(" ");
        }

        private Token CreateEmptyToken(StringBuilder tokenValue, int position, ParserOperator parserOperator)
        {
            RecoverTokenValue(tokenValue, parserOperator);
            return new Token(position, tokenValue.ToString(), TokenType.Simple);
        }

        private bool CheckCorrectTokenValue(StringBuilder value, ParserOperator parserOperator,
            string tokenStart, string tokenEnd)
        {
            var tokenValue = value.ToString();
            var isInsideDigitText = tokenValue.IsDigit() && (PartAfterTokenEnd?.IsDigit() ?? false);
            var isInsideInDifferentPartWords = IsTokenInPartWord(tokenStart, tokenValue);
            var isHaveCorrectStartAndEnd = !CheckCorrectDeclaration(tokenStart, tokenEnd);
            var isHaveUnpairedChars = !parserOperator.IsClose() && !IsTokenCorrupted;

            return isHaveCorrectStartAndEnd 
                || isHaveUnpairedChars 
                || isInsideDigitText 
                || isInsideInDifferentPartWords;
        }
    }
}
