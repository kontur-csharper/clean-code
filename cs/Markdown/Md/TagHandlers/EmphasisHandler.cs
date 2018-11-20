﻿using System;
using System.Collections.Generic;

namespace Markdown.Md.TagHandlers
{
    public class EmphasisHandler : TagHandler
    {
        public override TokenNode Handle(string str, int position, Stack<TokenNode> openingTokens)
        {
            var result = Recognize(str, position);
            result.Value = MdSpecification.Tags[result.Type];

            if (result.Type == MdSpecification.Text)
            {
                if (Successor == null)
                {
                    throw new InvalidOperationException(
                        "Can't transfer control to the next chain element because it was null");
                }

                return Successor.Handle(str, position, openingTokens);
            }

            if (result.PairType == TokenPairType.Open)
            {
                openingTokens.Push(result);
            }

            if (result.PairType == TokenPairType.Close)
            {
                if (openingTokens.Count != 0)
                {
                    var peek = openingTokens.Peek();

                    if (peek.Type == MdSpecification.Emphasis)
                    {
                        openingTokens.Pop();

                        return result;
                    }
                }

                result.Type = MdSpecification.Text;
                result.PairType = TokenPairType.NotPair;
            }

            return result;
        }

        private static TokenNode Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return new TokenNode(MdSpecification.Text, "");
            }

            if (IsEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position - 1 < 0 || str[position - 1] != '_'))
            {
                return new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Close);
            }

            if (IsEmphasis(str, position)
                && IsOpenedEmphasis(str, position))
            {
                return new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Open);
            }

            return new TokenNode(MdSpecification.Text, "");
        }

        private static bool IsOpenedEmphasis(string str, int position)
        {
            return
                position + 1 < str.Length
                && !char.IsWhiteSpace(str[position + 1])
                && !char.IsNumber(str[position + 1])
                && (position - 1 < 0 || !char.IsLetter(str[position - 1]));
        }

        private static bool IsClosedEmphasis(string str, int position)
        {
            return
                position - 1 >= 0
                && !char.IsWhiteSpace(str[position - 1])
                && !char.IsNumber(str[position - 1])
                && (position + 1 >= str.Length || !char.IsLetter(str[position + 1]));
        }

        private static bool IsEmphasis(string str, int position)
        {
            return str[position] == '_';
        }
    }
}