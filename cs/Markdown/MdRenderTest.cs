﻿using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdRenderTest
    {
        private Md md;
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }
        
        [Test]
        public void Render_CorrectWork_OnNotStyledText()
        {
            md.Render("asdf\nasdf").Should().Be("asdfasdf");
        }

        [Test]
        public void Render_CorrectWork_WithHeaderStrings()
        {
            md.Render("asdf\n#asdf").Should().Be("asdf<h1>asdf</h1>");
            
        }

        [Test]
        public void Render_CorrectWork_WithItalicToken()
        {
            md.Render("_asdf_").Should().Be("<em>asdf</em>");
        }

        [Test]
        public void Render_CorrectWork_WithBoldToken()
        {
            md.Render("__asdf__").Should().Be("<strong>asdf</strong>");
        }

        [Test]
        public void Render_CorrectWork_OnItalicTokenInBold()
        {
            md.Render("__one_two_three__").Should()
                .Be("<strong>one<em>two</em>three</strong>");
        }

        [Test]
        public void Render_CorrectWork_OnBoldInItalic()
        {
            md.Render("_one__two__three_").Should().Be("<em>one__two__three</em>");
        }

        [Test]
        public void Render_CorrectWork_OnStringWithHeaderBoldAndItalicTokens()
        {
            md.Render("#_one_ __two _three_ four__")
                .Should().Be("<h1><em>one</em> <strong>two <em>three</em> four</strong></h1>");
        }

        [Test]
        public void Render_ShouldIgnore_ShieldItalicTokenStart()
        {
            md.Render(@"one\_two_").Should().Be("one_two_");
        }

        [Test]
        public void Render_ShouldIgnore_ShieldItalicTokenEnd()
        {
            md.Render(@"one_two\_").Should().Be("one_two_");
        }

        [Test]
        public void Render_ShouldIgnore_ShieldHeaderToken()
        {
            md.Render(@"\#asdf").Should().Be("#asdf");
        }

        [Test]
        public void Render_ShouldIgnoreSimpleSlash()
        {
            md.Render(@"asdf\asdf").Should().Be(@"asdf\asdf");
        }
    }
}