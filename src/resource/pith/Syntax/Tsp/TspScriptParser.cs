namespace cc.isr.VI.Syntax.Tsp;

/// <summary>   A tsp script parser. </summary>
/// <remarks>   2025-04-11. </remarks>
public static class TspScriptParser
{
    #region " trim source code "

    /// <summary>   Remove comments and spaces from the TSP source code. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="inputFilePath">    Full pathname of the input file. </param>
    /// <param name="outputFilePath">   Full pathname of the output file. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    ///
    /// ### <returns>   Parsed script. </returns>
    public static void TrimTspSourceCode( string inputFilePath, string outputFilePath, bool retainOutline )
    {
        if ( inputFilePath is null ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( outputFilePath is null ) throw new ArgumentNullException( nameof( outputFilePath ) );

        using StreamReader? reader = new System.IO.StreamReader( inputFilePath )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the source file path" );

        using StreamWriter? writer = new System.IO.StreamWriter( outputFilePath )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader for the destination file path" );

        TspParserState parseState = new( retainOutline );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null
                && !string.IsNullOrWhiteSpace( line )
                && !line.Contains( Syntax.Tsp.Script.LoadScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.EndScriptCommand ) )
            {
                // if the line is not empty, not a comment, and not a command, write it to the new source.
                parseState.TrimTspSourceCode( line );
                if ( !string.IsNullOrWhiteSpace( parseState.SyntaxLine ) )
                    writer.WriteLine( parseState.SyntaxLine );
            }
        }
        // add an empty line
        writer.WriteLine();
    }

    /// <summary>   Remove comments and spaces from the TSP source code. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="source">           specifies the source code for the script. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    /// <returns>   Parsed script. </returns>
    public static string TrimTspSourceCode( string source, bool retainOutline )
    {
        if ( source is null ) throw new ArgumentNullException( nameof( source ) );

        System.Text.StringBuilder newSource = new();

        TspParserState parseState = new( retainOutline );

        using TextReader? reader = new System.IO.StringReader( source )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the script source" );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null
                && !string.IsNullOrWhiteSpace( line )
                && !line.Contains( Syntax.Tsp.Script.LoadScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.EndScriptCommand ) )
            {
                // if the line is not empty, not a comment, and not a command, write it to the new source.
                parseState.TrimTspSourceCode( line );
                if ( !string.IsNullOrWhiteSpace( parseState.SyntaxLine ) )
                    _ = newSource.AppendLine( parseState.SyntaxLine );
            }
        }
        return newSource.ToString();
    }

    /// <summary>
    /// Parses a Tsp line that was trimmed with tabs removed returning the <see cref="TspChunkLineContentType"/>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="trimmedLine">      Specifies the trimmed TSP line with tab removed. </param>
    /// <param name="isInCommentBlock"> <c>true</c> if this object is in comment block. </param>
    /// <returns>   The parsed <see cref="TspChunkLineContentType">content type.</see> </returns>
    public static TspChunkLineContentType ParseTrimmedTspLine( string trimmedLine, bool isInCommentBlock )
    {
        trimmedLine ??= string.Empty;

        if ( string.IsNullOrWhiteSpace( trimmedLine ) )
        {
            return TspChunkLineContentType.None;
        }

        // check if load string block
        else if ( trimmedLine.Contains( Syntax.Tsp.Lua.LoadStringCommand, StringComparison.OrdinalIgnoreCase ) )
        {
            return TspChunkLineContentType.LoadStringBlockStart;
        }

        else if ( trimmedLine.Contains( Syntax.Tsp.Lua.LoadStringCommandEnd, StringComparison.OrdinalIgnoreCase ) )
        {
            return TspChunkLineContentType.LoadStringBlockEnd;
        }

        // check if byte code script block
        else if ( trimmedLine.Contains( Syntax.Tsp.Script.StartOfBinaryScript, StringComparison.OrdinalIgnoreCase ) )
        {
            return TspChunkLineContentType.BinaryScriptBlockStart;
        }

        else if ( trimmedLine.Contains( Syntax.Tsp.Script.EndOfBinaryScript, StringComparison.OrdinalIgnoreCase ) )
        {
            return TspChunkLineContentType.BinaryScriptBlockEnding;
        }

        // check if start of comment block
        else if ( trimmedLine.StartsWith( Syntax.Tsp.Lua.StartCommentBlockChunk, StringComparison.OrdinalIgnoreCase ) )
        {
            return TspChunkLineContentType.StartCommentBlock;
        }
        else if ( trimmedLine.Contains( Syntax.Tsp.Lua.StartCommentBlockChunk ) )
        {
            return TspChunkLineContentType.SyntaxStartCommentBlock;
        }

        // check if in a comment block
        else if ( isInCommentBlock && trimmedLine.Contains( Syntax.Tsp.Lua.EndCommentBlockChunk ) )
        {
            // check if end of comment block
            return TspChunkLineContentType.EndCommentBlock;
        }

        // skip comment lines.
        else
        {
            return trimmedLine.StartsWith( Syntax.Tsp.Lua.CommentChunk, StringComparison.OrdinalIgnoreCase )
                ? TspChunkLineContentType.Comment
                : TspChunkLineContentType.Syntax;
        }
    }

    /// <summary>   Replace tabs with two spaces. </summary>
    /// <remarks>   2024-11-18. </remarks>
    /// <param name="chunkLine">    Specifies the line. </param>
    /// <returns>   A string. </returns>
    public static string ReplaceTabs( string chunkLine )
    {
        return chunkLine.Replace( Convert.ToChar( 9, System.Globalization.CultureInfo.CurrentCulture ).ToString(), "  " );
    }

    #endregion

}

