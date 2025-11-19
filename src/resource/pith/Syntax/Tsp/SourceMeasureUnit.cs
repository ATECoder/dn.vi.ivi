// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Source-Measure unit syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class SourceMeasureUnit
{
    #region " node "

    /// <summary> The node reference format. </summary>
    private const string NodeReferenceFormat = "_G.node[{0}]";

    /// <summary> Returns a TSP reference to the specified node. </summary>
    /// <param name="node">            Specifies the one-based node number. </param>
    /// <param name="localNodeNumber"> The local node number. </param>
    /// <returns> Then node reference. </returns>
    public static string NodeReference( int node, int localNodeNumber )
    {
        if ( node == localNodeNumber )
        {
            // if node is number one, use the local node as reference in case
            // we do not have other nodes.
            return Constants.LocalNode;
        }
        else
        {
            return Constants.Build( NodeReferenceFormat, node );
        }
    }

    #endregion

    #region " smu "

    /// <summary> The SMU number 'a'. </summary>
    public const string SourceMeasureUnitNumberA = "a";

    /// <summary> The SMU number 'b'. </summary>
    public const string SourceMeasureUnitNumberB = "b";

    /// <summary> The  SMU name format. </summary>
    private const string SmuNameFormat = "smu{0}";

    /// <summary>
    /// Builds the SMU name, e.g., smua or smub for the specified <paramref name="smuNumber">SMU
    /// number</paramref>.
    /// </summary>
    /// <param name="smuNumber"> Specifies the SMU Number (either 'a' or 'b'. </param>
    /// <returns> rThe node smu reference. </returns>
    public static string BuildSmuName( string smuNumber )
    {
        return Constants.Build( SmuNameFormat, smuNumber );
    }

    /// <summary> The  global node reference. </summary>
    private const string LocalNodeSmuFormat = "_G.localnode.smu{0}";

    /// <summary> The smu reference format. </summary>
    private const string SmuReferenceFormat = "_G.node[{0}].smu{1}";

    /// <summary>
    /// Builds a TSP reference to the specified <paramref name="smuNumber">SMU number</paramref> on
    /// the local node.
    /// </summary>
    /// <param name="smuNumber"> Specifies the SMU (either 'a' or 'b'. </param>
    /// <returns> rThe node smu reference. </returns>
    public static string BuildSmuReference( string smuNumber )
    {
        // use the local node as reference in case we do not have other nodes.
        return Constants.Build( LocalNodeSmuFormat, smuNumber );
    }

    /// <summary>
    /// Builds a TSP reference to the specified <paramref name="smuNumber">SMU number</paramref> on
    /// the specified node.
    /// </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="nodeNumber">      Specifies the node number (must be greater than 0). </param>
    /// <param name="localNodeNumber"> The local node number (must be greater than 0). </param>
    /// <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
    /// <returns> rThe node smu reference. </returns>
    public static string BuildSmuReference( int nodeNumber, int localNodeNumber, string smuNumber )
    {
        if ( nodeNumber <= 0 )
            throw new ArgumentException( "Node number must be greater than Or equal To 1." );
        else if ( localNodeNumber <= 0 )
            throw new ArgumentException( "Local node number must be greater than Or equal To 1." );

        if ( nodeNumber == localNodeNumber )
        {
            // if node is number one, use the local node as reference in case
            // we do not have other nodes.
            return BuildSmuReference( smuNumber );
        }
        else
            return Constants.Build( SmuReferenceFormat, nodeNumber, smuNumber );
    }
    #endregion
}
