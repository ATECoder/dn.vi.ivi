// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://source.dot.net/#Microsoft.VisualBasic.Core/Microsoft/VisualBasic/Collection.vb

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace cc.isr.VI.Pith;

/// <summary>   Implements a like operator matching a string to a pattern. </summary>
/// <remarks>   David, 2021-06-29. </remarks>
[System.ComponentModel.EditorBrowsable( System.ComponentModel.EditorBrowsableState.Never )]
public sealed class LikeOperator
{
    private LikeOperator()
    {
    }

    /// <summary>   Static constructor. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    static LikeOperator()
    {
        _ligatureMap = new byte[142];
        _ligatureMap[( int ) Ligatures.ssBeta - ( int ) Ligatures.Min] = 1;
        _ligatureMap[( int ) Ligatures.szBeta - ( int ) Ligatures.Min] = 2;
        _ligatureMap[( int ) Ligatures.aeUpper - ( int ) Ligatures.Min] = 3;
        _ligatureMap[( int ) Ligatures.ae - ( int ) Ligatures.Min] = 4;
        _ligatureMap[( int ) Ligatures.thUpper - ( int ) Ligatures.Min] = 5;
        _ligatureMap[( int ) Ligatures.th - ( int ) Ligatures.Min] = 6;
        _ligatureMap[( int ) Ligatures.oeUpper - ( int ) Ligatures.Min] = 7;
        _ligatureMap[( int ) Ligatures.oe - ( int ) Ligatures.Min] = 8;
    }

    /// <summary>   Checks if a string matches a pattern. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    /// <param name="source">           Source for the pattern matching. </param>
    /// <param name="pattern">          Specifies the pattern. </param>
    /// <param name="compareOption">    (Optional) The compare option; defaults to
    ///                                 <see cref="CompareOptions.Ordinal"/>, binary comparison. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool IsLike( string source, string pattern, CompareOptions compareOption = CompareOptions.Ordinal )
    {
        if ( source == null ) throw new ArgumentNullException( nameof( source ) );
        if ( pattern == null ) throw new ArgumentNullException( nameof( pattern ) );
        int sourceIndex = default;
        int patternIndex = default;
        int sourceLength;
        int patternLength;
        LigatureInfo[] sourceLigatureInfo = [];
        LigatureInfo[] patternLigatureInfo = [];
        CompareOptions options;
        CompareInfo? comparer;
        patternLength = pattern is null ? 0 : pattern.Length;

        sourceLength = source is null ? 0 : source.Length;

        //
        // We expand ligatures up front, but we need to keep track of
        // where they were.  We need the source ligature positions so
        // that "?" in the pattern will match both characters of the
        // ligature.  We need the pattern ligature positions for
        // bracketed character lists (e.g. [abc0-9]), because a
        // ligature would look like two separate characters. But note
        // that we do this only for option compare text mode.
        //

        if ( compareOption == CompareOptions.Ordinal )
        {
            options = CompareOptions.Ordinal;
            comparer = null;
        }
        else
        {
            comparer = CultureInfo.CurrentCulture.CompareInfo;
            options = CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType;
            byte[] localeSpecificLigatureTable = new byte[_ligatureExpansions.Length];
            bool argWidthChanged = false;
            ExpandString( ref source!, ref sourceLength, ref sourceLigatureInfo, localeSpecificLigatureTable, comparer, options, ref argWidthChanged, false );
            bool argWidthChanged1 = false;
            ExpandString( ref pattern!, ref patternLength, ref patternLigatureInfo, localeSpecificLigatureTable, comparer, options, ref argWidthChanged1, false );
        }

        // The first phase is an optimization for anything in the pattern
        // before the first "*".  (If the pattern has no "*" in it, this
        // will do the whole thing.)
        //
        // Visit each character in the pattern, and see if it matches the
        // source.
        //
        //
        char p;
        while ( patternIndex < patternLength && sourceIndex < sourceLength )
        {
            p = pattern![patternIndex];
            switch ( p )
            {
                case '?':
                case '？':
                    {
                        // AdvanceToNextChar(Source, SourceLength, SourceIndex, Options)
                        SkipToEndOfExpandedChar( sourceLigatureInfo, sourceLength, ref sourceIndex );
                        break;
                    }

                case '#':
                case '＃':
                    {
                        if ( !char.IsDigit( source![sourceIndex] ) )
                        {
                            return false;
                        }

                        break;
                    }

                case '[':
                case '［':
                    {
                        // Match ranges like "[ACE-TZ]"

                        bool rangePatternEmpty = default, rangeMismatch = default, rangePatternError = default;
                        bool argSeenNot = false;
                        MatchRange( source!, sourceLength, ref sourceIndex, sourceLigatureInfo, pattern, patternLength, ref patternIndex,
                            patternLigatureInfo, ref rangePatternEmpty, ref rangeMismatch, ref rangePatternError, comparer!, options, seenNot: ref argSeenNot );
                        if ( rangePatternError )
                        {
                            throw new ArgumentException( string.Format( $"Argument '{nameof( pattern )}' is not a valid value." ) );
                        }

                        if ( rangeMismatch )
                        {
                            return false;
                        }

                        if ( rangePatternEmpty )
                        {
                            patternIndex += 1;
                            continue;
                        }

                        break;
                    }

                case '*':
                case '＊':
                    {
                        bool asteriskMismatch = default, asteriskPatternError = default;
                        MatchAsterisk( source!, sourceLength, sourceIndex, sourceLigatureInfo, pattern, patternLength, patternIndex,
                            patternLigatureInfo, ref asteriskMismatch, ref asteriskPatternError, comparer!, options );
                        return asteriskPatternError
                            ? throw new ArgumentException( string.Format( $"Argument '{nameof( pattern )}' is not a valid value." ) )
                            : !asteriskMismatch;
                    }

                default:
                    {
                        // Not a special pattern character.  Just see if we have a match.
                        //
                        if ( CompareChars( source!, sourceLength, sourceIndex, ref sourceIndex, sourceLigatureInfo, pattern, patternLength,
                            patternIndex, ref patternIndex, patternLigatureInfo, comparer!, options ) != 0 )
                        {
                            return false;
                        }

                        break;
                    }
            }

            patternIndex += 1;
            sourceIndex += 1;
        }

        // Check for the special case that we're at the end of the source,
        // and the pattern has nothing left in it but *'s or empty []'s.
        //
        while ( patternIndex < patternLength )
        {
            p = pattern![patternIndex];
            if ( p is '*' or '＊' )
            {
                patternIndex += 1;
            }
            else if ( patternIndex + 1 < patternLength && ((p == '[' && pattern[patternIndex + 1] == ']') || (p == '［' && pattern[patternIndex + 1] == '］')) )
            {
                patternIndex += 2;
            }
            else
            {
                break;
            }
        }

        return patternIndex >= patternLength && sourceIndex >= sourceLength;
    }

    /// <summary>   The list of ligatures. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    private enum Ligatures
    {
        Invalid = 0,
        Min = 0xC6,
        ssBeta = 0xDF,
        szBeta = 0xDF,
        aeUpper = 0xC6,
        ae = 0xE6,
        thUpper = 0xDE,
        th = 0xFE,
        oeUpper = 0x152,
        oe = 0x153,
        Max = 0x153
    }

    // The expansions for the ligatures. Note that the order of these is the same as their
    // order in the Ligatures enum
    private static readonly string[] _ligatureExpansions = ["", "ss", "sz", "AE", "ae", "TH", "th", "OE", "oe"];
    private static readonly byte[] _ligatureMap;

    /// <summary>   Ligature index. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="ch">   The ch. </param>
    /// <returns>   A byte. </returns>
    private static byte LigatureIndex( char ch )
    {
        return ( int ) ch is < (( int ) Ligatures.Min) or > (( int ) Ligatures.Max) ? ( byte ) 0 : _ligatureMap[ch - ( int ) Ligatures.Min];
    }

