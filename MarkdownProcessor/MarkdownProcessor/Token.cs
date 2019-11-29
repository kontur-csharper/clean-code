﻿using System;
using System.Collections.Generic;
using Markdown.Wraps;

namespace MarkdownProcessor
{
    public class Token
    {
        private readonly int tokenStartIndex;

        public Token(int tokenStartIndex, IWrapType wrapType, Token parentToken = null)
        {
            this.tokenStartIndex = tokenStartIndex;
            WrapType = wrapType;
            ParentToken = parentToken;
            ChildTokens = new List<Token>();
        }

        public IWrapType WrapType { get; }
        public string Content { get; set; }
        public Token ParentToken { get; }
        public List<Token> ChildTokens { get; set; }

        public int ContentStartIndex => tokenStartIndex + WrapType.OpenWrapMarker.Length;

        public int ContentEndIndex =>
            Content is null
                ? throw new NotSupportedException($"You should to initialise {nameof(Content)} property before.")
                : ContentStartIndex + Content.Length - 1;
    }
}