using cc.isr.Std.TrimExtensions;

namespace cc.isr.VI.Tsp.Script;
/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="FirmwareScriptBase">builder entity</see>
/// items keyed by the <see cref="FirmwareScriptBase.Name">name.</see>
/// </summary>
/// <remarks>
/// David, 2009-03-02, 3.0.3348.x <para>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.</para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
/// <typeparam name="TItem">    Type of the item. </typeparam>
[CLSCompliant( false )]
public class FirmwareScriptBaseCollection<TItem> : System.Collections.ObjectModel.KeyedCollection<string, TItem> where TItem : FirmwareScriptBase
{
    #region " select script "

    /// <summary>   Gets key for item. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="item"> The item. </param>
    /// <returns>   The key for item. </returns>
    protected override string GetKeyForItem( TItem item )
    {
        return item.Name + item.ModelMask;
    }

    /// <summary>
    /// Returns reference to the boot builder for the specified node or nothing if a boot builder does
    /// not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>
    /// Reference to the boot builder for the specified node or nothing if a boot builder does not
    /// exist.
    /// </returns>
    [CLSCompliant( false )]
    public TItem? SelectBootScript( NodeEntityBase? node )
    {
        if ( node is not null )
        {
            foreach ( TItem script in this.Items )
            {
                if ( script.IsModelMatch( node.ModelNumber ) && script.IsBootScript )
                    return script;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns reference to the Serial Number builder for the specified node or nothing if a serial
    /// number builder does not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>
    /// Reference to the Serial Number builder for the specified node or nothing if a serial number
    /// builder does not exist.
    /// </returns>
    [CLSCompliant( false )]
    public TItem? SelectSerialNumberScript( NodeEntityBase? node )
    {
        if ( node is null ) return null;

        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( node.ModelNumber ) && script.IsPrimaryScript )
                return script;
        }

        return null;
    }

    /// <summary>
    /// Returns reference to the support builder for the specified node or nothing if a support builder
    /// does not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> The node. </param>
    /// <returns>
    /// Reference to the support builder for the specified node or nothing if a support builder does
    /// not exist.
    /// </returns>
    [CLSCompliant( false )]
    public TItem? SelectSupportScript( NodeEntityBase? node )
    {
        if ( node is null ) return null;

        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( node.ModelNumber ) && script.IsSupportScript )
                return script;
        }

        return null;
    }

    #endregion

    #region " firmware state "

    /// <summary>   Gets any builder identified using the test methods. </summary>
    /// <value> The identified builder. </value>
    public TItem? IdentifiedScript { get; private set; }

    /// <summary>   Returns <c>true</c> if any builder requires update from file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   <c>true</c> if any builder requires update from file. </returns>
    public bool RequiresReadParseWrite()
    {
        bool requires = false;
        foreach ( TItem script in this.Items )
        {
            requires |= script.RequiresReadParseWrite;
            if ( requires )
                break;
        }

        return requires;
    }

    #endregion

    #region " auto run "

    /// <summary>   Builds the commands to auto run these scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="indent">         The indent. </param>
    /// <param name="postRunDelay">   (optional) (0.5) The time in seconds to hold the display after the run.
    ///                                Set to zero to disable display. </param>
    /// <returns>   The auto run commands. </returns>
    public string BuildAutoRunScript( string indent, double postRunDelay = 0.5 )
    {
        System.Text.StringBuilder builder = new( 1024 );
        System.Collections.Specialized.ListDictionary uniqueScripts = [];
        // add code to run all scripts other than the boot builder
        foreach ( FirmwareScriptBase scriptEntity in this )
        {
            if ( !string.IsNullOrWhiteSpace( scriptEntity.Name ) )
            {
                if ( !scriptEntity.IsBootScript )
                {
                    if ( !uniqueScripts.Contains( scriptEntity.Name ) )
                    {
                        uniqueScripts.Add( scriptEntity.Name, scriptEntity );
                        if ( postRunDelay > 0 )
                        {
                            _ = builder.AppendLine( $"{indent}_G.display.clear()" );
                            _ = builder.AppendLine( $"{indent}_G.display.setcursor(1, 1)" );
                            _ = builder.AppendLine( $"{indent}_G.display.settext('{scriptEntity.Name}')" );
                            _ = builder.AppendLine( $"{indent}_G.display.setcursor(2, 1)" );
                            _ = builder.AppendLine( $"{indent}_G.display.settext('running {scriptEntity.Name} script...')" );
                        }
                        _ = builder.Append( $"{indent}{scriptEntity.Name}.run()" );
                        if ( postRunDelay > 0 )
                            _ = builder.AppendLine( $"{indent}_G.delay({postRunDelay:0.###})" );
                    }
                }
            }
        }
        return builder.ToString().TrimEndNewLine();
    }


    #endregion
}

