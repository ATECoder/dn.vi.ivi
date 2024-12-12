namespace cc.isr.VI.Pith;

/// <summary> A session factory for dummy session, interface and resource manager. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-29 </para>
/// </remarks>
[CLSCompliant( false )]
public class DummySessionFactory : SessionFactoryBase
{
    /// <summary> Creates gpib interface session. </summary>
    /// <returns> The new gpib interface session. </returns>
    public override InterfaceSessionBase GpibInterfaceSession()
    {
        return new DummyGpibInterfaceSession();
    }

    /// <summary>   Creates resources manager. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="usingGlobalResourceFinder">    (Optional) True to using global resource finder. </param>
    /// <returns>   The new resources manager. </returns>
    public override Pith.ResourcesProviderBase ResourcesProvider( bool usingGlobalResourceFinder = true )
    {
        return usingGlobalResourceFinder
            ? new DummyResourcesProvider() { ResourceFinder = new Pith.IntegratedResourceFinder() }
            : new DummyResourcesProvider() { ResourceFinder = new Pith.IntegratedResourceFinder() };
    }

    /// <summary> Creates a session. </summary>
    /// <returns> The new session. </returns>
    public override SessionBase Session()
    {
        return new DummySession();
    }
}
