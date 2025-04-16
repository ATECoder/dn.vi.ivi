namespace cc.isr.VI.Tsp.Script;

/// <summary>   Enumerates the script file formats. </summary>
/// <remarks>   David, 2020-10-12. </remarks>
[Flags()]
public enum ScriptFileFormats
{
    /// <summary> An enum constant representing the uncompressed human readable option. </summary>
    [System.ComponentModel.Description( "Uncompressed Human Readable" )]
    None = 0,

    /// <summary> An enum constant representing the compressed option. </summary>
    [System.ComponentModel.Description( "Compressed" )]
    Compressed = 1,

    /// <summary> An enum constant representing the binary option. </summary>
    [System.ComponentModel.Description( "Binary format" )]
    Binary = 2,
}
