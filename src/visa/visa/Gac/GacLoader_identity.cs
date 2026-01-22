namespace cc.isr.Visa.Gac;

public static partial class GacLoader
{
    /// <summary>   Attempts to query identity. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="visaSession">  The visa session. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    [CLSCompliant( false )]
    public static string TryQueryIdentity( Ivi.Visa.IVisaSession? visaSession, out string details )
    {
        if ( visaSession is null )
        {
            details = "VISA session is null.";
            return string.Empty;
        }

        if ( visaSession is Ivi.Visa.IMessageBasedSession messageBasedSession )
        {
            try
            {
                // Ensure termination character is enabled as here in example we use a SOCKET connection.
                messageBasedSession.TerminationCharacterEnabled = true;

                // Request information about an instrument.
                messageBasedSession.FormattedIO.WriteLine( "*IDN?" );
                string identity = messageBasedSession.FormattedIO.ReadLine();
                if ( string.IsNullOrWhiteSpace( identity ) )
                {
                    details = "Received empty or whitespace response from the instrument.";
                    return string.Empty;
                }
                details = string.Empty;
                return identity;
            }
            catch ( Exception ex )
            {
                details = GacLoader.BuildErrorMessage( ex );
                throw;
            }
        }
        else
        {
            details = $"{visaSession.GetType()} is not a message-based session.";
            return string.Empty;
        }
    }
}
