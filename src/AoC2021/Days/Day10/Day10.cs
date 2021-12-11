using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2021.Days
{
    public class Day10 : IDay
    {
        private string[] input;
        private Dictionary<char, int> syntaxErrorScores;
        private Dictionary<char, int> completionScores;

        public Day10(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            syntaxErrorScores = new Dictionary<char, int>() { {')', 3}, {']', 57}, {'}', 1197}, {'>', 25137} };
            completionScores = new Dictionary<char, int>() { {')', 1}, {']', 2}, {'}', 3}, {'>', 4} };
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var answer = input.Sum(line => SyntaxErrorLineScore(line));
            return answer.ToString();
        }

        public string PartTwo()
        {
            // not the most efficient to first filter away corrupt lines, 
            // as we go through every non-corrupt line twice, but it's fast enough
            var scores = input.Where(line => !LineIsCorrupt(line))
                              .Select(line => CompletionStringScore(line))
                              .ToArray();
            Array.Sort(scores);
            return scores[scores.Count()/2].ToString();
        }

        private long CompletionStringScore(string line)
        {
            var completionString = LineCompletionString(line);
            long total = 0;
            foreach (var c in completionString)
            {
                total *= 5;
                total += completionScores[c];
            }
            return total;
        }

        private string LineCompletionString(string line)
        {
            var bracketStack = new Stack<char>();
            foreach (var c in line)
            {
                if (IsOpenBracket(c))
                    bracketStack.Push(c);   
                else // close bracket
                    bracketStack.Pop(); // already excluded corrupt lines so this should always work
            }

            var completionStringBuilder = new StringBuilder("", bracketStack.Count());
            while (bracketStack.Count > 0)
                completionStringBuilder.Append(OpenToCloseBracket(bracketStack.Pop()));
                
            return completionStringBuilder.ToString();
        }

        private bool LineIsCorrupt(string line)
        {
            return SyntaxErrorLineScore(line) > 0;
        }

        private decimal SyntaxErrorLineScore(string line)
        {
            var bracketStack = new Stack<char>();
            foreach (var c in line)
            {
                if (IsOpenBracket(c))
                    bracketStack.Push(c);   
                else // close bracket
                {
                    var openBracket = CloseToOpenBracket(c);
                    if (bracketStack.Count == 0 || bracketStack.Pop() != openBracket)
                        return syntaxErrorScores[c];
                }
            }
            return 0;
        }

        private bool IsOpenBracket(char c)
        {
            // very lazy way:
            return !syntaxErrorScores.ContainsKey(c);
        }

        // was going to replace the next two functions with dictionaries but
        // apparently C# compiles switch-case tables to constant hash tables
        // so this is probably the best way performance wise.
        // https://stackoverflow.com/a/268223
        private char CloseToOpenBracket(char c)
        {
            switch (c)
            {
                case ')':
                    return '(';
                case ']':
                    return '[';
                case '}':
                    return '{';
                case '>':
                    return '<';
                default:
                    throw new InvalidDataException();
            }
        }

        private char OpenToCloseBracket(char c)
        {
            switch (c)
            {
                case '(':
                    return ')';
                case '[':
                    return ']';
                case '{':
                    return '}';
                case '<':
                    return '>';
                default:
                    throw new InvalidDataException();
            }
        }
    }
}