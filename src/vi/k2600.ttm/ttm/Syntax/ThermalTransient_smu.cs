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

        string foundName = string.Empty;

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
            string smuNameGetter;
            if ( meterEntity is ThermalTransientMeterEntity.MeterMain )
                smuNameGetter = $"{entityName}.smuNameGetter()";
            else
                smuNameGetter = $"{entityName}.smuI";
            foreach ( string smuName in Syntax.ThermalTransient.SourceMeasureUnitNames.Split( ',' ) )
            {
                if ( !session.IsNil( $"_G.{smuName}" ) && session.IsTrue( $"{smuNameGetter}==_G.{smuName}" ) )
                {
                    foundName = smuName;
                    break;
                }
            }
            if ( string.IsNullOrWhiteSpace( foundName ) )
                throw new InvalidOperationException( $"Failed reading Source Measure Unit;. {entityName}.smuI is neither one of {Syntax.ThermalTransient.SourceMeasureUnitNames}." );
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
                    _ = session.WriteLine( "{0}:smuNameSetter('{1}')", entityName, sourceMeasureUnitName );
            }
            else
                _ = session.WriteLine( "{0}:currentSourceChannelSetter('{1}')", entityName, sourceMeasureUnitName );
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
                    _ = session.WriteLine( $"{entityName}:smuNameSetter({defaultsName}.smuName)" );
            }
            else
                _ = session.WriteLine( $"{entityName}:currentSourceChannelSetter({{defaultsName}}.smuI)" );

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
                smuName = session.QueryTrimEnd( $"_G.print({defaultsName}.smuI)" );

            if ( string.IsNullOrWhiteSpace( smuName ) )
                throw new InvalidOperationException( $"failed reading default source measure unit name;. Sent:'{session.LastMessageSent}; Received:'{session.LastMessageReceived}'." );

        }
        return smuName;
    }

}
