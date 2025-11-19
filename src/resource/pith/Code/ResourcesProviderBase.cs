// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;

namespace cc.isr.VI.Pith;

/// <summary> The resources provider base class. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-21 </para>
/// </remarks>
public abstract class ResourcesProviderBase : IDisposable
{
    #region " resource finder "

    /// <summary> Gets or sets the resource finder. </summary>
    /// <value> The resource finder. </value>
    public ResourceFinderBase? ResourceFinder { get; set; }

    #endregion

    #region " parse resources "

    /// <summary> Gets or sets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public abstract bool IsDummy { get; }

    /// <summary> Parse resource. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName"> The resource name. </param>
    /// <returns> A <see cref="ResourceNameInfo"/>. </returns>
    public abstract ResourceNameInfo ParseResource( string resourceName );

    #endregion

    #region " find resources "

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <returns> List of all resources. </returns>
    public abstract IEnumerable<string> FindResources();

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public abstract IEnumerable<string> FindResources( string filter );

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details) TryFindResources();

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindResources( string filter );

    /// <summary> Returns true if the specified resource exists in the resource names cache. </summary>
    /// <param name="resourceName"> The resource name. </param>
    /// <returns> <c>true</c> if the resource was located; Otherwise, <c>false</c>. </returns>
    public abstract bool Exists( string resourceName );

    #endregion

    #region " interfaces "

    /// <summary> Searches for the interface in the resource names cache. </summary>
    /// <param name="resourceName"> The interface resource name. </param>
    /// <returns> <c>true</c> if the interface was located; Otherwise, <c>false</c>. </returns>
    public abstract bool InterfaceExists( string resourceName );

    /// <summary> Searches for all interfaces in the resource names cache. </summary>
    /// <returns> The found interface resource names. </returns>
    public abstract IEnumerable<string> FindInterfaces();

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces();

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public abstract IEnumerable<string> FindInterfaces( HardwareInterfaceType interfaceType );

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces( HardwareInterfaceType interfaceType );

    #endregion

    #region " instruments  "

    /// <summary> Searches for the instrument in the resource names cache. </summary>
    /// <param name="resourceName"> The instrument resource name. </param>
    /// <returns> <c>true</c> if the instrument was located; Otherwise, <c>false</c>. </returns>
    public abstract bool FindInstrument( string resourceName );

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments();

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments();

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType );

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType );

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType, int boardNumber );

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType">   Type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType, int interfaceNumber );

    /// <summary> Searches for the first resource in the resource names cache. </summary>
    /// <param name="resourceNames"> List of names of the resources. </param>
    /// <returns> The found resource. </returns>
    public string FindResource( IEnumerable<string> resourceNames )
    {
        string result = string.Empty;
        if ( resourceNames is object && resourceNames.Any() )
        {
            foreach ( string rn in resourceNames )
            {
                if ( this.FindInstrument( rn ) )
                {
                    result = rn;
                    break;
                }
            }
        }

        return result;
    }

    #endregion

    #region " revision validation "

    /// <summary>
    /// Validates the implementation and specification visa versions against settings values.
    /// </summary>
    /// <remarks> David, 2020-04-11. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public abstract (bool Success, string Details) ValidateFunctionalVisaVersions();

    #endregion

    #region " disposable support "

    /// <summary> Gets or sets the disposed sentinel. </summary>
    /// <value> The disposed. </value>
    protected bool IsDisposed { get; set; }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
            }
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// David, 2015-11-21: Override because Dispose(disposing As Boolean) above has code to free
    /// unmanaged resources.
    /// </remarks>
    ~ResourcesProviderBase()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( true );
        // uncommented because Finalize() is overridden above.
        GC.SuppressFinalize( this );
    }

    #endregion
}
/// <summary> Values that represent resource open states. </summary>
/// <remarks> David, 2020-04-10. </remarks>
public enum ResourceOpenState
{
    /// <summary> An enum constant representing the unknown option.
    /// Used as an initial value but will never be returned for an open operation. </summary>
    [Description( "Unknown" )]
    Unknown,

    /// <summary> An enum constant representing the success.
    /// The session opened successfully. </summary>
    [Description( "Success" )]
    Success,

    /// <summary> An enum constant representing the device not responding.
    /// The session opened successfully but the device at the other end did not respond. </summary>
    [Description( "Device Not Responding" )]
    DeviceNotResponding,

    /// <summary> An enum constant representing the configuration not loaded.
    /// The session opened successfully but the specified configuration either does not exists
    /// or could not be loaded. The session will use VISA-specified defaults. </summary>
    [Description( "Configuration Not Loaded" )]
    ConfigurationNotLoaded
}
