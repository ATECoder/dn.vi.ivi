using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls.DataGridViewExtensions;

/// <summary>   A methods. </summary>
/// <remarks>   David, 2020-12-04. </remarks>
public static class Methods
{
    #region " buffer readings "

    /// <summary> Configure the display of <see cref="VI.BufferReading"/>. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The buffer readings. </param>
    /// <returns> The column count. </returns>
    private static int ConfigureDisplay( this DataGridView grid, BufferReadingBindingList values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        values.Synchronizer = grid;
        grid.Enabled = false;
        grid.AllowUserToAddRows = false;
        grid.AutoGenerateColumns = false;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        grid.BorderStyle = BorderStyle.Fixed3D;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        grid.EnableHeadersVisualStyles = true;
        grid.MultiSelect = true;
        grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
        grid.ScrollBars = ScrollBars.Both;
        grid.Columns.Clear();
        grid.DataSource = values;
        int displayIndex = 0;
        DataGridViewTextBoxColumn column = new()
        {
            DataPropertyName = nameof( BufferReading.Reading ),
            Name = nameof( BufferReading.Reading ),
            Visible = true,
            DisplayIndex = displayIndex,
            HeaderText = "Value"
        };
        _ = grid.Columns.Add( column );
        displayIndex += 1;
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = nameof( BufferReading.UnitReading ),
            Name = nameof( BufferReading.UnitReading ),
            Visible = true,
            DisplayIndex = displayIndex,
            HeaderText = "Unit"
        };
        _ = grid.Columns.Add( column );
        displayIndex += 1;
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = nameof( BufferReading.FractionalSecond ),
            Name = nameof( BufferReading.FractionalSecond ),
            Visible = true,
            DisplayIndex = displayIndex,
            HeaderText = "Time"
        };
        column.DefaultCellStyle.Format = "0.000";
        _ = grid.Columns.Add( column );
        displayIndex += 1;
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = nameof( BufferReading.StatusReading ),
            Name = nameof( BufferReading.StatusReading ),
            Visible = true,
            DisplayIndex = displayIndex,
            HeaderText = "Status"
        };
        _ = grid.Columns.Add( column );
        grid.Enabled = wasEnabled;
        return grid.Columns is not null ? grid.Columns.Count : 0;
    }

    /// <summary>
    /// Binds the <see cref="cc.isr.VI.BufferReadingBindingList">readings</see> on the grid.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">        The grid. </param>
    /// <param name="values">      The buffer readings. </param>
    /// <param name="reconfigure"> True to reconfigure. </param>
    /// <returns> The column count. </returns>
    public static int Bind( this DataGridView grid, BufferReadingBindingList values, bool reconfigure )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        try
        {
            grid.Enabled = false;
            // 2017-02-24: had to configure each time otherwise, the grid would not display
            // when called from the thread.
            // 20190124: not sure if this limitations still applies when using a binding list.
            if ( grid.DataSource is null || reconfigure )
            {
                _ = grid.ConfigureDisplay( values );
                // grid.DataSource = Me
            }

            foreach ( DataGridViewColumn c in grid.Columns )
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            grid.AutoResizeColumns( DataGridViewAutoSizeColumnsMode.AllCells );
            grid.Invalidate();
            return grid.Columns.Count;
        }
        catch
        {
            throw;
        }
        finally
        {
            grid.Enabled = wasEnabled;
        }
    }

    #endregion

    #region " channel resistance "

    /// <summary> Configure display of <see cref="ChannelResistorCollection"/> values. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel resistors. </param>
    /// <returns> The column count. </returns>
    public static int ConfigureDisplayValues( this DataGridView grid, ChannelResistorCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        grid.Enabled = false;
        grid.DataSource = null;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.AutoGenerateColumns = false;
        grid.RowHeadersVisible = false;
        grid.ReadOnly = true;
        grid.DataSource = values;
        grid.Columns.Clear();
        grid.Refresh();
        int displayIndex;
        int width = 0;
        DataGridViewTextBoxColumn? column = null;
        try
        {
            displayIndex = 0;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelResistor.Title ),
                Name = nameof( ChannelSourceMeasure.Title ),
                Visible = true,
                Width = 50,
                DisplayIndex = displayIndex
            };
            _ = grid.Columns.Add( column );
            width += column.Width;
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        try
        {
            displayIndex += 1;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelResistor.Resistance ),
                Name = nameof( ChannelResistor.Resistance ),
                Visible = true,
                DisplayIndex = displayIndex,
                Width = grid.Width - width - grid.Columns.Count
            };
            column.DefaultCellStyle.Format = "G5";
            _ = grid.Columns.Add( column );
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        grid.Enabled = true;
        return grid.Columns is not null && grid.Columns.Count > 0 ? grid.Columns.Count : 0;
    }

    /// <summary> Displays <see cref="ChannelResistorCollection"/> </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel resistors. </param>
    /// <returns> The column count. </returns>
    public static int DisplayValues( this DataGridView grid, ChannelResistorCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        if ( grid.DataSource is null )
        {
            _ = grid.ConfigureDisplayValues( values );
            Application.DoEvents();
        }

        grid.DataSource = values.ToList();
        Application.DoEvents();
        return grid.Columns.Count;
    }

    /// <summary> Configure display of <see cref="ChannelResistorCollection"/> </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel resistors. </param>
    /// <returns> The column count. </returns>
    public static int ConfigureDisplay( this DataGridView grid, ChannelResistorCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        grid.Enabled = false;
        grid.DataSource = null;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.AutoGenerateColumns = true;
        grid.RowHeadersVisible = false;
        grid.ReadOnly = true;
        grid.DataSource = values;
        grid.Enabled = true;
        return grid.Columns is not null && grid.Columns.Count > 0 ? grid.Columns.Count : 0;
    }

    /// <summary> Displays <see cref="ChannelResistorCollection"/> </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel resistors. </param>
    /// <returns> The column count. </returns>
    public static int Display( this DataGridView grid, ChannelResistorCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        if ( grid.DataSource is null )
        {
            _ = grid.ConfigureDisplay( values );
            Application.DoEvents();
        }

        grid.DataSource = values.ToList();
        Application.DoEvents();
        return grid.Columns.Count;
    }

    #endregion

    #region " channel source measure "

    /// <summary> Configure display of the <see cref="ChannelSourceMeasureCollection"/> </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel source measures. </param>
    /// <returns> the column count. </returns>
    public static int ConfigureDisplayValues( this DataGridView grid, ChannelSourceMeasureCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        grid.Enabled = false;
        grid.DataSource = null;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.AutoGenerateColumns = false;
        grid.RowHeadersVisible = false;
        grid.ReadOnly = true;
        grid.DataSource = values;
        grid.Columns.Clear();
        grid.Refresh();
        int displayIndex;
        int width = 0;
        DataGridViewTextBoxColumn? column = null;
        try
        {
            displayIndex = 0;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelSourceMeasure.Title ),
                Name = nameof( ChannelSourceMeasure.Title ),
                Visible = true,
                Width = 50,
                DisplayIndex = displayIndex
            };
            _ = grid.Columns.Add( column );
            width += column.Width;
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        column = null;
        try
        {
            displayIndex += 1;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelSourceMeasure.Voltage ),
                Name = "Volt",
                Visible = true,
                DisplayIndex = displayIndex,
                Width = 80
            };
            column.DefaultCellStyle.Format = "G5";
            _ = grid.Columns.Add( column );
            width += column.Width;
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        column = null;
        try
        {
            displayIndex += 1;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelSourceMeasure.Current ),
                Name = "Ampere",
                Visible = true,
                DisplayIndex = displayIndex,
                Width = 80
            };
            column.DefaultCellStyle.Format = "G5";
            _ = grid.Columns.Add( column );
            width += column.Width;
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        column = null;
        try
        {
            displayIndex += 1;
            column = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof( ChannelSourceMeasure.Resistance ),
                Name = "Ohm",
                Visible = true,
                DisplayIndex = displayIndex,
                Width = grid.Width - width - grid.Columns.Count
            };
            column.DefaultCellStyle.Format = "G5";
            _ = grid.Columns.Add( column );
        }
        catch
        {
            if ( column is not null )
                column.Dispose();
            throw;
        }

        grid.Enabled = true;
        return grid.Columns is not null && grid.Columns.Count > 0 ? grid.Columns.Count : 0;
    }

    /// <summary> Displays the <see cref="ChannelSourceMeasureCollection"/> </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel source measures. </param>
    /// <returns> The column count. </returns>
    public static int DisplayValues( this DataGridView grid, ChannelSourceMeasureCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        if ( grid.DataSource is null )
        {
            _ = grid.ConfigureDisplayValues( values );
            Application.DoEvents();
        }

        grid.DataSource = values.ToList();
        Application.DoEvents();
        return grid.Columns.Count;
    }

    /// <summary> Configure display of <see cref="ChannelSourceMeasureCollection"/> . </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel source measures. </param>
    /// <returns> The column count. </returns>
    public static int ConfigureDisplay( this DataGridView grid, ChannelSourceMeasureCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        grid.Enabled = false;
        grid.DataSource = null;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.AutoGenerateColumns = true;
        grid.RowHeadersVisible = false;
        grid.ReadOnly = true;
        grid.DataSource = values;
        grid.Enabled = true;
        return grid.Columns is not null && grid.Columns.Count > 0 ? grid.Columns.Count : 0;
    }

    /// <summary>
    /// Displays the <see cref="ChannelSourceMeasureCollection"/> using default configuration.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The channel source measures. </param>
    /// <returns> The column count. </returns>
    public static int Display( this DataGridView grid, ChannelSourceMeasureCollection values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.Enabled = wasEnabled;
        if ( grid.DataSource is null )
        {
            _ = grid.ConfigureDisplay( values );
            Application.DoEvents();
        }

        grid.DataSource = values.ToList();
        Application.DoEvents();
        return grid.Columns.Count;
    }

    #endregion

    #region " reading amounts "

    /// <summary> Configure the display of <see cref="VI.ReadingAmounts"/>. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The values. </param>
    /// <returns> The column count. </returns>
    public static int ConfigureDisplay( this DataGridView grid, IEnumerable<ReadingAmounts> values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        grid.Enabled = false;
        grid.AllowUserToAddRows = false;
        grid.AutoGenerateColumns = false;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        grid.BorderStyle = BorderStyle.Fixed3D;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        grid.EnableHeadersVisualStyles = true;
        grid.MultiSelect = true;
        grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
        grid.ScrollBars = ScrollBars.Both;
        grid.Columns.Clear();
        grid.DataSource = values;
        int displayIndex = 0;
        DataGridViewTextBoxColumn column = new()
        {
            DataPropertyName = nameof( ReadingAmounts.RawReading ),
            Name = nameof( ReadingAmounts.RawReading ),
            Visible = true,
            DisplayIndex = displayIndex,
            HeaderText = "Reading"
        };
        _ = grid.Columns.Add( column );
        foreach ( DataGridViewColumn c in grid.Columns )
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        grid.Enabled = wasEnabled;
        return grid.Columns is not null ? grid.Columns.Count : 0;
    }

    /// <summary>
    /// Displays <see cref="ChannelResistorCollection"/> updating the data source on each call.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">        The grid. </param>
    /// <param name="values">      The values. </param>
    /// <param name="reconfigure"> True to reconfigure. </param>
    /// <returns> The column count. </returns>
    public static int Display( this DataGridView grid, IEnumerable<ReadingAmounts> values, bool reconfigure )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        bool wasEnabled = grid.Enabled;
        try
        {
            grid.Enabled = false;
            // 2017-02-24: had to configure each time otherwise, the grid would not display
            // when called from the thread.
            // 20190124: not sure if this limitations still applies when using a binding list.
            if ( grid.DataSource is null || reconfigure )
            {
                _ = grid.ConfigureDisplay( values );
                // grid.DataSource = Me
            }
            // For Each c As DataGridViewColumn In grid.Columns
            // c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            // Next
            grid.ScrollBars = ScrollBars.Both;
            grid.AutoResizeColumns( DataGridViewAutoSizeColumnsMode.AllCells );
            grid.DataSource = values.ToList();
            grid.Invalidate();
            return grid.Columns.Count;
        }
        catch
        {
            throw;
        }
        finally
        {
            grid.Enabled = wasEnabled;
        }
    }

    #endregion

}
