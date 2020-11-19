﻿using Markdown.Tokens;

namespace Markdown.Converters
{
    public class PlainTokenConverter : ITagTokenConverter
    {
        public string ConvertToken(IToken token)
        {
            return token.Text;
        }
    }
}