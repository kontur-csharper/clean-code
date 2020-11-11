﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Word
    {
        public readonly String Text;
        public readonly int StartPosition;
        public readonly int Length;

        public Word(string text, int startPosition, int length)
        {
            Text = text;
            StartPosition = startPosition;
            Length = length;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj as Word == null)
                return false;
            var otherWord = obj as Word;
            return Text == otherWord.Text && StartPosition == otherWord.StartPosition;
        }

        public bool ContainsDigit()
        {
            for (var i = StartPosition; i < StartPosition + Length; i++)
            {
                if (Char.IsDigit(Text[i]))
                    return true;
            }
            return false;
        }

        public bool IsInside(string tag, int tagPosition)
        {
            return tagPosition > StartPosition
                && tagPosition + tag.Length < StartPosition + Length;
        }
    }
}
