// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.Pith;

/// <summary> A dummy native error. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-24 </para>
/// </remarks>
public class DummyNativeError : NativeErrorBase
{
    /// <summary> Constructor. </summary>
    /// <param name="errorCode"> The error code. </param>
    public DummyNativeError( int errorCode ) : base( errorCode ) => this.InitErrorCode( errorCode );

    /// <summary> Initializes the error code. </summary>
    /// <param name="errorCode"> The error code. </param>
    private void InitErrorCode( int errorCode )
    {
        try
        {
            this.ErrorCodeName = errorCode.ToString();
        }
        catch ( Exception ex )
        {
            this.ErrorCodeName = "UnknownError";
            Debug.Assert( !Debugger.IsAttached, $"Check the code {ex}" );
        }

        this.ErrorCodeDescription = this.ErrorCodeName.SplitWords();
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="errorCode">       The error code. </param>
    /// <param name="resourceName">    Name of the resource. </param>
    /// <param name="lastMessageSent"> The last message sent. </param>
    /// <param name="lastAction">      The last action. </param>
    public DummyNativeError( int errorCode, string? resourceName, string? lastMessageSent, string? lastAction )
        : base( errorCode, resourceName, lastMessageSent, lastAction ) => this.InitErrorCode( errorCode );

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="errorCode">       The error code. </param>
    /// <param name="resourceName">    Name of the resource. </param>
    /// <param name="nodeNumber">      The node number. </param>
    /// <param name="lastMessageSent"> The last message sent. </param>
    /// <param name="lastAction">      The last action. </param>
    public DummyNativeError( int errorCode, string? resourceName, int nodeNumber, string? lastMessageSent, string? lastAction )
        : base( errorCode, resourceName, nodeNumber, lastMessageSent, lastAction ) => this.InitErrorCode( errorCode );

}
