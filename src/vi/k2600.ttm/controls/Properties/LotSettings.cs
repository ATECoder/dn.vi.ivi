using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class LotSettings() : System.ComponentModel.INotifyPropertyChanged
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
#if NET5_0_OR_GREATER
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

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " ttm properties "

    /// <summary>   Gets or sets a value indicating whether the automatically add part. </summary>
    /// <value> True if automatically add part, false if not. </value>
    public bool AutomaticallyAddPart
    {
        get;
        set => this.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets a value indicating whether the device is enabled. </summary>
    /// <value> True if device enabled, false if not. </value>
    public bool DeviceEnabled
    {
        get;
        set => this.SetProperty( ref field, value );
    } = true;

    /// <summary>   Gets or sets the FTP address. </summary>
    /// <value> The FTP address. </value>
    public string FtpAddress
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "ftp://ttm%40isr.cc:ttm2.2@ftp.isr.cc";

    /// <summary>   Gets or sets the lot number. </summary>
    /// <value> The lot number. </value>
    public string LotNumber
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "Lot 1";

    /// <summary>   Gets or sets the part number. </summary>
    /// <value> The part number. </value>
    public string PartNumber
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "Part 1";

    /// <summary>   Gets or sets the pathname of the data folder. </summary>
    /// <value> The pathname of the data folder. </value>
    public string DataFolder
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "c:\\users\\public\\documents\\ttm";

    /// <summary>   Gets or sets the pathname of the measurements folder. </summary>
    /// <value> The pathname of the measurements folder. </value>
    public string MeasurementsFolderName
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "measurements";

    /// <summary>   Gets the pathname of the measurements folder. </summary>
    /// <value> The pathname of the measurements folder. </value>
    public string MeasurementsFolder => System.IO.Path.Combine( this.DataFolder, this.MeasurementsFolderName );

    /// <summary>   Gets or sets the full path name of the parts file. </summary>
    /// <value> The full path name of the parts file. </value>
    public string PartsFileName
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "Part1.csv";

    /// <summary>   Gets the full path name of the parts file. </summary>
    /// <value> The full path name of the parts file. </value>
    [JsonIgnore]
    public string PartsFilePath => System.IO.Path.Combine( this.MeasurementsFolder, this.PartsFileName );

    /// <summary>   Gets or sets the identifier of the operator. </summary>
    /// <value> The identifier of the operator. </value>
    public string OperatorId
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "Operator 1";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string ResourceName
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "TCPIP0::192.168.0.150::inst0::INSTR";

    /// <summary>   Gets or sets the setting. </summary>
    /// <value> The setting. </value>
    public string Setting
    {
        get;
        set => this.SetProperty( ref field, value );
    } = string.Empty;

    /// <summary>   Gets or sets the full path name of the trace file. </summary>
    /// <value> The full path name of the trace file. </value>
    public string TraceFileName
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "Trace1.csv";

    /// <summary>   Gets the full path name of the trace file. </summary>
    /// <value> The full path name of the trace file. </value>
    [JsonIgnore]
    public string TraceFilePath => System.IO.Path.Combine( this.MeasurementsFolder, this.TraceFileName );

    /// <summary>   Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
    public double LineFrequency
    {
        get;
        set => this.SetProperty( ref field, value );
    } = 60;

    #endregion
}
