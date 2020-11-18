﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class Tag
    {
        public readonly string Opening;
        public readonly string Ending;
        private readonly HashSet<Tag> InvalidContext;

        public Tag(string openeing, string ending, HashSet<Tag> invalidContext = null)
        {
            Opening = openeing;
            Ending = ending;
            InvalidContext = invalidContext ?? new HashSet<Tag>();
        }

        public bool CheckForContext(Dictionary<Tag, TagSubstring> activeTags)
        {
            return InvalidContext.Count == 0 || !InvalidContext.Any(activeTags.ContainsKey);
        }

        public string GetTagValue(TagRole role)
        {
            if (role == TagRole.Opening)
                return Opening;
            if (role == TagRole.Ending)
                return Ending;
            throw new ArgumentException();
        }
    }
}
