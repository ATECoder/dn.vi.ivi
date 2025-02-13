namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

public static partial class ThermalTransient
{
    /// <summary>   Queries if a given source measure unit exists. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="session">                  The session. </param>
    /// <param name="sourceMeasureUnitName">    Name of the source measure unit, e.g., 'smua' or
    ///                                         'smub'. </param>
    /// <returns>   <c>true</c> if the source measure unit exists; otherwise <c>false</c> </returns>
    public static bool SourceMeasureUnitExists( Pith.SessionBase session, string sourceMeasureUnitName )
    {
        session.MakeTrueFalseReplyIfEmpty( string.Equals( sourceMeasureUnitName, Syntax.ThermalTransient.DefaultSourceMeterName, StringComparison.OrdinalIgnoreCase ) );
        return !session.IsNil( $"{cc.isr.VI.Syntax.Tsp.Constants.LocalNode}.{sourceMeasureUnitName}" );
    }

    /// <summary> Requires source measure unit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns>
    /// <c>true</c> if this entity requires the assignment of the source measure unit.
    /// </returns>
    public static bool RequiresSourceMeasureUnit( ThermalTransientMeterEntity meterEntity )
    {
        return meterEntity is ThermalTransientMeterEntity.MeterMain or ThermalTransientMeterEntity.FinalResistance
            or ThermalTransientMeterEntity.InitialResistance or ThermalTransientMeterEntity.Transient;
    }

    /// <summary>   Looks up a given key to find its associated source measure unit name. </summary>
    /// <remarks>   2025-02-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="candidateNames">   A comma-separated list of names of the candidates. </param>
    /// <param name="isSmuQueryFormat"> The is smu query format. </param>
    /// <returns>   A string. </returns>
    public static string LookupSourceMeasureUnitName( Pith.SessionBase session, string candidateNames, string isSmuQueryFormat )
    {
        if ( string.IsNullOrWhiteSpace( candidateNames ) ) return string.Empty;
        if ( string.IsNullOrWhiteSpace( isSmuQueryFormat ) ) return string.Empty;

        string foundName = string.Empty;

        foreach ( string smuName in candidateNames.Split( ',' ) )
        {
            string isSmuQuery = string.Format( isSmuQueryFormat, smuName );
            if ( !session.IsNil( $"{smuName}" ) )
            {
                // string boolValue = session.QueryTrimEnd( $"_G.print(_G.ttm.smuNameGetter()=='{smuName}')" );
                // if ( !session.IsNil( $"{smuName}" ) && session.IsTrue( $"{isSmuQuery}=='{smuName}'" ) )
                if ( !session.IsNil( $"{smuName}" ) && session.IsTrue( isSmuQuery ) )
                {
                    foundName = smuName;
                    break;
                }
            }
        }
        if ( string.IsNullOrWhiteSpace( foundName ) )
            throw new InvalidOperationException( $"Failed finding source Measure Unit;. {isSmuQueryFormat} matched none of the candidate names: '{candidateNames}'." );

        return foundName;
    }

    /// <summary>   Looks up a given key to find its associated source measure unit name. </summary>
    /// <remarks>   2025-02-12. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="isSmuQueryFormat"> The is smu query format. </param>
    /// <returns>   A string. </returns>
    public static string LookupSourceMeasureUnitName( Pith.SessionBase session, string isSmuQueryFormat )
    {
        return LookupSourceMeasureUnitName( session, Syntax.ThermalTransient.SourceMeasureUnitNames, isSmuQueryFormat );
    }


