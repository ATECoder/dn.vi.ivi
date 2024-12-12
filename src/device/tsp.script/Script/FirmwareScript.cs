namespace cc.isr.VI.Tsp.Script;

/// <summary> Encapsulate the firmware script information. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-07. From TSP Library.  </para><para>
/// David, 2009-03-02, 3.0.3348. </para>
/// </remarks>
[CLSCompliant( false )]
public class FirmwareScript : FirmwareScriptBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    internal FirmwareScript() : base()
    {
    }

    /// <summary> Constructor. </summary>
    /// <param name="name">      Specifies the script name. </param>
    /// <param name="modelMask"> Specifies the model families for this script. </param>
    public FirmwareScript( string name, string modelMask ) : base( name, modelMask )
    {
    }

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="script">   The script. </param>
    public FirmwareScript( FirmwareScriptBase script ) : this( script.Name, script.ModelMask )
    {
        this.FolderPath = script.FolderPath;
        this.FileName = script.FileName;
        this.IsBootScript = script.IsBootScript;
        this.IsPrimaryScript = script.IsPrimaryScript;
        this.IsSupportScript = script.IsSupportScript;
        this.NamespaceList = script.NamespaceList;
        this.ReleasedFirmwareVersion = script.ReleasedFirmwareVersion;
        this.ResourceFileName = script.ResourceFileName;
        this.ResourceFileFormat = script.ResourceFileFormat;
        this.SavedToFile = script.SavedToFile;
        this.Source = script.Source;
    }

    #endregion
}
