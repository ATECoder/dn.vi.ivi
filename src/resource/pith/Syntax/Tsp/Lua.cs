namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the syntax of the LUA base system. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
[CLSCompliant( false )]
public static class Lua
{
    #region " constants "

    /// <summary> Represents the LUA nil value. </summary>
    public const string NilValue = "nil";

    /// <summary> Represents the LUA true value </summary>
    public const string TrueValue = "true";

    /// <summary> Represents the LUA false value </summary>
    public const string FalseValue = "false";

    /// <summary> The global node reference. </summary>
    public const string GlobalNode = "_G";

    #endregion

    #region " chunk constants "

    /// <summary> Gets the chunk defining a start of comment block. </summary>
    public const string StartCommentChunk = "--[[";

    /// <summary> Gets the chunk defining an end of comment block. </summary>
    public const string EndCommentChunk = "]]--";

    /// <summary> Gets the chunk defining a comment. </summary>
    public const string CommentChunk = "--";

    /// <summary> Gets the signature of a chunk line defining a chunk name. </summary>
    public const string DeclareChunkNameSignature = "local chunkName =";

    /// <summary> Gets the signature of a chunk line defining a require statement for the chunk name. </summary>
    public const string RequireChunkNameSignature = "require(\"";

    /// <summary> Gets the signature of a chunk line defining a loaded statement for the chunk name. </summary>
    public const string LoadedChunkNameSignature = "_G._lOADED[";

    #endregion

    #region " script commands "

    /// <summary>
    /// Gets a command to retrieve a catalog from the local node.
    /// This command must be enclosed in a 'do end' construct.
    /// a print(names) or dataqueue.add(names) needs to be added to get the data through.
    /// </summary>
    public const string ScriptCatalogGetterCommand = "local names='' for name in script.user.catalog() do names = names .. name .. ',' end";

    /// <summary>   (Immutable) the load string LUA command. </summary>
    public const string LoadStringCommand = "loadstring";

    /// <summary>   (Immutable) the load string command end. </summary>
    public const string LoadStringCommandEnd = "))()";

    #endregion

    #region " system commands "

    /// <summary> The Clear Status command (same as '*CLS'). </summary>
    public const string ClearExecutionStateCommand = "_G.status.reset()";

    /// <summary>   (Immutable) the clear execution state operation complete command. </summary>
    public const string ClearExecutionStateOperationCompleteCommand = "_G.status.reset() _G.opc()";

    /// <summary>   (Immutable) the clear execution state wait command. </summary>
    public const string ClearExecutionStateWaitCommand = "_G.status.reset() _G.waitcomplete()";

    /// <summary>   (Immutable) the clear execution state query complete command. </summary>
    public const string ClearExecutionStateQueryCompleteCommand = "_G.status.reset() _G.waitcomplete() print('1') ";

    /// <summary> The collect garbage command. </summary>
    public const string CollectGarbageCommand = "_G.collectgarbage()";

    /// <summary> The  collect garbage command operation complete command. </summary>
    public const string CollectGarbageOperationCompleteCommand = "_G.collectgarbage() _G.opc()";

    /// <summary> The  collect garbage command wait complete command. </summary>
    public const string CollectGarbageWaitCommand = "_G.collectgarbage() _G.waitcomplete()";

    /// <summary> The  collect garbage command wait complete command. </summary>
    public const string CollectGarbageQueryCompleteCommand = "_G.collectgarbage() _G.waitcomplete() print('1') ";

    /// <summary>
    /// The operation complete command (Same as '*OPC'). This function sets the operation complete
    /// status bit when all overlapped commands are completed.
    /// </summary>
    /// <remarks> *OPC will fail on a compound command (e.g., _G.opc(); *OPC) </remarks>
    public const string OperationCompleteCommand = "_G.opc()";

    /// <summary> Gets or set the operation completed query command. </summary>
    /// <remarks> Same as '*OPC?'. </remarks>
    public const string OperationCompletedQueryCommand = "_G.waitcomplete() print('1') ";

    /// <summary> The query (print) command format. </summary>
    public const string PrintCommandFormat = "_G.print({0})";

    /// <summary> The query (print) command to send to the instrument. The
    /// format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    /// <remarks> The format string follows the same rules as the printf family of standard C
    /// functions. The only differences are that the options or modifiers *, l, L, n, p, and h are
    /// not supported and that there is an extra option, q. The q option formats a string in a form
    /// suitable to be safely read back by the Lua interpreter: the string is written between double
    /// quotes, and all double quotes, newlines, embedded zeros, and backslashes in the string are
    /// correctly escaped when written. For instance, the call string.format('%q', 'a string with
    /// ''quotes'' and [BS]n new line') will produce the string: a string with [BS]''quotes[BS]'' and
    /// [BS]new line The options c, d, E, e, f, g, G, i, o, u, X, and x all expect a number as
    /// argument, whereas q and s expect a string. This function does not accept string values
    /// containing embedded zeros. </remarks>
    public const string PrintCommandStringFormat = "_G.print(string.format('{0}',{1}))";

