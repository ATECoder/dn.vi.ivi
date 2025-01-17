using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Settings;

/// <summary>   The Route Subsystem Test Settings base class. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
[CLSCompliant( false )]
public class TriggerSubsystemSettings() : INotifyPropertyChanged
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

    private TriggerSources _initialTriggerSource = TriggerSources.Immediate;

    /// <summary> Gets or sets the initial trigger source. </summary>
    /// <value> The initial trigger source. </value>
	public virtual TriggerSources InitialTriggerSource
    {
        get => this._initialTriggerSource;
        set => _ = this.SetProperty( ref this._initialTriggerSource, value );
    }

    #endregion
}

