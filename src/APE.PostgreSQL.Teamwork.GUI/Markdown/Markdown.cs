﻿// <copyright file="Markdown.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APE.PostgreSQL.Teamwork.GUI.Markdown
{
    public class Markdown : DependencyObject
    {
        // Using a DependencyProperty as the backing store for DocumentStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentStyleProperty =
            DependencyProperty.Register("DocumentStyle", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Heading1Style.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Heading1StyleProperty =
            DependencyProperty.Register("Heading1Style", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Heading2Style.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Heading2StyleProperty =
            DependencyProperty.Register("Heading2Style", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Heading3Style.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Heading3StyleProperty =
            DependencyProperty.Register("Heading3Style", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Heading4Style.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Heading4StyleProperty =
            DependencyProperty.Register("Heading4Style", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CodeStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeStyleProperty =
            DependencyProperty.Register("CodeStyle", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for LinkStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkStyleProperty =
            DependencyProperty.Register("LinkStyle", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ImageStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for SeparatorStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeparatorStyleProperty =
            DependencyProperty.Register("SeparatorStyle", typeof(Style), typeof(Markdown), new PropertyMetadata(null));

        public static readonly DependencyProperty AssetPathRootProperty =
            DependencyProperty.Register("AssetPathRootRoot", typeof(string), typeof(Markdown), new PropertyMetadata(null));

        /// <summary>
        /// maximum nested depth of [] and () supported by the transform; implementation detail.
        /// </summary>
        private const int NestDepth = 6;

        /// <summary>
        /// Tabs are automatically converted to spaces as part of the transform
        /// this constant determines how "wide" those tabs become in spaces.
        /// </summary>
        private const int TabWidth = 4;

        private const string MarkerUL = @"[*+-]";
        private const string MarkerOL = @"\d+[.]";

        private static readonly Regex LeadingWhitespace = new Regex(@"^[ ]*", RegexOptions.Compiled);
        private static readonly Regex HeaderSetext = new Regex(
            @"
                ^(.+?)
                [ ]*
                \n
                (=+|-+)     # $1 = string of ='s or -'s
                [ ]*
                \n+",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex HeaderAtx = new Regex(
            @"
                ^(\#{1,6})  # $1 = string of #'s
                [ ]*
                (.+?)       # $2 = Header text
                [ ]*
                \#*         # optional closing #'s (not counted)
                \n+",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex HorizontalRules = new Regex(
            @"
            ^[ ]{0,3}         # Leading space
                ([-*_])       # $1: First marker
                (?>           # Repeated marker group
                    [ ]{0,2}  # Zero, one, or two spaces.
                    \1        # Marker character
                ){2,}         # Group repeated at least twice
                [ ]*          # Trailing spaces
                $             # End of line.
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly string WholeList = string.Format(
            @"
            (                               # $1 = whole list
              (                             # $2
                [ ]{{0,{1}}}
                ({0})                       # $3 = first list item marker
                [ ]+
              )
              (?s:.+?)
              (                             # $4
                  \z
                |
                  \n{{2,}}
                  (?=\S)
                  (?!                       # Negative lookahead for another list item marker
                    [ ]*
                    {0}[ ]+
                  )
              )
            )",
            string.Format("(?:{0}|{1})", MarkerUL, MarkerOL),
            TabWidth - 1);

        private static readonly Regex ListNested = new Regex(
            @"^" + WholeList,
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex ListTopLevel = new Regex(
            @"(?:(?<=\n\n)|\A\n?)" + WholeList,
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex CodeSpan = new Regex(
            @"
                    (?<!\\)   # Character before opening ` can't be a backslash
                    (`+)      # $1 = Opening run of `
                    (.+?)     # $2 = The code block
                    (?<!`)
                    \1
                    (?!`)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex Bold = new Regex(
            @"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StrictBold = new Regex(
            @"([\W_]|^) (\*\*|__) (?=\S) ([^\r]*?\S[\*_]*) \2 ([\W_]|$)",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex Italic = new Regex(
            @"(\*|_) (?=\S) (.+?) (?<=\S) \1",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StrictItalic = new Regex(
            @"([\W_]|^) (\*|_) (?=\S) ([^\r\*_]*?\S) \2 ([\W_]|$)",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static Regex imageInline = new Regex(
            string.Format(
            @"
                (                           # wrap whole match in $1
                    !\[
                        ({0})               # link text = $2
                    \]
                    \(                      # literal paren
                        [ ]*
                        ({1})               # href = $3
                        [ ]*
                        (                   # $4
                        (['""])           # quote char = $5
                        (.*?)               # title = $6
                        \5                  # matching quote
                        #[ ]*                # ignore any spaces between closing quote and )
                        )?                  # title is optional
                    \)
                )",
            GetNestedBracketsPattern(),
            GetNestedParensPatternWithWhiteSpace()),
            RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static Regex anchorInline = new Regex(
            string.Format(
            @"
                (                           # wrap whole match in $1
                    \[
                        ({0})               # link text = $2
                    \]
                    \(                      # literal paren
                        [ ]*
                        ({1})               # href = $3
                        [ ]*
                        (                   # $4
                        (['""])           # quote char = $5
                        (.*?)               # title = $6
                        \5                  # matching quote
                        [ ]*                # ignore any spaces between closing quote and )
                        )?                  # title is optional
                    \)
                )",
            GetNestedBracketsPattern(),
            GetNestedParensPattern()),
            RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static Regex newlinesLeadingTrailing = new Regex(@"^\n+|\n+\z", RegexOptions.Compiled);
        private static Regex newlinesMultiple = new Regex(@"\n{2,}", RegexOptions.Compiled);

        private static Regex eoln = new Regex("\\s+");

        private static string nestedBracketsPattern;
        private static string nestedParensPattern;
        private static string nestedParensPatternWithWhiteSpace;

        private static Regex outDent = new Regex(@"^[ ]{1," + TabWidth + @"}", RegexOptions.Multiline | RegexOptions.Compiled);

        private int listLevel;

        public Markdown()
        {
            this.HyperlinkCommand = NavigationCommands.GoToPage;
        }

        public static Regex ImageInline { get => ImageInline1; set => ImageInline1 = value; }

        public static Regex ImageInline1 { get => imageInline; set => imageInline = value; }

        public static Regex AnchorInline { get => anchorInline; set => anchorInline = value; }

        /// <summary>
        /// when true, bold and italic require non-word characters on either side
        /// WARNING: this is a significant deviation from the markdown spec.
        /// </summary>
        public bool StrictBoldItalic { get; set; }

        public ICommand HyperlinkCommand { get; set; }

        public Style DocumentStyle
        {
            get { return (Style)this.GetValue(DocumentStyleProperty); }
            set { this.SetValue(DocumentStyleProperty, value); }
        }

        public Style Heading1Style
        {
            get { return (Style)this.GetValue(Heading1StyleProperty); }
            set { this.SetValue(Heading1StyleProperty, value); }
        }

        public Style Heading2Style
        {
            get { return (Style)this.GetValue(Heading2StyleProperty); }
            set { this.SetValue(Heading2StyleProperty, value); }
        }

        public Style Heading3Style
        {
            get { return (Style)this.GetValue(Heading3StyleProperty); }
            set { this.SetValue(Heading3StyleProperty, value); }
        }

        public Style Heading4Style
        {
            get { return (Style)this.GetValue(Heading4StyleProperty); }
            set { this.SetValue(Heading4StyleProperty, value); }
        }

        public Style CodeStyle
        {
            get { return (Style)this.GetValue(CodeStyleProperty); }
            set { this.SetValue(CodeStyleProperty, value); }
        }

        public Style LinkStyle
        {
            get { return (Style)this.GetValue(LinkStyleProperty); }
            set { this.SetValue(LinkStyleProperty, value); }
        }

        public Style ImageStyle
        {
            get { return (Style)this.GetValue(ImageStyleProperty); }
            set { this.SetValue(ImageStyleProperty, value); }
        }

        public Style SeparatorStyle
        {
            get { return (Style)this.GetValue(SeparatorStyleProperty); }
            set { this.SetValue(SeparatorStyleProperty, value); }
        }

        public string AssetPathRoot
        {
            get { return (string)this.GetValue(AssetPathRootProperty); }
            set { this.SetValue(AssetPathRootProperty, value); }
        }

        public FlowDocument Transform(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            text = this.Normalize(text);
            var document = this.Create<FlowDocument, Block>(this.RunBlockGamut(text));

            if (this.DocumentStyle != null)
            {
                document.Style = this.DocumentStyle;
            }
            else
            {
                document.PagePadding = new Thickness(0);
            }

            return document;
        }

        public IEnumerable<Inline> DoText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var t = eoln.Replace(text, " ");
            yield return new Run(t);
        }

        public Block CreateHeader(int level, IEnumerable<Inline> content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var block = this.Create<Paragraph, Inline>(content);

            switch (level)
            {
                case 1:
                    if (this.Heading1Style != null)
                    {
                        block.Style = this.Heading1Style;
                    }

                    break;

                case 2:
                    if (this.Heading2Style != null)
                    {
                        block.Style = this.Heading2Style;
                    }

                    break;

                case 3:
                    if (this.Heading3Style != null)
                    {
                        block.Style = this.Heading3Style;
                    }

                    break;

                case 4:
                    if (this.Heading4Style != null)
                    {
                        block.Style = this.Heading4Style;
                    }

                    break;
            }

            return block;
        }

        /// <summary>
        /// this is to emulate what's evailable in PHP.
        /// </summary>
        private static string RepeatString(string text, int count)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var sb = new StringBuilder(text.Length * count);
            for (var i = 0; i < count; i++)
                sb.Append(text);
            return sb.ToString();
        }

        /// <summary>
        /// Reusable pattern to match balanced [brackets]. See Friedl's
        /// "Mastering Regular Expressions", 2nd Ed., pp. 328-331.
        /// </summary>
        private static string GetNestedBracketsPattern()
        {
            // in other words [this] and [this[also]] and [this[also[too]]]
            // up to _nestDepth
            if (nestedBracketsPattern == null)
            {
                nestedBracketsPattern =
                    RepeatString(
                        @"
                    (?>              # Atomic matching
                       [^\[\]]+      # Anything other than brackets
                     |
                       \[
                           ", NestDepth) + RepeatString(
                    @" \]
                    )*",
                    NestDepth);
            }

            return nestedBracketsPattern;
        }

        /// <summary>
        /// Reusable pattern to match balanced (parens). See Friedl's
        /// "Mastering Regular Expressions", 2nd Ed., pp. 328-331.
        /// </summary>
        private static string GetNestedParensPattern()
        {
            // in other words (this) and (this(also)) and (this(also(too)))
            // up to _nestDepth
            if (nestedParensPattern == null)
            {
                nestedParensPattern =
                    RepeatString(
                        @"
                    (?>              # Atomic matching
                       [^()\s]+      # Anything other than parens or whitespace
                     |
                       \(
                           ", NestDepth) + RepeatString(
                    @" \)
                    )*",
                    NestDepth);
            }

            return nestedParensPattern;
        }

        /// <summary>
        /// Reusable pattern to match balanced (parens), including whitespace. See Friedl's
        /// "Mastering Regular Expressions", 2nd Ed., pp. 328-331.
        /// </summary>
        private static string GetNestedParensPatternWithWhiteSpace()
        {
            // in other words (this) and (this(also)) and (this(also(too)))
            // up to _nestDepth
            if (nestedParensPatternWithWhiteSpace == null)
            {
                nestedParensPatternWithWhiteSpace =
                    RepeatString(
                        @"
                    (?>              # Atomic matching
                       [^()]+      # Anything other than parens
                     |
                       \(
                           ", NestDepth) + RepeatString(
                    @" \)
                    )*",
                    NestDepth);
            }

            return nestedParensPatternWithWhiteSpace;
        }

        /// <summary>
        /// Perform transformations that form block-level tags like paragraphs, headers, and list items.
        /// </summary>
        private IEnumerable<Block> RunBlockGamut(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return this.DoHeaders(
                text,
                s1 => this.DoHorizontalRules(
                    s1,
                    s2 => this.DoLists(
                        s2,
                        sn => this.FormParagraphs(sn))));
        }

        /// <summary>
        /// Perform transformations that occur *within* block-level tags like paragraphs, headers, and list items.
        /// </summary>
        private IEnumerable<Inline> RunSpanGamut(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return this.DoCodeSpans(
                text,
                s0 => this.DoImages(
                    s0,
                    s1 => this.DoAnchors(
                    s1,
                    s2 => this.DoItalicsAndBold(
                    s2,
                    s3 => this.DoText(s3)))));
        }

        /// <summary>
        /// splits on two or more newlines, to form "paragraphs";.
        /// </summary>
        private IEnumerable<Block> FormParagraphs(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // split on two or more newlines
            var grafs = newlinesMultiple.Split(newlinesLeadingTrailing.Replace(text, string.Empty));

            foreach (var g in grafs)
            {
                yield return this.Create<Paragraph, Inline>(this.RunSpanGamut(g));
            }
        }

        /// <summary>
        /// Turn Markdown images into images.
        /// </summary>
        /// <remarks>
        /// ![image alt](url).
        /// </remarks>
        private IEnumerable<Inline> DoImages(string text, Func<string, IEnumerable<Inline>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return this.Evaluate(text, ImageInline, this.ImageInlineEvaluator, defaultHandler);
        }

        private Inline ImageInlineEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var linkText = match.Groups[2].Value;
            var url = match.Groups[3].Value;
            BitmapImage imgSource = null;
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) && !System.IO.Path.IsPathRooted(url))
                {
                    url = System.IO.Path.Combine(this.AssetPathRoot ?? string.Empty, url);
                }

                imgSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
                return new Run("!" + url) { Foreground = Brushes.Red };
            }

            var image = new Image { Source = imgSource, Tag = linkText };
            if (this.ImageStyle == null)
            {
                image.Margin = new Thickness(0);
            }
            else
            {
                image.Style = this.ImageStyle;
            }

            // Bind size so document is updated when image is downloaded
            if (imgSource.IsDownloading)
            {
                var binding = new Binding(nameof(BitmapImage.Width))
                {
                    Source = imgSource,
                    Mode = BindingMode.OneWay,
                };

                var bindingExpression = BindingOperations.SetBinding(image, FrameworkElement.WidthProperty, binding);
                void DownloadCompletedHandler(object sender, EventArgs e)
                {
                    imgSource.DownloadCompleted -= DownloadCompletedHandler;
                    bindingExpression.UpdateTarget();
                }

                imgSource.DownloadCompleted += DownloadCompletedHandler;
            }
            else
            {
                image.Width = imgSource.Width;
            }

            return new InlineUIContainer(image);
        }

        /// <summary>
        /// Turn Markdown link shortcuts into hyperlinks.
        /// </summary>
        /// <remarks>
        /// [link text](url "title").
        /// </remarks>
        private IEnumerable<Inline> DoAnchors(string text, Func<string, IEnumerable<Inline>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // Next, inline-style links: [link text](url "optional title") or [link text](url "optional title")
            return this.Evaluate(text, AnchorInline, this.AnchorInlineEvaluator, defaultHandler);
        }

        private Inline AnchorInlineEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var linkText = match.Groups[2].Value;
            var url = match.Groups[3].Value;
            var title = match.Groups[6].Value;

            var result = this.Create<Hyperlink, Inline>(this.RunSpanGamut(linkText));
            result.Command = this.HyperlinkCommand;
            result.CommandParameter = url;
            if (this.LinkStyle != null)
            {
                result.Style = this.LinkStyle;
            }

            return result;
        }

        /// <summary>
        /// Turn Markdown headers into HTML header tags.
        /// </summary>
        /// <remarks>
        /// Header 1
        /// ========
        ///
        /// Header 2
        /// --------
        ///
        /// # Header 1
        /// ## Header 2
        /// ## Header 2 with closing hashes ##
        /// ...
        /// ###### Header 6.
        /// </remarks>
        private IEnumerable<Block> DoHeaders(string text, Func<string, IEnumerable<Block>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return this.Evaluate(
                text,
                HeaderSetext,
                m => this.SetextHeaderEvaluator(m),
                s => this.Evaluate(s, HeaderAtx, m => this.AtxHeaderEvaluator(m), defaultHandler));
        }

        private Block SetextHeaderEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var header = match.Groups[1].Value;
            var level = match.Groups[2].Value.StartsWith("=") ? 1 : 2;

            // TODO: Style the paragraph based on the header level
            return this.CreateHeader(level, this.RunSpanGamut(header.Trim()));
        }

        private Block AtxHeaderEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var header = match.Groups[2].Value;
            var level = match.Groups[1].Value.Length;
            return this.CreateHeader(level, this.RunSpanGamut(header));
        }

        /// <summary>
        /// Turn Markdown horizontal rules into HTML hr tags.
        /// </summary>
        /// <remarks>
        /// ***
        /// * * *
        /// ---
        /// - - -.
        /// </remarks>
        private IEnumerable<Block> DoHorizontalRules(string text, Func<string, IEnumerable<Block>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return this.Evaluate(text, HorizontalRules, this.RuleEvaluator, defaultHandler);
        }

        private Block RuleEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var line = new Line();
            if (this.SeparatorStyle == null)
            {
                line.X2 = 1;
                line.StrokeThickness = 1.0;
            }
            else
            {
                line.Style = this.SeparatorStyle;
            }

            var container = new BlockUIContainer(line);
            return container;
        }

        /// <summary>
        /// Turn Markdown lists into HTML ul and ol and li tags.
        /// </summary>
        private IEnumerable<Block> DoLists(string text, Func<string, IEnumerable<Block>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // We use a different prefix before nested lists than top-level lists.
            // See extended comment in _ProcessListItems().
            if (this.listLevel > 0)
                return this.Evaluate(text, ListNested, this.ListEvaluator, defaultHandler);
            else
                return this.Evaluate(text, ListTopLevel, this.ListEvaluator, defaultHandler);
        }

        private Block ListEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var list = match.Groups[1].Value;
            var listType = Regex.IsMatch(match.Groups[3].Value, MarkerUL) ? "ul" : "ol";

            // Turn double returns into triple returns, so that we can make a
            // paragraph for the last item in a list, if necessary:
            list = Regex.Replace(list, @"\n{2,}", "\n\n\n");

            var resultList = this.Create<List, ListItem>(this.ProcessListItems(list, listType == "ul" ? MarkerUL : MarkerOL));

            resultList.MarkerStyle = listType == "ul" ? TextMarkerStyle.Disc : TextMarkerStyle.Decimal;

            return resultList;
        }

        /// <summary>
        /// Process the contents of a single ordered or unordered list, splitting it
        /// into individual list items.
        /// </summary>
        private IEnumerable<ListItem> ProcessListItems(string list, string marker)
        {
            // The listLevel global keeps track of when we're inside a list.
            // Each time we enter a list, we increment it; when we leave a list,
            // we decrement. If it's zero, we're not in a list anymore.

            // We do this because when we're not inside a list, we want to treat
            // something like this:

            // I recommend upgrading to version
            // 8. Oops, now this line is treated
            // as a sub-list.

            // As a single paragraph, despite the fact that the second line starts
            // with a digit-period-space sequence.

            // Whereas when we're inside a list (or sub-list), that line will be
            // treated as the start of a sub-list. What a kludge, huh? This is
            // an aspect of Markdown's syntax that's hard to parse perfectly
            // without resorting to mind-reading. Perhaps the solution is to
            // change the syntax rules such that sub-lists must start with a
            // starting cardinal number; e.g. "1." or "a.".
            this.listLevel++;
            try
            {
                // Trim trailing blank lines:
                list = Regex.Replace(list, @"\n{2,}\z", "\n");

                var pattern = string.Format(
                  @"(\n)?                      # leading line = $1
                (^[ ]*)                    # leading whitespace = $2
                ({0}) [ ]+                 # list marker = $3
                ((?s:.+?)                  # list item text = $4
                (\n{{1,2}}))      
                (?= \n* (\z | \2 ({0}) [ ]+))", marker);

                var regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                var matches = regex.Matches(list);
                foreach (Match m in matches)
                {
                    yield return this.ListItemEvaluator(m);
                }
            }
            finally
            {
                this.listLevel--;
            }
        }

        private ListItem ListItemEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var item = match.Groups[4].Value;
            var leadingLine = match.Groups[1].Value;

            if (!string.IsNullOrEmpty(leadingLine) || Regex.IsMatch(item, @"\n{2,}"))
            {
                // we could correct any bad indentation here..
                return this.Create<ListItem, Block>(this.RunBlockGamut(item));
            }
            else
            {
                // recursion for sub-lists
                return this.Create<ListItem, Block>(this.RunBlockGamut(item));
            }
        }

        /// <summary>
        /// Turn Markdown `code spans` into HTML code tags.
        /// </summary>
        private IEnumerable<Inline> DoCodeSpans(string text, Func<string, IEnumerable<Inline>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            ////    * You can use multiple backticks as the delimiters if you want to
            ////        include literal backticks in the code span. So, this input:
            ////
            ////        Just type ``foo `bar` baz`` at the prompt.
            ////
            ////        Will translate to:
            ////
            ////          <p>Just type <code>foo `bar` baz</code> at the prompt.</p>
            ////
            ////        There's no arbitrary limit to the number of backticks you
            ////        can use as delimters. If you need three consecutive backticks
            ////        in your code, use four for delimiters, etc.
            ////
            ////    * You can use spaces to get literal backticks at the edges:
            ////
            ////          ... type `` `bar` `` ...
            ////
            ////        Turns to:
            ////
            ////          ... type <code>`bar`</code> ...
            ////

            return this.Evaluate(text, CodeSpan, this.CodeSpanEvaluator, defaultHandler);
        }

        private Inline CodeSpanEvaluator(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var span = match.Groups[2].Value;
            span = Regex.Replace(span, @"^[ ]*", string.Empty); // leading whitespace
            span = Regex.Replace(span, @"[ ]*$", string.Empty); // trailing whitespace

            var result = new Run(span);
            if (this.CodeStyle != null)
            {
                result.Style = this.CodeStyle;
            }

            return result;
        }

        /// <summary>
        /// Turn Markdown *italics* and **bold** into HTML strong and em tags.
        /// </summary>
        private IEnumerable<Inline> DoItalicsAndBold(string text, Func<string, IEnumerable<Inline>> defaultHandler)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // <strong> must go first, then <em>
            if (this.StrictBoldItalic)
            {
                return this.Evaluate(
                    text,
                    StrictBold,
                    m => this.BoldEvaluator(m, 3),
                    s1 => this.Evaluate(
                        s1,
                        StrictItalic,
                        m => this.ItalicEvaluator(m, 3),
                        s2 => defaultHandler(s2)));
            }
            else
            {
                return this.Evaluate(
                    text,
                    Bold,
                    m => this.BoldEvaluator(m, 2),
                    s1 => this.Evaluate(
                        s1,
                        Italic,
                        m => this.ItalicEvaluator(m, 2),
                        s2 => defaultHandler(s2)));
            }
        }

        private Inline ItalicEvaluator(Match match, int contentGroup)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var content = match.Groups[contentGroup].Value;
            return this.Create<Italic, Inline>(this.RunSpanGamut(content));
        }

        private Inline BoldEvaluator(Match match, int contentGroup)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var content = match.Groups[contentGroup].Value;
            return this.Create<Bold, Inline>(this.RunSpanGamut(content));
        }

        /// <summary>
        /// Remove one level of line-leading spaces.
        /// </summary>
        private string Outdent(string block)
        {
            return outDent.Replace(block, string.Empty);
        }

        /// <summary>
        /// convert all tabs to _tabWidth spaces;
        /// standardizes line endings from DOS (CR LF) or Mac (CR) to UNIX (LF);
        /// makes sure text ends with a couple of newlines;
        /// removes any blank lines (only spaces) in the text.
        /// </summary>
        private string Normalize(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var output = new StringBuilder(text.Length);
            var line = new StringBuilder();
            var valid = false;

            for (var i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                        if (valid)
                            output.Append(line);
                        output.Append('\n');
                        line.Length = 0;
                        valid = false;
                        break;
                    case '\r':
                        if ((i < text.Length - 1) && (text[i + 1] != '\n'))
                        {
                            if (valid)
                                output.Append(line);
                            output.Append('\n');
                            line.Length = 0;
                            valid = false;
                        }

                        break;
                    case '\t':
                        var width = TabWidth - (line.Length % TabWidth);
                        for (var k = 0; k < width; k++)
                            line.Append(' ');
                        break;
                    case '\x1A':
                        break;
                    default:
                        if (!valid && text[i] != ' ')
                            valid = true;
                        line.Append(text[i]);
                        break;
                }
            }

            if (valid)
                output.Append(line);
            output.Append('\n');

            // add two newlines to the end before return
            return output.Append("\n\n").ToString();
        }

        private TResult Create<TResult, TContent>(IEnumerable<TContent> content)
            where TResult : IAddChild, new()
        {
            var result = new TResult();
            foreach (var c in content)
            {
                result.AddChild(c);
            }

            return result;
        }

        private IEnumerable<T> Evaluate<T>(string text, Regex expression, Func<Match, T> build, Func<string, IEnumerable<T>> rest)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var matches = expression.Matches(text);
            var index = 0;
            foreach (Match m in matches)
            {
                if (m.Index > index)
                {
                    var prefix = text.Substring(index, m.Index - index);
                    foreach (var t in rest(prefix))
                    {
                        yield return t;
                    }
                }

                yield return build(m);

                index = m.Index + m.Length;
            }

            if (index < text.Length)
            {
                var suffix = text.Substring(index, text.Length - index);
                foreach (var t in rest(suffix))
                {
                    yield return t;
                }
            }
        }
    }
}
