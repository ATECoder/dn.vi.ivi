using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

/// <summary> A VISA Resources Manager Base class. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
public abstract class ResourceFinderBase
{
    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <see cref="VI.Pith.ResourceNameInfo"/>. </returns>
    public abstract ResourceNameInfo ParseResource( string resourceName );

    /// <summary> Gets or sets the using like pattern. </summary>
    /// <value> The using like pattern. </value>
    public abstract bool UsingLikePattern { get; }

    /// <summary> Builds minimal resources filter. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildMinimalResourcesFilter()
    {
        return ResourceNamesManager.BuildInstrumentFilter( HardwareInterfaceType.Gpib, HardwareInterfaceType.TcpIp, HardwareInterfaceType.Usb, this.UsingLikePattern );
    }

    #endregion

    #region " find resources "

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <returns> List of all resources. </returns>
    public abstract IEnumerable<string> FindResources();

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public (bool Success, string Details) TryFindResources()
    {
        try
        {
            return (this.FindResources().Any(), "Resources not found");
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage());
        }
    }

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public abstract IEnumerable<string> FindResources( string filter );

    /// <summary> Tries to find resources in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String, Resources As IEnumerable(Of String))
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindResources( string filter )
    {
        try
        {
            IEnumerable<string> resources = this.FindResources( filter );
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

    /// <summary> Returns true if the specified resource exists in the resource names cache. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resourceName"> The resource naResourceManagerBase. </param>
    /// <returns> <c>true</c> if the resource was located; Otherwise, <c>false</c>. </returns>
    public bool Exists( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        IEnumerable<string> resources = this.FindResources();
        return resources.Contains( resourceName, StringComparer.CurrentCultureIgnoreCase );
    }

    /// <summary> Searches for the interface in the resource names cache. </summary>
    /// <param name="resourceName"> The interface resource naResourceManagerBase. </param>
    /// <returns> <c>true</c> if the interface was located; Otherwise, <c>false</c>. </returns>
    public bool InterfaceExists( string resourceName )
    {
        ResourceNameInfo resourceNameInfo = this.ParseResource( resourceName );
        if ( resourceNameInfo.IsParsed )
        {
            (bool success, string _, IEnumerable<string> resources) = this.TryFindInterfaces( resourceNameInfo.InterfaceType );
            return success && resources.Contains( resourceName, StringComparer.CurrentCultureIgnoreCase );
        }

        return false;
    }

    /// <summary> Searches for all interfaces in the resource names cache. </summary>
    /// <returns> The found interface resource names. </returns>
    public abstract IEnumerable<string> FindInterfaces();

    /// <summary> Tries to find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns>
    /// <c>true</c> if interfaces were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces()
    {
        try
        {
            IEnumerable<string> resources = this.FindInterfaces();
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found interface resource names. </returns>
    public abstract IEnumerable<string> FindInterfaces( string filter );

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public abstract (bool Success, string Details, IEnumerable<string> Resources) FindInterfaces( HardwareInterfaceType interfaceType );

    /// <summary> Tries to find interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// <c>true</c> if interfaces were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindInterfaces( HardwareInterfaceType interfaceType )
    {
        try
        {
            return this.FindInterfaces( interfaceType );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

    /// <summary> Searches for the instrument in the resource names cache. </summary>
    /// <param name="resourceName"> The instrument resource naResourceManagerBase. </param>
    /// <returns> <c>true</c> if the instrument was located; Otherwise, <c>false</c>. </returns>
    public bool FindInstrument( string resourceName )
    {
        ResourceNameInfo resourceNameInfo = this.ParseResource( resourceName );
        if ( resourceNameInfo.IsParsed )
        {
            (bool success, string _, IEnumerable<string> resources) = this.TryFindInstruments( resourceNameInfo.InterfaceType, resourceNameInfo.InterfaceNumber );
            return success && resources.Contains( resourceName, StringComparer.CurrentCultureIgnoreCase );
        }

        return false;
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments( string filter );

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments();

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments()
    {
        try
        {
            IEnumerable<string> resources = this.FindInstruments();
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public abstract IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType );

    /// <summary> Tries to find instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType )
    {
        try
        {
            IEnumerable<string> resources = this.FindInstruments( interfaceType );
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

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
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of
    /// the.
    /// </returns>
    public (bool Success, string Details, IEnumerable<string> Resources) TryFindInstruments( HardwareInterfaceType interfaceType, int interfaceNumber )
    {
        try
        {
            IEnumerable<string> resources = this.FindInstruments( interfaceType, interfaceNumber );
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage(), Array.Empty<string>());
        }
    }

    #endregion
}
