namespace cc.isr.VI.Tsp.Script;

/// <summary> Encapsulate the script information. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-07. From TSP Library.  </para><para>
/// David, 2009-03-02, 3.0.3348. </para>
/// </remarks>
/// <remarks>   Constructor. </remarks>
/// <remarks>   2024-09-06. </remarks>
/// <param name="firmwareScript">   The firmware script. </param>
/// <param name="node">             The node. </param>
[CLSCompliant( false )]
public class ScriptEntity( FirmwareScriptBase firmwareScript, NodeEntityBase node ) : ScriptEntityBase( firmwareScript, node )
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="script">   The script. </param>
    public ScriptEntity( ScriptEntityBase script ) : this( script.FirmwareScript, script.Node )
    {
        this.Activated = script.Activated;
        this.EmbeddedFirmwareVersion = script.EmbeddedFirmwareVersion;
        this.FirmwareVersionGetter = script.FirmwareVersionGetter;
        this.HasFirmwareVersionGetter = script.HasFirmwareVersionGetter;
        this.IsDeleted = script.IsDeleted;
        this.Loaded = script.Loaded;
        this.Node = script.Node;
        this.RequiresDeletion = script.RequiresDeletion;
        this.Saved = script.Saved;
        this.VersionStatus = script.VersionStatus;
    }
}