    /// <summary>   Can character expand. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="ch">                           The character. </param>
    /// <param name="localeSpecificLigatureTable">  The locale specific ligature table. </param>
    /// <param name="comparer">                     The comparer. </param>
    /// <param name="options">                      Options for controlling the operation. </param>
    /// <returns>   An int. </returns>
    private static int CanCharExpand( char ch, byte[] localeSpecificLigatureTable, CompareInfo comparer, CompareOptions options )
    {
        Debug.Assert( options != CompareOptions.Ordinal, "Char expansion check unexpected during binary compare" );
        byte index = LigatureIndex( ch );
        if ( index == 0 )
        {
            return 0;
        }

        if ( localeSpecificLigatureTable[index] == 0 )
        {
            localeSpecificLigatureTable[index] = comparer.Compare( ch.ToString(), _ligatureExpansions[index] ) == 0 ? ( byte ) 1 : ( byte ) 2;
        }

        return localeSpecificLigatureTable[index] == 1 ? index : 0;
    }

    /// <summary>   Gets character expansion. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="ch">                           The ch. </param>
    /// <param name="localeSpecificLigatureTable">  The locale specific ligature table. </param>
    /// <param name="comparer">                     The comparer. </param>
    /// <param name="options">                      Options for controlling the operation. </param>
    /// <returns>   The character expansion. </returns>
    private static string GetCharExpansion( char ch, byte[] localeSpecificLigatureTable, CompareInfo comparer, CompareOptions options )
    {
        int index = CanCharExpand( ch, localeSpecificLigatureTable, comparer, options );
        return index == 0 ? ch.ToString() : _ligatureExpansions[index];
    }

    /// <summary>   Values that represent Character kinds. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    private enum CharKind
    {
        None,
        ExpandedChar1,
        ExpandedChar2
    }

    /// <summary>   Information about the ligature. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    private struct LigatureInfo
    {
        internal CharKind Kind { get; set; }
        internal char CharBeforeExpansion { get; set; }
    }

