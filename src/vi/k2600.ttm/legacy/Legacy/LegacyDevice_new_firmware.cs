using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{

    /*

    This code is needed for unit testing.

    */

    #region " cold resistance source output "

    /// <summary>   Gets or sets source output. </summary>
    /// <value> The source output. </value>
    [CLSCompliant( false )]
    public SourceOutputOption? SourceOutput { get; private set; }

    /// <summary>   Cold resistance source output setter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">    The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceSourceOutputSetter( SourceOutputOption value )
    {
        if ( !this.IsConnected ) return false;
        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        if ( Ttm.MeterSubsystem.LegacyFirmware )
            return true;

        bool outcome = false;
        if ( value != this.Meter.InitialResistance.SourceOutputOption )
        {
            SourceOutputOption? initialResistanceOption = this.Meter.InitialResistance.ApplySourceOutputOption( value );
            if ( !initialResistanceOption.HasValue || value != initialResistanceOption.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed setting the source output to {value}." );
            else
                outcome = true;
        }
        if ( value != this.Meter.FinalResistance.SourceOutputOption )
        {
            SourceOutputOption? finalResistanceOption = this.Meter.FinalResistance.ApplySourceOutputOption( value );
            if ( !finalResistanceOption.HasValue || value != finalResistanceOption.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed setting the source output to {value}." );
            else
                outcome = true;
        }
        this.SourceOutput = this.Meter.FinalResistance.SourceOutputOption;
        return outcome;
    }

    /// <summary>   Cold resistance source output getter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <param name="value">    [out] The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceSourceOutputGetter( out SourceOutputOption value )
    {
        if ( !this.IsConnected )
        {
            value = SourceOutputOption.Current;
            return false;
        }
        if ( Ttm.MeterSubsystem.LegacyFirmware )
        {
            value = SourceOutputOption.Current;
            return true;
        }

        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        SourceOutputOption? initialResistanceOutputOption = this.Meter.InitialResistance.QuerySourceOutputOption();
        SourceOutputOption? finalResistanceOutputOption = this.Meter.FinalResistance.QuerySourceOutputOption();
        if ( initialResistanceOutputOption is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed fetching the output option." );
        if ( finalResistanceOutputOption is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed fetching the output option." );
        this.SourceOutput = finalResistanceOutputOption;
        if ( finalResistanceOutputOption != initialResistanceOutputOption )
        {
            value = SourceOutputOption.Current;
            return false;
        }
        else
        {
            value = finalResistanceOutputOption.Value;
            return true;
        }

    }

    #endregion

    #region " cold resistance Current Limit "

    /// <summary> Gets the current limit for measuring cold resistance. The system sets a limit on the
    /// cold resistance current. </summary>
    /// <value> The cold resistance current limit. </value>
    public double? ColdResistanceCurrentLimit { get; private set; }

    /// <summary>   Cold resistance Current Limit setter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">    The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceCurrentLimitSetter( double value )
    {
        if ( !this.IsConnected ) return false;
        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        if ( Ttm.MeterSubsystem.LegacyFirmware )
            return true;

        bool outcome = false;
        if ( value != this.Meter.InitialResistance.CurrentLimit )
        {
            double? initialResistanceCurrentLimit = this.Meter.InitialResistance.ApplyCurrentLimit( value );
            if ( !initialResistanceCurrentLimit.HasValue || value != initialResistanceCurrentLimit.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed setting the Current Limit to {value}." );
            else
                outcome = true;
        }
        if ( value != this.Meter.FinalResistance.CurrentLimit )
        {
            double? finalResistanceCurrentLimit = this.Meter.FinalResistance.ApplyCurrentLimit( value );
            if ( !finalResistanceCurrentLimit.HasValue || value != finalResistanceCurrentLimit.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed setting the Current Limit to {value}." );
            else
                outcome = true;
        }
        this.ColdResistanceCurrentLimit = this.Meter.FinalResistance.CurrentLimit;
        return outcome;
    }

    /// <summary>   Cold resistance Current Limit getter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">    [out] The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceCurrentLimitGetter( out double value )
    {
        if ( !this.IsConnected )
        {
            value = default;
            return false;
        }
        if ( Ttm.MeterSubsystem.LegacyFirmware )
        {
            value = default;
            return true;
        }

        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        double? initialResistanceCurrentLimit = this.Meter.InitialResistance.QueryCurrentLimit();
        double? finalResistanceCurrentLimit = this.Meter.FinalResistance.QueryCurrentLimit();
        if ( initialResistanceCurrentLimit is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed fetching the Current Limit." );
        if ( finalResistanceCurrentLimit is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed fetching the Current Limit." );

        this.ColdResistanceCurrentLimit = this.Meter.FinalResistance.CurrentLimit;
        if ( finalResistanceCurrentLimit != initialResistanceCurrentLimit )
        {
            value = default;
            return false;
        }
        else
        {
            value = finalResistanceCurrentLimit.Value;
            return true;
        }
    }

    #endregion

    #region " cold resistance Voltage Level "

    /// <summary> Gets the voltage level for measuring cold resistance. The system sets a limit on the
    /// cold resistance voltage. </summary>
    /// <value> The cold resistance voltage level. </value>
    public double? ColdResistanceVoltageLevel { get; private set; }

    /// <summary>   Cold resistance Voltage Level setter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">    The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceVoltageLevelSetter( double value )
    {
        if ( !this.IsConnected ) return false;
        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        if ( Ttm.MeterSubsystem.LegacyFirmware )
            return true;

        bool outcome = false;
        if ( value != this.Meter.InitialResistance.VoltageLevel )
        {
            double? initialResistanceVoltageLevel = this.Meter.InitialResistance.ApplyVoltageLevel( value );
            if ( !initialResistanceVoltageLevel.HasValue || value != initialResistanceVoltageLevel.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed setting the Voltage Level to {value}." );
            else
                outcome = true;
        }
        if ( value != this.Meter.FinalResistance.VoltageLevel )
        {
            double? finalResistanceVoltageLevel = this.Meter.FinalResistance.ApplyVoltageLevel( value );
            if ( !finalResistanceVoltageLevel.HasValue || value != finalResistanceVoltageLevel.Value )
                throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed setting the Voltage Level to {value}." );
            else
                outcome = true;
        }
        this.ColdResistanceVoltageLevel = this.Meter.FinalResistance.VoltageLevel;
        return outcome;
    }

    /// <summary>   Cold resistance Voltage Level getter. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">    [out] The value. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool ColdResistanceVoltageLevelGetter( out double value )
    {
        if ( !this.IsConnected )
        {
            value = default;
            return false;
        }
        if ( Ttm.MeterSubsystem.LegacyFirmware )
        {
            value = default;
            return true;
        }

        if ( this.Meter.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} is null." );
        if ( this.Meter.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} is null." );

        double? initialResistanceVoltageLevel = this.Meter.InitialResistance.QueryVoltageLevel();
        double? finalResistanceVoltageLevel = this.Meter.InitialResistance.QueryVoltageLevel();
        if ( initialResistanceVoltageLevel is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.InitialResistance )} failed fetching the Voltage Level." );
        if ( finalResistanceVoltageLevel is null )
            throw new InvalidOperationException( $"{nameof( Ttm.Meter.FinalResistance )} failed fetching the Voltage Level." );
        this.ColdResistanceVoltageLevel = this.Meter.FinalResistance.VoltageLevel;
        if ( finalResistanceVoltageLevel != initialResistanceVoltageLevel )
        {
            value = default;
            return false;
        }
        else
        {
            value = finalResistanceVoltageLevel.Value;
            return true;
        }
    }

    #endregion

}
