using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

/// <summary> A Dummy local visa resources manager. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
public class DummyResourcesProvider : ResourcesProviderBase
{
    #region " constructor "

    /// <summary> Default constructor. </summary>
    public DummyResourcesProvider() : base()
    {
    }

    #region "idisposable support"


    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
            }
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception occurred disposing resource manager", $"Exception {ex.BuildMessage()}" );
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #endregion

    #region " resource manager "

    /// <summary> Gets or sets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public override bool IsDummy => true;

    #endregion

    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A VI.ResourceNameInfo. </returns>
    public override ResourceNameInfo ParseResource( string resourceName )
    {
        return new ResourceNameInfo( resourceName );
    }

    #endregion

    #region " find resources "

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources()
    {
        return [];
    }

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details) TryFindResources()
    {
        return (true, string.Empty);
    }

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources( string filter )
    {
        return [];
    }

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindResources( string filter )
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    /// <summary> Returns true if the specified resource exists in the resource names cache. </summary>
    /// <param name="resourceName"> The resource name. </param>
    /// <returns> <c>true</c> if the resource was located; Otherwise, <c>false</c>. </returns>
    public override bool Exists( string resourceName )
    {
        return true;
    }

    #endregion

    #region " interfaces "

    /// <summary> Searches for the interface in the resource names cache. </summary>
    /// <param name="resourceName"> The interface resource name. </param>
    /// <returns> <c>true</c> if the interface was located; Otherwise, <c>false</c>. </returns>
    public override bool InterfaceExists( string resourceName )
    {
        return true;
    }

    /// <summary> Searches for all interfaces in the resource names cache. </summary>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces()
    {
        return [];
    }

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces()
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces( HardwareInterfaceType interfaceType )
    {
        return [];
    }

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces( HardwareInterfaceType interfaceType )
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    #endregion

    #region " instruments  "

    /// <summary> Searches for the instrument in the resource names cache. </summary>
    /// <param name="resourceName"> The instrument resource name. </param>
    /// <returns> <c>true</c> if the instrument was located; Otherwise, <c>false</c>. </returns>
    public override bool FindInstrument( string resourceName )
    {
        return true;
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments()
    {
        return [];
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments()
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType )
    {
        return [];
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType )
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType, int boardNumber )
    {
        return [];
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="interfaceType">   Type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType, int interfaceNumber )
    {
        return (true, string.Empty, Array.Empty<string>());
    }

    #endregion

    #region " validate visa version "

    /// <summary>
    /// Validates the specification and implementation visa versions against settings values.
    /// </summary>
    /// <remarks> David, 2020-04-11. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public override (bool Success, string Details) ValidateFunctionalVisaVersions()
    {
        return (true, string.Empty);
    }

    #endregion
}
