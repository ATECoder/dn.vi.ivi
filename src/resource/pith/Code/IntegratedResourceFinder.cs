// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

/// <summary> An Integrated Scientific Resources-based Resource finder. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
public class IntegratedResourceFinder : ResourceFinderBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="object" /> class. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public IntegratedResourceFinder() : base()
    {
    }

    #endregion

    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <see cref="VI.Pith.ResourceNameInfo"/>. </returns>
    public override ResourceNameInfo ParseResource( string resourceName )
    {
        return new ResourceNameInfo( resourceName );
    }

    /// <summary> Gets or sets the using like pattern. </summary>
    /// <value> The using like pattern. </value>
    public override bool UsingLikePattern { get; } = true;

    #endregion

    #region " find resources "

    /// <summary>   Enumerates the items in this collection that meet given criteria. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="filter">   A pattern specifying the search. </param>
    /// <returns>   An enumerator that allows foreach to be used to process the matched items. </returns>
    private IEnumerable<string> Find( string filter )
    {
        filter = ResourceNamesManager.TranslatePattern( filter, this.UsingLikePattern );
        return FetchResourceCache().Find( filter );
    }

    /// <summary> Fetches resource cache. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns> The resource cache. </returns>
    private static ResourceNameInfoCollection FetchResourceCache()
    {
        ResourceNameInfoCollection resourceNameInfoCollection = [];
        resourceNameInfoCollection.ReadResources();
        return resourceNameInfoCollection;
    }

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources()
    {
        return FetchResourceCache().Find( ResourceNamesManager.AllResourcesFilter );
    }

    /// <summary> Lists all resources in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for all interfaces in the resource names cache. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces()
    {
        string filter = string.Empty;
        try
        {
            filter = ResourceNamesManager.BuildInterfaceFilter();
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for the interfaces in the resource names cache. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) FindInterfaces( HardwareInterfaceType interfaceType )
    {
        string filter = string.Empty;
        try
        {
            filter = ResourceNamesManager.BuildInterfaceFilter( interfaceType );
            IEnumerable<string> resources = this.Find( filter );
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments()
    {
        string filter = string.Empty;
        try
        {
            filter = ResourceNamesManager.BuildInstrumentFilter();
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType )
    {
        string filter = string.Empty;
        try
        {
            filter = ResourceNamesManager.BuildInstrumentFilter( interfaceType );
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    /// <summary> Searches for instruments in the resource names cache. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType, int boardNumber )
    {
        string filter = string.Empty;
        try
        {
            filter = ResourceNamesManager.BuildInstrumentFilter( interfaceType, boardNumber );
            return this.Find( filter );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Failed finding resources using filter='{filter}'", ex );
        }
    }

    #endregion
}
