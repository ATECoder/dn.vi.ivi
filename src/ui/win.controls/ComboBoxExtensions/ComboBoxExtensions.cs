using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using cc.isr.Enums;

namespace cc.isr.VI.WinControls.ComboBoxExtensions;

/// <summary>   A combo box extensions methods. </summary>
/// <remarks>   David, 2020-12-04. </remarks>
public static partial class ComboBoxMethods
{
    #region " arm sources "

    /// <summary> Lists the arm sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">         The list control. </param>
    /// <param name="supportedArmSources"> The supported arm sources. </param>
    public static void ListSupportedArmSources( this ListControl listControl, ArmSources supportedArmSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.DataSource = typeof( ArmSources ).EnumValues().IncludeFilter( ( long ) supportedArmSources ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    /// <summary> Lists the arm sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">         The list control. </param>
    /// <param name="supportedArmSources"> The supported arm sources. </param>
    public static void ListSupportedArmSources( this ComboBox listControl, ArmSources supportedArmSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        int selectedIndex = listControl.SelectedIndex;
        listControl.DataSource = null;
        listControl.Items.Clear();
        listControl.DataSource = typeof( ArmSources ).EnumValues().IncludeFilter( ( long ) supportedArmSources ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( listControl.Items.Count > 0 )
        {
            listControl.SelectedIndex = Math.Min( listControl.Items.Count - 1, Math.Max( selectedIndex, 0 ) );
        }
    }

    #endregion

    #region " adater type "

    /// <summary> List adapter types. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">              The list control. </param>
    /// <param name="supportedAdapterTypes"> List of types of the supported adapters. </param>
    public static void ListSupportedAdapters( this ComboBox comboBox, AdapterTypes supportedAdapterTypes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif
        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( AdapterTypes ).EnumValues().IncludeFilter( ( long ) supportedAdapterTypes ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the adapter type selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SenseAdapterType. </returns>
    public static AdapterTypes SelectedAdapterType( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is AdapterTypes adapterType
            ? adapterType
            : default;
    }

    /// <summary> Select adapter type. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">    The list control. </param>
    /// <param name="adapterType"> The adapter type. </param>
    /// <returns> A VI.Scpi.AdapterType. </returns>
    public static AdapterTypes SelectAdapterType( this ComboBox comboBox, AdapterTypes? adapterType )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( adapterType.HasValue && adapterType.Value != AdapterTypes.None && adapterType.Value != comboBox.SelectedAdapterType() )
            comboBox.SelectedItem = adapterType.Value.ValueDescriptionPair();

        return comboBox.SelectedAdapterType();
    }

    /// <summary> Safe select adapter type. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">    The list control. </param>
    /// <param name="adapterType"> The adapter type. </param>
    /// <returns> The VI.AdapterTypes. </returns>
    public static AdapterTypes SafeSelectAdapterType( this ComboBox comboBox, AdapterTypes? adapterType )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( adapterType.HasValue && adapterType.Value != AdapterTypes.None && adapterType.Value != comboBox.SelectedAdapterType() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, AdapterTypes?, AdapterTypes>( SafeSelectAdapterType ), [comboBox, adapterType] );
            else
                comboBox.SelectedItem = adapterType.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedAdapterType();
    }

    #endregion

    #region " digital active level "

    /// <summary> List digital active levels. </summary>
    /// <remarks> David, 2020-11-13. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">                      The list control. </param>
    /// <param name="supportedListDigitalActiveLevels"> The supported list digital active levels. </param>
    public static void ListDigitalActiveLevels( this ListControl listControl, DigitalActiveLevels supportedListDigitalActiveLevels )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.DataSource = typeof( DigitalActiveLevels ).EnumValues().IncludeFilter( ( long ) supportedListDigitalActiveLevels ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    /// <summary> List digital active levels. </summary>
    /// <remarks> David, 2020-11-13. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">                      The list control. </param>
    /// <param name="supportedListDigitalActiveLevels"> The supported list digital active levels. </param>
    public static void ListDigitalActiveLevels( this ComboBox listControl, DigitalActiveLevels supportedListDigitalActiveLevels )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.Items.Clear();
        listControl.DataSource = typeof( DigitalActiveLevels ).EnumValues().IncludeFilter( ( long ) supportedListDigitalActiveLevels ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    #endregion

    #region " feed controls "

    /// <summary> Lists the Feed Controls. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">           The list control. </param>
    /// <param name="supportedFeedControls"> The supported Feed Controls. </param>
    public static void ListSupportedFeedControls( this ListControl listControl, FeedControls supportedFeedControls )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.DataSource = typeof( FeedControls ).EnumValues().IncludeFilter( ( long ) supportedFeedControls ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    /// <summary> Lists the Feed Controls. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">           The list control. </param>
    /// <param name="supportedFeedControls"> The supported Feed Controls. </param>
    public static void ListSupportedFeedControls( this ComboBox listControl, FeedControls supportedFeedControls )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.Items.Clear();
        listControl.DataSource = typeof( FeedControls ).EnumValues().IncludeFilter( ( long ) supportedFeedControls ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    #endregion

    #region " feed sources "

    /// <summary> Lists the Feed sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">          The list control. </param>
    /// <param name="supportedFeedSources"> The supported Feed sources. </param>
    public static void ListSupportedFeedSources( this ListControl listControl, FeedSources supportedFeedSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.DataSource = typeof( FeedSources ).EnumValues().IncludeFilter( ( long ) supportedFeedSources ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    /// <summary> Lists the Feed sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">          The list control. </param>
    /// <param name="supportedFeedSources"> The supported Feed sources. </param>
    public static void ListSupportedFeedSources( this ComboBox listControl, FeedSources supportedFeedSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.Items.Clear();
        listControl.DataSource = typeof( FeedSources ).EnumValues().IncludeFilter( ( long ) supportedFeedSources ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    #endregion

    #region " multimeter function mode "

    /// <summary> List Multimeter function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox"> The list control. </param>
    public static void ListMultimeterFunctionModes( this ComboBox comboBox )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( MultimeterFunctionModes ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the Multimeter function mode selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The MultimeterMultimeterFunctionModes. </returns>
    public static MultimeterFunctionModes SelectedMultimeterFunctionMode( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is MultimeterFunctionModes multimeterFunctionModes
            ? multimeterFunctionModes
            : default;
    }

    /// <summary> Select multimeter function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">               The list control. </param>
    /// <param name="multimeterFunctionMode"> The multimeter function modes. </param>
    /// <returns> The vi.tsp2.MultimeterFunctionMode. </returns>
    public static MultimeterFunctionModes SelectMultimeterFunctionMode( this ComboBox comboBox, MultimeterFunctionModes? multimeterFunctionMode )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( multimeterFunctionMode.HasValue && multimeterFunctionMode.Value != MultimeterFunctionModes.None && multimeterFunctionMode.Value != comboBox.SelectedMultimeterFunctionMode() )
            comboBox.SelectedItem = multimeterFunctionMode.Value.ValueDescriptionPair();

        return comboBox.SelectedMultimeterFunctionMode();
    }

    /// <summary> Safe select multimeter function mode. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">               The list control. </param>
    /// <param name="multimeterFunctionMode"> The multimeter function mode. </param>
    /// <returns> A VI.Tsp2.MultimeterFunctionMode. </returns>
    public static MultimeterFunctionModes SafeSelectMultimeterFunctionMode( this ComboBox comboBox, MultimeterFunctionModes? multimeterFunctionMode )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( multimeterFunctionMode.HasValue && multimeterFunctionMode.Value != MultimeterFunctionModes.None && multimeterFunctionMode.Value != comboBox.SelectedMultimeterFunctionMode() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, MultimeterFunctionModes?, MultimeterFunctionModes>( ComboBoxMethods.SafeSelectMultimeterFunctionMode ), [comboBox, multimeterFunctionMode] );
            else
                comboBox.SelectedItem = multimeterFunctionMode.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedMultimeterFunctionMode();
    }

    #endregion

    #region " reading element types "

    /// <summary> List Reading Element Types. </summary>
    /// <param name="comboBox">    The list control. </param>
    /// <param name="includeMask"> The include mask. </param>
    /// <param name="excludeMask"> The exclude mask. </param>
    public static void ListReadingElementTypes( this ComboBox comboBox, ReadingElementTypes includeMask, ReadingElementTypes excludeMask )
    {
        ComboBoxMethods.ListSupportedReadingElementTypes( comboBox, includeMask & ~excludeMask );
    }

    /// <summary> List supported reading Element types. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                     The list control. </param>
    /// <param name="supportedReadingElementTypes"> List of types of the supported reading element. </param>
    public static void ListSupportedReadingElementTypes( this ComboBox comboBox, ReadingElementTypes supportedReadingElementTypes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( ReadingElementTypes ).EnumValues().IncludeFilter( ( long ) supportedReadingElementTypes ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the Reading Element Types selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The VI.ReadingElementTypes. </returns>
    public static ReadingElementTypes SelectedReadingElementType( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is ReadingElementTypes readingElementTypes
            ? readingElementTypes
            : default;
    }

    /// <summary> Select Reading Element Type. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">           The list control. </param>
    /// <param name="readingElementType"> Types of the reading element. </param>
    /// <returns> The VI.ReadingElementTypes. </returns>
    public static ReadingElementTypes SelectReadingElementType( this ComboBox comboBox, ReadingElementTypes? readingElementType )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( readingElementType.HasValue && readingElementType.Value != ReadingElementTypes.None && readingElementType.Value != comboBox.SelectedReadingElementType() )
            comboBox.SelectedItem = readingElementType.Value.ValueDescriptionPair();

        return comboBox.SelectedReadingElementType();
    }

    /// <summary> Thread Safe select Reading Element Types. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">           The list control. </param>
    /// <param name="readingElementType"> The Reading Element Type. </param>
    /// <returns> The VI.ReadingElementTypes. </returns>
    public static ReadingElementTypes SafeSelectReadingElementType( this ComboBox comboBox, ReadingElementTypes? readingElementType )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( readingElementType.HasValue && readingElementType.Value != ReadingElementTypes.None && readingElementType.Value != comboBox.SelectedReadingElementType() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, ReadingElementTypes?, ReadingElementTypes>( ComboBoxMethods.SafeSelectReadingElementType ), [comboBox, readingElementType] );
            else
                comboBox.SelectedItem = readingElementType.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedReadingElementType();
    }

    #endregion

    #region " resistance range current "

    /// <summary> List resistance range currents. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                The list control. </param>
    /// <param name="resistanceRangeCurrents"> The resistance range currents. </param>
    /// <param name="excludedIndexes">         The excluded indexes. </param>
    public static void ListResistanceRangeCurrents( this ComboBox comboBox, ResistanceRangeCurrentCollection resistanceRangeCurrents, IEnumerable<int> excludedIndexes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
        ArgumentNullException.ThrowIfNull( resistanceRangeCurrents, nameof( resistanceRangeCurrents ) );
        ArgumentNullException.ThrowIfNull( excludedIndexes, nameof( excludedIndexes ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
        if ( resistanceRangeCurrents is null ) throw new ArgumentNullException( nameof( resistanceRangeCurrents ) );
        if ( excludedIndexes is null ) throw new ArgumentNullException( nameof( excludedIndexes ) );
#endif

        ResistanceRangeCurrentCollection clonedValues = [.. resistanceRangeCurrents];
        if ( excludedIndexes?.Any() == true )
        {
            List<int> l = [.. excludedIndexes];
            l.Sort();
            l.Reverse();
            foreach ( int i in l )
                clonedValues.RemoveAt( i );
        }

        comboBox.ListResistanceRangeCurrents( clonedValues );
    }

    /// <summary> List resistance range currents. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                The list control. </param>
    /// <param name="resistanceRangeCurrents"> The resistance range currents. </param>
    public static void ListResistanceRangeCurrents( this ComboBox comboBox, ResistanceRangeCurrentCollection resistanceRangeCurrents )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
        ArgumentNullException.ThrowIfNull( resistanceRangeCurrents, nameof( resistanceRangeCurrents ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
        if ( resistanceRangeCurrents is null ) throw new ArgumentNullException( nameof( resistanceRangeCurrents ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        bool wasEnabled = comboBox.Enabled;
        comboBox.Enabled = false;
        comboBox.DataSource = null;
        comboBox.Items.Clear();
        comboBox.DataSource = resistanceRangeCurrents;
        comboBox.ValueMember = nameof( ResistanceRangeCurrent.ResistanceRange );
        comboBox.DisplayMember = nameof( ResistanceRangeCurrent.Caption );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );

        comboBox.Enabled = wasEnabled;
    }

    /// <summary> Returns the Resistance Range Current selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SenseResistanceRangeCurrents. </returns>
    public static ResistanceRangeCurrent? SelectedResistanceRangeCurrent( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is ResistanceRangeCurrent range ? range : default;
    }

    /// <summary> Select Resistance Range Currents. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">               The list control. </param>
    /// <param name="resistanceRangeCurrent"> The resistance range current. </param>
    /// <returns> A VI.ResistanceRangeCurrent. </returns>
    public static ResistanceRangeCurrent? SelectResistanceRangeCurrent( this ComboBox comboBox, ResistanceRangeCurrent resistanceRangeCurrent )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( resistanceRangeCurrent is not null && (comboBox.SelectedResistanceRangeCurrent() is null
            || resistanceRangeCurrent.ResistanceRange != comboBox.SelectedResistanceRangeCurrent()!.ResistanceRange) )
        {
            comboBox.SelectedItem = resistanceRangeCurrent;
        }

        return comboBox.SelectedResistanceRangeCurrent();
    }

    /// <summary> Select Resistance Range Currents. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                The list control. </param>
    /// <param name="resistanceRangeCurrents"> The resistance range currents. </param>
    /// <param name="range">                   The range. </param>
    /// <returns> A VI.ResistanceRangeCurrent. </returns>
    public static ResistanceRangeCurrent? SelectResistanceRangeCurrent( this ComboBox comboBox, ResistanceRangeCurrentCollection resistanceRangeCurrents, decimal range )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
        ArgumentNullException.ThrowIfNull( resistanceRangeCurrents, nameof( resistanceRangeCurrents ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
        if ( resistanceRangeCurrents is null ) throw new ArgumentNullException( nameof( resistanceRangeCurrents ) );
#endif

        _ = comboBox.SelectResistanceRangeCurrent( resistanceRangeCurrents.MatchResistanceRange( range ) );
        return comboBox.SelectedResistanceRangeCurrent();
    }

    /// <summary> Select Resistance Range Currents. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                The list control. </param>
    /// <param name="resistanceRangeCurrents"> The resistance range currents. </param>
    /// <param name="range">                   The range. </param>
    /// <param name="current">                 The current. </param>
    /// <returns> A VI.ResistanceRangeCurrent. </returns>
    public static ResistanceRangeCurrent? SelectResistanceRangeCurrent( this ComboBox comboBox, ResistanceRangeCurrentCollection resistanceRangeCurrents, decimal range, decimal current )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
        ArgumentNullException.ThrowIfNull( resistanceRangeCurrents, nameof( resistanceRangeCurrents ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
        if ( resistanceRangeCurrents is null ) throw new ArgumentNullException( nameof( resistanceRangeCurrents ) );
#endif

        _ = comboBox.SelectResistanceRangeCurrent( resistanceRangeCurrents.MatchResistanceRange( range, current ) );
        return comboBox.SelectedResistanceRangeCurrent();
    }

    #endregion

    #region " sense function modes "

    /// <summary> List supported sense function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">               The list control. </param>
    /// <param name="supportedFunctionModes"> The supported function modes. </param>
    public static void ListSupportedSenseFunctionModes( this ComboBox comboBox, SenseFunctionModes supportedFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( SenseFunctionModes ).EnumValues().IncludeFilter( ( long ) supportedFunctionModes ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the sense function modes selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SenseSenseFunctionModes. </returns>
    public static SenseFunctionModes SelectedSenseFunctionModes( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is SenseFunctionModes senseFunctionModes
            ? senseFunctionModes
            : default;
    }

    /// <summary> Select sense function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">           The list control. </param>
    /// <param name="senseFunctionModes"> The sense function mode. </param>
    /// <returns> The VI.SenseFunctionModes. </returns>
    public static SenseFunctionModes SelectSenseFunctionModes( this ComboBox comboBox, SenseFunctionModes? senseFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( senseFunctionModes.HasValue && senseFunctionModes.Value != SenseFunctionModes.None && senseFunctionModes.Value != comboBox.SelectedSenseFunctionModes() )
            comboBox.SelectedItem = senseFunctionModes.Value.ValueDescriptionPair();

        return comboBox.SelectedSenseFunctionModes();
    }

    /// <summary> Safe select sense function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">           The list control. </param>
    /// <param name="senseFunctionModes"> The sense function mode. </param>
    /// <returns> The VI.SenseFunctionModes. </returns>
    public static SenseFunctionModes SafeSelectSenseFunctionModes( this ComboBox comboBox, SenseFunctionModes? senseFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( senseFunctionModes.HasValue && senseFunctionModes.Value != SenseFunctionModes.None && senseFunctionModes.Value != comboBox.SelectedSenseFunctionModes() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, SenseFunctionModes?, SenseFunctionModes>( ComboBoxMethods.SafeSelectSenseFunctionModes ), [comboBox, senseFunctionModes] );
            else
                comboBox.SelectedItem = senseFunctionModes.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedSenseFunctionModes();
    }

    #endregion

    #region " source function modes "

    /// <summary> List supported source function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                     The list control. </param>
    /// <param name="supportedSourceFunctionModes"> The supported source function modes. </param>
    public static void ListSupportedSourceFunctionModes( this ComboBox comboBox, SourceFunctionModes supportedSourceFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( SourceFunctionModes ).EnumValues().IncludeFilter( ( long ) supportedSourceFunctionModes ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the Source function modes selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SourceSourceFunctionModes. </returns>
    public static SourceFunctionModes SelectedSourceFunctionModes( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is SourceFunctionModes sourceFunctionModes
            ? sourceFunctionModes
            : default;
    }

    /// <summary> Select Source function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">            The list control. </param>
    /// <param name="sourceFunctionModes"> The Source function mode. </param>
    /// <returns> The VI.SourceFunctionModes. </returns>
    public static SourceFunctionModes SelectSourceFunctionModes( this ComboBox comboBox, SourceFunctionModes? sourceFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( sourceFunctionModes.HasValue && sourceFunctionModes.Value != SourceFunctionModes.None && sourceFunctionModes.Value != comboBox.SelectedSourceFunctionModes() )
            comboBox.SelectedItem = sourceFunctionModes.Value.ValueDescriptionPair();

        return comboBox.SelectedSourceFunctionModes();
    }

    /// <summary> Safe select Source function modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">            The list control. </param>
    /// <param name="sourceFunctionModes"> The Source function mode. </param>
    /// <returns> The VI.SourceFunctionModes. </returns>
    public static SourceFunctionModes SafeSelectSourceFunctionModes( this ComboBox comboBox, SourceFunctionModes? sourceFunctionModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( sourceFunctionModes.HasValue && sourceFunctionModes.Value != SourceFunctionModes.None && sourceFunctionModes.Value != comboBox.SelectedSourceFunctionModes() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, SourceFunctionModes?, SourceFunctionModes>( ComboBoxMethods.SafeSelectSourceFunctionModes ), [comboBox, sourceFunctionModes] );
            else
                comboBox.SelectedItem = sourceFunctionModes.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedSourceFunctionModes();
    }

    #endregion

    #region " trigger events "

    /// <summary> List supported trigger events. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">               The list control. </param>
    /// <param name="supportedTriggerEvents"> The supported trigger events. </param>
    public static void ListSupportedTriggerEvents( this ComboBox comboBox, TriggerEvents supportedTriggerEvents )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( TriggerEvents ).EnumValues().IncludeFilter( ( long ) supportedTriggerEvents ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the trigger event selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SenseTriggerEvent. </returns>
    public static TriggerEvents SelectedTriggerEvent( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is TriggerEvents triggerEvents
            ? triggerEvents
            : default;
    }

    /// <summary> Select trigger event. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">     The list control. </param>
    /// <param name="triggerEvent"> The trigger event. </param>
    /// <returns> A VI.Scpi.TriggerEvent. </returns>
    public static TriggerEvents SelectTriggerEvent( this ComboBox comboBox, TriggerEvents? triggerEvent )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( triggerEvent.HasValue && triggerEvent.Value != TriggerEvents.None && triggerEvent.Value != comboBox.SelectedTriggerEvent() )
            comboBox.SelectedItem = triggerEvent.Value.ValueDescriptionPair();

        return comboBox.SelectedTriggerEvent();
    }

    /// <summary> Safe select trigger event. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">     The list control. </param>
    /// <param name="triggerEvent"> The trigger event. </param>
    /// <returns> A VI.Scpi.TriggerEvent. </returns>
    public static TriggerEvents SafeSelectTriggerEvent( this ComboBox comboBox, TriggerEvents? triggerEvent )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( triggerEvent.HasValue && triggerEvent.Value != TriggerEvents.None && triggerEvent.Value != comboBox.SelectedTriggerEvent() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, TriggerEvents?, TriggerEvents>( ComboBoxMethods.SafeSelectTriggerEvent ), [comboBox, triggerEvent] );
            else
                comboBox.SelectedItem = triggerEvent.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedTriggerEvent();
    }

    #endregion

    #region " trigger sources "

    /// <summary> List supported trigger sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                The list control. </param>
    /// <param name="supportedTriggerSources"> The supported trigger sources. </param>
    public static void ListSupportedTriggerSources( this ComboBox comboBox, TriggerSources supportedTriggerSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( TriggerSources ).EnumValues().IncludeFilter( ( long ) supportedTriggerSources ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Min( comboBox.Items.Count - 1, Math.Max( selectedIndex, 0 ) );
    }

    /// <summary> Selected trigger sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The VI.TriggerSources. </returns>
    public static TriggerSources SelectedTriggerSources( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is TriggerSources triggerSources
            ? triggerSources
            : default;
    }

    /// <summary> Select trigger Sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">       The list control. </param>
    /// <param name="triggerSources"> The trigger Sources. </param>
    /// <returns> A VI.TriggerSources. </returns>
    public static TriggerSources SelectTriggerSources( this ComboBox comboBox, TriggerSources? triggerSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( triggerSources.HasValue && triggerSources.Value != TriggerSources.None && triggerSources.Value != comboBox.SelectedTriggerSources() )
            comboBox.SelectedItem = triggerSources.Value.ValueDescriptionPair();

        return comboBox.SelectedTriggerSources();
    }

    /// <summary> Safe select trigger Sources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">       The list control. </param>
    /// <param name="triggerSources"> The trigger Sources. </param>
    /// <returns> A VI.TriggerSources. </returns>
    public static TriggerSources SafeSelectTriggerSources( this ComboBox comboBox, TriggerSources? triggerSources )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( triggerSources.HasValue && triggerSources.Value != TriggerSources.None && triggerSources.Value != comboBox.SelectedTriggerSources() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, TriggerSources?, TriggerSources>( ComboBoxMethods.SafeSelectTriggerSources ), [comboBox, triggerSources] );
            else
                comboBox.SelectedItem = triggerSources.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedTriggerSources();
    }

    /// <summary> Returns the trigger source selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">     The list control. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <returns> The SenseTriggerSources. </returns>
    public static TriggerSources SelectedTriggerSources( this ComboBox comboBox, TriggerSources defaultValue )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        return comboBox.SelectedValue is TriggerSources triggerSources
            ? triggerSources
            : default;
    }

    #endregion

    #region " trace parameters "

    /// <summary> List supported trace parameters. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">                 The list control. </param>
    /// <param name="supportedTraceParameters"> The supported trace parameters. </param>
    public static void ListSupportedTraceParameters( this ComboBox comboBox, TraceParameters supportedTraceParameters )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        int selectedIndex = comboBox.SelectedIndex;
        comboBox.DataSource = null;
        comboBox.DataSource = typeof( TraceParameters ).EnumValues().IncludeFilter( ( long ) supportedTraceParameters ).ValueDescriptionPairs().ToList();
        comboBox.DisplayMember = nameof( KeyValuePair<,>.Value );
        comboBox.ValueMember = nameof( KeyValuePair<,>.Key );
        if ( comboBox.Items.Count > 0 )
            comboBox.SelectedIndex = Math.Max( selectedIndex, 0 );
    }

    /// <summary> Returns the Trace Parameter selected by the list control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The SenseTraceParameters. </returns>
    public static TraceParameters SelectedTraceParameters( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is TraceParameters traceParameters
            ? traceParameters
            : default;
    }

    /// <summary> Select trace parameters. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">        The list control. </param>
    /// <param name="traceParameters"> The trace parameters. </param>
    /// <returns> The VI.TraceParameters. </returns>
    public static TraceParameters SelectTraceParameters( this ComboBox comboBox, TraceParameters? traceParameters )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( traceParameters.HasValue && traceParameters.Value != TraceParameters.None && traceParameters.Value != comboBox.SelectedTraceParameters() )
            comboBox.SelectedItem = traceParameters.Value.ValueDescriptionPair();

        return comboBox.SelectedTraceParameters();
    }

    /// <summary> Safe select trace parameters. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">        The list control. </param>
    /// <param name="traceParameters"> The trace parameters. </param>
    /// <returns> The VI.TraceParameters. </returns>
    public static TraceParameters SafeSelectTraceParameters( this ComboBox comboBox, TraceParameters? traceParameters )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( traceParameters.HasValue && traceParameters.Value != TraceParameters.None && traceParameters.Value != comboBox.SelectedTraceParameters() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, TraceParameters?, TraceParameters>( ComboBoxMethods.SafeSelectTraceParameters ), [comboBox, traceParameters] );
            else
                comboBox.SelectedItem = traceParameters.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedTraceParameters();
    }

    #endregion

    #region " vent log modes "

    /// <summary> List supported event log modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl">            The list control. </param>
    /// <param name="supportedEventLogModes"> The supported event log modes. </param>
    public static void ListSupportedEventLogModes( this ListControl listControl, Syntax.Tsp.EventLogModes supportedEventLogModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        listControl.DataSource = null;
        listControl.DataSource = typeof( Syntax.Tsp.EventLogModes ).EnumValues().IncludeFilter( ( long ) supportedEventLogModes ).ValueDescriptionPairs().ToList();
        listControl.DisplayMember = nameof( KeyValuePair<,>.Value );
        listControl.ValueMember = nameof( KeyValuePair<,>.Key );
    }

    /// <summary> Selected event log modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="listControl"> The list control. </param>
    /// <returns> The VI.Tsp.Ieee488Syntax.EventLogModes. </returns>
    public static Syntax.Tsp.EventLogModes SelectedEventLogModes( this ListControl listControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( listControl, nameof( listControl ) );
#else
        if ( listControl is null ) throw new ArgumentNullException( nameof( listControl ) );
#endif

        return listControl.SelectedValue is Syntax.Tsp.EventLogModes eventLogModes
            ? eventLogModes
            : default;
    }

    /// <summary> Select event log modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">      The list control. </param>
    /// <param name="eventLogModes"> The event log modes. </param>
    /// <returns> The VI.Tsp.Ieee488Syntax.EventLogModes. </returns>
    public static Syntax.Tsp.EventLogModes SelectEventLogModes( this ComboBox comboBox, Syntax.Tsp.EventLogModes? eventLogModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( eventLogModes.HasValue && eventLogModes.Value != Syntax.Tsp.EventLogModes.None && eventLogModes.Value != comboBox.SelectedEventLogModes() )
            comboBox.SelectedItem = eventLogModes.Value.ValueDescriptionPair();

        return comboBox.SelectedEventLogModes();
    }

    /// <summary> Safe select event log modes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="comboBox">      The list control. </param>
    /// <param name="eventLogModes"> The event log modes. </param>
    /// <returns> The VI.Tsp.Ieee488Syntax.EventLogModes. </returns>
    public static Syntax.Tsp.EventLogModes SafeSelectEventLogModes( this ComboBox comboBox, Syntax.Tsp.EventLogModes? eventLogModes )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( comboBox, nameof( comboBox ) );
#else
        if ( comboBox is null ) throw new ArgumentNullException( nameof( comboBox ) );
#endif

        if ( eventLogModes.HasValue && eventLogModes.Value != Syntax.Tsp.EventLogModes.None && eventLogModes.Value != comboBox.SelectedEventLogModes() )
        {
            if ( comboBox.InvokeRequired )
                _ = comboBox.Invoke( new Func<ComboBox, Syntax.Tsp.EventLogModes?, Syntax.Tsp.EventLogModes>( SafeSelectEventLogModes ), [comboBox, eventLogModes] );
            else
                comboBox.SelectedItem = eventLogModes.Value.ValueDescriptionPair();
        }

        return comboBox.SelectedEventLogModes();
    }

    #endregion
}
