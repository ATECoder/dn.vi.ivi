namespace cc.isr.VI;

/// <summary> Dictionary of bitmasks. </summary>
/// <remarks>
/// David, 2019-12-21 <para>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.</para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
public class BitmasksDictionary : Dictionary<int, int>
{
    /// <summary> Default constructor. </summary>
    public BitmasksDictionary() : base()
    {
    }

    /// <summary> Adds a bitmask. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="key">     The bit mask key. </param>
    /// <param name="bitmask"> The bitmask. </param>
    public new void Add( int key, int bitmask )
    {
        if ( this.ContainsValue( bitmask ) )
            throw new ArgumentException( $"Bitmask {bitmask} already exists; bitmasks must be unique", nameof( bitmask ) );
        base.Add( key, bitmask );
    }

    /// <summary> Adds a bitmask. </summary>
    /// <param name="key">            The bit mask key. </param>
    /// <param name="bitmask">        The bitmask. </param>
    /// <param name="excludeFromAll"> True to exclude, false to include from all. </param>
    public void Add( int key, int bitmask, bool excludeFromAll )
    {
        this.Add( key, bitmask );
        if ( !excludeFromAll )
            this.All |= bitmask;
    }

    /// <summary>
    /// Gets or sets the value include all bitmask key values except those excluded .
    /// </summary>
    /// <value> all. </value>
    public int All { get; private set; }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( int status, int key )
    {
        return this.ContainsKey( key ) ? 0 != (status & this[key]) : throw new ArgumentException( $"Bitmask not found for {key}", nameof( key ) );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( int status, int key )
    {
        return this.ContainsKey( key ) ? key == (status & key) : throw new ArgumentException( $"Bitmask not found for {key}", nameof( key ) );
    }

    /// <summary> Return the masked status value. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( int status, int key )
    {
        return this.ContainsKey( key ) ? status & key : throw new ArgumentException( $"Bitmask not found for {key}", nameof( key ) );
    }

    /// <summary> Gets or sets the status. </summary>
    /// <value> The status. </value>
    public int Status { get; set; }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( int key )
    {
        return this.IsAnyBitOn( this.Status, key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( int key )
    {
        return this.AreAllBitsOn( this.Status, key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( int key )
    {
        return this.MaskedValue( this.Status, key );
    }

    #region " serialization "

    /// <summary> Initializes a new instance of the class with serialized data. </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo" />
    /// that holds the serialized object data about the exception being
    /// thrown. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext" />
    /// that contains contextual information about the source or destination.
    /// </param>
#if NET8_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    protected BitmasksDictionary( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
        if ( info is null )
            return;
        this.Status = info.GetInt32( nameof( this.Status ) );
        this.All = info.GetInt32( nameof( this.All ) );
    }

    /// <summary>
    /// Overrides the <see cref="GetObjectData" /> method to serialize custom values.
    /// </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo">serialization
    /// information</see>. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext">streaming
    /// context</see> for the exception. </param>
    [System.Security.Permissions.SecurityPermission( System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
#if NET8_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    public override void GetObjectData( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
    {
        if ( info is null )
            return;
        info.AddValue( nameof( this.All ), this.All, typeof( int ) );
        info.AddValue( nameof( this.Status ), this.Status, typeof( int ) );
        base.GetObjectData( info, context );
    }

    #endregion
}
