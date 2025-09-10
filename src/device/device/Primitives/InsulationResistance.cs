using System.ComponentModel;

namespace cc.isr.VI;

/// <summary> An insulation test configuration. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-03-09 </para>
/// </remarks>
[CLSCompliant( false )]
public class InsulationResistance : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public InsulationResistance() : base()
    {
    }

    #endregion

    #region " i presettable "

    /// <summary> Defines values for the known clear state. </summary>
    public virtual void DefineClearExecutionState()
    {
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public virtual void InitKnownState()
    {
    }

    /// <summary> Sets the known preset state. </summary>
    public virtual void PresetKnownState()
    {
    }

    /// <summary> Sets the known reset (default) state. </summary>
    public virtual void ResetKnownState()
    {
        this.DwellTime = TimeSpan.FromSeconds( 2d );
        this.CurrentLimit = 0.00001d;
        this.PowerLineCycles = 1d;
        this.VoltageLevel = 10d;
        this.ResistanceLowLimit = 10000000d;
        this.ResistanceRange = 1000000000d;
        this.ContactCheckEnabled = true;
    }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        base.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    #endregion

    #region " fields "

    /// <summary> The dwell time. </summary>
    private TimeSpan _dwellTime;

    /// <summary> Gets or sets the dwell time. </summary>
    /// <value> The dwell time. </value>
    public TimeSpan DwellTime
    {
        get => this._dwellTime;
        set
        {
            if ( this.DwellTime != value )
            {
                this._dwellTime = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the current limit. </summary>
    /// <value> The current limit. </value>
    public double CurrentLimit
    {
        get;
        set
        {
            if ( value != this.CurrentLimit )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.CurrentRange ) );
            }
        }
    }

    /// <summary> Gets or sets the power line cycles. </summary>
    /// <value> The power line cycles. </value>
    public double PowerLineCycles
    {
        get;
        set
        {
            if ( value != this.PowerLineCycles )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the voltage level. </summary>
    /// <value> The voltage level. </value>
    public double VoltageLevel
    {
        get;
        set
        {
            if ( value != this.VoltageLevel )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.CurrentRange ) );
            }
        }
    }

    /// <summary> Gets or sets the resistance low limit. </summary>
    /// <value> The resistance low limit. </value>
    public double ResistanceLowLimit
    {
        get;
        set
        {
            if ( value != this.ResistanceLowLimit )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.CurrentRange ) );
            }
        }
    }

    /// <summary> Gets or sets the resistance range. </summary>
    /// <value> The resistance range. </value>
    public double ResistanceRange
    {
        get;
        set
        {
            if ( value != this.ResistanceRange )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the contact check enabled. </summary>
    /// <value> The contact check enabled. </value>
    public bool ContactCheckEnabled
    {
        get;
        set
        {
            if ( value != this.ContactCheckEnabled )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets the current range. Based on the ratio of the voltage to the minimum resistance. If the
    /// resistance is zero, returns the current limit.
    /// </summary>
    /// <value> The current range. </value>
    public double CurrentRange => this.ResistanceLowLimit > 0d ? this.VoltageLevel / this.ResistanceLowLimit : 1.01d * this.CurrentLimit;

    #endregion
}
