// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary>   Encapsulate the node information. </summary>
/// <remarks>   (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
///             Licensed under The MIT License. </para><para>
///             David, 2009-03-02, 3.0.3348.x. </para> </remarks>
/// <remarks>   Constructs the class. </remarks>
/// <param name="number">           Specifies the node number. </param>
/// <param name="controllerNode">   The controller node or null if this node is the controller node. </param>
[CLSCompliant( false )]
public class NodeEntity( int number, NodeEntityBase? controllerNode ) : NodeEntityBase( number, controllerNode )
{
}
