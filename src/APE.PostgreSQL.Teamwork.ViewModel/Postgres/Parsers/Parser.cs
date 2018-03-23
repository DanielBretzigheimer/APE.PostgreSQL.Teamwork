// <copyright file="Parser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Globalization;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Class for parsing strings.
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// Creates new instance of Parser.
        /// </summary>
        public Parser(string value)
        {
            this.String = value;
            this.SkipWhitespace();
        }

        /// <summary>
        /// Checks whether the whole string has been consumed.
        /// </summary>
        /// <returns> true if there is nothing left to parse, otherwise false </returns>
        public bool Consumed
        {
            get
            {
                return (this.Position == this.String.Length
                    || this.Position + 1 == this.String.Length)
                    && this.String[this.Position] == ';';
            }
        }

        /// <summary>
        /// Returns current position in the string.
        /// </summary>
        /// <returns> current position in the string </returns>
        public int Position { get; set; }

        /// <summary>
        /// Returns parsed string.
        /// </summary>
        /// <returns> parsed string </returns>
        public string String { get; }

        /// <summary>
        /// Returns rest of the string. If the string ends with ';' then it is
        /// removed from the string before returned. If there is nothing more in the
        /// string, null is returned.
        /// </summary>
        /// <returns> rest of the string, without trailing ';' if present, or null if
        /// there is nothing more in the string </returns>
        [return: NullGuard.AllowNull]
        public string Rest()
        {
            string result;

            if (this.String[this.String.Length - 1] == ';')
            {
                if (this.Position == this.String.Length - 1)
                {
                    return null;
                }
                else
                {
                    result = this.String.Substring(this.Position, this.String.Length - 1 - this.Position);
                }
            }
            else
            {
                result = this.String.Substring(this.Position);
            }

            this.Position = this.String.Length;

            return result;
        }

        /// <summary>
        /// Returns expression that is ended either with ',', ')' or with end of the
        /// string. If expression is empty then exception is thrown.
        /// </summary>
        /// <returns> expression string </returns>
        public string Expression()
        {
            var endPos = this.GetExpressionEnd();
            if (this.Position == endPos)
                throw new TeamworkParserException($"Could not find expression in {this.String}.");

            var result = this.String.Substring(this.Position, endPos - this.Position).Trim();
            this.Position = endPos;
            return result;
        }

        /// <summary>
        /// Checks whether the string contains given word on current position. If not then throws an exception.
        /// </summary>
        /// <param name="words">List of words to check.</param>
        public void Expect(params string[] words)
        {
            foreach (var word in words)
            {
                this.Expect(word, false);
            }
        }

        /// <summary>
        /// Checks whether the string contains given word on current position. If not and expectation is optional then position is not changed and method returns true. If expectation is
        /// not optional, exception with error description is thrown. If word is found, position is moved at first non-whitespace character following the word.
        /// </summary>
        /// <param name="word">Word to expect.</param>
        /// <param name="optional">True if word is optional, otherwise false.</param>
        /// <returns>True if word was found, otherwise false.</returns>
        public bool Expect(string word, bool optional)
        {
            var wordEnd = this.Position + word.Length;

            if (wordEnd <= this.String.Length
                && this.String.Substring(this.Position, wordEnd - this.Position).Equals(word, StringComparison.CurrentCultureIgnoreCase))
            {
                var isEnd = wordEnd == this.String.Length;
                var followingChar = isEnd ? char.MinValue : this.String[wordEnd];
                var followedByWhiteSpace = char.IsWhiteSpace(followingChar);
                var followedBySemicolon = followingChar == ';';
                var followedByEndParentheses = followingChar == ')';
                var followedByComma = followingChar == ',';
                var followedByEndSquareBracket = followingChar == '[';
                if (isEnd || followedByWhiteSpace || followedBySemicolon || followedByEndParentheses || followedByComma || followedByEndSquareBracket
                    || "(".Equals(word) || ",".Equals(word) || "[".Equals(word) || "]".Equals(word))
                {
                    this.Position = wordEnd;
                    this.SkipWhitespace();

                    return true;
                }
            }

            if (optional)
                return false;

            throw new TeamworkParserException($"Expected \"{word}\" as next word in {this.String.Substring(this.Position)}.");
        }

        /// <summary>
        /// Checks whether string contains at current position sequence of the words.
        /// </summary>
        /// <param name="words">Array of words.</param>
        /// <returns>True if whole sequence was found, otherwise false.</returns>
        public bool ExpectOptional(params string[] words)
        {
            var found = this.Expect(words[0], true);

            if (!found)
            {
                return false;
            }

            for (var i = 1; i < words.Length; i++)
            {
                this.SkipWhitespace();
                this.Expect(words[i]);
            }

            return true;
        }

        /// <summary>
        /// Moves position in the string to next non-whitespace string.
        /// </summary>
        public void SkipWhitespace()
        {
            for (; this.Position < this.String.Length; this.Position++)
            {
                if (!char.IsWhiteSpace(this.String[this.Position]))
                    break;
            }
        }

        /// <summary>
        /// Parses identifier from current position. If identifier is quoted, it is returned quoted. If the identifier is not quoted, it is converted to
        /// lowercase. If identifier does not start with letter then exception is thrown. Position is placed at next first non-whitespace character.
        /// </summary>
        /// <returns>Parsed identifier.</returns>
        public string ParseIdentifier()
        {
            var identifier = this.ParseIdentifierInternal();

            while (this.String[this.Position] == '.')
            {
                this.Position++;
                identifier += '.' + this.ParseIdentifierInternal();
            }

            this.SkipWhitespace();

            return identifier;
        }

        /// <summary>
        /// Parses integer from the string. If next word is not integer then exception is thrown.
        /// </summary>
        /// <returns>Parsed integer value.</returns>
        public int ParseInteger()
        {
            var endPos = this.Position;

            for (; endPos < this.String.Length; endPos++)
            {
                if (!char.IsLetterOrDigit(this.String[endPos]))
                {
                    break;
                }
            }

            try
            {
                var result = Convert.ToInt32(this.String.Substring(this.Position, endPos - this.Position));

                this.Position = endPos;
                this.SkipWhitespace();

                return result;
            }
            catch (FormatException ex)
            {
                throw new TeamworkParserException("CannotParseStringExpectedInteger", ex);
            }
        }

        /// <summary>
        /// Parses string from the string. String can be either quoted or unquoted.
        /// Quoted string is parsed till next unescaped quote. Unquoted string is
        /// parsed till whitespace, ',' ')' or ';' is found. If string should be
        /// empty, exception is thrown.
        /// </summary>
        /// <returns>Parsed string, if quoted then including quotes.</returns>
        [Obsolete("Use ParseString instead.")]
        public string ParseStringCompat()
        {
            var quoted = this.String[this.Position] == '\'';

            if (quoted)
            {
                var escape = false;
                var endPos = this.Position + 1;

                for (; endPos < this.String.Length; endPos++)
                {
                    var chr = this.String[endPos];

                    if (chr == '\\')
                    {
                        escape = !escape;
                    }
                    else if (!escape && chr == '\'')
                    {
                        if (endPos + 1 < this.String.Length && this.String[endPos + 1] == '\'')
                        {
                            endPos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                string result;

                try
                {
                    result = this.String.Substring(this.Position, endPos + 1 - this.Position);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get substring: " + this.String + " start pos: " + this.Position + " end pos: " + (endPos + 1), ex);
                }

                this.Position = endPos + 1;
                this.SkipWhitespace();

                return result;
            }
            else
            {
                var endPos = this.Position;

                for (; endPos < this.String.Length; endPos++)
                {
                    var chr = this.String[endPos];

                    if (char.IsWhiteSpace(chr) || chr == ',' || chr == ')' || chr == ';')
                    {
                        break;
                    }
                }

                if (this.Position == endPos)
                {
                    throw new TeamworkParserException("CannotParseStringExpectedString");
                }

                var result = this.String.Substring(this.Position, endPos - this.Position);

                this.Position = endPos;
                this.SkipWhitespace();

                return result;
            }
        }

        /// <summary>
        /// Throws exception about unsupported command in statement.
        /// </summary>
        public void ThrowUnsupportedCommand()
        {
            throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
        }

        /// <summary>
        /// Checks whether one of the words is present at current position. If the
        /// word is present then the word is returned and position is updated.
        /// </summary>
        /// <param name="words">Words to check.</param>
        /// <returns>Found word or null if non of the words has been found.</returns>
        [return: NullGuard.AllowNull]
        public string ExpectOptionalOneOf(params string[] words)
        {
            foreach (var word in words)
            {
                if (this.ExpectOptional(word))
                {
                    return word;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns substring from the string.
        /// </summary>
        public string GetSubString(int startPos, int endPos)
        {
            return this.String.Substring(startPos, endPos - startPos);
        }

        /// <summary>
        /// Parses data type from the string. Position is updated. If data type
        /// definition is not found then exception is thrown.
        /// </summary>
        /// <returns>Data type string.</returns>
        public string ParseDataType()
        {
            var endPos = this.Position;

            while (endPos < this.String.Length
                && !char.IsWhiteSpace(this.String[endPos])
                && this.String[endPos] != '('
                && this.String[endPos] != ')'
                && this.String[endPos] != ',')
            {
                endPos++;
            }

            if (endPos == this.Position)
            {
                throw new TeamworkParserException("CannotParseStringExpectedDataType");
            }

            var dataType = this.String.Substring(this.Position, endPos - this.Position);

            this.Position = endPos;
            this.SkipWhitespace();

            if ("character".Equals(dataType, StringComparison.CurrentCultureIgnoreCase))
            {
                if (this.ExpectOptional("varying"))
                {
                    dataType = $"{dataType} varying";
                }
                else
                {
                    var varyingSuffix = this.ParseString();
                    dataType = $"{dataType} {varyingSuffix}";
                }
            }
            else if ("double".Equals(dataType, StringComparison.CurrentCultureIgnoreCase) && this.ExpectOptional("precision"))
            {
                dataType = "double precision";
            }

            var timestamp = "timestamp".Equals(dataType, StringComparison.CurrentCultureIgnoreCase) || "time".Equals(dataType, StringComparison.CurrentCultureIgnoreCase);

            if (this.String[this.Position] == '(')
            {
                dataType += this.Expression();
            }

            if (timestamp)
            {
                if (this.ExpectOptional("with", "time", "zone"))
                {
                    dataType += " with time zone";
                }
                else if (this.ExpectOptional("without", "time", "zone"))
                {
                    dataType += " without time zone";
                }
            }

            if (this.ExpectOptional("["))
            {
                this.Expect("]");
                dataType += "[]";
            }

            return dataType;
        }

        /// <summary>
        /// Returns the next string after <see cref="Position"/> until a whitespace is found or the string is at its end.
        /// </summary>
        private string ParseString()
        {
            var retval = string.Empty;
            for (; this.Position < this.String.Length; this.Position++)
            {
                var c = this.String[this.Position];
                if (char.IsWhiteSpace(c))
                    break;
                retval += c;
            }

            return retval;
        }

        /// <summary>
        /// Returns position of last character of single command within statement
        /// (like CREATE TABLE). Last character is either ',' or ')'. If no such
        /// character is found and method reaches the end of the command then
        /// position after the last character in the command is returned.
        /// </summary>
        /// <returns> end position of the command </returns>
        private int GetExpressionEnd()
        {
            var bracesCount = 0;
            var singleQuoteOn = false;
            var charPos = this.Position;

            for (; charPos < this.String.Length; charPos++)
            {
                var chr = this.String[charPos];

                if (chr == '(')
                {
                    bracesCount++;
                }
                else if (chr == ')')
                {
                    if (bracesCount == 0)
                    {
                        break;
                    }
                    else
                    {
                        bracesCount--;
                    }
                }
                else if (chr == '\'')
                {
                    singleQuoteOn = !singleQuoteOn;
                }
                else if ((chr == ',') && !singleQuoteOn && (bracesCount == 0))
                {
                    break;
                }
                else if (chr == ';' && bracesCount == 0 && !singleQuoteOn)
                {
                    break;
                }
            }

            return charPos;
        }

        /// <summary>
        /// Parses single part of the identifier.
        /// </summary>
        /// <returns>Parsed identifier.</returns>
        private string ParseIdentifierInternal()
        {
            var quoted = this.String[this.Position] == '"';

            if (quoted)
            {
                var endPos = this.String.IndexOf('"', this.Position + 1);
                var result = this.String.Substring(this.Position, endPos + 1 - this.Position);
                this.Position = endPos + 1;

                return result;
            }
            else
            {
                var endPos = this.Position;

                for (; endPos < this.String.Length; endPos++)
                {
                    var chr = this.String[endPos];

                    if (char.IsWhiteSpace(chr) || chr == ',' || chr == ')' || chr == '(' || chr == ';' || chr == '.')
                    {
                        break;
                    }
                }

                var result = this.String.Substring(this.Position, endPos - this.Position).ToLower(new CultureInfo("en"));

                this.Position = endPos;

                return result;
            }
        }
    }
}