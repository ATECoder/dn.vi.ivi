namespace cc.isr.VI.Pith;

/// <summary> A session factory base. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-29 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SessionFactoryBase
{
    /// <summary> Initializes a new instance of the <see cref="SessionFactoryBase" /> class. </summary>
    protected SessionFactoryBase()
    {
    }

    /// <summary> Creates GPIB interface SessionBase. </summary>
    /// <returns> The new GPIB interface SessionBase. </returns>
    public abstract InterfaceSessionBase GpibInterfaceSession();

    /// <summary>   Creates resources manager. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="usingGlobalResourceFinder">    (Optional) True to using global resource finder. </param>
    /// <returns>   The new resources manager. </returns>
    public abstract ResourcesProviderBase ResourcesProvider( bool usingGlobalResourceFinder = true );

    /// <summary> Creates the SessionBase. </summary>
    /// <returns> The new SessionBase. </returns>
    public abstract SessionBase Session();
}