    /// <summary>
    /// What I've been able to divine about this function is that its purpose is to normalize the
    /// string that is going to be used in the Like operator.  The string may contain ligatures (two
    /// letters being represented by a single glyph) that need to be expanded.  It also may contain
    /// Katakana characters that need to be mapped to narrow width characters.
    /// </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="input">                        [in,out] The input. </param>
    /// <param name="length">                       [in,out] The length. </param>
    /// <param name="inputLigatureInfo">            [in,out] Information describing the input
    ///                                             ligature. </param>
    /// <param name="localeSpecificLigatureTable">  The locale specific ligature table. </param>
    /// <param name="comparer">                     The comparer. </param>
    /// <param name="options">                      Options for controlling the operation. </param>
    /// <param name="widthChanged">                 [in,out] True if width changed. </param>
    /// <param name="useFullWidth">                 True to use full width. </param>
    [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    private static void ExpandString( ref string input, ref int length, ref LigatureInfo[] inputLigatureInfo,
                                      byte[] localeSpecificLigatureTable, CompareInfo comparer, CompareOptions options, ref bool widthChanged, bool useFullWidth )
    {
        widthChanged = false;
        if ( length == 0 )
            return;
        input = input.ToLowerInvariant();
        int extraChars = default;
        for ( int i = 0, loopTo = length - 1; i <= loopTo; i++ )
        {
            char ch = input[i];
            if ( CanCharExpand( ch, localeSpecificLigatureTable, comparer, options ) != 0 )
            {
                extraChars += 1;
            }
        }

        if ( extraChars > 0 )
        {
            inputLigatureInfo = new LigatureInfo[(length + extraChars)];
            System.Text.StringBuilder newInput = new( length + extraChars - 1 );
            int newCharIndex = 0;
            for ( int i = 0, loopTo1 = length - 1; i <= loopTo1; i++ )
            {
                char ch = input[i];
                if ( CanCharExpand( ch, localeSpecificLigatureTable, comparer, options ) != 0 )
                {
                    string expansion = GetCharExpansion( ch, localeSpecificLigatureTable, comparer, options );
                    _ = newInput.Append( expansion );
                    inputLigatureInfo[newCharIndex].Kind = CharKind.ExpandedChar1;
                    inputLigatureInfo[newCharIndex].CharBeforeExpansion = ch;
                    newCharIndex += 1;
                    inputLigatureInfo[newCharIndex].Kind = CharKind.ExpandedChar2;
                    inputLigatureInfo[newCharIndex].CharBeforeExpansion = ch;
                }
                else
                {
                    _ = newInput.Append( ch );
                }

                newCharIndex += 1;
            }

            input = newInput.ToString();
            length = newInput.Length;
        }
    }

    /// <summary>   Skip to end of expanded character. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="inputLigatureInfo">    Information describing the input ligature. </param>
    /// <param name="length">               The length. </param>
    /// <param name="current">              [in,out] The current. </param>
    private static void SkipToEndOfExpandedChar( LigatureInfo[] inputLigatureInfo, int length, ref int current )
    {
        if ( inputLigatureInfo is null )
        {
        }
        // Nothing to do for the option compare binary case or the simple option compare text case
        else if ( current < length && inputLigatureInfo[current].Kind == CharKind.ExpandedChar1 )
        {
            current++;
        }
    }

    /// <summary>   Compare characters. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="left">                                 The left. </param>
    /// <param name="leftLength">                           Length of the left. </param>
    /// <param name="leftStart">                            The left start. </param>
    /// <param name="leftEnd">                              [in,out] The left end. </param>
    /// <param name="leftLigatureInfo">                     Information describing the left ligature. </param>
    /// <param name="right">                                The right. </param>
    /// <param name="rightLength">                          Length of the right. </param>
    /// <param name="rightStart">                           The right start. </param>
    /// <param name="rightEnd">                             [in,out] The right end. </param>
    /// <param name="rightLigatureInfo">                    Information describing the right
    ///                                                     ligature. </param>
    /// <param name="comparer">                             The comparer. </param>
    /// <param name="options">                              Options for controlling the operation. </param>
    /// <param name="matchBothCharsOfExpandedCharInRight">  (Optional) True to match both characters
    ///                                                     of expanded character in right. </param>
    /// <param name="useUnexpandedCharForRight">            (Optional) True to use unexpanded
    ///                                                     character for right. </param>
    /// <returns>   An int. </returns>
    [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    private static int CompareChars( string left, int leftLength, int leftStart, ref int leftEnd,
                                     LigatureInfo[] leftLigatureInfo, string right, int rightLength, int rightStart,
                                     ref int rightEnd, LigatureInfo[] rightLigatureInfo, CompareInfo comparer, CompareOptions options,
                                     bool matchBothCharsOfExpandedCharInRight = false, bool useUnexpandedCharForRight = false )
    {
        if ( left is null ) throw new ArgumentNullException( nameof( left ) );
        if ( right is null ) throw new ArgumentNullException( nameof( right ) );
        if ( comparer is null ) throw new ArgumentNullException( nameof( comparer ) );
        leftEnd = leftStart;
        rightEnd = rightStart;
        if ( options == CompareOptions.Ordinal )
        {
            // Ordinal compare
            return left[leftStart] - right[rightStart];
        }

        Debug.Assert( comparer is not null, "Like Operator - Comparer expected for option compare text" );
        Debug.Assert( !matchBothCharsOfExpandedCharInRight || !useUnexpandedCharForRight, "Conflicting compare options" );
        if ( useUnexpandedCharForRight )
        {
            if ( rightLigatureInfo is not null && rightLigatureInfo[rightEnd].Kind == CharKind.ExpandedChar1 )
            {
                right = right[rightStart..rightEnd];
                right += rightLigatureInfo[rightEnd].CharBeforeExpansion;
                rightEnd += 1;
                return CompareChars( left.Substring( leftStart, leftEnd - leftStart + 1 ), right, comparer!, options );
            }
        }
        else if ( matchBothCharsOfExpandedCharInRight )
        {
            int savedRightEnd = rightEnd;
            SkipToEndOfExpandedChar( rightLigatureInfo, rightLength, ref rightEnd );

            // If matching both expanded characters on the right, then consider multiple characters on the left too
            //
            if ( savedRightEnd < rightEnd )
            {
                int numberOfExtraCharsToCompare = 0;
                if ( leftEnd + 1 < leftLength )
                {
                    numberOfExtraCharsToCompare = 1;
                }

                int matchResult = CompareChars( left.Substring( leftStart, leftEnd - leftStart + 1 + numberOfExtraCharsToCompare ),
                    right.Substring( rightStart, rightEnd - rightStart + 1 ), comparer!, options );
                if ( matchResult == 0 )
                {
                    leftEnd += numberOfExtraCharsToCompare;
                }

                return matchResult;
            }
        }

        Debug.Assert( leftEnd < leftLength && rightEnd < rightLength, "Comparing chars beyond end of string" );
        return leftEnd == leftStart && rightEnd == rightStart
            ? comparer!.Compare( left[leftStart].ToString(), right[rightStart].ToString(), options )
            : CompareChars( left.Substring( leftStart, leftEnd - leftStart + 1 ), right.Substring( rightStart, rightEnd - rightStart + 1 ), comparer!, options );
    }

    /// <summary>   Compare characters. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="left">     The left. </param>
    /// <param name="right">    The right. </param>
    /// <param name="comparer"> The comparer. </param>
    /// <param name="options">  Options for controlling the operation. </param>
    /// <returns>   An int. </returns>
    private static int CompareChars( string left, string right, CompareInfo comparer, CompareOptions options )
    {
        if ( left is null ) throw new ArgumentNullException( nameof( left ) );
        if ( right is null ) throw new ArgumentNullException( nameof( right ) );
        if ( comparer is null ) throw new ArgumentNullException( nameof( comparer ) );
        if ( options == CompareOptions.Ordinal )
        {
            // Ordinal compare
            //
            return left[0] - right[0];
        }

        Debug.Assert( comparer is not null, "Like Operator - Comparer expected for option compare text" );
        return comparer!.Compare( left, right, options );
    }

    /// <summary>   Compare characters. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="left">     The left. </param>
    /// <param name="right">    The right. </param>
    /// <param name="comparer"> The comparer. </param>
    /// <param name="options">  Options for controlling the operation. </param>
    /// <returns>   An int. </returns>
    private static int CompareChars( char left, char right, CompareInfo comparer, CompareOptions options )
    {
        if ( comparer is null ) throw new ArgumentNullException( nameof( comparer ) );
        if ( options == CompareOptions.Ordinal )
        {
            // Ordinal compare
            return left - right;
        }

        Debug.Assert( comparer is not null, "Like Operator - Comparer expected for option compare text" );
        return comparer!.Compare( left.ToString(), right.ToString(), options );
    }

    /// <summary>   Character was within range. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="sourceNextIndex">                  Zero-based index of the source next. </param>
    /// <param name="sourceIndex">                      [in,out] Zero-based index of the source. </param>
    /// <param name="pattern">                          Specifies the pattern. </param>
    /// <param name="patternLength">                    Length of the pattern. </param>
    /// <param name="patternIndex">                     [in,out] Zero-based index of the pattern. </param>
    /// <param name="patternError">                     [in,out] True to pattern error. </param>
    /// <param name="validatePatternWithoutMatching">   True to validate pattern without matching. </param>
    /// <param name="seenNot">                          True to seen not. </param>
    /// <param name="mismatch">                         [in,out] True to mismatch. </param>
    private static void OneCharMatch( int sourceNextIndex, ref int sourceIndex,
                                      string pattern, int patternLength, ref int patternIndex, ref bool patternError,
                                      bool validatePatternWithoutMatching, bool seenNot, ref bool mismatch )
    {
        Debug.Assert( !validatePatternWithoutMatching, "Unexpected string matching when validating pattern string" );
        if ( seenNot )
        {
            mismatch = true;
            return;
        }

        do
        {
            patternIndex += 1;
            if ( patternIndex >= patternLength )
            {
                patternError = true;
                return;
            }
        }
        while ( pattern[patternIndex] is not ']' and not '］' );
        sourceIndex = sourceNextIndex;
        return;  // Match

    }

    /// <summary>   Match range. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="source">                           Source for the. </param>
    /// <param name="sourceLength">                     Length of the source. </param>
    /// <param name="sourceIndex">                      [in,out] Zero-based index of the source. </param>
    /// <param name="sourceLigatureInfo">               Information describing the source ligature. </param>
    /// <param name="pattern">                          Specifies the pattern. </param>
    /// <param name="patternLength">                    Length of the pattern. </param>
    /// <param name="patternIndex">                     [in,out] Zero-based index of the pattern. </param>
    /// <param name="patternLigatureInfo">              Information describing the pattern ligature. </param>
    /// <param name="rangePatternEmpty">                [in,out] True to range pattern empty. </param>
    /// <param name="mismatch">                         [in,out] True to mismatch. </param>
    /// <param name="patternError">                     [in,out] True to pattern error. </param>
    /// <param name="comparer">                         The comparer. </param>
    /// <param name="options">                          Options for controlling the operation. </param>
    /// <param name="seenNot">                          [in,out] True to seen not. </param>
    /// <param name="rangeList">                        (Optional) List of ranges. </param>
    /// <param name="validatePatternWithoutMatching">   (Optional) True to validate pattern without
    ///                                                 matching. </param>
    private static void MatchRange( string source, int sourceLength, ref int sourceIndex, LigatureInfo[]? sourceLigatureInfo,
                                   string pattern, int patternLength, ref int patternIndex, LigatureInfo[]? patternLigatureInfo,
                                   ref bool rangePatternEmpty, ref bool mismatch, ref bool patternError, CompareInfo comparer,
                                   CompareOptions options, [Optional, DefaultParameterValue( false )] ref bool seenNot,
                                   List<Range>? rangeList = default, bool validatePatternWithoutMatching = false )
    {
        Debug.Assert( patternIndex <= patternLength && (pattern[patternIndex].ToString() == "[" || pattern[patternIndex] == '［'), "Like operator - Unexpected range matching" );
        Debug.Assert( rangeList is null || validatePatternWithoutMatching, "Unexpected options to MatchRange" );
        string rangeStart, rangeEnd;
        Range range = new();
        rangePatternEmpty = false;
        mismatch = false;
        patternError = false;
        seenNot = false;
        patternIndex += 1;
        if ( patternIndex >= patternLength )
        {
            patternError = true;
            return;
        }

        char p = pattern[patternIndex];
        if ( p is '!' or '！' )
        {
            seenNot = true;
            patternIndex += 1;
            if ( patternIndex >= patternLength )
            {
                mismatch = true;
                return;
            }

            p = pattern[patternIndex];
        }

        if ( p is ']' or '］' )
        {
            if ( seenNot )
            {
                // We got "[!]" ?    Treat it as the single literal character "!".
                //
                seenNot = false;
                if ( !validatePatternWithoutMatching )
                {
                    mismatch = !(CompareChars( source[sourceIndex], '!', comparer, options ) == 0);
                }

                if ( rangeList is not null )
                {
                    range = new Range( patternIndex - 1, 1, -1, 0 );
                    rangeList.Add( range );
                }

                return;
            }

            // Ignore empty brackets
            rangePatternEmpty = true;
            return;
        }

        // Scan through character list
        //
        do
        {
            if ( p is ']' or '］' )
            {
                mismatch = !seenNot;
                return;      // End of "[...]" match
            }

            // Try to match the expanded ligature
            //
            int sourceNextIndex = default, patternNextIndex = default;
            int compareResult;
            if ( !validatePatternWithoutMatching && patternLigatureInfo is not null && patternLigatureInfo[patternIndex].Kind == CharKind.ExpandedChar1 )
            {
                // VB6 compat - Match expanded char and return in this case without even validating RangeStart > RangeEnd
                //
                compareResult = CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo!, pattern, patternLength,
                    patternIndex, ref patternNextIndex, patternLigatureInfo, comparer, options, matchBothCharsOfExpandedCharInRight: true );
                if ( compareResult == 0 )
                {
                    sourceIndex = sourceNextIndex;
                    patternIndex = patternNextIndex;
                    OneCharMatch( sourceNextIndex, ref sourceIndex, pattern, patternLength, ref patternIndex, ref patternError,
                        validatePatternWithoutMatching, seenNot, ref mismatch );
                    return;
                    // goto OneCharMatch;
                }
            }
            else
            {
                if ( patternLigatureInfo is null ) throw new ArgumentNullException( nameof( patternLigatureInfo ) );
                patternNextIndex = patternIndex;
                SkipToEndOfExpandedChar( patternLigatureInfo, patternLength, ref patternNextIndex );
            }

            range.Start = patternIndex;
            range.StartLength = patternNextIndex - patternIndex + 1;

            // Store the range start char
            //
            if ( options == CompareOptions.Ordinal )
            {
                rangeStart = pattern[patternIndex].ToString();
            }
            else if ( patternLigatureInfo is not null && patternLigatureInfo[patternIndex].Kind == CharKind.ExpandedChar1 )
            {
                rangeStart = patternLigatureInfo[patternIndex].CharBeforeExpansion.ToString();
                patternIndex = patternNextIndex;
            }
            else
            {
                rangeStart = pattern.Substring( patternIndex, patternNextIndex - patternIndex + 1 );
                patternIndex = patternNextIndex;
            }

            if ( patternNextIndex + 2 < patternLength && (pattern[patternNextIndex + 1] == '-' || pattern[patternNextIndex + 1] == '－') && pattern[patternNextIndex + 2] != ']' && pattern[patternNextIndex + 2] != '］' )
            {
                // We're at the last character of a range.
                //
                patternIndex += 2;

                // Try to match one char
                //
                if ( !validatePatternWithoutMatching && patternLigatureInfo is not null && patternLigatureInfo[patternIndex].Kind == CharKind.ExpandedChar1 )
                {
                    // VB6 compat - Match expanded char and return in this case without even validating RangeStart > RangeEnd
                    //
                    compareResult = CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo!, pattern, patternLength, patternIndex, ref patternNextIndex, patternLigatureInfo, comparer, options, matchBothCharsOfExpandedCharInRight: true );
                    if ( compareResult == 0 )
                    {
                        patternIndex = patternNextIndex;
                        OneCharMatch( sourceNextIndex, ref sourceIndex, pattern, patternLength, ref patternIndex, ref patternError, validatePatternWithoutMatching, seenNot, ref mismatch );
                        return;
                        // goto OneCharMatch;
                    }
                }
                else
                {
                    if ( patternLigatureInfo is null ) throw new ArgumentNullException( nameof( patternLigatureInfo ) );
                    patternNextIndex = patternIndex;
                    SkipToEndOfExpandedChar( patternLigatureInfo, patternLength, ref patternNextIndex );
                }

                range.End = patternIndex;
                range.EndLength = patternNextIndex - patternIndex + 1;

                // Store the range end char
                //
                if ( options == CompareOptions.Ordinal )
                {
                    rangeEnd = pattern[patternIndex].ToString();
                }
                else if ( patternLigatureInfo is not null && patternLigatureInfo[patternIndex].Kind == CharKind.ExpandedChar1 )
                {
                    rangeEnd = patternLigatureInfo[patternIndex].CharBeforeExpansion.ToString();
                    patternIndex = patternNextIndex;
                }
                else
                {
                    rangeEnd = pattern.Substring( patternIndex, patternNextIndex - patternIndex + 1 );
                    patternIndex = patternNextIndex;
                }

                if ( CompareChars( rangeStart, rangeEnd, comparer, options ) > 0 )
                {
                    patternError = true;
                    return;
                }

                int argRightEnd = default;
                int argRightEnd1 = default;
                if ( !validatePatternWithoutMatching && CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo!, pattern,
                    range.Start + range.StartLength, range.Start, ref argRightEnd, patternLigatureInfo!, comparer, options,
                    useUnexpandedCharForRight: true ) >= 0 && CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo!,
                    pattern, range.End + range.EndLength, range.End, ref argRightEnd1, patternLigatureInfo!, comparer, options, useUnexpandedCharForRight: true ) <= 0 )
                {
                    // Character was within range
                    OneCharMatch( sourceNextIndex, ref sourceIndex, pattern, patternLength, ref patternIndex, ref patternError, validatePatternWithoutMatching,
                        seenNot, ref mismatch );
                    return;
                    // OneCharMatch:
#if false
                    OneCharMatch:
                    ;
                    Debug.Assert(!ValidatePatternWithoutMatching, "Unexpected string matching when validating pattern string");
                    if (SeenNot)
                    {
                        Mismatch = true;
                        return;
                    }

                    do
                    {
                        PatternIndex += 1;
                        if (PatternIndex >= PatternLength)
                        {
                            PatternError = true;
                            return;
                        }
                    }
                    while (Pattern[PatternIndex] != ']' && Pattern[PatternIndex] != '］');
                    SourceIndex = SourceNextIndex;
                    return;  // Match
#endif
                }
            }
            else
            {
                // Single character match
                //
                //
                int argRightEnd2 = default;
                if ( !validatePatternWithoutMatching && CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo!,
                    pattern, range.Start + range.StartLength, range.Start, ref argRightEnd2, patternLigatureInfo!, comparer, options, useUnexpandedCharForRight: true ) == 0 )
                {
                    OneCharMatch( sourceNextIndex, ref sourceIndex, pattern, patternLength, ref patternIndex, ref patternError, validatePatternWithoutMatching,
                        seenNot, ref mismatch );
                    return;
                    // OneCharMatch:
                }

