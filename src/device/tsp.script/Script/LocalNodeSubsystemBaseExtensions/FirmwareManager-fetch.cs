using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.LocalNodeSubsystemBaseExtensions;

public static partial class FirmwareManager
{
    #region " fetch user script names "

    /// <summary>   Gets all users scripts from the instrument. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="interactiveSubsystem">     The interactive subsystem. </param>
    /// <returns>   The number of user script names that were fetched. </returns>
    public static string[] FetchUserScriptNames( Tsp.LocalNodeSubsystemBase? interactiveSubsystem )
    {
        if ( interactiveSubsystem is null ) throw new ArgumentNullException( nameof( interactiveSubsystem ) );
        if ( interactiveSubsystem.Session is null ) throw new ArgumentNullException( nameof( Tsp.LocalNodeSubsystemBase.Session ) );

        string[] useScriptNames = [];
        // store state of prompts and errors.
        interactiveSubsystem.StoreStatus();
        interactiveSubsystem.Session.LastNodeNumber = default;
        bool processExecutionStateWasEnabled = interactiveSubsystem.ProcessExecutionStateEnabled;
        // disable session events.
        interactiveSubsystem.ProcessExecutionStateEnabled = false;
        try
        {
            useScriptNames = interactiveSubsystem.Session.FetchUserScriptNames();
            if ( interactiveSubsystem.ShowPromptsEnabled || interactiveSubsystem.ShowPromptsEnabled )
                _ = interactiveSubsystem.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100d ) );
        }
        catch
        {
            throw;
        }
        finally
        {
            // restore state of prompts and errors.
            interactiveSubsystem.RestoreStatus();

            // add a wait to ensure the system returns the last status.
            System.Threading.Thread.Sleep( 100 );

            Pith.ServiceRequests bits = interactiveSubsystem.Session.MeasurementEventBitmask;
            interactiveSubsystem.Session.MeasurementEventBitmask = interactiveSubsystem.Session.MessageAvailableBitmask;

            interactiveSubsystem.Session.MeasurementEventBitmask = bits;
            interactiveSubsystem.ProcessExecutionStateEnabled = processExecutionStateWasEnabled;
        }
        return useScriptNames;
    }

    #endregion
}