    /// <summary> The query (print) command string number format. </summary>
    /// <remarks> _G.print(string.format('%9.6f',smu.source.ilimit.level))\n </remarks>
    public const string PrintCommandStringNumberFormat = "_G.print(string.format('%{0}f',{1}))";

    /// <summary> The query (print) command string integer format. </summary>
    public const string PrintCommandStringIntegerFormat = "_G.print(string.format('%d',{0}))";

    /// <summary> Returns the query (print) command for the specified arguments. </summary>
    /// <param name="args"> Specifies the function arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string PrintCommand( string args )
    {
        return Syntax.Tsp.Constants.Build( PrintCommandFormat, args );
    }

    /// <summary> The reset to known state command (Same as '*RST'). </summary>
    public const string ResetKnownStateCommand = "_G.reset()";

    /// <summary>   (Immutable) the reset known state operation complete command. </summary>
    public const string ResetKnownStateOperationCompleteCommand = "_G.reset() _G.opc()";

    /// <summary>   (Immutable) the reset known state wait command. </summary>
    public const string ResetKnownStateWaitCommand = "_G.reset() _G.waitcomplete()";

    /// <summary> The wait command. </summary>
    public const string WaitGroupCommandFormat = "_G.waitcomplete({0})";

    /// <summary>
    /// (Immutable) the wait command. This command is used to suspend the execution of subsequent
    /// commands until the device operations of all previous overlapped commands are finished. This
    /// command is not needed for sequential commands, which are allowed to finish before the next
    /// command is executed.
    /// </summary>
    public const string WaitCommand = "_G.waitcomplete()";

    #endregion

    #region " command builders "

