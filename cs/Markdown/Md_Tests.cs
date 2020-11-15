﻿using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Tests
    {
        private Md md;
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("hello")]
        [TestCase("Hello")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("hello world!")]
        public void Render_ReturnsInputValue_OnNoTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("_hello_", "<em>hello<\\em>")]
        [TestCase("_Hello_ _world_", "<em>Hello<\\em> <em>world<\\em>")]
        [TestCase("_Hello world_", "<em>Hello world<\\em>")]
        [TestCase("_Italic_ NotItalic", "<em>Italic<\\em> NotItalic")]
        [TestCase("_Italic_ NotItalic _Italic_", "<em>Italic<\\em> NotItalic <em>Italic<\\em>")]
        public void Render_SupportsItalicTag_OnDifferentWords(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("nn_III_nn", "nn<em>III<\\em>nn")]
        [TestCase("nn_III_", "nn<em>III<\\em>")]
        [TestCase("_III_nn", "<em>III<\\em>nn")]
        public void Render_SupportsItalicTag_InsideOneWord(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("__hello__", "<strong>hello<\\strong>")]
        [TestCase("__Hello__ __world__", "<strong>Hello<\\strong> <strong>world<\\strong>")]
        [TestCase("__Hello world__", "<strong>Hello world<\\strong>")]
        [TestCase("__Strong__ NotStrong", "<strong>Strong<\\strong> NotStrong")]
        [TestCase("__Strong__ NotStrong __Strong__", "<strong>Strong<\\strong> NotStrong <strong>Strong<\\strong>")]
        public void Render_SupportsStrongTag_OnDifferentWords(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("nn__SSS__nn", "nn<strong>SSS<\\strong>nn")]
        [TestCase("nn__SSS__", "nn<strong>SSS<\\strong>")]
        [TestCase("__SSS__nn", "<strong>SSS<\\strong>nn")]
        public void Render_SupportsStrongTag_InsideOneWord(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("hel__lo wo__rld")]
        [TestCase("hel_lo wo_rld")]
        public void Render_ReturnsInputValue_OnTagsInsideTwoDifferentWords(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__Strong _Intersection__ Italic_")]
        public void Render_ReturnsInputValue_OnIntersectionTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("hel__12__lo")]
        [TestCase("hel_12_lo")]
        public void Render_ReturnsInputValue_OnNumberInTagInsideWord(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__")]
        [TestCase("____")]
        public void Render_ReturnsInputValue_OnEmptyTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello")]
        [TestCase("_hello")]
        [TestCase("hello__")]
        [TestCase("hello_")]
        [TestCase("hel__lo")]
        [TestCase("hel_lo")]
        public void Render_ReturnsInputValue_OnSingleTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello \n world__")]
        [TestCase("_hello \n world_")]
        [TestCase("__hello_ \n __world_")]
        public void Render_ReturnsInputValue_OnSingleTags_SupportingNewLine(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello __")]
        [TestCase("__ hello__")]
        [TestCase("_hello _")]
        [TestCase("_ hello_")]
        public void Render_ReturnsInputValue_OnWhiteSpacesAroundTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__Hello _my_ world__", "<strong>Hello <em>my<\\em> world<\\strong>")]
        [TestCase("_Hello __my__ world_", "_Hello __my__ world_")]
        [TestCase("___Hello my_ world__", "<strong><em>Hello my<\\em> world<\\strong>")]
        [TestCase("__Hello _my world___", "<strong>Hello <em>my world<\\em><\\strong>")]
        [TestCase("___Hello my world___", "<strong><em>Hello my world<\\em><\\strong>")]
        [TestCase("___Hello my__ world_", "___Hello my__ world_")]
        public void Render_SupportsNestedTags(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("#Hello", "<h1>Hello<\\h1>")]
        [TestCase("#_Hello_", "<h1><em>Hello<\\em><\\h1>")]
        [TestCase("#__Hello__", "<h1><strong>Hello<\\strong><\\h1>")]
        [TestCase("#__Hello _my_ world__", "<h1><strong>Hello <em>my<\\em> world<\\strong><\\h1>")]
        [TestCase("#Hello\n world", "<h1>Hello<\\h1> world")]
        [TestCase("_#Hello_", "_#Hello_")]
        [TestCase("__#Hello__", "__#Hello__")]
        public void Render_SupportsHeaderTag(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }
    }
}
