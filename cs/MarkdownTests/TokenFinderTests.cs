﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class TokenFinderTests
    {
        [TestCase("__f _d_ f__", new[] { 4, 6 }, new[] { 0, 9 }, TestName = "Should find simple and double token")]
        public void FindDoubleAndSimple(string paragraph, int[] simpleUnderscorePositions,
            int[] doubleUnderscorePositions)
        {
            var finder = new InlineTokenFinder();
            var validator = new InlineTokensValidator();
            var tokensPositions = finder.FindInlineTokensInMdText(paragraph);
            var tokensWithPositions = validator.GetPositionsForTokens(tokensPositions);

            var positionsForSimpleUnderscore =
                tokensWithPositions.Where(token => token.TokenType.Name == "simpleUnderscore")
                    .Select(token => token.TokenPosition);
            var positionsForDoubleUnderscore =
                tokensWithPositions.Where(token => token.TokenType.Name == "doubleUnderscore")
                    .Select(token => token.TokenPosition);

            positionsForSimpleUnderscore.ShouldBeEquivalentTo(simpleUnderscorePositions);
            positionsForDoubleUnderscore.ShouldBeEquivalentTo(doubleUnderscorePositions);
        }

        [Test]
        public void ShouldFindDoubleUnderscore()
        {
            var paragraph = "__f__";

            var finder = new InlineTokenFinder();
            var validator = new InlineTokensValidator();
            var tokensPositions = finder.FindInlineTokensInMdText(paragraph);
            var tokensWithPositions = validator.GetPositionsForTokens(tokensPositions);

            tokensWithPositions.Where(token => token.TokenType.Name == "doubleUnderscore")
                .Select(token => token.TokenPosition)
                .ShouldBeEquivalentTo(new []{0, 3});
        }

        [TestCase("_ff\\_", TestName = "Should not find finishing token with screening")]
        [TestCase("\\_ff_", TestName = "Should not find starting token with screening")]
        [TestCase("f_", TestName = "Should not find token without starting token")]
        [TestCase("_f", TestName = "Should not find token without finishing token")]
        public void FindSimpleUnderscore(string paragraph)
        {
            var finder = new InlineTokenFinder();
            var validator = new InlineTokensValidator();
            var tokensPositions = finder.FindInlineTokensInMdText(paragraph);
            var tokensWithPositions = validator.GetPositionsForTokens(tokensPositions);

            tokensWithPositions.Select(token => token.TokenType.Name).Should().NotContain("simpleUnderscore");
        }

        [TestCase("_ff_", new[] { 0, 3 }, TestName = "Should find simple token")]
        [TestCase("_f _f_ _f_ f_", new[] { 0, 12, 3, 5, 7, 9 }, TestName =
            "Should find multiple token on one nesting level")]
        [TestCase("_f _f _f_ f_ f_", new[] { 0, 14, 3, 11, 6, 8 }, TestName = "Should find multiple nesting")]
        public void FindSimpleUnderscore(string paragraph, int[] positions)
        {

            var finder = new InlineTokenFinder();
            var validator = new InlineTokensValidator();
            var tokensPositions = finder.FindInlineTokensInMdText(paragraph);
            var tokensWithPositions = validator.GetPositionsForTokens(tokensPositions);

            tokensWithPositions.Where(token => token.TokenType.Name == "simpleUnderscore")
                .Select(token => token.TokenPosition)
                .ShouldBeEquivalentTo(positions);
        }
    }
}