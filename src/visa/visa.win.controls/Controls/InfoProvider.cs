using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary> Information provider. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-04-04 </para>
/// </remarks>
[DesignerCategory( "code" )]
[Description( "Information Provider" )]
public class InfoProvider : ErrorProvider
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    public InfoProvider() : base()
    {
    }

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="container"> The container. </param>
    public InfoProvider( IContainer container ) : base( container )
    {
    }

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="parentControl"> The parent control. </param>
    public InfoProvider( ContainerControl parentControl ) : base( parentControl )
    {
    }

    #endregion

    #region " clear "

    /// <summary> Clears this object to its blank/initial state. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The event sender. </param>
    public void Clear( object? sender )
    {
        if ( sender is Control control )
        {
            this.Clear( control );
        }
        else
        {
            if ( sender is ToolStripItem toolStripItem )
            {
                this.Clear( toolStripItem );
            }
        }
    }

    /// <summary> Clears this object to its blank/initial state. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The event sender. </param>
    public void Clear( Control sender )
    {
        if ( sender is not null )
        {
            if ( sender.Container is ToolStripItem or System.Windows.Forms.ToolStripMenuItem )
            {
                this.Clear( sender.Container );
            }
            else
            {
                this.SetError( sender, "" );
            }
        }
    }

    /// <summary> Clears this object to its blank/initial state. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The event sender. </param>
    public void Clear( ToolStripItem sender )
    {
        if ( sender is not null && sender.Owner is not null )
        {
            this.SetError( sender.Owner, "" );
        }
    }

    #endregion

    #region " annunciate - object "

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">  The event sender. </param>
    /// <param name="level">   The level. </param>
    /// <param name="details"> The details. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( object? sender, InfoProviderLevel level, string details )
    {
        if ( sender is Control control )
        {
            _ = this.Annunciate( control, level, details );
        }
        else
        {
            if ( sender is System.Windows.Forms.ToolStripMenuItem toolStripMenuItem )
            {
                _ = this.Annunciate( toolStripMenuItem, level, details );
            }
            else
            {
                if ( sender is ToolStripItem toolStripItem )
                {
                    _ = this.Annunciate( toolStripItem, level, details );
                }
            }
        }

        return details;
    }

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The event sender. </param>
    /// <param name="level">  The level. </param>
    /// <param name="format"> Describes the format to use. </param>
    /// <param name="args">   A variable-length parameters list containing arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( object? sender, InfoProviderLevel level, string format, params object[] args )
    {
        return this.Annunciate( sender, level, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    #endregion

    #region " pad / align - control "

    /// <summary> Aligns the icon. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">        The sender object; must be a control. </param>
    /// <param name="iconAlignment"> The icon alignment. </param>
    protected void AlignIcon( object? sender, ErrorIconAlignment iconAlignment )
    {
        if ( sender is not null )
        {
            this.SetIconAlignment( ( Control ) sender, iconAlignment );
        }
    }

    /// <summary> Set icon padding. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">  The sender object; must be a control. </param>
    /// <param name="padding"> The padding. </param>
    protected void PadIcon( object? sender, int padding )
    {
        if ( sender is not null )
        {
            this.SetIconPadding( ( Control ) sender, padding );
        }
    }

    #endregion

    #region " annunciate - control "

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">  The event sender. </param>
    /// <param name="level">   The level. </param>
    /// <param name="details"> The details. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( Control sender, InfoProviderLevel level, string details )
    {
        if ( sender is not null )
        {
            if ( sender.Container is ToolStripItem or System.Windows.Forms.ToolStripMenuItem )
            {
                _ = this.Annunciate( sender.Container, level, details );
            }
            else
            {
                this.SelectIcon( level );
                this.SetError( sender, details );
            }
        }

        return details;
    }

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The event sender. </param>
    /// <param name="level">  The level. </param>
    /// <param name="format"> Describes the format to use. </param>
    /// <param name="args">   A variable-length parameters list containing arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( Control sender, InfoProviderLevel level, string format, params object[] args )
    {
        return this.Annunciate( sender, level, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    #endregion

    #region " annunciate -- tool strip "

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">  The sender. </param>
    /// <param name="level">   The level. </param>
    /// <param name="details"> The details. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( ToolStripItem sender, InfoProviderLevel level, string details )
    {
        if ( sender is not null && sender.Owner is not null )
        {
            this.SelectIcon( level );
            this.SetIconAlignment( sender.Owner, ErrorIconAlignment.BottomLeft );
            this.SetIconPadding( sender.Owner, -(10 + sender.Bounds.X) );
            this.SetError( sender.Owner, details );
        }

        return details;
    }

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The sender. </param>
    /// <param name="level">  The level. </param>
    /// <param name="format"> Describes the format to use. </param>
    /// <param name="args">   A variable-length parameters list containing arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( ToolStripItem sender, InfoProviderLevel level, string format, params object[] args )
    {
        return this.Annunciate( sender, level, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender">  The sender. </param>
    /// <param name="level">   The level. </param>
    /// <param name="details"> The details. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( System.Windows.Forms.ToolStripMenuItem sender, InfoProviderLevel level, string details )
    {
        if ( sender is not null && sender.Owner is not null )
        {
            ToolStripItem? item = sender;
            ToolStripItem? ownerItem = sender.OwnerItem;
            while ( ownerItem is not null )
            {
                item = item?.OwnerItem;
                ownerItem = item?.OwnerItem;
            }

            if ( item is not null )
                _ = this.Annunciate( item, level, details );
        }

        return details;
    }

    /// <summary> Annunciates error. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The sender. </param>
    /// <param name="level">  The level. </param>
    /// <param name="format"> Describes the format to use. </param>
    /// <param name="args">   A variable-length parameters list containing arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string Annunciate( System.Windows.Forms.ToolStripMenuItem sender, InfoProviderLevel level, string format, params object[] args )
    {
        return this.Annunciate( sender, level, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    /// <summary> Select icon. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="level"> The level. </param>
    private void SelectIcon( InfoProviderLevel level )
    {
        switch ( level )
        {
            case InfoProviderLevel.Alert:
                {
                    this.Icon = Properties.Resources.exclamation;
                    break;
                }

            case InfoProviderLevel.Error:
                {
                    this.Icon = Properties.Resources.dialog_error_2;
                    break;
                }

            case InfoProviderLevel.Info:
                {
                    this.Icon = Properties.Resources.dialog_information_3;
                    break;
                }

            default:
                {
                    this.Icon = Properties.Resources.dialog_information_3;
                    break;
                }
        }
    }

    #endregion
}
/// <summary> Values that represent information provider levels. </summary>
/// <remarks> David, 2020-09-24. </remarks>
public enum InfoProviderLevel
{
    /// <summary> An enum constant representing the Information option. </summary>
    [Description( "Information" )]
    Info,

    /// <summary> An enum constant representing the alert option. </summary>
    [Description( "Alert" )]
    Alert,

    /// <summary> An enum constant representing the error] option. </summary>
    [Description( "Error" )]
    Error
}
