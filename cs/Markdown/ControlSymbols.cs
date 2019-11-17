using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class ControlSymbols
    {
        public static readonly Dictionary<string, Func<string, int, StopSymbolDecision>> ControlSymbolDecisionOnChar =
            new Dictionary<string, Func<string, int, StopSymbolDecision>>()
            {
                {"_", (input, position) => MakeDecision(input, position, "_")},
                {"__", (input, position) => MakeDecision(input, position, "__")}
            };

        private static readonly HashSet<string> ControlSymbol = new HashSet<string>() {"_", "__"};
        private static readonly HashSet<char> previousStopSymbol = new HashSet<char>() {' ', '\\'};
        private static readonly int maxLengthOfControlSymbol = 2;

        public static readonly Dictionary<string, string> ControlSymbolTags = new Dictionary<string, string>()
        {
            {"_", "em"},
            {"__", "strong"}
        };

        public static readonly Dictionary<string, HashSet<string>> TagCloseNextTag =
            new Dictionary<string, HashSet<string>>
            {
                {"_", new HashSet<string> {"_", "__"}},
                {"__", new HashSet<string> {"__"}}
            };

        public static Symbol AnalyzeSymbol(string input, int position)
        {
            if (input[position] == '\\' && IsControlSymbol(input, position + 1))
                return Symbol.Screen;
            if (position != 0 && input[position - 1] != ' ')
                return Symbol.AnotherSymbol;
            return IsControlSymbol(input, position) ? Symbol.ControlSymbol : Symbol.AnotherSymbol;
        }

        public static bool IsControlSymbol(string input, int position)
        {
            return !(ResolveControlSymbol(input, position) is null);
        }

        public static string ResolveControlSymbol(string input, int position)
        {
            for (var lengthOfControlSymbols = maxLengthOfControlSymbol;
                lengthOfControlSymbols > 0;
                lengthOfControlSymbols--)
            {
                if (position + lengthOfControlSymbols <= input.Length &&
                    ControlSymbol.Contains(input.Substring(position, lengthOfControlSymbols)))
                    return input.Substring(position, lengthOfControlSymbols);
            }

            return null;
        }

        private static StopSymbolDecision MakeDecision(string input, int position,
            string control)
        {
            if (input[position] == '\\' && IsControlSymbol(input, position + 1))
                return StopSymbolDecision.Skip;
            var controlSymbol = ResolveControlSymbol(input, position);
            if (controlSymbol is null)
                return StopSymbolDecision.AddChar;
            var indexAfter = position + controlSymbol.Length;
            if ((position == 0 || input[position - 1] == ' ') && indexAfter < input.Length && input[indexAfter] != ' ')
                return StopSymbolDecision.NestedToken;
            if (controlSymbol == control && input[position - 1] != ' ' && input[position - 1] != '\\' &&
                !IsControlSymbol(input, position - 1) && (indexAfter == input.Length || input[indexAfter] == ' '))
                return StopSymbolDecision.Stop;
            return StopSymbolDecision.AddChar;
        }
    }
}