    /// <summary> Builds a command. </summary>
    /// <param name="format"> Specifies a format string for the command. </param>
    /// <param name="args">   Specifies the arguments for the command. </param>
    /// <returns> The command. </returns>
    public static string Build( string format, params object[] args )
    {
        return string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args );
    }

    #endregion

    #region " system commands "

    /// <summary> Gets the function call command for a function w/o arguments. </summary>
    private const string CallFunctionCommandFormat = "_G.pcall( {0} )";

    /// <summary>
    /// Gets the function call command for a function with arguments.
    /// </summary>
    private const string CallFunctionArgumentsCommandFormat = "_G.pcall( {0} , {1} )";

    /// <summary> Returns a command to run the specified function with arguments. </summary>
    /// <param name="functionName"> Specifies the function name. </param>
    /// <param name="args">         Specifies the function arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string CallFunctionCommand( string functionName, string args )
    {
        return string.IsNullOrWhiteSpace( args ) ? Build( CallFunctionCommandFormat, functionName ) : Build( CallFunctionArgumentsCommandFormat, functionName, args );
    }

    #endregion

    #region " function "

    /// <summary>
    /// Returns a string from the parameter array of arguments for use when running the function.
    /// </summary>
    /// <param name="args"> Specifies a parameter array of arguments. </param>
    /// <returns> A comma-separated string. </returns>
    public static string Parameterize( params string[] args )
    {
        System.Text.StringBuilder arguments = new();
        int i;
        if ( args is not null && args.Length >= 0 )
        {
            int loopTo = args.Length - 1;
            for ( i = 0; i <= loopTo; i++ )
            {
                if ( i > 0 ) _ = arguments.Append( "," );

                _ = arguments.Append( args[i] );
            }
        }

        return arguments.ToString();
    }

    #endregion

    #region " trim Lua source code "

    /// <summary>   Remove comments and spaces from the Lua source code. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="source">           specifies the source code for the script. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    /// <returns>   Parsed script. </returns>
    public static string TrimLuaSourceCode( string source, bool retainOutline )
    {
        if ( source is null ) throw new ArgumentNullException( nameof( source ) );
        string[] sourceLines = source.Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        System.Text.StringBuilder newSource = new();
        bool isInCommentBlock = false;
        bool wasInCommentBlock;
        bool wasInLoadStringBlock;
        bool isInLoadStringBlock = false;
        LuaChunkLineContentType lineType;
        foreach ( string line in sourceLines )
        {
            string chunkLine = ReplaceTabs( line );
            chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();
            wasInCommentBlock = isInCommentBlock;
            wasInLoadStringBlock = isInLoadStringBlock;

            // this just gets the line type.
            lineType = ParseLuaChunkLine( chunkLine, isInCommentBlock );

            if ( lineType == LuaChunkLineContentType.None )
            {
            }
            else if ( lineType == LuaChunkLineContentType.LoadStringBlockStart )
            {
                isInLoadStringBlock = true;
                _ = newSource.AppendLine( chunkLine );
            }
            else if ( wasInLoadStringBlock )
            {
                if ( lineType == LuaChunkLineContentType.LoadStringBlockEnd )
                {
                    isInLoadStringBlock = true;
                    _ = newSource.AppendLine( chunkLine );
                }
            }

            // if no data, nothing to do.

            else if ( wasInCommentBlock )
            {
                // if was in a comment block exit the comment block if
                // received a end of comment block
                if ( lineType == LuaChunkLineContentType.EndCommentBlock )
                {
                    isInCommentBlock = false;
                }
            }
            else if ( lineType == LuaChunkLineContentType.StartCommentBlock )
            {
                isInCommentBlock = true;
            }
            else if ( lineType == LuaChunkLineContentType.Comment )
            {
            }

            // if comment line do nothing

            else if ( lineType is LuaChunkLineContentType.Syntax or LuaChunkLineContentType.SyntaxStartCommentBlock )
            {
                if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                {
                    chunkLine = chunkLine[..chunkLine.IndexOf( Syntax.Tsp.Lua.StartCommentChunk, StringComparison.OrdinalIgnoreCase )];
                }

                if ( !string.IsNullOrWhiteSpace( chunkLine ) )
                {
                    _ = newSource.AppendLine( chunkLine );
                }

                if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                {
                    isInCommentBlock = true;
                }
            }
        }

        return newSource.ToString();
    }

    /// <summary>
    /// Parses a Lua chuck line returning the <see cref="LuaChunkLineContentType"/>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="chunkLine">        Specifies the chunk line. </param>
    /// <param name="isInCommentBlock"> <c>true</c> if this object is in comment block. </param>
    /// <returns>   The parsed <see cref="LuaChunkLineContentType">content type.</see> </returns>
    public static LuaChunkLineContentType ParseLuaChunkLine( string chunkLine, bool isInCommentBlock )
    {
        chunkLine ??= string.Empty;

        // if outline is retained, we must remove the leasing tabs and spaces in order to detect the line type.,
        chunkLine = chunkLine.Trim().Replace( Convert.ToChar( 9, System.Globalization.CultureInfo.CurrentCulture ).ToString(), " " ).Trim();
        if ( string.IsNullOrWhiteSpace( chunkLine ) )
        {
            return LuaChunkLineContentType.None;
        }

        // check if load string block
        else if ( chunkLine.Contains( Syntax.Tsp.Lua.LoadStringCommand, StringComparison.OrdinalIgnoreCase ) )
        {
            return LuaChunkLineContentType.LoadStringBlockStart;
        }

        else if ( chunkLine.Contains( Syntax.Tsp.Lua.LoadStringCommandEnd, StringComparison.OrdinalIgnoreCase ) )
        {
            return LuaChunkLineContentType.LoadStringBlockEnd;
        }

        // check if start of comment block
        else if ( chunkLine.StartsWith( Syntax.Tsp.Lua.StartCommentChunk, StringComparison.OrdinalIgnoreCase ) )
        {
            return LuaChunkLineContentType.StartCommentBlock;
        }
        else if ( chunkLine.Contains( Syntax.Tsp.Lua.StartCommentChunk ) )
        {
            return LuaChunkLineContentType.SyntaxStartCommentBlock;
        }

        // check if in a comment block
        else if ( isInCommentBlock && chunkLine.Contains( Syntax.Tsp.Lua.EndCommentChunk ) )
        {
            // check if end of comment block
            return LuaChunkLineContentType.EndCommentBlock;
        }

        // skip comment lines.
        else
        {
            return chunkLine.StartsWith( Syntax.Tsp.Lua.CommentChunk, StringComparison.OrdinalIgnoreCase )
                ? LuaChunkLineContentType.Comment
                : LuaChunkLineContentType.Syntax;
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
/// <remarks>   2024-09-05. </remarks>
public enum LuaChunkLineContentType
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

    /// <summary>   An enum constant representing the <see cref="Lua.LoadStringCommand"/> option. </summary>
    [System.ComponentModel.Description( "Load String Command Line" )]
    LoadStringBlockStart,

    /// <summary>   An enum constant representing the <see cref="Lua.LoadStringCommandEnd"/> option. </summary>
    [System.ComponentModel.Description( "Load String End Command Line" )]
    LoadStringBlockEnd,

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

