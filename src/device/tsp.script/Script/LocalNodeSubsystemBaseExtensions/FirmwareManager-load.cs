using System.Diagnostics;
using cc.isr.VI.Pith;
using cc.isr.VI.Syntax.Tsp;

namespace cc.isr.VI.Tsp.Script.LocalNodeSubsystemBaseExtensions;

public static partial class FirmwareManager
{
    #region " Load from file "

    /// <summary>
    /// Loads a named script into the instrument allowing control over how errors and prompts are
    /// handled. For loading a script that does not includes functions, turn off errors and turn on
    /// the prompt.
    /// </summary>
    /// <remarks>   2024-09-05. This method uses the interactive subsystem to monitor the instrument state. </remarks>
    /// <exception cref="NativeException">              Thrown when a Native error condition occurs. </exception>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="DeviceException">              Thrown when a Device error condition occurs. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when operation failed to execute. </exception>
    /// <param name="interactiveSubsystem"> The interactive subsystem. </param>
    /// <param name="script">               Specifies reference to a valid <see cref="ScriptEntity">
    ///                                     script</see> </param>
    /// <param name="showErrors">           Specifies the condition for turning off or on error
    ///                                           checking while the script is loaded. </param>
    /// <param name="showPrompts">          Specifies the condition for turning off or on the TSP
    ///                                           prompts while the script is loaded. </param>
    /// <param name="retainOutline">        Specifies if the code outline is retained or trimmed. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    public static void LoadScriptFile( this Tsp.LocalNodeSubsystemBase interactiveSubsystem, ScriptEntityBase script, bool showErrors, bool showPrompts, bool retainOutline )
    {
        if ( interactiveSubsystem is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( interactiveSubsystem )} is null." );
        if ( interactiveSubsystem.Session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( interactiveSubsystem.Session )} is null." );
        if ( interactiveSubsystem.StatusSubsystem is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( interactiveSubsystem.StatusSubsystem )} is null." );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( script.FirmwareScript.Name is null || string.IsNullOrWhiteSpace( script.FirmwareScript.Name ) ) throw new NativeException( $"{nameof( script.FirmwareScript.Name )} is null." );
        if ( script.FirmwareScript.FileName is null || string.IsNullOrWhiteSpace( script.FirmwareScript.FileName ) ) throw new NativeException( $"{nameof( script.FirmwareScript.FileName )} is null." );
        if ( script.FirmwareScript.ResourceFileName is null || string.IsNullOrWhiteSpace( script.FirmwareScript.ResourceFileName ) ) throw new NativeException( $"{nameof( script.FirmwareScript.ResourceFileName )} is null." );
        if ( script.FirmwareScript.FolderPath is null || string.IsNullOrWhiteSpace( script.FirmwareScript.FolderPath ) ) throw new NativeException( $"{nameof( script.FirmwareScript.FolderPath )} is null." );

        Pith.SessionBase session = interactiveSubsystem.Session;
        session.LastNodeNumber = script.Node.Number;

        // store the status
        interactiveSubsystem.StoreStatus();
        string chunkLine;
        try
        {
            bool isInCommentBlock;
            isInCommentBlock = false;
            int lineNumber;
            lineNumber = 0;

            if ( interactiveSubsystem.ShowErrorsEnabled || interactiveSubsystem.ShowPromptsEnabled )
                // flush the buffer until empty and update the TSP status.
                _ = session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );

            // store state of prompts and errors.
            // statusSubsystem.StoreStatus()

            if ( interactiveSubsystem.ShowErrors is null || (interactiveSubsystem.ShowErrors != showErrors) )
                // Disable automatic display of errors - leave error messages in queue
                _ = interactiveSubsystem.WriteShowErrors( showErrors );

            // set prompts
            if ( interactiveSubsystem.ShowPrompts is null || (interactiveSubsystem.ShowPrompts != showPrompts) )
                _ = interactiveSubsystem.WriteShowPrompts( showPrompts );

            if ( interactiveSubsystem.ShowErrorsEnabled || interactiveSubsystem.ShowPromptsEnabled )
                // flush the buffer until empty and update the TSP status.
                _ = session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );

            bool isFirstLine;
            isFirstLine = true;
            string commandLine;
            bool wasInCommentBlock;
            LuaChunkLineContentType lineType;
            string activity;
            string actionDetails;
            using ( StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( script.FirmwareScript.ResourceFilePath ) )
            {
                if ( tspFile is null )
                    throw new System.IO.FileNotFoundException( "Failed opening TSP Script file", script.FirmwareScript.ResourceFilePath );

                string trimmedFileSuffix = ".trimmed.tsp";
                string trimmedFilePath = script.FirmwareScript.ResourceFilePath.Replace( ".tsp", trimmedFileSuffix );
                if ( string.Equals( trimmedFilePath, script.FirmwareScript.ResourceFilePath ) )
                    trimmedFilePath = script.FirmwareScript.ResourceFilePath + trimmedFileSuffix;
                using StreamWriter trimmedFile = new( trimmedFilePath );
                session.SetLastAction( $"sending a 'loadscript' for script '{script.FirmwareScript.Name}' from file '{script.FirmwareScript.ResourceFilePath}'" );
                session.LastNodeNumber = default;
                while ( !tspFile.EndOfStream )
                {
                    string line = tspFile.ReadLine();
                    chunkLine = Lua.ReplaceTabs( line );
                    chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();
                    lineNumber += 1;
                    wasInCommentBlock = isInCommentBlock;
                    lineType = Lua.ParseLuaChuckLine( chunkLine, isInCommentBlock );
                    if ( lineType == LuaChunkLineContentType.None )
                    {
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

                        // end each line with a space
                        chunkLine += " ";
                        if ( isFirstLine )
                        {
                            // issue a start of script command.  The command
                            // 'loadscript' identifies the beginning of the named script.
                            commandLine = "loadscript " + script.FirmwareScript.Name + " ";
                            activity = $"sending a {commandLine}";
                            actionDetails = $"sending a {commandLine} from file '{script.FirmwareScript.ResourceFilePath}'";
                            session.SetLastAction( actionDetails );
                            _ = session.WriteLine( commandLine );
                            if ( ( int? ) interactiveSubsystem.ExecutionState != ( int? ) TspExecutionState.IdleError )

                                isFirstLine = false;

                            else
                            {
                                session.ThrowDeviceExceptionIfError();
                                throw new InvalidOperationException( $"{session.LastAction} failed;. {actionDetails}; unexpected execution state of '{interactiveSubsystem.ExecutionState}'; No device errors" );
                            }
                        }
                        // Continuation prompt. TSP received script line successfully; waiting for next line.
                        switch ( interactiveSubsystem.ExecutionState )
                        {
                            case TspExecutionState.IdleContinuation:
                                {
                                    break;
                                }

                            case TspExecutionState.IdleError:
                                {
                                    Debug.Assert( !Debugger.IsAttached, "this should not happen :)" );
                                    break;
                                }

                            case TspExecutionState.IdleReady:
                                {
                                    // Ready prompt. TSP received script successfully; ready for next command.
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                                // do nothing
                        }

                        activity = $"sending a syntax line";
                        actionDetails = $"sending a syntax line: \n{chunkLine}\nfor script {script.FirmwareScript.Name} from file '{script.FirmwareScript.ResourceFilePath}'";
                        session.SetLastAction( activity );
                        session.SetLastActionDetails( actionDetails );
                        _ = session.WriteLine( chunkLine );
                        if ( ( int? ) interactiveSubsystem.ExecutionState == ( int? ) TspExecutionState.IdleError )
                        {
                            // now report the error to the calling module; Read Status Register invokes the query for the device errors.
                            session.ThrowDeviceExceptionIfError();
                            throw new InvalidOperationException(
                                $"{session.LastAction} failed;. {actionDetails}; unexpected execution state of '{interactiveSubsystem.ExecutionState}'; No device errors" );
                        }
                        else
                            // increment debug line number
                            trimmedFile.WriteLine( chunkLine );

                        switch ( interactiveSubsystem.ExecutionState )
                        {
                            case TspExecutionState.IdleError:
                                {
                                    Debug.Assert( !Debugger.IsAttached, "this should not happen :)" );
                                    break;
                                }

                            case TspExecutionState.IdleReady:
                                {
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                                // do nothing
                        }

                        if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                        {
                            isInCommentBlock = true;
                        }
                    }
                }
            }

            // Tell TSP complete script has been downloaded.
            commandLine = $"endscript {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} ";

            activity = $"sending a {commandLine}";
            actionDetails = $"sending a {commandLine} from file '{script.FirmwareScript.ResourceFilePath}'";
            session.SetLastAction( activity );
            session.SetLastActionDetails( actionDetails );

            _ = session.WriteLine( commandLine );
            if ( ( int? ) interactiveSubsystem.ExecutionState == ( int? ) TspExecutionState.IdleError )
            {
                session.ThrowDeviceExceptionIfError();
                throw new InvalidOperationException(
                    $"{session.LastAction} failed;. {actionDetails}; unexpected execution state of '{interactiveSubsystem.ExecutionState}'; No device errors" );
            }

            // wait till we get a reply from the instrument or completionTimeout.

            // The command above does not seem to work!  It looks like the print does not get executed!
            Stopwatch sw = Stopwatch.StartNew();
            TimeSpan timeout = TimeSpan.FromMilliseconds( 3000d );
            string value = string.Empty;
            do
            {
                do
                    System.Threading.Thread.Sleep( 50 );
                while ( !session.QueryMessageAvailableBit() && sw.Elapsed <= timeout );
                if ( session.QueryMessageAvailableBit() )
                {
                    value = session.ReadLine();
                    if ( !value.StartsWith( cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue, StringComparison.OrdinalIgnoreCase ) )
                    {
                        _ = interactiveSubsystem.ParseExecutionState( value, TspExecutionState.IdleReady );
                    }
                }
            }
            while ( !value.StartsWith( cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue, StringComparison.OrdinalIgnoreCase ) && sw.Elapsed <= timeout );
            if ( sw.Elapsed > timeout )
            {
                // Throw New cc.isr.Tsp.ScriptCallException("Timeout waiting operation completion loading the script '" & Me._name & "'")
            }

            // add a wait to ensure the system returns the last status.
            System.Threading.Thread.Sleep( 100 );

            if ( interactiveSubsystem.ShowErrorsEnabled || interactiveSubsystem.ShowPromptsEnabled )
                // flush the buffer until empty and update the TSP status.
                _ = session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );

            // get the script state if showing prompts
            switch ( interactiveSubsystem.ExecutionState )
            {
                case TspExecutionState.IdleError:
                    {
                        Debug.Assert( !Debugger.IsAttached, "this should not happen :)" );
                        break;
                    }

                default:
                    {
                        break;
                    }
                    // do nothing
            }
        }
        catch
        {
            if ( interactiveSubsystem.ShowErrorsEnabled || interactiveSubsystem.ShowPromptsEnabled )
                // flush the buffer until empty and update the TSP status.
                _ = session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            throw;
        }
        finally
        {
            // restore state of prompts and errors.
            interactiveSubsystem.RestoreStatus();
        }
    }

    #endregion
}
