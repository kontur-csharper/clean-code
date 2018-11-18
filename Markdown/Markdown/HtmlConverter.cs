﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        public string Convert(string rawString, Span span)
        {
            return Assembly(rawString, span);
        }

        private string Assembly(string rawString, Span span)
        {
            if (span.EndIndex - span.StartIndex == 1 && rawString[span.StartIndex] == '\\')
                return "";

            var builder = new StringBuilder();

            builder.Append(GetTagValue(span, t => t.Open));
            builder.Append(span.Children.Count == 0 ? GetSpanRowString(rawString, span) : GetSpanAssembledString(rawString, span));
            builder.Append(GetTagValue(span, t => t.Close));

            return builder.ToString();
        }

        private static string GetTagValue(Span initialSpan, Func<Tag, string> getTag)
        {
            var tag = Markups.Html.Tags.FirstOrDefault(t => t.Type == initialSpan.Tag.Type);
            if (tag == null)
                return getTag(initialSpan.Tag);

            return initialSpan.Parent?.Parent != null && !initialSpan.Tag.CanBeInside.Contains(initialSpan.Parent.Tag.Type)
                ? getTag(initialSpan.Tag)
                : getTag(tag);
        }

        private static string GetSpanRowString(string rawString, Span span)
        {
            return rawString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.EndIndex - (span.StartIndex + span.Tag.Open.Length));
        }

        private string GetSpanAssembledString(string rawString, Span span)
        {
            var builder = new StringBuilder();

            builder.Append(rawString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.Children[0].StartIndex - (span.StartIndex + span.Tag.Open.Length)));

            for (var i = 0; i < span.Children.Count - 1; i++)
            {
                builder.Append(Assembly(rawString, span.Children[i]));
                builder.Append(rawString.Substring(span.Children[i].EndIndex + span.Children[i].Tag.Close.Length,
                    span.Children[i + 1].StartIndex - (span.Children[i].EndIndex + span.Children[i].Tag.Close.Length)));
            }

            var lastSpan = span.Children[span.Children.Count - 1];
            builder.Append(Assembly(rawString, lastSpan));

            builder.Append(rawString.Substring(lastSpan.EndIndex + lastSpan.Tag.Close.Length,
                span.EndIndex - (lastSpan.EndIndex + lastSpan.Tag.Close.Length)));

            return builder.ToString();
        }
    }
}