/// <summary>   Enumerates the content type of a TSP chunk line. </summary>
/// <remarks>   2025-04-11. </remarks>
public enum TspChunkLineContentType
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined" )]
    None,

    /// <summary> An enum constant representing the start comment block option. </summary>
    [System.ComponentModel.Description( "Start Comment Block" )]
    StartCommentBlock,

    /// <summary> An enum constant representing the end comment block option. </summary>
    [System.ComponentModel.Description( "End Comment Block" )]
    EndCommentBlock,

    /// <summary> An enum constant representing the comment option. </summary>
    [System.ComponentModel.Description( "Comment" )]
    Comment,

    /// <summary>   An enum constant representing the load string start block option. </summary>
    [System.ComponentModel.Description( "Load String Start Block" )]
    LoadStringBlockStart,

    /// <summary>   An enum constant representing the load string end block option. </summary>
    [System.ComponentModel.Description( "Load String End Block" )]
    LoadStringBlockEnd,

    /// <summary>   An enum constant representing the byte code script start block option. </summary>
    [System.ComponentModel.Description( "byte code script Start Block" )]
    BinaryScriptBlockStart,

    /// <summary>   An enum constant representing the byte code script block ending option. </summary>
    /// <remarks> The block ends if the next line includes a closing braces. </remarks>
    [System.ComponentModel.Description( "byte code script Block Ending" )]
    BinaryScriptBlockEnding,

    /// <summary>   An enum constant representing the byte code script end block option. </summary>
    [System.ComponentModel.Description( "byte code script End Block" )]
    BinaryScriptBlockEnd,

    /// <summary> An enum constant representing the syntax option. </summary>
    [System.ComponentModel.Description( "Ieee488Syntax" )]
    Syntax,

    /// <summary> An enum constant representing the syntax-start-comment block option. </summary>
    [System.ComponentModel.Description( "Ieee488Syntax and Start Comment Block" )]
    SyntaxStartCommentBlock,

    /// <summary> An enum constant representing the chunk-name-declaration option. </summary>
    [System.ComponentModel.Description( "Chunk Name Declaration" )]
    ChunkNameDeclaration,

    /// <summary> An enum constant representing the chunk-name-require option. </summary>
    [System.ComponentModel.Description( "Chunk Name Requirement" )]
    ChunkNameRequire,

    /// <summary> An enum constant representing the chunk-name-loaded option. </summary>
    [System.ComponentModel.Description( "Chunk Name Loaded" )]
    ChunkNameLoaded
}

