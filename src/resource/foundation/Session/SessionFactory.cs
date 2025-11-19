// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Foundation;

/// <summary> A session factory. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-29 </para>
/// </remarks>
[CLSCompliant( false )]
public class SessionFactory : Pith.SessionFactoryBase
{
    /// <summary> Initializes a new instance of the <see cref="SessionFactory" /> class. </summary>
    public SessionFactory() : base() => cc.isr.Visa.Gac.GacLoader.LoadInstalledVisaAssemblies();

    /// <summary> Creates GPIB interface session. </summary>
    /// <returns> The new GPIB interface session. </returns>
    public override Pith.InterfaceSessionBase GpibInterfaceSession()
    {
        return new GpibInterfaceSession();
    }

    /// <summary>   Creates resources manager. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="usingGlobalResourceFinder">    (Optional) True to using global resource finder. </param>
    /// <returns>   The new resources manager. </returns>
    public override Pith.ResourcesProviderBase ResourcesProvider( bool usingGlobalResourceFinder = true )
    {
        return usingGlobalResourceFinder
            ? new ResourcesProvider() { ResourceFinder = new FoundationResourceFinder() }
            : new ResourcesProvider() { ResourceFinder = new Pith.IntegratedResourceFinder() };
    }

    /// <summary> Creates a session. </summary>
    /// <returns> The new session. </returns>
    public override Pith.SessionBase Session()
    {
        return new Session();
    }
}
