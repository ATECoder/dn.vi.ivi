using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Settings;

/// <summary>   The Route Subsystem Test Settings base class. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
[CLSCompliant( false )]
public class SenseSubsystemSettings() : System.ComponentModel.INotifyPropertyChanged
{
    #region " notify property change implementation "

    /// <summary>   Occurs when a property value changes. </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>   Executes the 'property changed' action. </summary>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void OnPropertyChanged( string? propertyName )
    {
        if ( !string.IsNullOrEmpty( propertyName ) )
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Executes the 'property changed' action. </summary>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="backingField"> [in,out] The backing field. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected virtual bool OnPropertyChanged<T>( ref T backingField, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = "" )
    {
        if ( EqualityComparer<T>.Default.Equals( backingField, value ) )
            return false;

        backingField = value;
        this.OnPropertyChanged( propertyName );
        return true;
    }

    /// <summary>   Sets a property. </summary>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="prop">         [in,out] The property. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected bool SetProperty<T>( ref T prop, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null )
    {
        if ( EqualityComparer<T>.Default.Equals( prop, value ) ) return false;
        prop = value;
        this.OnPropertyChanged( propertyName );
        return true;
    }

    /// <summary>   Sets a property. </summary>
    /// <remarks>   2023-03-24. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="oldValue">     The old value. </param>
    /// <param name="newValue">     The new value. </param>
    /// <param name="callback">     The callback. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected bool SetProperty<T>( T oldValue, T newValue, Action callback, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null )
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( callback, nameof( callback ) );
#else
        if ( callback is null ) throw new ArgumentNullException( nameof( callback ) );
#endif

        if ( EqualityComparer<T>.Default.Equals( oldValue, newValue ) )
        {
            return false;
        }

        callback();

        this.OnPropertyChanged( propertyName );

        return true;
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;
        if ( handler is not null )
        {
            foreach ( Delegate? item in handler.GetInvocationList() )
            {
                handler -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion

    #region " initial values "

    private bool _frontTerminalsControlEnabled;

    /// <summary>
    /// Gets or sets the front terminals control enabled. With manual front terminals switch, this
    /// would normally be set to <c><see langword="False"/></c>.
    /// </summary>
    /// <value> The front terminals control enabled. </value>
    public virtual bool FrontTerminalsControlEnabled
    {
        get => this._frontTerminalsControlEnabled;
        set => _ = this.SetProperty( ref this._frontTerminalsControlEnabled, value );
    }

    private bool _initialFrontTerminalsSelected = true;

    /// <summary> Gets or sets the initial front terminals selected. </summary>
    /// <value> The initial front terminals selected. </value>
    public virtual bool InitialFrontTerminalsSelected
    {
        get => this._initialFrontTerminalsSelected;
        set => _ = this.SetProperty( ref this._initialFrontTerminalsSelected, value );
    }

    private double _initialPowerLineCycles = 1;

    /// <summary> Gets or sets the Initial power line cycles settings. </summary>
    /// <value> The power line cycles settings. </value>
    public virtual double InitialPowerLineCycles
    {
        get => this._initialPowerLineCycles;
        set => _ = this.SetProperty( ref this._initialPowerLineCycles, value );
    }

    private bool _initialAutoDelayEnabled;

    /// <summary> Gets or sets the Initial auto Delay Exists settings. </summary>
    /// <value> The auto Delay settings. </value>
    public virtual bool InitialAutoDelayEnabled
    {
        get => this._initialAutoDelayEnabled;
        set => _ = this.SetProperty( ref this._initialAutoDelayEnabled, value );
    }

    private bool _initialAutoRangeEnabled = true;

    /// <summary> Gets or sets the Initial auto Range enabled settings. </summary>
    /// <value> The auto Range settings. </value>
    public virtual bool InitialAutoRangeEnabled
    {
        get => this._initialAutoRangeEnabled;
        set => _ = this.SetProperty( ref this._initialAutoRangeEnabled, value );
    }

    private bool _initialAutoZeroEnabled = true;

    /// <summary> Gets or sets the Initial auto zero Exists settings. </summary>
    /// <value> The auto zero settings. </value>
    public virtual bool InitialAutoZeroEnabled
    {
        get => this._initialAutoZeroEnabled;
        set => _ = this.SetProperty( ref this._initialAutoZeroEnabled, value );
    }

    private SenseFunctionModes _initialSenseFunction = SenseFunctionModes.VoltageDC;

    /// <summary> Gets or sets the initial sense function. </summary>
    /// <value> The initial sense function. </value>
    public virtual SenseFunctionModes InitialSenseFunction
    {
        get => this._initialSenseFunction;
        set => _ = this.SetProperty( ref this._initialSenseFunction, value );
    }

    private MultimeterFunctionModes _initialMultimeterFunction = MultimeterFunctionModes.VoltageDC;

    /// <summary> Gets or sets the initial multimeter function. </summary>
    /// <value> The initial multimeter function. </value>
    public virtual MultimeterFunctionModes InitialMultimeterFunction
    {
        get => this._initialMultimeterFunction;
        set => _ = this.SetProperty( ref this._initialMultimeterFunction, value );
    }

    private bool _initialFilterEnabled;

    /// <summary> Gets or sets the initial filter enabled. </summary>
    /// <value> The initial filter enabled. </value>
    public virtual bool InitialFilterEnabled
    {
        get => this._initialFilterEnabled;
        set => _ = this.SetProperty( ref this._initialFilterEnabled, value );
    }

    private bool _initialMovingAverageFilterEnabled;

    /// <summary> Gets or sets the initial moving average filter enabled. </summary>
    /// <value> The initial moving average filter enabled. </value>
    public virtual bool InitialMovingAverageFilterEnabled
    {
        get => this._initialMovingAverageFilterEnabled;
        set => _ = this.SetProperty( ref this._initialMovingAverageFilterEnabled, value );
    }

    private int _initialFilterCount = 10;

    /// <summary> Gets or sets the number of initial filter points. </summary>
    /// <value> The number of initial filter points. </value>
    public virtual int InitialFilterCount
    {
        get => this._initialFilterCount;
        set => _ = this.SetProperty( ref this._initialFilterCount, value );
    }

    private double _initialFilterWindow = 0.001;

    /// <summary> Gets or sets the initial filter window. </summary>
    /// <value> The initial filter window. </value>
    public virtual double InitialFilterWindow
    {
        get => this._initialFilterWindow;
        set => _ = this.SetProperty( ref this._initialFilterWindow, value );
    }

    private bool _initialRemoteSenseSelected = true;

    /// <summary> Gets or sets the initial remote sense selected. </summary>
    /// <value> The initial remote sense selected. </value>
    public virtual bool InitialRemoteSenseSelected
    {
        get => this._initialRemoteSenseSelected;
        set => _ = this.SetProperty( ref this._initialRemoteSenseSelected, value );
    }

    #endregion

    #region " measure Settings "

    private bool _autoZeroEnabled = true;

    /// <summary>   Gets or sets a value indicating whether the automatic zero is enabled. </summary>
    /// <value> True if automatic zero enabled, false if not. </value>
    public bool AutoZeroEnabled
    {
        get => this._autoZeroEnabled;
        set => this.SetProperty( ref this._autoZeroEnabled, value );
    }

    private bool _autoRangeEnabled = true;

    /// <summary>
    /// Gets or sets a value indicating whether the automatic range is enabled.
    /// </summary>
    /// <value> True if automatic range enabled, false if not. </value>
    public bool AutoRangeEnabled
    {
        get => this._autoRangeEnabled;
        set => this.SetProperty( ref this._autoRangeEnabled, value );
    }

    private SenseFunctionModes _senseFunction = SenseFunctionModes.ResistanceFourWire;

    /// <summary>   Gets or sets the sense function. </summary>
    /// <value> The sense function. </value>
    public SenseFunctionModes SenseFunction
    {
        get => this._senseFunction;
        set => this.SetProperty( ref this._senseFunction, value );
    }

    private double _powerLineCycles = 1;

    /// <summary>   Gets or sets the power line cycles. </summary>
    /// <value> The power line cycles. </value>
    public double PowerLineCycles
    {
        get => this._powerLineCycles;
        set => this.SetProperty( ref this._powerLineCycles, value );
    }

    private bool _remoteSenseSelected = true;

    /// <summary>   Gets or sets a value indicating whether the remote sense selected. </summary>
    /// <value> True if remote sense selected, false if not. </value>
    public bool RemoteSenseSelected
    {
        get => this._remoteSenseSelected;
        set => this.SetProperty( ref this._remoteSenseSelected, value );
    }

    #endregion
}

