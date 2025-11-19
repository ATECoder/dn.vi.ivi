namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Part Cold resistance. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public partial class ColdResistance : ColdResistanceBase, ICloneable
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public ColdResistance() : base()
    {
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public ColdResistance( ColdResistance value ) : base( value )
    {
        if ( value is not null )
        {
        }
    }

    /// <summary> Creates a new object that is a copy of the current instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A new object that is a copy of this instance. </returns>
    public object Clone()
    {
        return new ColdResistance( this );
    }

    #endregion
}
