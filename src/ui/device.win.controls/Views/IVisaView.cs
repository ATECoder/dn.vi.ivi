using System;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls.Views;

/// <summary> Interface for visa view. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
public interface IVisaView : IDisposable
{
    /// <summary> Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    public VisaSessionBase? VisaSessionBase { get; }

    /// <summary> Gets or sets the display view. </summary>
    /// <value> The display view. </value>
    public DisplayView? DisplayView { get; }

    /// <summary> Gets or sets the status view. </summary>
    /// <value> The status view. </value>
    public StatusView? StatusView { get; }

    /// <summary> Gets or sets the number of views. </summary>
    /// <value> The number of views. </value>
    public int ViewsCount { get; }

    /// <summary> Adds (inserts) a View. </summary>
    /// <param name="view"> The control. </param>
    public void AddView( VisaViewControl view );

    /// <summary> Adds (inserts) a View. </summary>
    /// <param name="view">        The View control. </param>
    /// <param name="viewIndex">   Zero-based index of the view. </param>
    /// <param name="viewName">    Name of the view. </param>
    /// <param name="viewCaption"> The view caption. </param>
    public void AddView( Control view, int viewIndex, string viewName, string viewCaption );

    /// <summary> Adds (inserts) a View. </summary>
    /// <param name="view">        The control. </param>
    /// <param name="viewIndex">   Zero-based index of the view. </param>
    /// <param name="viewName">    The name of the view. </param>
    /// <param name="viewCaption"> The caption of the view. </param>
    public void AddView( isr.WinControls.ModelViewLoggerBase view, int viewIndex, string viewName, string viewCaption );

    /// <summary> Gets or sets the number of internal resource names. </summary>
    /// <value> The number of internal resource names. </value>
    public int ResourceNamesCount { get; }

    /// <summary> Gets or sets the name of the internal selected resource. </summary>
    /// <value> The name of the internal selected resource. </value>
    public string SelectedResourceName { get; }
}

/// <summary> A visa view control. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
/// <remarks> Constructor. </remarks>
/// <param name="control"> The control. </param>
/// <param name="index">   Zero-based index of the control. </param>
/// <param name="name">    Name of the control. </param>
/// <param name="caption"> The control caption. </param>
public struct VisaViewControl( Control control, int index, string name, string caption )
{
    /// <summary> Gets or sets the control. </summary>
    /// <value> The control. </value>
    public Control Control { get; private set; } = control;

    /// <summary> Gets or sets the zero-based index of this object. </summary>
    /// <value> The index. </value>
    public int Index { get; private set; } = index;

    /// <summary> Gets or sets the name. </summary>
    /// <value> The name. </value>
    public string Name { get; private set; } = name;

    /// <summary> Gets or sets the caption. </summary>
    /// <value> The caption. </value>
    public string Caption { get; private set; } = caption;
}
