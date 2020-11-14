﻿using System.Security.Authentication.ExtendedProtection;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class ReadHeaderToken
    {
        [Test]
        public void ReadHeaderToken_ShouldReturnEmptyArray_OnSimpleString()
        {
            MarkdownParser.ReadHeaderToken("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadHeaderToken_ShouldReturnCorrectArray_OnHeaderSting()
        {
            var result = MarkdownParser.ReadHeaderToken("#asdf");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new Tag("<h1>", 0));
            result[1].Should().BeEquivalentTo(new Tag("</h1>", 4));
        }
        
    }
}