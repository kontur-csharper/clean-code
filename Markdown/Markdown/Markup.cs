﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Markup
    {
        public string Name { get; }
        public IReadOnlyList<Tag> Tags => tags;
        private List<Tag> tags;
        public Markup(string name, List<Tag> tags)
        {
            Name = name;
            this.tags = tags;
        }
    }
}
