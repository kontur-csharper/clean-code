﻿namespace Markdown
{
    public class EmTagType : TagType
    {
        public EmTagType() : base("<em>", "</em>", "_", "_")
        {
        }

        public override bool IsOpenedTag(string text, int position)
        {
            if (text[position] != '_')
                return false;
            if (position == 0)
                return char.IsLetter(text[position + 1]);
            return position + 1 < text.Length && char.IsLetter(text[position + 1]) && text[position - 1] == ' ';
        }

        public override bool IsClosedTag(string text, int position)
        {
            if (text[position] != '_' || position < 1)
                return false;
            if (position == text.Length - 1)
                return char.IsLetter(text[position - 1]);
            return text[position + 1] == ' ' && char.IsLetter(text[position - 1]);
        }

        public override string GetOpenedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.EmOpened, markup);
        }

        public override string GetClosedTag(Tag.Markup markup)
        {
            return Tag.GetTag(Tag.TagName.EmClosed, markup);
        }
    }
}