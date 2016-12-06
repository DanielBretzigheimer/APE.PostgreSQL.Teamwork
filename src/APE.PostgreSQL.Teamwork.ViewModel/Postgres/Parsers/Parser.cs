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
        /// String to be parsed.
        /// </summary>
        private string value;

        /// <summary>
        /// Current position.
        /// </summary>
        private int position;

        /// <summary>
        /// Creates new instance of Parser.
        /// </summary>
        public Parser(string value)
        {
            this.value = value;
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
                return (this.position == this.value.Length
                    || this.position + 1 == this.value.Length)
                    && this.value[this.position] == ';';
            }
        }

        /// <summary>
        /// Returns rest of the string. If the string ends with ';' then it is
        /// removed from the string before returned. If there is nothing more in the
        /// string, null is returned.
        /// </summary>
        /// <returns> rest of the string, without trailing ';' if present, or null if
        /// there is nothing more in the string </returns>
        public string Rest
        {
            get
            {
                string result;

                if (this.value[this.value.Length - 1] == ';')
                {
                    if (this.position == this.value.Length - 1)
                        return null;
                    else
                    {
                        result = this.value.Substring(this.position, this.value.Length - 1 - this.position);
                    }
                }
                else
                {
                    result = this.value.Substring(this.position);
                }

                this.position = this.value.Length;

                return result;
            }
        }

        /// <summary>
        /// Returns expression that is ended either with ',', ')' or with end of the
        /// string. If expression is empty then exception is thrown.
        /// </summary>
        /// <returns> expression string </returns>
        public string Expression
        {
            get
            {
                int endPos = this.ExpressionEnd;

                if (this.position == endPos)
                    throw new TeamworkParserException(string.Format("CannotParseStringExpectedExpression", this.value, this.position + 1, this.value.Substring(this.position, 20)));

                string result = this.value.Substring(this.position, endPos - this.position).Trim();

                this.position = endPos;

                return result;
            }
        }

        /// <summary>
        /// Returns current position in the string.
        /// </summary>
        /// <returns> current position in the string </returns>
        public int Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Returns parsed string.
        /// </summary>
        /// <returns> parsed string </returns>
        public string String
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Returns position of last character of single command within statement
        /// (like CREATE TABLE). Last character is either ',' or ')'. If no such
        /// character is found and method reaches the end of the command then
        /// position after the last character in the command is returned.
        /// </summary>
        /// <returns> end position of the command </returns>
        private int ExpressionEnd
        {
            get
            {
                int bracesCount = 0;
                bool singleQuoteOn = false;
                int charPos = this.position;

                for (; charPos < this.value.Length; charPos++)
                {
                    char chr = this.value[charPos];

                    if (chr == '(')
                        bracesCount++;
                    else if (chr == ')')
                    {
                        if (bracesCount == 0)
                            break;
                        else
                        {
                            bracesCount--;
                        }
                    }
                    else if (chr == '\'')
                        singleQuoteOn = !singleQuoteOn;
                    else if ((chr == ',') && !singleQuoteOn && (bracesCount == 0))
                        break;
                    else if (chr == ';' && bracesCount == 0 && !singleQuoteOn)
                        break;
                }

                return charPos;
            }
        }

        /// <summary>
        /// Checks whether the string contains given word on current position. If not then throws an exception.
        /// </summary>
        /// <param name="words">List of words to check.</param>
        public void Expect(params string[] words)
        {
            foreach (string word in words)
            {
                this.Expect(word, false);
            }
        }

        /// <summary>
        /// Checks whether the string contains given word on current position. If not
        /// and expectation is optional then position is not changed and method
        /// returns true. If expectation is not optional, exception with error
        /// description is thrown. If word is found, position is moved at first
        /// non-whitespace character following the word.
        /// </summary>
        /// <param name="word">Word to expect.</param>
        /// <param name="optional">True if word is optional, otherwise false.</param>
        /// <returns>True if word was found, otherwise false.</returns>
        public bool Expect(string word, bool optional)
        {
            int wordEnd = this.position + word.Length;

            if (wordEnd <= this.value.Length && this.value.Substring(this.position, wordEnd - this.position).Equals(word, StringComparison.CurrentCultureIgnoreCase) && (wordEnd == this.value.Length || char.IsWhiteSpace(this.value[wordEnd]) || this.value[wordEnd] == ';' || this.value[wordEnd] == ')' || this.value[wordEnd] == ',' || this.value[wordEnd] == '[' || "(".Equals(word) || ",".Equals(word) || "[".Equals(word) || "]".Equals(word)))
            {
                this.position = wordEnd;
                this.SkipWhitespace();

                return true;
            }

            if (optional)
                return false;

            throw new TeamworkParserException(string.Format("CannotParseStringExpectedWord", this.value, word, this.position + 1, this.value.Substring(this.position, 20)));
        }

        /// <summary>
        /// Checks whether string contains at current position sequence of the words.
        /// </summary>
        /// <param name="words">Array of words.</param>
        /// <returns>True if whole sequence was found, otherwise false.</returns>
        public bool ExpectOptional(params string[] words)
        {
            bool found = this.Expect(words[0], true);

            if (!found)
                return false;

            for (int i = 1; i < words.Length; i++)
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
            for (; this.position < this.value.Length; this.position++)
            {
                if (!char.IsWhiteSpace(this.value[this.position]))
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
            string identifier = this.ParseIdentifierInternal();

            if (this.value[this.position] == '.')
            {
                this.position++;
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
            int endPos = this.position;

            for (; endPos < this.value.Length; endPos++)
            {
                if (!char.IsLetterOrDigit(this.value[endPos]))
                    break;
            }

            try
            {
                int result = Convert.ToInt32(this.value.Substring(this.position, endPos - this.position));

                this.position = endPos;
                this.SkipWhitespace();

                return result;
            }
            catch (FormatException ex)
            {
                throw new TeamworkParserException(string.Format("CannotParseStringExpectedInteger", this.value, this.position + 1, this.value.Substring(this.position, 20)), ex);
            }
        }

        /// <summary>
        /// Parses string from the string. String can be either quoted or unquoted.
        /// Quoted string is parsed till next unescaped quote. Unquoted string is
        /// parsed till whitespace, ',' ')' or ';' is found. If string should be
        /// empty, exception is thrown.
        /// </summary>
        /// <returns>Parsed string, if quoted then including quotes.</returns>
        public string ParseString()
        {
            bool quoted = this.value[this.position] == '\'';

            if (quoted)
            {
                bool escape = false;
                int endPos = this.position + 1;

                for (; endPos < this.value.Length; endPos++)
                {
                    char chr = this.value[endPos];

                    if (chr == '\\')
                        escape = !escape;
                    else if (!escape && chr == '\'')
                    {
                        if (endPos + 1 < this.value.Length && this.value[endPos + 1] == '\'')
                            endPos++;
                        else
                        {
                            break;
                        }
                    }
                }

                string result;

                try
                {
                    result = this.value.Substring(this.position, endPos + 1 - this.position);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get substring: " + this.value + " start pos: " + this.position + " end pos: " + (endPos + 1), ex);
                }

                this.position = endPos + 1;
                this.SkipWhitespace();

                return result;
            }
            else
            {
                int endPos = this.position;

                for (; endPos < this.value.Length; endPos++)
                {
                    char chr = this.value[endPos];

                    if (char.IsWhiteSpace(chr) || chr == ',' || chr == ')' || chr == ';')
                        break;
                }

                if (this.position == endPos)
                    throw new TeamworkParserException(string.Format("CannotParseStringExpectedString", this.value, this.position + 1));

                string result = this.value.Substring(this.position, endPos - this.position);

                this.position = endPos;
                this.SkipWhitespace();

                return result;
            }
        }

        /// <summary>
        /// Throws exception about unsupported command in statement.
        /// </summary>
        public void ThrowUnsupportedCommand()
        {
            throw new TeamworkParserException(string.Format("CannotParseStringUnsupportedCommand", this.value, this.position + 1, this.value.Substring(this.position, 20)));
        }

        /// <summary>
        /// Checks whether one of the words is present at current position. If the
        /// word is present then the word is returned and position is updated.
        /// </summary>
        /// <param name="words">Words to check.</param>
        /// <returns>Found word or null if non of the words has been found.</returns>
        public string ExpectOptionalOneOf(params string[] words)
        {
            foreach (string word in words)
            {
                if (this.ExpectOptional(word))
                    return word;
            }

            return null;
        }

        /// <summary>
        /// Returns substring from the string.
        /// </summary>
        public string GetSubString(int startPos, int endPos)
        {
            return this.value.Substring(startPos, endPos - startPos);
        }

        /// <summary>
        /// Parses data type from the string. Position is updated. If data type
        /// definition is not found then exception is thrown.
        /// </summary>
        /// <returns>Data type string.</returns>
        public string ParseDataType()
        {
            int endPos = this.position;

            while (endPos < this.value.Length
                && !char.IsWhiteSpace(this.value[endPos])
                && this.value[endPos] != '('
                && this.value[endPos] != ')'
                && this.value[endPos] != ',')
            {
                endPos++;
            }

            if (endPos == this.position)
                throw new TeamworkParserException(string.Format("CannotParseStringExpectedDataType", this.value, this.position + 1, this.value.Substring(this.position, 20)));

            string dataType = this.value.Substring(this.position, endPos - this.position);

            this.position = endPos;
            this.SkipWhitespace();

            if ("character".Equals(dataType, StringComparison.CurrentCultureIgnoreCase) && this.ExpectOptional("varying"))
                dataType = "character varying";
            else if ("double".Equals(dataType, StringComparison.CurrentCultureIgnoreCase) && this.ExpectOptional("precision"))
                dataType = "double precision";

            bool timestamp = "timestamp".Equals(dataType, StringComparison.CurrentCultureIgnoreCase) || "time".Equals(dataType, StringComparison.CurrentCultureIgnoreCase);

            if (this.value[this.position] == '(')
                dataType += this.Expression;

            if (timestamp)
            {
                if (this.ExpectOptional("with", "time", "zone"))
                    dataType += " with time zone";
                else if (this.ExpectOptional("without", "time", "zone"))
                    dataType += " without time zone";
            }

            if (this.ExpectOptional("["))
            {
                this.Expect("]");
                dataType += "[]";
            }

            return dataType;
        }

        /// <summary>
        /// Parses single part of the identifier.
        /// </summary>
        /// <returns>Parsed identifier.</returns>
        private string ParseIdentifierInternal()
        {
            bool quoted = this.value[this.position] == '"';

            if (quoted)
            {
                int endPos = this.value.IndexOf('"', this.position + 1);

                string result = this.value.Substring(this.position, endPos + 1 - this.position);
                this.position = endPos + 1;

                return result;
            }
            else
            {
                int endPos = this.position;

                for (; endPos < this.value.Length; endPos++)
                {
                    char chr = this.value[endPos];

                    if (char.IsWhiteSpace(chr) || chr == ',' || chr == ')' || chr == '(' || chr == ';' || chr == '.')
                        break;
                }

                string result = this.value.Substring(this.position, endPos - this.position).ToLower(new CultureInfo("en"));

                this.position = endPos;

                return result;
            }
        }
    }
}