/// <summary>   A tsp parser state. </summary>
/// <remarks>   2025-04-11. </remarks>
public class TspParserState( bool retainOutline )
{
    /// <summary>   Gets or sets the initial input line before . </summary>
    /// <value> The input line. </value>
    public string InputLine { get; set; } = string.Empty;

    /// <summary>   Gets or sets the line after stripping leading and lagging tabs and spaces. </summary>
    /// <value> The chunk line. </value>
    public string TrimmedLine { get; set; } = string.Empty;

    /// <summary>   Gets or sets the syntax line (non-comment or empty line) that might still retain the outline. </summary>
    /// <value> The syntax line. </value>
    public string SyntaxLine { get; set; } = string.Empty;

    /// <summary>   Gets or sets a value indicating whether the retain the code outline. </summary>
    /// <value> True if retain the code outline, false if not. </value>
    public bool RetainOutline { get; set; } = retainOutline;

    /// <summary>
    /// Gets or sets a value indicating whether this object is in comment block.
    /// </summary>
    /// <value> True if this object is in comment block, false if not. </value>
    public bool IsInCommentBlock { get; set; } = false;

    /// <summary>   Gets or sets a value indicating whether the was in comment block. </summary>
    /// <value> True if was in comment block, false if not. </value>
    public bool WasInCommentBlock { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this object is in LoadString block.
    /// </summary>
    /// <value> True if this object is in LoadString block, false if not. </value>
    public bool IsInLoadStringBlock { get; set; } = false;

    /// <summary>   Gets or sets a value indicating whether the was in LoadString block. </summary>
    /// <value> True if was in LoadString block, false if not. </value>
    public bool WasInLoadStringBlock { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this object is in BinaryScript block.
    /// </summary>
    /// <value> True if this object is in BinaryScript block, false if not. </value>
    public bool IsInBinaryScriptBlock { get; set; } = false;

    /// <summary>   Gets or sets a value indicating whether the was in BinaryScript block. </summary>
    /// <value> True if was in BinaryScript block, false if not. </value>
    public bool WasInBinaryScriptBlock { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this object is in BinaryScriptEnding block.
    /// </summary>
    /// <value> True if this object is in BinaryScriptEnding block, false if not. </value>
    public bool IsInBinaryScriptEndingBlock { get; set; } = false;

    /// <summary>   Gets or sets a value indicating whether the was in BinaryScriptEnding block. </summary>
    /// <value> True if was in BinaryScriptEnding block, false if not. </value>
    public bool WasInBinaryScriptEndingBlock { get; set; } = false;

    /// <summary>   Gets or sets the line number. </summary>
    /// <value> The line number. </value>
    public int LineNumber { get; set; } = 0;

    /// <summary>   Gets or sets a value indicating whether this object is first line. </summary>
    /// <value> True if this object is first line, false if not. </value>
    public bool IsFirstLine => this.LineNumber == 1;

    /// <summary>   Gets or sets the command line. </summary>
    /// <value> The command line. </value>
    public string CommandLine { get; set; } = string.Empty;

    /// <summary>   Gets or sets the type of the line. </summary>
    /// <value> The type of the line. </value>
    public TspChunkLineContentType LineType { get; set; } = TspChunkLineContentType.None;

    /// <summary>   Gets or sets the type of the previous line. </summary>
    /// <value> The type of the previous line. </value>
    public TspChunkLineContentType PreviousLineType { get; set; } = TspChunkLineContentType.None;

    /// <summary>   Remove comments and spaces from the TSP source code. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="chunkLine">    The chunk line. </param>
    public void TrimTspSourceCode( string chunkLine )
    {
        if ( chunkLine is null ) throw new ArgumentNullException( nameof( chunkLine ) );

        TspChunkLineContentType lineType;
        this.InputLine = chunkLine;
        string candidateSyntaxLine = TspScriptParser.ReplaceTabs( chunkLine );
        this.TrimmedLine = candidateSyntaxLine.Trim();
        candidateSyntaxLine = this.RetainOutline ? candidateSyntaxLine.TrimEnd() : candidateSyntaxLine.Trim();
        this.SyntaxLine = string.Empty;

        // this just gets the line type.
        lineType = TspScriptParser.ParseTrimmedTspLine( this.TrimmedLine, this.IsInCommentBlock );

        this.PreviousLineType = this.LineType;
        this.LineType = lineType;

        this.WasInCommentBlock = this.IsInCommentBlock;
        this.WasInLoadStringBlock = this.IsInLoadStringBlock;
        this.WasInBinaryScriptBlock = this.IsInBinaryScriptBlock;
        this.WasInBinaryScriptEndingBlock = this.IsInBinaryScriptEndingBlock;

        if ( lineType == TspChunkLineContentType.None )
        {
        }

        else if ( lineType == TspChunkLineContentType.LoadStringBlockStart )
        {
            this.IsInLoadStringBlock = true;
            this.SyntaxLine = candidateSyntaxLine;
        }
        else if ( this.WasInLoadStringBlock )
        {
            this.SyntaxLine = candidateSyntaxLine;

            if ( lineType == TspChunkLineContentType.LoadStringBlockEnd )
            {
                this.IsInLoadStringBlock = true;
            }
            else if ( lineType == TspChunkLineContentType.BinaryScriptBlockStart )
            {
                this.IsInBinaryScriptBlock = true;
            }
            else if ( this.WasInBinaryScriptBlock )
            {
                if ( lineType == TspChunkLineContentType.BinaryScriptBlockEnding )
                {
                    this.IsInBinaryScriptEndingBlock = true;
                }
            }
            else if ( this.WasInBinaryScriptEndingBlock )
            {
                if ( candidateSyntaxLine.Contains( "}" ) )
                {
                    this.IsInBinaryScriptEndingBlock = false;
                    this.IsInBinaryScriptBlock = false;
                }
                else
                {
                    this.IsInBinaryScriptEndingBlock = false;
                }
            }
        }

        // if no data, nothing to do.

        else if ( this.WasInCommentBlock )
        {
            // if was in a comment block exit the comment block if
            // received a end of comment block
            if ( lineType == TspChunkLineContentType.EndCommentBlock )
                this.IsInCommentBlock = false;
        }
        else if ( lineType == TspChunkLineContentType.StartCommentBlock )
        {
            this.IsInCommentBlock = true;
        }
        else if ( lineType == TspChunkLineContentType.Comment )
        {
        }

        // if comment line do nothing

        else if ( lineType is TspChunkLineContentType.Syntax or TspChunkLineContentType.SyntaxStartCommentBlock )
        {
            if ( lineType == TspChunkLineContentType.SyntaxStartCommentBlock )
                candidateSyntaxLine = candidateSyntaxLine[..candidateSyntaxLine.IndexOf( Syntax.Tsp.Lua.StartCommentBlockChunk, StringComparison.OrdinalIgnoreCase )];
            else if ( !string.IsNullOrWhiteSpace( candidateSyntaxLine ) && candidateSyntaxLine.Contains( Syntax.Tsp.Lua.CommentChunk ) )
                // remove a trailing comment.
                candidateSyntaxLine = candidateSyntaxLine[..candidateSyntaxLine.IndexOf( Syntax.Tsp.Lua.CommentChunk, StringComparison.OrdinalIgnoreCase )].TrimEnd();

            if ( !string.IsNullOrWhiteSpace( candidateSyntaxLine ) )
            {
                this.SyntaxLine = candidateSyntaxLine;
            }

            if ( lineType == TspChunkLineContentType.SyntaxStartCommentBlock )
                this.IsInCommentBlock = true;
        }

        if ( !string.IsNullOrWhiteSpace( this.SyntaxLine ) )
        {
            // The trimmed line up to and including version 9181 ended with a space except for the last line.
            // this.SyntaxLine += " ";
            this.LineNumber += 1;
        }
    }
}

