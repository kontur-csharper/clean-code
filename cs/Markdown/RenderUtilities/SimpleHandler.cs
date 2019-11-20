﻿using Markdown.RenderUtilities.TokenHandleDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class SimpleHandler: ITokenHandler
    {
        private readonly List<MarkdownSimpleTokenHandleDescription> handleDescriptions;
        private readonly Dictionary<TokenType, MarkdownSimpleTokenHandleDescription> typeToHandler;

        public SimpleHandler(IEnumerable<MarkdownSimpleTokenHandleDescription> handleDescriptions)
        {
            this.handleDescriptions = handleDescriptions.ToList();
            typeToHandler = this.handleDescriptions.ToDictionary(token => token.TokenType);
        }

        public List<TokenType> GetAcceptedTokenTypes()
        {
            return handleDescriptions.Select(token => token.TokenType).ToList();
        }

        public void InitHandle()
        {
            return;
        }

        public void HandleToken(List<Token> tokens, int tokenIndex)
        {
            return;
        }

        public void EndHandle()
        {
            return;
        }

        public bool TryGetTokenString(List<Token> tokens, int currentTokenIndex, out string tokenString)
        {
            tokenString = null;
            MarkdownSimpleTokenHandleDescription handler = null;
            if (!typeToHandler.TryGetValue(tokens[currentTokenIndex].TokenType, out handler))
                return false;
            tokenString = handler.GetRenderedTokenText(tokens[currentTokenIndex]);
            return true;
        }
    }
}
