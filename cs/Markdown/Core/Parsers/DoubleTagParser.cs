﻿using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core.Parsers
{
    class DoubleTagParser : IParser
    {
        private readonly List<IDoubleTag> tags;

        public DoubleTagParser(IEnumerable<ITag> tags)
        {
            this.tags = tags.OfType<IDoubleTag>()
                .OrderByDescending(tag => tag.Opening.Length)
                .ToList();
        }

        private bool IsSuitableTag(IDoubleTag tag, string line, int index) =>
            DoubleTagValidator.TagStartsFromPosition(line, index, tag.Opening) ||
            DoubleTagValidator.TagStartsFromPosition(line, index, tag.Closing);

        private bool IsSkippedTag(string line, int index) => index != 0 && line[index - 1] == '\\';

        public List<TagToken> ParseLine(string line)
        {
            var result = new List<TagToken>();
            if (line == null)
                return result;

            var tagTokenStack = new Stack<TagToken>();
            var index = 0;
            while (index < line.Length)
            {
                var currentTag = tags.FirstOrDefault(tag => IsSuitableTag(tag, line, index));
                if (currentTag == null)
                {
                    index++;
                    continue;
                }

                if (DoubleTagValidator.IsPossibleOpeningTag(line, index, currentTag))
                {
                    var newOpeningTagToken = new TagToken(index, currentTag, currentTag.Opening, true,
                        IsSkippedTag(line, index));
                    tagTokenStack.Push(newOpeningTagToken);
                }
                else if (DoubleTagValidator.IsPossibleClosingTag(line, index, currentTag))
                {
                    while (tagTokenStack.Count > 0 && tagTokenStack.Peek().Tag != currentTag)
                    {
                        var deletedTag = tagTokenStack.Pop();
                        if (deletedTag.IsSkipped)
                            result.Add(deletedTag);
                    }

                    if (tagTokenStack.Count == 0)
                    {
                        index++;
                        continue;
                    }

                    var newOpeningTagToken = tagTokenStack.Pop();
                    var newClosingTagToken = new TagToken(index, currentTag, currentTag.Closing, false,
                        IsSkippedTag(line, index));

                    result.Add(newOpeningTagToken);
                    result.Add(newClosingTagToken);
                }

                index += currentTag.Opening.Length;
            }

            return result.Concat(tagTokenStack.Where(token => token.IsSkipped)).ToList();
        }
    }
}