﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var markdownParser = new TagParser(text);
            var allTags = markdownParser.GetTags();
            return MarkdownToHtmlConverter.Convert(text, GetCorrectTags(allTags, text));
        }

        private List<Tag> PairWordTags(IEnumerable<WordTag> wordTags, string text)
        {
            var openTags = new Dictionary<Type, WordTag>();
            var result = new List<Tag>();
            foreach (var wordTag in wordTags)
            {
                var tagType = wordTag.GetType();
                openTags.TryGetValue(tagType, out var openTag);
                if (openTag == null)
                {
                    if (wordTag.CanBeOpen(text))
                        openTags[tagType] = wordTag;
                }
                else
                {
                    if (openTag.TryPairCloseTag(wordTag, text))
                    {
                        result.Add(openTag);
                        result.Add(wordTag);
                        openTags[tagType] = null;
                    }
                }
            }

            return result;
        }

        private IEnumerable<Tag> GetCorrectTags(IEnumerable<Tag> tags, string text)
        {
            var pairedWordTags = PairWordTags(tags.Where(x => x.GetType().BaseType == typeof(WordTag))
                .Select(x => (WordTag) x), text);
            var tagsWithoutIntersecting = RemoveIntersectingPairTags(pairedWordTags);
            var escapeTags = tags.Where(x => x is EscapeTag);
            var headerTags = tags.Where(x => x is HeaderTag);
            return tagsWithoutIntersecting.Concat(escapeTags).Concat(headerTags);
        }
        
        private IEnumerable<Tag> RemoveIntersectingPairTags(IEnumerable<Tag> tags)
        {
            var openTags = new Stack<Tag>();
            var result = new List<Tag>();
            foreach (var tag in tags.OrderBy(x=>x.Position))
            {
                if (!openTags.TryPeek(out var openTag) || openTag.GetType()!= tag.GetType())
                {
                    openTags.Push(tag);
                }
                else
                {
                    result.Add(openTags.Pop());
                    result.Add(tag);
                }
            }

            return result;
        }
    }
}