                // No range end for single characters in list
                //
                range.End = -1;
                range.EndLength = 0;
            }

            rangeList?.Add( range );

            patternIndex += 1;
            if ( patternIndex >= patternLength )
            {
                patternError = true;
                return;
            }

            p = pattern[patternIndex];
        }
        while ( true );
    }

    /// <summary>   Validates the range pattern. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="pattern">              Specifies the pattern. </param>
    /// <param name="patternLength">        Length of the pattern. </param>
    /// <param name="patternIndex">         [in,out] Zero-based index of the pattern. </param>
    /// <param name="patternLigatureInfo">  Information describing the pattern ligature. </param>
    /// <param name="comparer">             The comparer. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    /// <param name="seenNot">              [in,out] True to seen not. </param>
    /// <param name="rangeList">            [in,out] List of ranges. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    private static bool ValidateRangePattern( string pattern, int patternLength, ref int patternIndex, LigatureInfo[] patternLigatureInfo,
                                             CompareInfo comparer, CompareOptions options, ref bool seenNot, ref List<Range> rangeList )
    {
        const bool validPatternWithoutMatching = true;
        bool patternError = default;
        int argSourceIndex = -1;
        bool argRangePatternEmpty = default;
        bool argMismatch = default;
        MatchRange( string.Empty, -1, ref argSourceIndex, null, pattern, patternLength, ref patternIndex, patternLigatureInfo,
            ref argRangePatternEmpty, ref argMismatch, ref patternError, comparer, options, ref seenNot, rangeList, validPatternWithoutMatching );
        return !patternError;
    }

    /// <summary>   Values that represent pattern types. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    private enum PatternType
    {
        STRING,
        EXCLIST,
        INCLIST,
        DIGIT,
        ANYCHAR,
        STAR,
        NONE
    }

    /// <summary>   A pattern group. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    private struct PatternGroup
    {
        internal PatternType PatType { get; set; }
        internal int MaxSourceIndex { get; set; }
        internal int CharCount { get; set; }

        // StringPatternStart, StringPatternEnd - there are the indices into the original source string
        // and are NOT indices into StringPattern.
#if DEBUG

        /// <summary>
        /// For PatternType.[STRING]. Indices into the original source string; NOT indices into
        /// StringPattern.
        /// </summary>
        /// <value> The string pattern start. </value>
        internal int StringPatternStart
        {
            readonly get
            {
                Debug.Assert( this.PatType == PatternType.STRING, "Unexpected pattern group type" );
                return field;
            }

            set
            {
                Debug.Assert( this.PatType == PatternType.STRING, "Unexpected pattern group type" );
                field = value;
            }
        }

        /// <summary>
        /// For PatternType.[STRING]. Indices into the original source string; NOT indices into
        /// StringPattern.
        /// </summary>
        /// <value> The string pattern end. </value>
        internal int StringPatternEnd
        {
            readonly get
            {
                Debug.Assert( this.PatType == PatternType.STRING, "Unexpected pattern group type" );
                return field;
            }

            set
            {
                Debug.Assert( this.PatType == PatternType.STRING, "Unexpected pattern group type" );
                field = value;
            }
        }
#else
        internal int StringPatternStart { get; set; }
        internal int StringPatternEnd { get; set; }
#endif

#if DEBUG

        internal int MinSourceIndex
        {
            readonly get
            {
                Debug.Assert( this.PatType is PatternType.STAR or PatternType.NONE, "Unexpected pattern group type" );
                return field;
            }

            set
            {
                Debug.Assert( this.PatType is PatternType.STAR or PatternType.NONE, "Unexpected pattern group type" );
                field = value;
            }
        }
#else
        internal int MinSourceIndex { get; set; }
#endif

#if DEBUG

        public string[] RangeStarts
        {
            readonly get
            {
                Debug.Assert( this.PatType is PatternType.EXCLIST or PatternType.INCLIST, "Unexpected pattern group type" );
                return field;
            }

            set
            {
                Debug.Assert( this.PatType is PatternType.EXCLIST or PatternType.INCLIST, "Unexpected pattern group type" );
                field = value;
            }
        }

        public List<Range> RangeList
        {
            readonly get
            {
                Debug.Assert( this.PatType is PatternType.EXCLIST or PatternType.INCLIST, "Unexpected pattern group type" );
                return field;
            }

            set
            {
                Debug.Assert( this.PatType is PatternType.EXCLIST or PatternType.INCLIST, "Unexpected pattern group type" );
                field = value;
            }
        }
#else
        internal List<Range> RangeList { get; set; }
#endif
        public int StartIndexOfPossibleMatch { get; set; }
    }

    private class Range
    {
        public Range() : base()
        {
        }
        /// <summary>   Constructor. </summary>
        /// <remarks>   David, 2021-06-29. </remarks>
        /// <param name="start">        The start. </param>
        /// <param name="startLength">  The start length. </param>
        /// <param name="end">          The end. </param>
        /// <param name="endLength">    The end length. </param>
        public Range( int start, int startLength, int end, int endLength ) : base()
        {
            this.Start = start;
            this.StartLength = startLength;
            this.End = end;
            this.EndLength = endLength;
        }
        /// <summary>   Index into the pattern string. </summary>
        /// <value> The start. </value>
        internal int Start { get; set; }

        internal int StartLength { get; set; }

        /// <summary>   Index into the pattern string. </summary>
        /// <value> The end. </value>
        internal int End { get; set; }
        internal int EndLength { get; set; }
    }

    /// <summary>   Builds pattern groups. </summary>
    /// <remarks>   2024-07-16.
    /// Pattern groups: <para>
    /// 1. A <see cref="string" /> of characters not containing a special pattern character. </para><para>
    /// 2. Any number of consecutive "?". </para><para>
    /// 3. Any number of consecutive "#". </para><para>
    /// 4. A bracketed character list. </para><para>
    /// 5. Any number of consecutive "*" (collapsed together). </para><para>
    /// We have a local array that is good for small patterns.
    /// If the pattern gets large, we allocate additional memory. </para><para>
    /// PG - pattern group </para> </remarks>
    /// <param name="source">                   Source for the. </param>
    /// <param name="sourceLength">             Length of the source. </param>
    /// <param name="sourceIndex">              [in,out] Zero-based index of the source. </param>
    /// <param name="sourceLigatureInfo">       Information describing the source ligature. </param>
    /// <param name="pattern">                  Specifies the pattern. </param>
    /// <param name="patternLength">            Length of the pattern. </param>
    /// <param name="patternIndex">             [in,out] Zero-based index of the pattern. </param>
    /// <param name="patternLigatureInfo">      Information describing the pattern ligature. </param>
    /// <param name="patternError">             [in,out] True to pattern error. </param>
    /// <param name="pgIndexForLastAsterisk">   [in,out] The page index for last asterisk. </param>
    /// <param name="comparer">                 The comparer. </param>
    /// <param name="options">                  Options for controlling the operation. </param>
    /// <param name="patternGroups">            [in,out] Groups the pattern belongs to. </param>
    [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    private static void BuildPatternGroups( string source, int sourceLength, ref int sourceIndex, LigatureInfo[] sourceLigatureInfo,
                                           string pattern, int patternLength, ref int patternIndex, LigatureInfo[] patternLigatureInfo,
                                           ref bool patternError, ref int pgIndexForLastAsterisk, CompareInfo comparer, CompareOptions options, ref PatternGroup[] patternGroups )
    {
        patternError = false;
        pgIndexForLastAsterisk = 0;
        int pgIndex;
        const int pgMaxCount = 16;
        patternGroups = new PatternGroup[16];
        int pGLast = pgMaxCount - 1;
        PatternType prevPatType = PatternType.NONE;
        pgIndex = 0;
        do
        {
            // Increase the size of the Pattern groups array if required
            //
            if ( pgIndex >= pGLast )
            {
                PatternGroup[] newPatternGroups = new PatternGroup[pGLast + pgMaxCount + 1];
                patternGroups.CopyTo( newPatternGroups, 0 );
                patternGroups = newPatternGroups;
                pGLast += pgMaxCount;
            }

            char p = pattern[patternIndex];
            switch ( p )
            {
                case '*':
                case '＊':
                    {
                        // Record the "*" pattern and collapse multiple contiguous "*"'s if possible
                        //
                        if ( prevPatType != PatternType.STAR )
                        {
                            prevPatType = PatternType.STAR;
                            patternGroups[pgIndex].PatType = PatternType.STAR;
                            pgIndexForLastAsterisk = pgIndex;
                            pgIndex += 1;
                        }

                        break;
                    }

                case '[':
                case '［':
                    {
                        bool seenNot = false;
                        List<Range> rangeList = [];
                        if ( !ValidateRangePattern( pattern, patternLength, ref patternIndex, patternLigatureInfo, comparer, options, ref seenNot, ref rangeList ) )
                        {
                            patternError = true;
                            return;
                        }

                        // Ignore empty "[]" and don't build a pattern group for it
                        //
                        if ( rangeList.Count == 0 )
                        {
                            break;
                        }

                        prevPatType = seenNot ? PatternType.EXCLIST : PatternType.INCLIST;

                        patternGroups[pgIndex].PatType = prevPatType;
                        patternGroups[pgIndex].CharCount = 1;
                        patternGroups[pgIndex].RangeList = rangeList;
                        pgIndex += 1;
                        break;
                    }

                case '#':
                case '＃':
                    {
                        if ( prevPatType == PatternType.DIGIT )
                        {
                            patternGroups[pgIndex - 1].CharCount += 1;
                        }
                        else
                        {
                            patternGroups[pgIndex].PatType = PatternType.DIGIT;
                            patternGroups[pgIndex].CharCount = 1;
                            pgIndex += 1;
                            prevPatType = PatternType.DIGIT;
                        }

                        break;
                    }

                case '?':
                case '？':
                    {
                        if ( prevPatType == PatternType.ANYCHAR )
                        {
                            patternGroups[pgIndex - 1].CharCount += 1;
                        }
                        else
                        {
                            patternGroups[pgIndex].PatType = PatternType.ANYCHAR;
                            patternGroups[pgIndex].CharCount = 1;
                            pgIndex += 1;
                            prevPatType = PatternType.ANYCHAR;
                        }

                        break;
                    }

                default:
                    {
                        int stringPatternStart = patternIndex;
                        int stringPatternEnd = patternIndex;
                        if ( stringPatternEnd >= patternLength )
                        {
                            stringPatternEnd = patternLength - 1;
                        }

                        if ( prevPatType == PatternType.STRING )
                        {
                            patternGroups[pgIndex - 1].CharCount += 1;
                            patternGroups[pgIndex - 1].StringPatternEnd = stringPatternEnd;
                        }
                        else
                        {
                            patternGroups[pgIndex].PatType = PatternType.STRING;
                            patternGroups[pgIndex].CharCount = 1;
                            patternGroups[pgIndex].StringPatternStart = stringPatternStart;
                            patternGroups[pgIndex].StringPatternEnd = stringPatternEnd;
                            pgIndex += 1;
                            prevPatType = PatternType.STRING;
                        }

                        break;
                    }
            }

            patternIndex += 1;
        }
        while ( patternIndex < patternLength );

        // Add ending mark
        //
        patternGroups[pgIndex].PatType = PatternType.NONE;
        patternGroups[pgIndex].MinSourceIndex = sourceLength;

        // Pattern is compiled into an array of Pattern groups.  Walk backward through list to assign max positions.
        //
        int maxPossibleStart = sourceLength;
        while ( pgIndex > 0 )
        {
            switch ( patternGroups[pgIndex].PatType )
            {
                case PatternType.STRING:
                    {
                        maxPossibleStart -= patternGroups[pgIndex].CharCount;
                        break;
                    }

                case PatternType.DIGIT:
                case PatternType.ANYCHAR:
                    {
                        maxPossibleStart -= patternGroups[pgIndex].CharCount;
                        break;
                    }

                case PatternType.EXCLIST:
                case PatternType.INCLIST:
                    {
                        maxPossibleStart -= 1;
                        break;
                    }
                // Can start anywhere

                case PatternType.STAR:
                case PatternType.NONE:
                    {
                        break;
                    }

                default:
                    {
                        Debug.Assert( false, "Unexpected pattern kind" );
                        break;
                    }
            }

            patternGroups[pgIndex].MaxSourceIndex = maxPossibleStart;
            pgIndex -= 1;
        }
    }

    /// <summary>   Match asterisk. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="source">               Source for the. </param>
    /// <param name="sourceLength">         Length of the source. </param>
    /// <param name="sourceIndex">          Zero-based index of the source. </param>
    /// <param name="sourceLigatureInfo">   Information describing the source ligature. </param>
    /// <param name="pattern">              Specifies the pattern. </param>
    /// <param name="patternLength">        Length of the pattern. </param>
    /// <param name="patternIndex">         Zero-based index of the pattern. </param>
    /// <param name="patternLigatureInfo">  Information describing the pattern ligature. </param>
    /// <param name="mismatch">             [in,out] True to mismatch. </param>
    /// <param name="patternError">         [in,out] True to pattern error. </param>
    /// <param name="comparer">             The comparer. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    private static void MatchAsterisk( string source, int sourceLength, int sourceIndex, LigatureInfo[] sourceLigatureInfo,
                                      string pattern, int patternLength, int patternIndex, LigatureInfo[] patternLigatureInfo,
                                      ref bool mismatch, ref bool patternError, CompareInfo comparer, CompareOptions options )
    {
        Debug.Assert( patternIndex <= patternLength && (pattern[patternIndex] == '*' || pattern[patternIndex] == '＊'), "Like operator - Unexpected asterisk matching" );
        mismatch = false;
        patternError = false;
        if ( patternIndex >= patternLength )
        {
            return;  // Successful match
        }

        // We've found a "*" in the pattern that is not at the end.
        // Now we need to scan ahead in the pattern and compile it
        // into an array of structures describing each pattern group.
        //

        PatternGroup[] patternGroups = [];
        int pgIndex;
        int pgIndexForLastAsterisk = default;
        BuildPatternGroups( source, sourceLength, ref sourceIndex, sourceLigatureInfo, pattern, patternLength, ref patternIndex, patternLigatureInfo, ref patternError, ref pgIndexForLastAsterisk, comparer, options, ref patternGroups );
        if ( patternError )
        {
            return;
        }

        Debug.Assert( patternGroups is not null && patternGroups.Length > 0 && patternGroups[0].PatType == PatternType.STAR, "Pattern parsing failed" );

        // Start the search
        //

        if ( patternGroups![pgIndexForLastAsterisk + 1].PatType != PatternType.NONE )
        {
            //
            // Optimize for the "<AnyPattern>*<NonStarPatterns>" case
            // Helps discard mismatches faster and in some cases, the match are also
            // faster
            //
            int savedSourceIndex = sourceIndex;
            int numberOfCharsToMatch = default;
            pgIndex = pgIndexForLastAsterisk + 1;
            do
            {
                numberOfCharsToMatch += patternGroups[pgIndex].CharCount;
                pgIndex += 1;
            }
            while ( patternGroups[pgIndex].PatType != PatternType.NONE );
            sourceIndex = sourceLength;
            SubtractChars( source, sourceLength, ref sourceIndex, numberOfCharsToMatch, sourceLigatureInfo, options );
            MatchAsterisk( source, sourceLength, sourceIndex, sourceLigatureInfo, pattern, patternLigatureInfo, patternGroups, pgIndexForLastAsterisk, ref mismatch, ref patternError, comparer, options );
            if ( patternError || mismatch )
            {
                return;
            }

            sourceLength = patternGroups[pgIndexForLastAsterisk + 1].StartIndexOfPossibleMatch;
            if ( sourceLength <= 0 )
            {
                return;
            }

            // Move the end marker to just after the last asterisk because everything afterwards have been
            // matched successfully.
            //
            Debug.Assert( patternGroups[pgIndex].PatType == PatternType.NONE, "Unexpected pattern end" );
            patternGroups[pgIndex].MaxSourceIndex = sourceLength;
            patternGroups[pgIndex].MinSourceIndex = sourceLength;
            patternGroups[pgIndex].StartIndexOfPossibleMatch = 0;
            patternGroups[pgIndexForLastAsterisk + 1] = patternGroups[pgIndex];

            // Reset the pattern group corresponding to the last asterisk because it needs to be reused in
            // the next phase of matching
            //
            patternGroups[pgIndexForLastAsterisk].MinSourceIndex = 0;
            patternGroups[pgIndexForLastAsterisk].StartIndexOfPossibleMatch = 0;
            pgIndex = pgIndexForLastAsterisk + 1;
            int maxPossibleStart = sourceLength;
            while ( pgIndex > 0 )
            {
                switch ( patternGroups[pgIndex].PatType )
                {
                    case PatternType.STRING:
                        {
                            maxPossibleStart -= patternGroups[pgIndex].CharCount;
                            break;
                        }

                    case PatternType.DIGIT:
                    case PatternType.ANYCHAR:
                        {
                            maxPossibleStart -= patternGroups[pgIndex].CharCount;
                            break;
                        }

                    case PatternType.EXCLIST:
                    case PatternType.INCLIST:
                        {
                            maxPossibleStart -= 1;
                            break;
                        }
                    // Can start anywhere

                    case PatternType.STAR:
                    case PatternType.NONE:
                        {
                            break;
                        }

                    default:
                        {
                            Debug.Assert( false, "Unexpected pattern kind" );
                            break;
                        }
                }

                patternGroups[pgIndex].MaxSourceIndex = maxPossibleStart;
                pgIndex -= 1;
            }

            sourceIndex = savedSourceIndex;
        }

        MatchAsterisk( source, sourceLength, sourceIndex, sourceLigatureInfo, pattern, patternLigatureInfo, patternGroups, 0, ref mismatch, ref patternError, comparer, options );
    }

    /// <summary>   Match asterisk. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="source">               Source for the. </param>
    /// <param name="sourceLength">         Length of the source. </param>
    /// <param name="sourceIndex">          Zero-based index of the source. </param>
    /// <param name="sourceLigatureInfo">   Information describing the source ligature. </param>
    /// <param name="pattern">              Specifies the pattern. </param>
    /// <param name="patternLigatureInfo">  Information describing the pattern ligature. </param>
    /// <param name="patternGroups">        Groups the pattern belongs to. </param>
    /// <param name="pgIndex">              Zero-based index of the page. </param>
    /// <param name="mismatch">             [in,out] True to mismatch. </param>
    /// <param name="patternError">         [in,out] True to pattern error. </param>
    /// <param name="comparer">             The comparer. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    private static void MatchAsterisk( string source, int sourceLength, int sourceIndex, LigatureInfo[] sourceLigatureInfo,
                                      string pattern, LigatureInfo[] patternLigatureInfo, PatternGroup[] patternGroups, int pgIndex,
                                      ref bool mismatch, ref bool patternError, CompareInfo comparer, CompareOptions options )
    {
        int pgPrevMismatchIndex = pgIndex;
        int prevMismatchSourceIndex = sourceIndex;
        int pgSaved = -1;
        int pgRestartAsteriskIndex = -1;
        Debug.Assert( patternGroups[pgIndex].PatType == PatternType.STAR, "Unexpected start of pattern groups list" );
        patternGroups[pgIndex].MinSourceIndex = sourceIndex;
        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
        pgIndex += 1;
        do
        {
            PatternGroup pgCurrent = patternGroups[pgIndex];
            switch ( pgCurrent.PatType )
            {
                case PatternType.STRING:
                    {
                    MatchString:
                        ;
                        if ( sourceIndex > pgCurrent.MaxSourceIndex )
                        {
                            mismatch = true;
                            return;
                        }

                        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
                        int stringPatternIndex = pgCurrent.StringPatternStart;
                        int sourceSecondCharIndex = 0;
                        int sourceMatchIndex = sourceIndex;
                        bool firstIteration = true;
                        do
                        {
                            int compareResult = CompareChars( source, sourceLength, sourceMatchIndex, ref sourceMatchIndex, sourceLigatureInfo, pattern, pgCurrent.StringPatternEnd + 1, stringPatternIndex, ref stringPatternIndex, patternLigatureInfo, comparer, options );
                            if ( firstIteration )
                            {
                                firstIteration = false;
                                sourceSecondCharIndex = sourceMatchIndex + 1;
                            }

                            if ( compareResult != 0 )
                            {
                                sourceIndex = sourceSecondCharIndex;
                                pgPrevMismatchIndex = pgIndex - 1;
                                prevMismatchSourceIndex = sourceIndex;
                                goto MatchString;
                            }

                            stringPatternIndex += 1;
                            sourceMatchIndex += 1;
                            if ( stringPatternIndex > pgCurrent.StringPatternEnd )
                            {
                                sourceIndex = sourceMatchIndex;
                                break;
                            }

                            if ( sourceMatchIndex >= sourceLength )
                            {
                                mismatch = true;
                                return;
                            }
                        }
                        while ( true );
                        break;
                    }

                case PatternType.DIGIT:
                    {
                    MatchDigits:
                        ;
                        if ( sourceIndex > pgCurrent.MaxSourceIndex )
                        {
                            mismatch = true;
                            return;
                        }

                        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
                        for ( int i = 1, loopTo = pgCurrent.CharCount; i <= loopTo; i++ )
                        {
                            char c = source[sourceIndex];
                            sourceIndex += 1;
                            if ( !char.IsDigit( c ) )
                            {
                                pgPrevMismatchIndex = pgIndex - 1;
                                prevMismatchSourceIndex = sourceIndex;
                                goto MatchDigits;
                            }
                        }

                        break;
                    }

                // Match

                case PatternType.EXCLIST:
                case PatternType.INCLIST:
                    {
                    MatchList:
                        ;
                        if ( sourceIndex > pgCurrent.MaxSourceIndex )
                        {
                            mismatch = true;
                            return;
                        }

                        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
                        if ( !MatchRangeAfterAsterisk( source, sourceLength, ref sourceIndex, sourceLigatureInfo, pattern, patternLigatureInfo, pgCurrent, comparer, options ) )
                        {
                            pgPrevMismatchIndex = pgIndex - 1;
                            prevMismatchSourceIndex = sourceIndex;
                            goto MatchList;
                        }

                        break;
                    }

                // Match

                case PatternType.ANYCHAR:
                    {
                        if ( sourceIndex > pgCurrent.MaxSourceIndex )
                        {
                            mismatch = true;
                            return;
                        }

                        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
                        for ( int i = 1, loopTo1 = pgCurrent.CharCount; i <= loopTo1; i++ )
                        {
                            if ( sourceIndex >= sourceLength )
                            {
                                mismatch = true;
                                return;
                            }

                            SkipToEndOfExpandedChar( sourceLigatureInfo, sourceLength, ref sourceIndex );
                            sourceIndex += 1;
                        }

                        break;
                    }

                case PatternType.NONE:
                    {
                        patternGroups[pgIndex].StartIndexOfPossibleMatch = pgCurrent.MaxSourceIndex;
                        Debug.Assert( sourceIndex <= pgCurrent.MaxSourceIndex, "Pattern matching lost" );
                        if ( sourceIndex < pgCurrent.MaxSourceIndex )
                        {
                            pgPrevMismatchIndex = pgIndex - 1;
                            prevMismatchSourceIndex = pgCurrent.MaxSourceIndex;
                        }

                        if ( patternGroups[pgPrevMismatchIndex].PatType is not PatternType.STAR and not PatternType.NONE )
                        {
                            // goto ShiftPosition;
                            // ShiftPosition:
                            ;
                            pgSaved = pgIndex;
                            sourceIndex = prevMismatchSourceIndex;
                            pgIndex = pgPrevMismatchIndex;
                            do
                            {
                                SubtractChars( source, sourceLength, ref sourceIndex, patternGroups[pgIndex].CharCount, sourceLigatureInfo, options );
                                pgIndex -= 1;
                            }
                            while ( patternGroups[pgIndex].PatType != PatternType.STAR );
                            sourceIndex = Math.Max( sourceIndex, patternGroups[pgIndex].MinSourceIndex + 1 );
                            patternGroups[pgIndex].MinSourceIndex = sourceIndex;
                            pgRestartAsteriskIndex = pgIndex;

                            pgIndex += 1;
                            continue;
                        }
                        else
                        {
                            return;  // Match
                        }
                    }

                case PatternType.STAR:
                    {
                        patternGroups[pgIndex].StartIndexOfPossibleMatch = sourceIndex;
                        pgCurrent.MinSourceIndex = sourceIndex;

                        // See if we've moved our starting point.  If so, we
                        // back up from the last place it moved, assigning a
                        // new minimum position in the source string.  Then
                        // we can start the search over from the new minimum
                        // position.
                        //

                        Debug.Assert( patternGroups[pgPrevMismatchIndex].PatType != PatternType.NONE, "Bad previous mismatch index" );
                        if ( patternGroups[pgPrevMismatchIndex].PatType != PatternType.STAR )
                        {
                            if ( sourceIndex > pgCurrent.MaxSourceIndex )
                            {
                                mismatch = true;
                                return;
                            }

                            // ShiftPosition:
                            // ;
                            pgSaved = pgIndex;
                            sourceIndex = prevMismatchSourceIndex;
                            pgIndex = pgPrevMismatchIndex;
                            do
                            {
                                SubtractChars( source, sourceLength, ref sourceIndex, patternGroups[pgIndex].CharCount, sourceLigatureInfo, options );
                                pgIndex -= 1;
                            }
                            while ( patternGroups[pgIndex].PatType != PatternType.STAR );
                            sourceIndex = Math.Max( sourceIndex, patternGroups[pgIndex].MinSourceIndex + 1 );
                            patternGroups[pgIndex].MinSourceIndex = sourceIndex;
                            pgRestartAsteriskIndex = pgIndex;
                        }

                        pgIndex += 1;
                        continue;
                    }

                default:
                    break;
            }

            if ( pgIndex == pgPrevMismatchIndex )
            {
                // Reached a point where we've matched before.

                if ( sourceIndex == prevMismatchSourceIndex )
                {
                    // Reached a point where we've matched before.  Jump ahead
                    // to where that left off.
                    //
                    sourceIndex = patternGroups[pgSaved].MinSourceIndex;
                    pgIndex = pgSaved;
                    pgPrevMismatchIndex = pgSaved;
                }
                else if ( sourceIndex < prevMismatchSourceIndex )
                {
                    // In certain cases involving ligatures/modifiers, the source
                    // index could be moved too far back and thus result in this
                    // scenario.
                    //
                    patternGroups[pgRestartAsteriskIndex].MinSourceIndex += 1;
                    sourceIndex = patternGroups[pgRestartAsteriskIndex].MinSourceIndex;
                    pgIndex = pgRestartAsteriskIndex + 1;
                }
                else // SourceIndex > PrevMismatchSourceIndex
                {
                    // In certain cases involving ligatures/modifiers, more source
                    // chars than the number of chars between the previous match
                    // corresponding to a "*" and the start of match for the pattern
                    // groups where PGIndex > PGPrevMismatchIndex may be matched
                    // against the pattern groups between the "*" and PGPrevMismatchIndex
                    // and thus result in this scenario.
                    //
                    pgIndex += 1;
                    pgPrevMismatchIndex = pgRestartAsteriskIndex;
                }
            }
            else
            {
                pgIndex += 1;
            }
        }
        while ( true );
    }

    /// <summary>   Match range after asterisk. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="source">               Source for the. </param>
    /// <param name="sourceLength">         Length of the source. </param>
    /// <param name="sourceIndex">          [in,out] Zero-based index of the source. </param>
    /// <param name="sourceLigatureInfo">   Information describing the source ligature. </param>
    /// <param name="pattern">              Specifies the pattern. </param>
    /// <param name="patternLigatureInfo">  Information describing the pattern ligature. </param>
    /// <param name="patternGroup">         Group the pattern belongs to. </param>
    /// <param name="comparer">             The comparer. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    private static bool MatchRangeAfterAsterisk( string source, int sourceLength, ref int sourceIndex, LigatureInfo[] sourceLigatureInfo,
                                                string pattern, LigatureInfo[] patternLigatureInfo, PatternGroup patternGroup, CompareInfo comparer, CompareOptions options )
    {
        Debug.Assert( patternGroup.PatType is PatternType.EXCLIST or PatternType.INCLIST, "Unexpected pattern group" );

        List<Range>? rangeList = patternGroup.RangeList;

        // empty [] match can be ignored

        Debug.Assert( rangeList is not null && rangeList.Count > 0, "Empty RangeList unexpected" );
        int sourceNextIndex = sourceIndex;
        bool match = false;
        foreach ( Range range in rangeList! )
        {
            Debug.Assert( range.Start >= 0, "NULL Range start unexpected" );
            int compareResultEnd = 1;
            int compareResultStart;
            if ( patternLigatureInfo is not null && patternLigatureInfo[range.Start].Kind == CharKind.ExpandedChar1 )
            {
                int argRightEnd = 0;
                compareResultStart = CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo, pattern, range.Start + range.StartLength, range.Start, ref argRightEnd, patternLigatureInfo, comparer, options, matchBothCharsOfExpandedCharInRight: true );
                if ( compareResultStart == 0 )
                {
                    match = true;
                    break;
                }
            }

            int argRightEnd1 = 0;
            compareResultStart = CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo, pattern, range.Start + range.StartLength,
                range.Start, ref argRightEnd1, patternLigatureInfo!, comparer, options, useUnexpandedCharForRight: true );
            if ( compareResultStart > 0 && range.End >= 0 )
            {
                int argRightEnd2 = 0;
                compareResultEnd = CompareChars( source, sourceLength, sourceIndex, ref sourceNextIndex, sourceLigatureInfo, pattern, range.End + range.EndLength,
                    range.End, ref argRightEnd2, patternLigatureInfo!, comparer, options, useUnexpandedCharForRight: true );
            }

            if ( compareResultStart == 0 || (compareResultStart > 0 && compareResultEnd <= 0) )
            {
                match = true;
                break;
            }
        }

        if ( patternGroup.PatType == PatternType.EXCLIST )
        {
            match = !match;
        }

        sourceIndex = sourceNextIndex + 1;
        return match;
    }

    /// <summary>   Subtract characters. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="input">                The input. </param>
    /// <param name="inputLength">          Length of the input. </param>
    /// <param name="current">              [in,out] The current. </param>
    /// <param name="charsToSubtract">      The characters to subtract. </param>
    /// <param name="inputLigatureInfo">    Information describing the input ligature. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    private static void SubtractChars( string input, int inputLength, ref int current, int charsToSubtract, LigatureInfo[] inputLigatureInfo, CompareOptions options )
    {
        if ( options == CompareOptions.Ordinal )
        {
            current -= charsToSubtract;
            if ( current < 0 )
                current = 0;
            return;
        }

        for ( int i = 1, loopTo = charsToSubtract; i <= loopTo; i++ )
        {
            SubtractOneCharInTextCompareMode( input, inputLength, ref current, inputLigatureInfo, options );
            if ( current < 0 )
            {
                current = 0;
                break;
            }
        }
    }

    /// <summary>   Subtract one character in text compare mode. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="input">                The input. </param>
    /// <param name="inputLength">          Length of the input. </param>
    /// <param name="current">              [in,out] The current. </param>
    /// <param name="inputLigatureInfo">    Information describing the input ligature. </param>
    /// <param name="options">              Options for controlling the operation. </param>
    [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    private static void SubtractOneCharInTextCompareMode( string input, int inputLength, ref int current, LigatureInfo[] inputLigatureInfo, CompareOptions options )
    {
        Debug.Assert( options != CompareOptions.Ordinal, "This method should not be invoked in Option compare binary mode" );
        if ( current >= inputLength )
        {
            current -= 1;
        }
        else if ( inputLigatureInfo is not null && inputLigatureInfo[current].Kind == CharKind.ExpandedChar2 )
        {
            current -= 2;
        }
        else
        {
            current -= 1;
        }
    }
}

