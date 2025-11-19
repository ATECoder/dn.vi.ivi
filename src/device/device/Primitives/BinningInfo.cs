// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;
using System.Reflection;

namespace cc.isr.VI;

/// <summary> Defines the binning information . </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05, . based on SCPI 5.1 library. </para>
/// </remarks>
[CLSCompliant( false )]
public class BinningInfo : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public BinningInfo() : base()
    {
    }

    #endregion

    #region " i presettable "

    /// <summary> Defines values for the known clear state. </summary>
    public void DefineClearExecutionState()
    {
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public void InitKnownState()
    {
    }

    /// <summary> Sets the known preset state. </summary>
    public void PresetKnownState()
    {
    }

    /// <summary> Sets the known reset (default) state. </summary>
    private void ResetKnownStateThis()
    {
        this._limitFailed = new bool?();
        this._enabled = false;
        this._upperLimit = 1d;
        this._upperLimitFailureBits = 15;
        this._lowerLimit = -1;
        this._lowerLimitFailureBits = 15;
        this._passBits = 15;
        this._strobePulseWidth = TimeSpan.FromTicks( ( long ) (0.01d * TimeSpan.TicksPerMillisecond) );
        this._inputLineNumber = 1;
        this._outputLineNumber = 2;
        this._armCount = 0;
        this._armDirection = TriggerLayerBypassModes.Acceptor;
        this._armSource = ArmSources.Immediate;
        this._triggerDirection = TriggerLayerBypassModes.Acceptor;
        this._triggerSource = TriggerSources.Immediate;
    }

    /// <summary> Sets the known reset (default) state. </summary>
    public void ResetKnownState()
    {
        this.ResetKnownStateThis();
    }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        MulticastDelegate? event_delegate = ( MulticastDelegate ) this.GetType().GetField( nameof( this.PropertyChanged ),
                                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField ).GetValue( this );
        Delegate[]? delegates = event_delegate.GetInvocationList();
        if ( delegates is not null )
        {
            foreach ( Delegate? item in delegates )
            {
                this.PropertyChanged -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " arm source "

    /// <summary> The arm source mode. </summary>
    private ArmSources _armSource;

    /// <summary> Gets or sets the arm Source. </summary>
    /// <value>
    /// The <see cref="ArmSource">source Function Mode</see> or none if not set or unknown.
    /// </value>
    public ArmSources ArmSource
    {
        get => this._armSource;
        set
        {
            if ( !this.ArmSource.Equals( value ) )
            {
                this._armSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " count "

    /// <summary> Number of arms. </summary>
    private int _armCount;

    /// <summary> Gets or sets the arm Count. </summary>
    /// <value> The source Count or none if not set or unknown. </value>
    public int ArmCount
    {
        get => this._armCount;
        set
        {
            if ( !Equals( this.ArmCount, value ) )
            {
                this._armCount = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " arm layer bypass mode "

    /// <summary> The Arm Layer Bypass Mode. </summary>
    private TriggerLayerBypassModes _armDirection;

    /// <summary> Gets or sets the Arm Layer Bypass Mode. </summary>
    /// <value>
    /// The <see cref="ArmDirection">Arm Layer Bypass Mode</see> or none if not set or unknown.
    /// </value>
    public TriggerLayerBypassModes ArmDirection
    {
        get => this._armDirection;
        set
        {
            if ( !this.ArmDirection.Equals( value ) )
            {
                this._armDirection = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " enabled "

    /// <summary> The on/off state. </summary>
    private bool _enabled;

    /// <summary> Gets or sets the enabled state. </summary>
    /// <value>
    /// <c>true</c> if the Enabled; <c>false</c> if not, or none if unknown or not set.
    /// </value>
    public bool Enabled
    {
        get => this._enabled;
        set
        {
            if ( this.Enabled != value )
            {
                this._enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " failure bits "

    /// <summary> The Failure Bits. </summary>

    /// <summary> Gets or sets the Output fail bit pattern (15). </summary>
    /// <value> The Failure Bits or none if not set or unknown. </value>
    public int FailureBits
    {
        get;
        set
        {
            if ( this.FailureBits != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " input line number "

    /// <summary> The Input Line Number. </summary>
    private int _inputLineNumber;

    /// <summary> Gets or sets the arm Input Line Number. </summary>
    /// <value> The source Input Line Number or none if not set or unknown. </value>
    public int InputLineNumber
    {
        get => this._inputLineNumber;
        set
        {
            if ( !Equals( this.InputLineNumber, value ) )
            {
                this._inputLineNumber = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " limit failed "

    /// <summary> The Limit Failed. </summary>
    private bool? _limitFailed;

    /// <summary> Gets or sets the fail condition (False) state. </summary>
    /// <value>
    /// <c>true</c> if the Limit Failed; <c>false</c> if not, or none if unknown or not set.
    /// </value>
    public bool? LimitFailed
    {
        get => this._limitFailed;
        set
        {
            if ( !Equals( this.LimitFailed, value ) )
            {
                this._limitFailed = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " lower limit "

    /// <summary> The Lower Limit. </summary>
    private double _lowerLimit;

    /// <summary> Gets or sets the lower limit. </summary>
    /// <value> The Lower Limit or none if not set or unknown. </value>
    public double LowerLimit
    {
        get => this._lowerLimit;
        set
        {
            if ( this.LowerLimit != value )
            {
                this._lowerLimit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " lower limit failure bits "

    /// <summary> The Lower Limit Failure Bits. </summary>
    private int _lowerLimitFailureBits;

    /// <summary> Gets or sets the lower limit failure bit pattern (15). </summary>
    /// <value> The Lower Limit FailureBits or none if not set or unknown. </value>
    public int LowerLimitFailureBits
    {
        get => this._lowerLimitFailureBits;
        set
        {
            if ( this.LowerLimitFailureBits != value )
            {
                this._lowerLimitFailureBits = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " output line number "

    /// <summary> The Output Line Number. </summary>
    private int _outputLineNumber;

    /// <summary> Gets or sets the Output Line Number. </summary>
    /// <value> The source Output Line Number or none if not set or unknown. </value>
    public int OutputLineNumber
    {
        get => this._outputLineNumber;
        set
        {
            if ( !Equals( this.OutputLineNumber, value ) )
            {
                this._outputLineNumber = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " pass bits "

    /// <summary> The Pass Bits. </summary>
    private int _passBits;

    /// <summary> Gets or sets the Output pass bit pattern (15). </summary>
    /// <value> The Pass Bits or none if not set or unknown. </value>
    public int PassBits
    {
        get => this._passBits;
        set
        {
            if ( this.PassBits != value )
            {
                this._passBits = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " strobe pulse width "

    /// <summary> The Strobe Pulse Width. </summary>
    private TimeSpan _strobePulseWidth;

    /// <summary> Gets or sets the end of test strobe pulse width. </summary>
    /// <value> The Strobe Pulse Width or none if not set or unknown. </value>
    public TimeSpan StrobePulseWidth
    {
        get => this._strobePulseWidth;
        set
        {
            if ( this.StrobePulseWidth != value )
            {
                this._strobePulseWidth = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " trigger source "

    /// <summary> The Trigger source mode. </summary>
    private TriggerSources _triggerSource;

    /// <summary> Gets or sets the Trigger Source. </summary>
    /// <value>
    /// The <see cref="TriggerSource">source Function Mode</see> or none if not set or unknown.
    /// </value>
    public TriggerSources TriggerSource
    {
        get => this._triggerSource;
        set
        {
            if ( !this.TriggerSource.Equals( value ) )
            {
                this._triggerSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " trigger direction "

    /// <summary> The Trigger Direction. </summary>
    private TriggerLayerBypassModes _triggerDirection;

    /// <summary> Gets or sets the trigger Direction. </summary>
    /// <value>
    /// The <see cref="TriggerDirection">Trigger Direction</see> or none if not set or unknown.
    /// </value>
    public TriggerLayerBypassModes TriggerDirection
    {
        get => this._triggerDirection;
        set
        {
            if ( !this.TriggerDirection.Equals( value ) )
            {
                this._triggerDirection = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " upper limit "

    /// <summary> The Upper Limit. </summary>
    private double _upperLimit;

    /// <summary> Gets or sets the Upper limit. </summary>
    /// <value> The Upper Limit or none if not set or unknown. </value>
    public double UpperLimit
    {
        get => this._upperLimit;
        set
        {
            if ( this.UpperLimit != value )
            {
                this._upperLimit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " upper limit failure bits "

    /// <summary> The Upper Limit Failure Bits. </summary>
    private int _upperLimitFailureBits;

    /// <summary> Gets or sets the Upper limit failure bit pattern (15). </summary>
    /// <value> The Upper Limit FailureBits or none if not set or unknown. </value>
    public int UpperLimitFailureBits
    {
        get => this._upperLimitFailureBits;
        set
        {
            if ( this.UpperLimitFailureBits != value )
            {
                this._upperLimitFailureBits = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion
}
