﻿using System.Collections.Generic;

namespace Markdown
{
    class HorLineRegister : IReadable
    {
        readonly HashSet<char> ableDigits = new HashSet<char>(new [] { '*', '-', '_' });

        public Token TryGetToken(string input, int startPos)
        {
            bool isStartSpaces = true;
            
            char currDigit = '\0';
            int digitCount = 0, i;

            for (i = startPos; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (isStartSpaces && i == 3)
                    {
                        return null;
                    }
                    continue;
                }
                isStartSpaces = false;

                if (currDigit == '\0')
                {
                    if (ableDigits.Contains(input[i]))
                    {
                        currDigit = input[i];
                        digitCount += 1;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (input[i] == currDigit)
                {
                    digitCount += 1;
                }
                else
                {
                    if (input[i] == '\n')
                        break;

                    return null;
                }
            }

            if (digitCount < 3)
                return null;

            return new Token("", "<hr />", "", 1, i - startPos); 
        }
    }
}
