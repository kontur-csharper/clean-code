﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace Markdown
{
    internal class MdTests
    {
        [Test]
        public void Md_ShoudRenderHeadingStyle_InTextWithOneParagraph()
        {
            Md.Render("#heading").Should().BeEquivalentTo("<h1>heading</h1>");
        }

        [Test]
        public void Md_ShoudRenderHeadingStyle_InTextWithMoreThanOneParagraph()
        {
            Md.Render("#heading1\nnotheading\n#heading2").Should().
                BeEquivalentTo("<h1>heading1</h1>\nnotheading\n<h1>heading2</h1>");
        }

        [Test]
        public void Md_ShouldRenderBoldStyle_WithOneWordInContent()
        {
            Md.Render("__bold__").Should().BeEquivalentTo("<strong>bold</strong>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_WithOneWordInContent()
        {
            Md.Render("_italic_").Should().BeEquivalentTo("<em>italic</em>");
        }

        [Test]
        public void Md_ShoudRenderItalicStyle_InHeadingStyle()
        {
            Md.Render("#_italic_").Should().BeEquivalentTo("<h1><em>italic</em></h1>");
        }

        [Test]
        public void Md_ShoudRenderBoldStyle_InHeadingStyle()
        {
            Md.Render("#__bold__").Should().BeEquivalentTo("<h1><strong>bold</strong></h1>");
        }

        [Test]
        public void Md_ShoudRenderItalicStyle_InBoldStyle()
        {
            Md.Render("__bo_italic_ld__").Should().BeEquivalentTo("<strong>bo<em>italic</em>ld</strong>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_InBoldStyle_InHeadingStyle()
        {
            Md.Render("#__bo_italic_ld__").Should().BeEquivalentTo("<h1><strong>bo<em>italic</em>ld</strong></h1>");
        }

        [TestCase("b__old__", "b<strong>old</strong>")]
        [TestCase("b__ol__d", "b<strong>ol</strong>d")]
        [TestCase("__bol__d", "<strong>bol</strong>d")]
        public void Md_ShouldRenderBoldStyle_InsideWordWithoutDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }

        [TestCase("b__old1__", "b__old1__")]
        [TestCase("b__ol__d1", "b__ol__d1")]
        [TestCase("__bol__d1", "__bol__d1")]
        public void Md_ShouldNotRenderBoldStyle_InsideWordWithDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }
    }
}