    /// <summary>
    /// Queries the Source Measure Unit. Also sets the <see cref="SourceMeasureUnit">Source Measure
    /// Unit</see>.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <returns> The Source Measure Unit. </returns>
    public static string QuerySourceMeasureUnit( Pith.SessionBase session, ThermalTransientMeterEntity meterEntity )
    {
        if ( !ThermalTransient.RequiresSourceMeasureUnit( meterEntity ) ) return string.Empty;

        string foundName;

        string entityName = ThermalTransient.SelectEntityName( meterEntity );

        session.MakeEmulatedReplyIfEmpty( true );

        if ( meterEntity is ThermalTransientMeterEntity.MeterMain && MeterSubsystem.LegacyFirmware )
        {
            if ( session.IsNil( $"_G.{Syntax.ThermalTransient.DefaultSourceMeterName}" ) )
                throw new InvalidOperationException( $"Failed reading Source Measure Unit;. Local node SMU is not {Syntax.ThermalTransient.DefaultSourceMeterName}." );
            else
                // the legacy firmware knows only of the default SMU.
                foundName = Syntax.ThermalTransient.DefaultSourceMeterName;
        }
        else
        {
            string isSmuQueryFormat = meterEntity is ThermalTransientMeterEntity.MeterMain
                ? $"{entityName}.smuNameGetter()=='{{0}}'"
                : $"{entityName}.smuI=={{0}}";
            foundName = ThermalTransient.LookupSourceMeasureUnitName( session, isSmuQueryFormat );
        }
        return foundName;
    }

    /// <summary>   Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="meterEntity">  The meter entity. </param>
    /// <param name="sourceMeasureUnitName">        the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    /// <returns>   The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public static string WriteSourceMeasureUnit( Pith.SessionBase session, ThermalTransientMeterEntity meterEntity, string sourceMeasureUnitName )
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( meterEntity ) )
        {
            if ( !MeterMain.ValidateSourceMeasureUnitName( session, sourceMeasureUnitName, out string details ) )
                throw new ArgumentOutOfRangeException( nameof( sourceMeasureUnitName ), details );


            string entityName = ThermalTransient.SelectEntityName( meterEntity );

            if ( meterEntity is ThermalTransientMeterEntity.MeterMain )
            {
                if ( !MeterSubsystem.LegacyFirmware )
                    _ = session.WriteLine( $"{entityName}:smuNameSetter('{sourceMeasureUnitName}')" );
            }
            else
                _ = session.WriteLine( $"{entityName}:currentSourceChannelSetter({sourceMeasureUnitName})" );
        }
        return sourceMeasureUnitName;
    }

    /// <summary>   Writes a source measure unit defaults. </summary>
    /// <remarks>   2024-11-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="meterEntity">  The meter entity. </param>
    public static void WriteSourceMeasureUnitDefaults( Pith.SessionBase session, ThermalTransientMeterEntity meterEntity )
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( meterEntity ) )
        {
            string entityName = ThermalTransient.SelectEntityName( meterEntity );
            string defaultsName = ThermalTransient.SelectEntityDefaultsName( meterEntity );

            if ( meterEntity is ThermalTransientMeterEntity.MeterMain )
            {
                if ( !MeterSubsystem.LegacyFirmware )
                {
                    _ = session.WriteLine( $"{entityName}.smuNameSetter({defaultsName}.smuName)" );
                }
            }
            else
                _ = session.WriteLine( $"{entityName}:currentSourceChannelSetter({defaultsName}.smuI)" );

            _ = session.QueryOperationCompleted();
        }
    }

    /// <summary>   Reads source measure unit defaults. </summary>
    /// <remarks>   2024-11-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="meterEntity">  The meter entity. </param>
    /// <returns>   The source measure unit defaults. </returns>
    public static string ReadSourceMeasureUnitDefaults( Pith.SessionBase session, ThermalTransientMeterEntity meterEntity )
    {

        string smuName = string.Empty;
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( meterEntity ) )
        {
            string defaultsName = ThermalTransient.SelectEntityDefaultsName( meterEntity );

            if ( meterEntity is ThermalTransientMeterEntity.MeterMain )
            {
                if ( MeterSubsystem.LegacyFirmware )
                    smuName = Syntax.ThermalTransient.DefaultSourceMeterName;
                else
                    smuName = session.QueryTrimEnd( $"_G.print({defaultsName}.smuName)" );
            }
            else
            {
                string isSmuQueryFormat = $"{defaultsName}.smuI=={{0}}";
                smuName = ThermalTransient.LookupSourceMeasureUnitName( session, isSmuQueryFormat );
            }

            if ( string.IsNullOrWhiteSpace( smuName ) )
                throw new InvalidOperationException( $"failed reading default source measure unit name;. Sent:'{session.LastMessageSent}; Received:'{session.LastMessageReceived}'." );

        }
        return smuName;
    }

}
