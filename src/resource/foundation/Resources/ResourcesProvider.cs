using System.Diagnostics;

namespace cc.isr.VI.Foundation;

/// <summary> The VISA resources provider. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
[CLSCompliant( false )]
public class ResourcesProvider : Pith.ResourcesProviderBase
{
    #region " constructor "

    /// <summary> Default constructor. </summary>
    public ResourcesProvider() : base()
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
            Debug.Assert( !Debugger.IsAttached, "Exception occurred disposing resource manager", $"Exception {ex}" );
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
    public override bool IsDummy { get; } = false;

    #endregion

    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <see cref="VI.Pith.ResourceNameInfo"/>. </returns>
    public override Pith.ResourceNameInfo ParseResource( string resourceName )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        string activity = $"IVI.Visa {Ivi.Visa.GlobalResourceManager.ImplementationVersion}/{Ivi.Visa.GlobalResourceManager.SpecificationVersion} parsing resource {resourceName}";
        try
        {
            return this.ResourceFinder.ParseResource( resourceName );
        }
        catch ( MissingMethodException ex )
        {
            if ( Ivi.Visa.GlobalResourceManager.ImplementationVersion < new Version( cc.isr.Visa.Gac.GacLoader.IviVisaAssemblyInfo.ImplementationVersion ) )
            {
                throw new InvalidOperationException( $"Failed {activity}; Expecting {cc.isr.Visa.Gac.GacLoader.IviVisaAssemblyInfo.ImplementationVersion} implementation and {cc.isr.Visa.Gac.GacLoader.IviVisaAssemblyInfo.SpecificationVersion} specification versions.", ex );
            }
            else
            {
                throw new InvalidOperationException( $"Failed {activity}", ex );
            }
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region " find resources "

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindResources();
    }

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details) TryFindResources()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindResources();
    }

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources( string filter )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindResources( filter );
    }

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindResources( string filter )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindResources( filter );
    }

    /// <summary> Returns true if the specified resource exists in the resource names cache. </summary>
    /// <param name="resourceName"> The resource name. </param>
    /// <returns> <c>true</c> if the resource was located; Otherwise, <c>false</c>. </returns>
    public override bool Exists( string resourceName )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return (this.ResourceFinder.Exists( resourceName ) && !Pith.ResourceNamesManager.IsTcpIpResource( resourceName ))
            || Pith.ResourceNamesManager.PingTcpIpResource( resourceName );
    }

    #endregion

    #region " interfaces "

    /// <summary> Searches for the interface in the resource names cache. </summary>
    /// <param name="resourceName"> The interface resource name. </param>
    /// <returns> <c>true</c> if the interface was located; Otherwise, <c>false</c>. </returns>
    public override bool InterfaceExists( string resourceName )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.InterfaceExists( resourceName );
    }

    /// <summary> Searches for all interfaces in the resource names cache. </summary>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInterfaces( Pith.ResourceNamesManager.BuildInterfaceFilter() );
    }

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindInterfaces();
    }

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces( Pith.HardwareInterfaceType interfaceType )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInterfaces( Pith.ResourceNamesManager.BuildInterfaceFilter( interfaceType ) );
    }

    /// <summary> Try find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces( Pith.HardwareInterfaceType interfaceType )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindInterfaces( interfaceType );
    }

    #endregion

    #region " instruments  "

    /// <summary> Searches for the instrument in the resource names cache. </summary>
    /// <param name="resourceName"> The instrument resource name. </param>
    /// <returns> <c>true</c> if the instrument was located; Otherwise, <c>false</c>. </returns>
    public override bool FindInstrument( string resourceName )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInstrument( resourceName );
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInstruments( Pith.ResourceNamesManager.BuildInstrumentFilter() );
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments()
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindInstruments();
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( Pith.HardwareInterfaceType interfaceType )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInstruments( Pith.ResourceNamesManager.BuildInstrumentFilter( interfaceType ) );
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( Pith.HardwareInterfaceType interfaceType )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindInstruments( interfaceType );
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( Pith.HardwareInterfaceType interfaceType, int boardNumber )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.FindInstruments( Pith.ResourceNamesManager.BuildInstrumentFilter( interfaceType, boardNumber ) );
    }

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType">   Type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( Pith.HardwareInterfaceType interfaceType, int interfaceNumber )
    {
        if ( this.ResourceFinder is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ResourceFinder )} is null." );
        return this.ResourceFinder.TryFindInstruments( interfaceType, interfaceNumber );
    }

    #endregion

    #region " version validation "

    /// <summary>
    /// Validates the implementation specification visa versions against settings values.
    /// </summary>
    /// <remarks> David, 2020-04-12. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public override (bool Success, string Details) ValidateFunctionalVisaVersions()
    {
        return VisaVersionValidator.ValidateFunctionalVisaVersions();
    }

    #endregion
}
