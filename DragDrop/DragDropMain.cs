using CodingDad.DragAndDrop;
using CodingDad.NET.Common.DragDrop.Drag;
using CodingDad.NET.Common.DragDrop.Drop;
using CodingDad.NET.Common.DragDrop.Enums;
using CodingDad.NET.Common.DragDrop.Icons;
using CodingDad.NET.Common.DragDrop.Interfaces;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CodingDad.NET.Common.DragDrop.Utilities
{
    public static class DragDropMain
    {
        /// <summary>
        /// Gets or sets whether the control can be used as drag source together with the right mouse.
        /// </summary>
        public static readonly DependencyProperty CanDragWithMouseRightButtonProperty
            = DependencyProperty.RegisterAttached("CanDragWithMouseRightButton",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false, OnCanDragWithMouseRightButtonChanged));

        /// <summary>
        /// Gets or sets the data format which will be used for the drag and drop operations.
        /// </summary>
        public static readonly DependencyProperty DataFormatProperty
            = DependencyProperty.RegisterAttached("DataFormat",
                                                  typeof(DataFormat),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(DragDropMain.DataFormat));

        /// <summary>
        /// Gets or sets the opacity of the default DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DefaultDragAdornerOpacityProperty
            = DependencyProperty.RegisterAttached("DefaultDragAdornerOpacity",
                                                  typeof(double),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(0.8));

        /// <summary>
        /// Gets or sets a ItemsPanel for the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DragAdornerItemsPanelProperty
            = DependencyProperty.RegisterAttached("DragAdornerItemsPanel",
                                                  typeof(ItemsPanelTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(ItemsControl.ItemsPanelProperty.DefaultMetadata.DefaultValue));

        /// <summary>
        /// Gets or sets a DragAdorner DataTemplate for multiple item selection.
        /// </summary>
        public static readonly DependencyProperty DragAdornerMultiItemTemplateProperty
            = DependencyProperty.RegisterAttached("DragAdornerMultiItemTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets a DragAdorner DataTemplateSelector for multiple item selection.
        /// </summary>
        public static readonly DependencyProperty DragAdornerMultiItemTemplateSelectorProperty
            = DependencyProperty.RegisterAttached("DragAdornerMultiItemTemplateSelector",
                                                  typeof(DataTemplateSelector),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// Gets or sets a DataTemplate for the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DragAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("DragAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets a DataTemplateSelector for the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DragAdornerTemplateSelectorProperty
            = DependencyProperty.RegisterAttached("DragAdornerTemplateSelector",
                                                  typeof(DataTemplateSelector),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// Gets or sets the translation transform which will be used for the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DragAdornerTranslationProperty
            = DependencyProperty.RegisterAttached("DragAdornerTranslation",
                                                  typeof(Point),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(new Point(-4, -4)));

        /// <summary>
        /// Gets or sets whether the drag action should be started only directly on a selected item.
        /// or also on the free control space (e.g. in a ListBox).
        /// </summary>
        public static readonly DependencyProperty DragDirectlySelectedOnlyProperty
            = DependencyProperty.RegisterAttached("DragDirectlySelectedOnly",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a context for a control. Only controls with the same context are allowed for drag or drop actions.
        /// </summary>
        public static readonly DependencyProperty DragDropContextProperty
            = DependencyProperty.RegisterAttached("DragDropContext",
                                                  typeof(string),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(string.Empty));

        /// <summary>
        /// The drag drop copy key state property (default None).
        /// So the drag drop action is
        /// - Move, within the same control or from one to another, if the drag drop key state is None
        /// - Copy, from one to another control with the given drag drop copy key state
        /// </summary>
        public static readonly DependencyProperty DragDropCopyKeyStateProperty
            = DependencyProperty.RegisterAttached("DragDropCopyKeyState",
                                                  typeof(DragDropKeyStates),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(default(DragDropKeyStates)));

        /// <summary>
        /// Gets or sets the handler for the drag operation.
        /// </summary>
        public static readonly DependencyProperty DragHandlerProperty
            = DependencyProperty.RegisterAttached("DragHandler",
                                                  typeof(IDragSource),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets the drag info builder for the drag operation.
        /// </summary>
        public static readonly DependencyProperty DragInfoBuilderProperty
            = DependencyProperty.RegisterAttached("DragInfoBuilder",
                                                  typeof(IDragInfoBuilder),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets the horizontal and vertical proportion at which the pointer will anchor on the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty DragMouseAnchorPointProperty
            = DependencyProperty.RegisterAttached("DragMouseAnchorPoint",
                                                  typeof(Point),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(new Point(0, 1)));

        /// <summary>
        /// Gets or sets the handler for the dragged preview items sorter
        /// </summary>
        public static readonly DependencyProperty DragPreviewItemsSorterProperty
            = DependencyProperty.RegisterAttached("DragPreviewItemsSorter",
                                                  typeof(IDragPreviewItemsSorter),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the maximum items count which will be used for the dragged preview.
        /// </summary>
        public static readonly DependencyProperty DragPreviewMaxItemsCountProperty
            = DependencyProperty.RegisterAttached("DragPreviewMaxItemsCount",
                                                  typeof(int),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(10, null, (_, baseValue) =>
                                                  {
                                                      var itemsCount = (int)baseValue;
                                                      // Checking for MaxValue is maybe not necessary
                                                      return itemsCount < 0 ? 0 : itemsCount >= int.MaxValue ? int.MaxValue : itemsCount;
                                                  }));

        /// <summary>
        /// Gets or sets whether an element under the mouse should be ignored for the drag operation.
        /// </summary>
        public static readonly DependencyProperty DragSourceIgnoreProperty
            = DependencyProperty.RegisterAttached("DragSourceIgnore",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets or sets a ItemsPanel for the DragAdorner based on the DropTarget.
        /// </summary>
        public static readonly DependencyProperty DropAdornerItemsPanelProperty
            = DependencyProperty.RegisterAttached("DropAdornerItemsPanel",
                                                  typeof(ItemsPanelTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(ItemsControl.ItemsPanelProperty.DefaultMetadata.DefaultValue));

        /// <summary>
        /// Gets or sets a DropAdorner DataTemplate for multiple item selection.
        /// </summary>
        public static readonly DependencyProperty DropAdornerMultiItemTemplateProperty
            = DependencyProperty.RegisterAttached("DropAdornerMultiItemTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets a DropAdorner DataTemplateSelector for multiple item selection.
        /// </summary>
        public static readonly DependencyProperty DropAdornerMultiItemTemplateSelectorProperty
            = DependencyProperty.RegisterAttached("DropAdornerMultiItemTemplateSelector",
                                                  typeof(DataTemplateSelector),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// Gets or sets a DataTemplate for the DragAdorner based on the DropTarget.
        /// </summary>
        public static readonly DependencyProperty DropAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("DropAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets a DataTemplateSelector for the DragAdorner based on the DropTarget.
        /// </summary>
        public static readonly DependencyProperty DropAdornerTemplateSelectorProperty
            = DependencyProperty.RegisterAttached("DropAdornerTemplateSelector",
                                                  typeof(DataTemplateSelector),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// Gets or sets the events which are subscribed for the drag and drop events.
        /// </summary>
        public static readonly DependencyProperty DropEventTypeProperty
            = DependencyProperty.RegisterAttached("DropEventType",
                                                  typeof(EventType),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(EventType.Auto, OnDropEventTypeChanged));

        /// <summary>
        /// Gets or sets the handler for the drop operation.
        /// </summary>
        public static readonly DependencyProperty DropHandlerProperty
            = DependencyProperty.RegisterAttached("DropHandler",
                                                  typeof(IDropTarget),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets the drop info builder for the drop operation.
        /// </summary>
        public static readonly DependencyProperty DropInfoBuilderProperty
            = DependencyProperty.RegisterAttached("DropInfoBuilder",
                                                  typeof(IDropInfoBuilder),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets the ScrollingMode for the drop operation.
        /// </summary>
        public static readonly DependencyProperty DropScrollingModeProperty
            = DependencyProperty.RegisterAttached("DropScrollingMode",
                                                  typeof(ScrollingMode),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(ScrollingMode.Both));

        /// <summary>
        /// Gets or sets the brush for the DropTargetAdorner.
        /// </summary>
        public static readonly DependencyProperty DropTargetAdornerBrushProperty
            = DependencyProperty.RegisterAttached("DropTargetAdornerBrush",
                                                  typeof(Brush),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((Brush)null));

        /// <summary>
        /// Gets or sets the pen for the DropTargetAdorner.
        /// </summary>
        public static readonly DependencyProperty DropTargetAdornerPenProperty
            = DependencyProperty.RegisterAttached("DropTargetAdornerPen",
                                                  typeof(Pen),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((Pen)null));

        /// <summary>
        /// Gets or sets the handler for the drop target items sorter
        /// </summary>
        public static readonly DependencyProperty DropTargetItemsSorterProperty
            = DependencyProperty.RegisterAttached("DropTargetItemsSorter",
                                                  typeof(IDropTargetItemsSorter),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="ScrollViewer"/> that will be used as <see cref="DropInfo.TargetScrollViewer"/>.
        /// </summary>
        public static readonly DependencyProperty DropTargetScrollViewerProperty
            = DependencyProperty.RegisterAttached("DropTargetScrollViewer",
                                                  typeof(ScrollViewer),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((ScrollViewer)null));

        /// <summary>
        /// Gets or sets the translation transform which will be used for the EffectAdorner.
        /// </summary>
        public static readonly DependencyProperty EffectAdornerTranslationProperty
            = DependencyProperty.RegisterAttached("EffectAdornerTranslation",
                                                  typeof(Point),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(new Point(16, 16)));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type All.
        /// </summary>
        public static readonly DependencyProperty EffectAllAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectAllAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type Copy.
        /// </summary>
        public static readonly DependencyProperty EffectCopyAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectCopyAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type Link.
        /// </summary>
        public static readonly DependencyProperty EffectLinkAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectLinkAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type Move.
        /// </summary>
        public static readonly DependencyProperty EffectMoveAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectMoveAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type None.
        /// </summary>
        public static readonly DependencyProperty EffectNoneAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectNoneAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets a EffectAdorner DataTemplate for effect type Scroll.
        /// </summary>
        public static readonly DependencyProperty EffectScrollAdornerTemplateProperty
            = DependencyProperty.RegisterAttached("EffectScrollAdornerTemplate",
                                                  typeof(DataTemplate),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets whether the control can be used as drag source.
        /// </summary>
        public static readonly DependencyProperty IsDragSourceProperty
            = DependencyProperty.RegisterAttached("IsDragSource",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false, OnIsDragSourceChanged));

        /// <summary>
        /// Gets or sets whether the control can be used as drop target.
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty
            = DependencyProperty.RegisterAttached("IsDropTarget",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false, OnIsDropTargetChanged));

        /// <summary>
        /// Gets or sets the Orientation which should be used for the drag drop action (default null).
        /// Normally it will be look up to find the correct orientation of the inner ItemsPanel,
        /// but sometimes it's necessary to force the orientation, if the look up is wrong.
        /// </summary>
        public static readonly DependencyProperty ItemsPanelOrientationProperty
            = DependencyProperty.RegisterAttached("ItemsPanelOrientation",
                                                  typeof(Orientation?),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the minimum horizontal drag distance to allow for limited movement of the mouse pointer before a drag operation begins.
        /// Default is SystemParameters.MinimumHorizontalDragDistance.
        /// </summary>
        public static readonly DependencyProperty MinimumHorizontalDragDistanceProperty
            = DependencyProperty.RegisterAttached("MinimumHorizontalDragDistance",
                                                  typeof(double),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(SystemParameters.MinimumHorizontalDragDistance));

        /// <summary>
        /// Gets or sets the minimum vertical drag distance to allow for limited movement of the mouse pointer before a drag operation begins.
        /// Default is SystemParameters.MinimumVerticalDragDistance.
        /// </summary>
        public static readonly DependencyProperty MinimumVerticalDragDistanceProperty
            = DependencyProperty.RegisterAttached("MinimumVerticalDragDistance",
                                                  typeof(double),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(SystemParameters.MinimumVerticalDragDistance));

        /// <summary>
        /// Gets or sets the root element finder.
        /// </summary>
        public static readonly DependencyProperty RootElementFinderProperty
            = DependencyProperty.RegisterAttached("RootElementFinder",
                                                  typeof(IRootElementFinder),
                                                  typeof(DragDropMain));

        /// <summary>
        /// Gets or sets whether if the dropped items should be select again (should keep the selection).
        /// Default is false.
        /// </summary>
        public static readonly DependencyProperty SelectDroppedItemsProperty
            = DependencyProperty.RegisterAttached("SelectDroppedItems",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets whether to show the DropTargetAdorner (DropTargetInsertionAdorner) on an empty target too.
        /// </summary>
        public static readonly DependencyProperty ShowAlwaysDropTargetAdornerProperty
            = DependencyProperty.RegisterAttached("ShowAlwaysDropTargetAdorner",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets whether if the default DragAdorner should be use.
        /// </summary>
        public static readonly DependencyProperty UseDefaultDragAdornerProperty
            = DependencyProperty.RegisterAttached("UseDefaultDragAdorner",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets whether if the default DataTemplate for the effects should be use.
        /// </summary>
        public static readonly DependencyProperty UseDefaultEffectDataTemplateProperty
            = DependencyProperty.RegisterAttached("UseDefaultEffectDataTemplate",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        /// <summary>
        /// Use descendant bounds of the VisualSourceItem as MinWidth for the DragAdorner.
        /// </summary>
        public static readonly DependencyProperty UseVisualSourceItemSizeForDragAdornerProperty
            = DependencyProperty.RegisterAttached("UseVisualSourceItemSizeForDragAdorner",
                                                  typeof(bool),
                                                  typeof(DragDropMain),
                                                  new PropertyMetadata(false));

        internal static readonly DependencyProperty IsDragLeavedProperty
                                                                                                                                                                                                                                                                                                                                                                                                                            = DependencyProperty.RegisterAttached("IsDragLeaved",
                                                          typeof(bool),
                                                          typeof(DragDropMain),
                                                          new PropertyMetadata(true));

        internal static readonly DependencyProperty IsDragOverProperty
                    = DependencyProperty.RegisterAttached("IsDragOver",
                                                          typeof(bool),
                                                          typeof(DragDropMain),
                                                          new PropertyMetadata(default(bool)));

        private static object? _clickSupressItem;

        private static DragInfo? _dragInfo;

        private static bool _dragInProgress;

        private static DragDropEffectPreview dragDropEffectPreview;

        private static DragDropPreview dragDropPreview;

        private static DropTargetAdorner dropTargetAdorner;

        /// <summary>
        /// The default data format which will be used for the drag and drop actions.
        /// </summary>
        public static DataFormat DataFormat { get; } = DataFormats.GetDataFormat("CodingDad.DragDrop.DragDrop");

        /// <summary>
        /// Gets the default DragHandler.
        /// </summary>
        public static IDragSource DefaultDragHandler { get; } = new DefaultDragHandler();

        /// <summary>
        /// Gets the default DropHandler.
        /// </summary>
        public static IDropTarget DefaultDropHandler { get; } = new DefaultDropHandler();

        /// <summary>
        /// Gets the default RootElementFinder.
        /// </summary>
        public static IRootElementFinder DefaultRootElementFinder { get; } = new RootElementFinder();

        private static DragDropEffectPreview? DragDropEffectPreview
        {
            get => dragDropEffectPreview;
            set
            {
                if (dragDropEffectPreview is { })
                {
                    dragDropEffectPreview.SetCurrentValue(Popup.PopupAnimationProperty, PopupAnimation.None);
                    dragDropEffectPreview.SetCurrentValue(Popup.IsOpenProperty, false);
                }

                dragDropEffectPreview = value;
            }
        }

        private static DragDropPreview? DragDropPreview
        {
            get => dragDropPreview;
            set
            {
                dragDropPreview?.SetCurrentValue(Popup.IsOpenProperty, false);
                dragDropPreview = value;
            }
        }

        private static DropTargetAdorner? DropTargetAdorner
        {
            get => dropTargetAdorner;
            set
            {
                dropTargetAdorner?.Detatch();
                dropTargetAdorner = value;
            }
        }

        /// <summary>Helper for getting <see cref="CanDragWithMouseRightButtonProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="CanDragWithMouseRightButtonProperty"/> from.</param>
        /// <remarks>Gets whether the control can be used as drag source together with the right mouse.</remarks>
        /// <returns>CanDragWithMouseRightButton property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetCanDragWithMouseRightButton (DependencyObject element)
        {
            return (bool)element.GetValue(CanDragWithMouseRightButtonProperty);
        }

        /// <summary>Helper for getting <see cref="DataFormatProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DataFormatProperty"/> from.</param>
        /// <remarks>Gets the data format which will be used for the drag and drop operations.</remarks>
        /// <returns>DataFormat property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataFormat GetDataFormat (DependencyObject element)
        {
            return (DataFormat)element.GetValue(DataFormatProperty);
        }

        /// <summary>Helper for getting <see cref="DefaultDragAdornerOpacityProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DefaultDragAdornerOpacityProperty"/> from.</param>
        /// <remarks>Gets the opacity of the default DragAdorner.</remarks>
        /// <returns>DefaultDragAdornerOpacity property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static double GetDefaultDragAdornerOpacity (DependencyObject element)
        {
            return (double)element.GetValue(DefaultDragAdornerOpacityProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerItemsPanelProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerItemsPanelProperty"/> from.</param>
        /// <remarks>Gets the ItemsPanel for the DragAdorner.</remarks>
        /// <returns>DragAdornerItemsPanel property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ItemsPanelTemplate GetDragAdornerItemsPanel (DependencyObject element)
        {
            return (ItemsPanelTemplate)element.GetValue(DragAdornerItemsPanelProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerMultiItemTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerMultiItemTemplateProperty"/> from.</param>
        /// <remarks>Gets the DragAdorner DataTemplate for multiple item selection.</remarks>
        /// <returns>DragAdornerMultiItemTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetDragAdornerMultiItemTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(DragAdornerMultiItemTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerMultiItemTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerMultiItemTemplateSelectorProperty"/> from.</param>
        /// <remarks>Gets the DragAdorner DataTemplateSelector for multiple item selection.</remarks>
        /// <returns>DragAdornerMultiItemTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetDragAdornerMultiItemTemplateSelector (DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(DragAdornerMultiItemTemplateSelectorProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets the DataTemplate for the DragAdorner.</remarks>
        /// <returns>DragAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetDragAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(DragAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerTemplateSelectorProperty"/> from.</param>
        /// <remarks>Gets the DataTemplateSelector for the DragAdorner.</remarks>
        /// <returns>DragAdornerTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetDragAdornerTemplateSelector (DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(DragAdornerTemplateSelectorProperty);
        }

        /// <summary>Helper for getting <see cref="DragAdornerTranslationProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragAdornerTranslationProperty"/> from.</param>
        /// <remarks>Gets the translation transform which will be used for the DragAdorner.</remarks>
        /// <returns>DragAdornerTranslation property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Point GetDragAdornerTranslation (DependencyObject element)
        {
            return (Point)element.GetValue(DragAdornerTranslationProperty);
        }

        /// <summary>Helper for getting <see cref="DragDirectlySelectedOnlyProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragDirectlySelectedOnlyProperty"/> from.</param>
        /// <remarks>Gets whether the drag action should be started only directly on a selected item.</remarks>
        /// <returns>DragDirectlySelectedOnly property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetDragDirectlySelectedOnly (DependencyObject element)
        {
            return (bool)element.GetValue(DragDirectlySelectedOnlyProperty);
        }

        /// <summary>Helper for getting <see cref="DragDropContextProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragDropContextProperty"/> from.</param>
        /// <remarks>Gets a context for a control. Only controls with the same context are allowed for drag or drop actions.</remarks>
        /// <returns>DragDropContext property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static string GetDragDropContext (DependencyObject element)
        {
            return (string)element.GetValue(DragDropContextProperty);
        }

        /// <summary>Helper for getting <see cref="DragDropCopyKeyStateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragDropCopyKeyStateProperty"/> from.</param>
        /// <remarks>Gets the copy key state which indicates the effect of the drag drop operation.</remarks>
        /// <returns>DragDropCopyKeyState property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DragDropKeyStates GetDragDropCopyKeyState (DependencyObject element)
        {
            return (DragDropKeyStates)element.GetValue(DragDropCopyKeyStateProperty);
        }

        /// <summary>Helper for getting <see cref="DragHandlerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragHandlerProperty"/> from.</param>
        /// <remarks>Gets the handler for the drag operation.</remarks>
        /// <returns>DragHandler property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDragSource GetDragHandler (DependencyObject element)
        {
            return (IDragSource)element.GetValue(DragHandlerProperty);
        }

        /// <summary>Helper for getting <see cref="DragInfoBuilderProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragInfoBuilderProperty"/> from.</param>
        /// <remarks>Gets the drag info builder for the drag operation.</remarks>
        /// <returns>DragInfoBuilder property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDragInfoBuilder GetDragInfoBuilder (DependencyObject element)
        {
            return (IDragInfoBuilder)element.GetValue(DragInfoBuilderProperty);
        }

        /// <summary>Helper for getting <see cref="DragMouseAnchorPointProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragMouseAnchorPointProperty"/> from.</param>
        /// <remarks>Gets the horizontal and vertical proportion at which the pointer will anchor on the DragAdorner.</remarks>
        /// <returns>DragMouseAnchorPoint property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Point GetDragMouseAnchorPoint (DependencyObject element)
        {
            return (Point)element.GetValue(DragMouseAnchorPointProperty);
        }

        /// <summary>Helper for getting <see cref="DragPreviewItemsSorterProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragPreviewItemsSorterProperty"/> from.</param>
        /// <remarks>Gets the drag preview items sorter handler</remarks>
        /// <returns>DragPreviewItemsSorter property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDragPreviewItemsSorter GetDragPreviewItemsSorter (DependencyObject element)
        {
            return (IDragPreviewItemsSorter)element.GetValue(DragPreviewItemsSorterProperty);
        }

        /// <summary>Helper for getting <see cref="DragPreviewMaxItemsCountProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragPreviewMaxItemsCountProperty"/> from.</param>
        /// <remarks>Gets the maximum items count which will be used for the dragged preview.</remarks>
        /// <returns>DragPreviewMaxItemsCount property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static int GetDragPreviewMaxItemsCount (DependencyObject element)
        {
            return (int)element.GetValue(DragPreviewMaxItemsCountProperty);
        }

        /// <summary>Helper for getting <see cref="DragSourceIgnoreProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DragSourceIgnoreProperty"/> from.</param>
        /// <remarks>Gets whether an element under the mouse should be ignored for the drag operation.</remarks>
        /// <returns>DragSourceIgnore property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetDragSourceIgnore (DependencyObject element)
        {
            return (bool)element.GetValue(DragSourceIgnoreProperty);
        }

        /// <summary>Helper for getting <see cref="DropAdornerItemsPanelProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropAdornerItemsPanelProperty"/> from.</param>
        /// <remarks>Gets the ItemsPanel for the DragAdorner based on the DropTarget.</remarks>
        /// <returns>DropAdornerItemsPanel property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ItemsPanelTemplate GetDropAdornerItemsPanel (DependencyObject element)
        {
            return (ItemsPanelTemplate)element.GetValue(DropAdornerItemsPanelProperty);
        }

        /// <summary>Helper for getting <see cref="DropAdornerMultiItemTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropAdornerMultiItemTemplateProperty"/> from.</param>
        /// <remarks>Gets the DropAdorner DataTemplate for multiple item selection.</remarks>
        /// <returns>DropAdornerMultiItemTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetDropAdornerMultiItemTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(DropAdornerMultiItemTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="DropAdornerMultiItemTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropAdornerMultiItemTemplateSelectorProperty"/> from.</param>
        /// <remarks>Gets the DropAdorner DataTemplateSelector for multiple item selection.</remarks>
        /// <returns>DropAdornerMultiItemTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetDropAdornerMultiItemTemplateSelector (DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(DropAdornerMultiItemTemplateSelectorProperty);
        }

        /// <summary>Helper for getting <see cref="DropAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets the DataTemplate for the DragAdorner based on the DropTarget.</remarks>
        /// <returns>DropAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetDropAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(DropAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="DropAdornerTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropAdornerTemplateSelectorProperty"/> from.</param>
        /// <remarks>Gets the DataTemplateSelector for the DragAdorner based on the DropTarget.</remarks>
        /// <returns>DropAdornerTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetDropAdornerTemplateSelector (DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(DropAdornerTemplateSelectorProperty);
        }

        /// <summary>Helper for getting <see cref="DropEventTypeProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropEventTypeProperty"/> from.</param>
        /// <remarks>Gets which type of events are subscribed for the drag and drop events.</remarks>
        /// <returns>DropEventType property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static EventType GetDropEventType (DependencyObject element)
        {
            return (EventType)element.GetValue(DropEventTypeProperty);
        }

        /// <summary>Helper for getting <see cref="DropHandlerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropHandlerProperty"/> from.</param>
        /// <remarks>Gets the handler for the drop operation.</remarks>
        /// <returns>DropHandler property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDropTarget GetDropHandler (DependencyObject element)
        {
            return (IDropTarget)element.GetValue(DropHandlerProperty);
        }

        /// <summary>Helper for getting <see cref="DropInfoBuilderProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropInfoBuilderProperty"/> from.</param>
        /// <remarks>Gets the drop info builder for the drop operation.</remarks>
        /// <returns>DropInfoBuilder property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDropInfoBuilder GetDropInfoBuilder (DependencyObject element)
        {
            return (IDropInfoBuilder)element.GetValue(DropInfoBuilderProperty);
        }

        /// <summary>Helper for getting <see cref="DropScrollingModeProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropScrollingModeProperty"/> from.</param>
        /// <remarks>Gets the ScrollingMode for the drop operation.</remarks>
        /// <returns>DropScrollingMode property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ScrollingMode GetDropScrollingMode (DependencyObject element)
        {
            return (ScrollingMode)element.GetValue(DropScrollingModeProperty);
        }

        /// <summary>Helper for getting <see cref="DropTargetAdornerBrushProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropTargetAdornerBrushProperty"/> from.</param>
        /// <remarks>Gets the brush for the DropTargetAdorner.</remarks>
        /// <returns>DropTargetAdornerBrush property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Brush GetDropTargetAdornerBrush (DependencyObject element)
        {
            return (Brush)element.GetValue(DropTargetAdornerBrushProperty);
        }

        /// <summary>Helper for getting <see cref="DropTargetAdornerPenProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropTargetAdornerPenProperty"/> from.</param>
        /// <remarks>Gets the pen for the DropTargetAdorner.</remarks>
        /// <returns>DropTargetAdornerPen property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Pen GetDropTargetAdornerPen (DependencyObject element)
        {
            return (Pen)element.GetValue(DropTargetAdornerPenProperty);
        }

        /// <summary>Helper for getting <see cref="DropTargetItemsSorterProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropTargetItemsSorterProperty"/> from.</param>
        /// <remarks>Gets the drop target items sorter handler</remarks>
        /// <returns>DropTargetItemsSorter property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IDropTargetItemsSorter GetDropTargetItemsSorter (DependencyObject element)
        {
            return (IDropTargetItemsSorter)element.GetValue(DropTargetItemsSorterProperty);
        }

        /// <summary>Helper for getting <see cref="DropTargetScrollViewerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="DropTargetScrollViewerProperty"/> from.</param>
        /// <remarks>Gets the <see cref="ScrollViewer"/> that will be used as <see cref="DropInfo.TargetScrollViewer"/>.</remarks>
        /// <returns>DropTargetScrollViewer property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ScrollViewer? GetDropTargetScrollViewer (DependencyObject element)
        {
            return element?.GetValue(DropTargetScrollViewerProperty) as ScrollViewer;
        }

        /// <summary>Helper for getting <see cref="EffectAdornerTranslationProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectAdornerTranslationProperty"/> from.</param>
        /// <remarks>Gets the translation transform which will be used for the EffectAdorner.</remarks>
        /// <returns>EffectAdornerTranslation property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Point GetEffectAdornerTranslation (DependencyObject element)
        {
            return (Point)element.GetValue(EffectAdornerTranslationProperty);
        }

        /// <summary>Helper for getting <see cref="EffectAllAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectAllAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type All.</remarks>
        /// <returns>EffectAllAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectAllAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectAllAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="EffectCopyAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectCopyAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type Copy.</remarks>
        /// <returns>EffectCopyAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectCopyAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectCopyAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="EffectLinkAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectLinkAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type Link.</remarks>
        /// <returns>EffectLinkAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectLinkAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectLinkAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="EffectMoveAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectMoveAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type Move.</remarks>
        /// <returns>EffectMoveAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectMoveAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectMoveAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="EffectNoneAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectNoneAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type None.</remarks>
        /// <returns>EffectNoneAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectNoneAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectNoneAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="EffectScrollAdornerTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EffectScrollAdornerTemplateProperty"/> from.</param>
        /// <remarks>Gets a EffectAdorner DataTemplate for effect type Scroll.</remarks>
        /// <returns>EffectScrollAdornerTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetEffectScrollAdornerTemplate (DependencyObject element)
        {
            return (DataTemplate)element.GetValue(EffectScrollAdornerTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="IsDragSourceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsDragSourceProperty"/> from.</param>
        /// <remarks>Gets whether the control can be used as drag source.</remarks>
        /// <returns>IsDragSource property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsDragSource (DependencyObject element)
        {
            return (bool)element.GetValue(IsDragSourceProperty);
        }

        /// <summary>Helper for getting <see cref="IsDropTargetProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsDropTargetProperty"/> from.</param>
        /// <remarks>Gets whether the control can be used as drop target.</remarks>
        /// <returns>IsDropTarget property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsDropTarget (DependencyObject element)
        {
            return (bool)element.GetValue(IsDropTargetProperty);
        }

        /// <summary>Helper for getting <see cref="ItemsPanelOrientationProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ItemsPanelOrientationProperty"/> from.</param>
        /// <remarks>
        /// Gets the Orientation which should be used for the drag drop action (default null).
        /// Normally it will be look up to find the correct orientation of the inner ItemsPanel,
        /// but sometimes it's necessary to force the orientation, if the look up is wrong.
        /// </remarks>
        /// <returns>ItemsPanelOrientation property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Orientation? GetItemsPanelOrientation (DependencyObject element)
        {
            return (Orientation?)element.GetValue(ItemsPanelOrientationProperty);
        }

        /// <summary>Helper for getting <see cref="MinimumHorizontalDragDistanceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="MinimumHorizontalDragDistanceProperty"/> from.</param>
        /// <remarks>Gets the minimum horizontal drag distance.</remarks>
        /// <returns>MinimumHorizontalDragDistance property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static double GetMinimumHorizontalDragDistance (DependencyObject element)
        {
            return (double)element.GetValue(MinimumHorizontalDragDistanceProperty);
        }

        /// <summary>Helper for getting <see cref="MinimumVerticalDragDistanceProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="MinimumVerticalDragDistanceProperty"/> from.</param>
        /// <remarks>Gets the minimum vertical drag distance.</remarks>
        /// <returns>MinimumVerticalDragDistance property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static double GetMinimumVerticalDragDistance (DependencyObject element)
        {
            return (double)element.GetValue(MinimumVerticalDragDistanceProperty);
        }

        /// <summary>Helper for getting <see cref="RootElementFinderProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="RootElementFinderProperty"/> from.</param>
        /// <remarks>Gets the root element finder.</remarks>
        /// <returns>RootElementFinder property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IRootElementFinder GetRootElementFinder (DependencyObject element)
        {
            return (IRootElementFinder)element.GetValue(RootElementFinderProperty);
        }

        /// <summary>Helper for getting <see cref="SelectDroppedItemsProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="SelectDroppedItemsProperty"/> from.</param>
        /// <remarks>Gets whether if the dropped items should be select again (should keep the selection).</remarks>
        /// <returns>SelectDroppedItems property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectDroppedItems (DependencyObject element)
        {
            return (bool)element.GetValue(SelectDroppedItemsProperty);
        }

        /// <summary>Helper for getting <see cref="ShowAlwaysDropTargetAdornerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ShowAlwaysDropTargetAdornerProperty"/> from.</param>
        /// <remarks>Gets whether to show the DropTargetAdorner (DropTargetInsertionAdorner) on an empty target too.</remarks>
        /// <returns>ShowAlwaysDropTargetAdorner property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetShowAlwaysDropTargetAdorner (DependencyObject element)
        {
            return (bool)element.GetValue(ShowAlwaysDropTargetAdornerProperty);
        }

        /// <summary>Helper for getting <see cref="UseDefaultDragAdornerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="UseDefaultDragAdornerProperty"/> from.</param>
        /// <remarks>Gets whether if the default DragAdorner is used.</remarks>
        /// <returns>UseDefaultDragAdorner property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetUseDefaultDragAdorner (DependencyObject element)
        {
            return (bool)element.GetValue(UseDefaultDragAdornerProperty);
        }

        /// <summary>Helper for getting <see cref="UseDefaultEffectDataTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="UseDefaultEffectDataTemplateProperty"/> from.</param>
        /// <remarks>Gets whether if the default DataTemplate for the effects should be use.</remarks>
        /// <returns>UseDefaultEffectDataTemplate property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetUseDefaultEffectDataTemplate (DependencyObject element)
        {
            return (bool)element.GetValue(UseDefaultEffectDataTemplateProperty);
        }

        /// <summary>Helper for getting <see cref="UseVisualSourceItemSizeForDragAdornerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="UseVisualSourceItemSizeForDragAdornerProperty"/> from.</param>
        /// <remarks>Gets the flag which indicates if the DragAdorner use the descendant bounds of the VisualSourceItem as MinWidth.</remarks>
        /// <returns>UseVisualSourceItemSizeForDragAdorner property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetUseVisualSourceItemSizeForDragAdorner (DependencyObject element)
        {
            return (bool)element.GetValue(UseVisualSourceItemSizeForDragAdornerProperty);
        }

        /// <summary>Helper for setting <see cref="CanDragWithMouseRightButtonProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="CanDragWithMouseRightButtonProperty"/> on.</param>
        /// <param name="value">CanDragWithMouseRightButton property value.</param>
        /// <remarks>Sets whether the control can be used as drag source together with the right mouse.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetCanDragWithMouseRightButton (DependencyObject element, bool value)
        {
            element.SetValue(CanDragWithMouseRightButtonProperty, value);
        }

        /// <summary>Helper for setting <see cref="DataFormatProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DataFormatProperty"/> on.</param>
        /// <param name="value">DataFormat property value.</param>
        /// <remarks>Sets the data format which will be used for the drag and drop operations.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDataFormat (DependencyObject element, DataFormat value)
        {
            element.SetValue(DataFormatProperty, value);
        }

        /// <summary>Helper for setting <see cref="DefaultDragAdornerOpacityProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DefaultDragAdornerOpacityProperty"/> on.</param>
        /// <param name="value">DefaultDragAdornerOpacity property value.</param>
        /// <remarks>Sets the opacity of the default DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDefaultDragAdornerOpacity (DependencyObject element, double value)
        {
            element.SetValue(DefaultDragAdornerOpacityProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerItemsPanelProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerItemsPanelProperty"/> on.</param>
        /// <param name="value">DragAdornerItemsPanel property value.</param>
        /// <remarks>Sets the ItemsPanel for the DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerItemsPanel (DependencyObject element, ItemsPanelTemplate value)
        {
            element.SetValue(DragAdornerItemsPanelProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerMultiItemTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerMultiItemTemplateProperty"/> on.</param>
        /// <param name="value">DragAdornerMultiItemTemplate property value.</param>
        /// <remarks>Sets the DragAdorner DataTemplate for multiple item selection.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerMultiItemTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(DragAdornerMultiItemTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerMultiItemTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerMultiItemTemplateSelectorProperty"/> on.</param>
        /// <param name="value">DragAdornerMultiItemTemplateSelector property value.</param>
        /// <remarks>Sets the DragAdorner DataTemplateSelector for multiple item selection.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerMultiItemTemplateSelector (DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(DragAdornerMultiItemTemplateSelectorProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerTemplateProperty"/> on.</param>
        /// <param name="value">DragAdornerTemplate property value.</param>
        /// <remarks>Sets the DataTemplate for the DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(DragAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerTemplateSelectorProperty"/> on.</param>
        /// <param name="value">DragAdornerTemplateSelector property value.</param>
        /// <remarks>Sets the DataTemplateSelector for the DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerTemplateSelector (DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(DragAdornerTemplateSelectorProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragAdornerTranslationProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragAdornerTranslationProperty"/> on.</param>
        /// <param name="value">DragAdornerTranslation property value.</param>
        /// <remarks>Sets the translation transform which will be used for the DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragAdornerTranslation (DependencyObject element, Point value)
        {
            element.SetValue(DragAdornerTranslationProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragDirectlySelectedOnlyProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragDirectlySelectedOnlyProperty"/> on.</param>
        /// <param name="value">DragDirectlySelectedOnly property value.</param>
        /// <remarks>Sets whether the drag action should be started only directly on a selected item.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragDirectlySelectedOnly (DependencyObject element, bool value)
        {
            element.SetValue(DragDirectlySelectedOnlyProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragDropContextProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragDropContextProperty"/> on.</param>
        /// <param name="value">DragDropContext property value.</param>
        /// <remarks>Sets a context for a control. Only controls with the same context are allowed for drag or drop actions.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragDropContext (DependencyObject element, string value)
        {
            element.SetValue(DragDropContextProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragDropCopyKeyStateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragDropCopyKeyStateProperty"/> on.</param>
        /// <param name="value">DragDropCopyKeyState property value.</param>
        /// <remarks>Sets the copy key state which indicates the effect of the drag drop operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragDropCopyKeyState (DependencyObject element, DragDropKeyStates value)
        {
            element.SetValue(DragDropCopyKeyStateProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragHandlerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragHandlerProperty"/> on.</param>
        /// <param name="value">DragHandler property value.</param>
        /// <remarks>Sets the handler for the drag operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragHandler (DependencyObject element, IDragSource value)
        {
            element.SetValue(DragHandlerProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragInfoBuilderProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragInfoBuilderProperty"/> on.</param>
        /// <param name="value">DragInfoBuilder property value.</param>
        /// <remarks>Sets the drag info builder for the drag operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragInfoBuilder (DependencyObject element, IDragInfoBuilder value)
        {
            element.SetValue(DragInfoBuilderProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragMouseAnchorPointProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragMouseAnchorPointProperty"/> on.</param>
        /// <param name="value">DragMouseAnchorPoint property value.</param>
        /// <remarks>Sets the horizontal and vertical proportion at which the pointer will anchor on the DragAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragMouseAnchorPoint (DependencyObject element, Point value)
        {
            element.SetValue(DragMouseAnchorPointProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragPreviewItemsSorterProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragPreviewItemsSorterProperty"/> on.</param>
        /// <param name="value">DragPreviewItemsSorter property value.</param>
        /// <remarks>Sets the handler for the drag preview items sorter</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragPreviewItemsSorter (DependencyObject element, IDragPreviewItemsSorter value)
        {
            element.SetValue(DragPreviewItemsSorterProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragPreviewMaxItemsCountProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragPreviewMaxItemsCountProperty"/> on.</param>
        /// <param name="value">DragPreviewMaxItemsCount property value.</param>
        /// <remarks>Sets the maximum items count which will be used for the dragged preview.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragPreviewMaxItemsCount (DependencyObject element, int value)
        {
            element.SetValue(DragPreviewMaxItemsCountProperty, value);
        }

        /// <summary>Helper for setting <see cref="DragSourceIgnoreProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DragSourceIgnoreProperty"/> on.</param>
        /// <param name="value">DragSourceIgnore property value.</param>
        /// <remarks>Sets whether an element under the mouse should be ignored for the drag operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDragSourceIgnore (DependencyObject element, bool value)
        {
            element.SetValue(DragSourceIgnoreProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropAdornerItemsPanelProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropAdornerItemsPanelProperty"/> on.</param>
        /// <param name="value">DropAdornerItemsPanel property value.</param>
        /// <remarks>Sets the ItemsPanel for the DragAdorner based on the DropTarget.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropAdornerItemsPanel (DependencyObject element, ItemsPanelTemplate value)
        {
            element.SetValue(DropAdornerItemsPanelProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropAdornerMultiItemTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropAdornerMultiItemTemplateProperty"/> on.</param>
        /// <param name="value">DropAdornerMultiItemTemplate property value.</param>
        /// <remarks>Sets the DropAdorner DataTemplate for multiple item selection.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropAdornerMultiItemTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(DropAdornerMultiItemTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropAdornerMultiItemTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropAdornerMultiItemTemplateSelectorProperty"/> on.</param>
        /// <param name="value">DropAdornerMultiItemTemplateSelector property value.</param>
        /// <remarks>Sets the DropAdorner DataTemplateSelector for multiple item selection.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropAdornerMultiItemTemplateSelector (DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(DropAdornerMultiItemTemplateSelectorProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropAdornerTemplateProperty"/> on.</param>
        /// <param name="value">DropAdornerTemplate property value.</param>
        /// <remarks>Sets the DataTemplate for the DragAdorner based on the DropTarget.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(DropAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropAdornerTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropAdornerTemplateSelectorProperty"/> on.</param>
        /// <param name="value">DropAdornerTemplateSelector property value.</param>
        /// <remarks>Sets the DataTemplateSelector for the DragAdorner based on the DropTarget.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropAdornerTemplateSelector (DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(DropAdornerTemplateSelectorProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropEventTypeProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropEventTypeProperty"/> on.</param>
        /// <param name="value">DropEventType property value.</param>
        /// <remarks>Sets which type of events are subscribed for the drag and drop events.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropEventType (DependencyObject element, EventType value)
        {
            element.SetValue(DropEventTypeProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropHandlerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropHandlerProperty"/> on.</param>
        /// <param name="value">DropHandler property value.</param>
        /// <remarks>Sets the handler for the drop operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropHandler (DependencyObject element, IDropTarget value)
        {
            element.SetValue(DropHandlerProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropInfoBuilderProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropInfoBuilderProperty"/> on.</param>
        /// <param name="value">DropInfoBuilder property value.</param>
        /// <remarks>Sets the drop info builder for the drop operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropInfoBuilder (DependencyObject element, IDropInfoBuilder value)
        {
            element.SetValue(DropInfoBuilderProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropScrollingModeProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropScrollingModeProperty"/> on.</param>
        /// <param name="value">DropScrollingMode property value.</param>
        /// <remarks>Sets the ScrollingMode for the drop operation.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropScrollingMode (DependencyObject element, ScrollingMode value)
        {
            element.SetValue(DropScrollingModeProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropTargetAdornerBrushProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropTargetAdornerBrushProperty"/> on.</param>
        /// <param name="value">DropTargetAdornerBrush property value.</param>
        /// <remarks>Sets the brush for the DropTargetAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropTargetAdornerBrush (DependencyObject element, Brush value)
        {
            element.SetValue(DropTargetAdornerBrushProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropTargetAdornerPenProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropTargetAdornerPenProperty"/> on.</param>
        /// <param name="value">DropTargetAdornerPen property value.</param>
        /// <remarks>Sets the pen for the DropTargetAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropTargetAdornerPen (DependencyObject element, Pen value)
        {
            element.SetValue(DropTargetAdornerPenProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropTargetItemsSorterProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropTargetItemsSorterProperty"/> on.</param>
        /// <param name="value">DropTargetItemsSorter property value.</param>
        /// <remarks>Sets the handler for the drop target items sorter</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropTargetItemsSorter (DependencyObject element, IDropTargetItemsSorter value)
        {
            element.SetValue(DropTargetItemsSorterProperty, value);
        }

        /// <summary>Helper for setting <see cref="DropTargetScrollViewerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="DropTargetScrollViewerProperty"/> on.</param>
        /// <param name="value">DropTargetScrollViewer property value.</param>
        /// <remarks>Sets the <see cref="ScrollViewer"/> that will be used as <see cref="DropInfo.TargetScrollViewer"/>.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetDropTargetScrollViewer (DependencyObject element, ScrollViewer value)
        {
            element.SetValue(DropTargetScrollViewerProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectAdornerTranslationProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectAdornerTranslationProperty"/> on.</param>
        /// <param name="value">EffectAdornerTranslation property value.</param>
        /// <remarks>Sets the translation transform which will be used for the EffectAdorner.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectAdornerTranslation (DependencyObject element, Point value)
        {
            element.SetValue(EffectAdornerTranslationProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectAllAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectAllAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectAllAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type All.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectAllAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectAllAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectCopyAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectCopyAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectCopyAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type Copy.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectCopyAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectCopyAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectLinkAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectLinkAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectLinkAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type Link.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectLinkAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectLinkAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectMoveAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectMoveAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectMoveAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type Move.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectMoveAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectMoveAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectNoneAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectNoneAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectNoneAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type None.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectNoneAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectNoneAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="EffectScrollAdornerTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="EffectScrollAdornerTemplateProperty"/> on.</param>
        /// <param name="value">EffectScrollAdornerTemplate property value.</param>
        /// <remarks>Sets a EffectAdorner DataTemplate for effect type Scroll.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetEffectScrollAdornerTemplate (DependencyObject element, DataTemplate value)
        {
            element.SetValue(EffectScrollAdornerTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="IsDragSourceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="IsDragSourceProperty"/> on.</param>
        /// <param name="value">IsDragSource property value.</param>
        /// <remarks>Sets whether the control can be used as drag source.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetIsDragSource (DependencyObject element, bool value)
        {
            element.SetValue(IsDragSourceProperty, value);
        }

        /// <summary>Helper for setting <see cref="IsDropTargetProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="IsDropTargetProperty"/> on.</param>
        /// <param name="value">IsDropTarget property value.</param>
        /// <remarks>Sets whether the control can be used as drop target.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetIsDropTarget (DependencyObject element, bool value)
        {
            element.SetValue(IsDropTargetProperty, value);
        }

        /// <summary>Helper for setting <see cref="ItemsPanelOrientationProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ItemsPanelOrientationProperty"/> on.</param>
        /// <param name="value">ItemsPanelOrientation property value.</param>
        /// <remarks>
        /// Sets the Orientation which should be used for the drag drop action (default null).
        /// Normally it will be look up to find the correct orientation of the inner ItemsPanel,
        /// but sometimes it's necessary to force the orientation, if the look up is wrong.
        /// </remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetItemsPanelOrientation (DependencyObject element, Orientation? value)
        {
            element.SetValue(ItemsPanelOrientationProperty, value);
        }

        /// <summary>Helper for setting <see cref="MinimumHorizontalDragDistanceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="MinimumHorizontalDragDistanceProperty"/> on.</param>
        /// <param name="value">MinimumHorizontalDragDistance property value.</param>
        /// <remarks>Sets the minimum horizontal drag distance.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetMinimumHorizontalDragDistance (DependencyObject element, double value)
        {
            element.SetValue(MinimumHorizontalDragDistanceProperty, value);
        }

        /// <summary>Helper for setting <see cref="MinimumVerticalDragDistanceProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="MinimumVerticalDragDistanceProperty"/> on.</param>
        /// <param name="value">MinimumVerticalDragDistance property value.</param>
        /// <remarks>Sets the minimum vertical drag distance.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetMinimumVerticalDragDistance (DependencyObject element, double value)
        {
            element.SetValue(MinimumVerticalDragDistanceProperty, value);
        }

        /// <summary>Helper for setting <see cref="RootElementFinderProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="RootElementFinderProperty"/> on.</param>
        /// <param name="value">RootElementFinder property value.</param>
        /// <remarks>Sets the root element finder.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetRootElementFinder (DependencyObject element, IRootElementFinder value)
        {
            element.SetValue(RootElementFinderProperty, value);
        }

        /// <summary>Helper for setting <see cref="SelectDroppedItemsProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="SelectDroppedItemsProperty"/> on.</param>
        /// <param name="value">SelectDroppedItems property value.</param>
        /// <remarks>Sets whether if the dropped items should be select again (should keep the selection).</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetSelectDroppedItems (DependencyObject element, bool value)
        {
            element.SetValue(SelectDroppedItemsProperty, value);
        }

        /// <summary>Helper for setting <see cref="ShowAlwaysDropTargetAdornerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ShowAlwaysDropTargetAdornerProperty"/> on.</param>
        /// <param name="value">ShowAlwaysDropTargetAdorner property value.</param>
        /// <remarks>Sets whether to show the DropTargetAdorner (DropTargetInsertionAdorner) on an empty target too.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetShowAlwaysDropTargetAdorner (DependencyObject element, bool value)
        {
            element.SetValue(ShowAlwaysDropTargetAdornerProperty, value);
        }

        /// <summary>Helper for setting <see cref="UseDefaultDragAdornerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="UseDefaultDragAdornerProperty"/> on.</param>
        /// <param name="value">UseDefaultDragAdorner property value.</param>
        /// <remarks>Sets whether if the default DragAdorner should be use.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetUseDefaultDragAdorner (DependencyObject element, bool value)
        {
            element.SetValue(UseDefaultDragAdornerProperty, value);
        }

        /// <summary>Helper for setting <see cref="UseDefaultEffectDataTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="UseDefaultEffectDataTemplateProperty"/> on.</param>
        /// <param name="value">UseDefaultEffectDataTemplate property value.</param>
        /// <remarks>Sets whether if the default DataTemplate for the effects should be use.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetUseDefaultEffectDataTemplate (DependencyObject element, bool value)
        {
            element.SetValue(UseDefaultEffectDataTemplateProperty, value);
        }

        /// <summary>Helper for setting <see cref="UseVisualSourceItemSizeForDragAdornerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="UseVisualSourceItemSizeForDragAdornerProperty"/> on.</param>
        /// <param name="value">UseVisualSourceItemSizeForDragAdorner property value.</param>
        /// <remarks>Sets the flag which indicates if the DragAdorner use the descendant bounds of the VisualSourceItem as MinWidth.</remarks>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetUseVisualSourceItemSizeForDragAdorner (DependencyObject element, bool value)
        {
            element.SetValue(UseVisualSourceItemSizeForDragAdornerProperty, value);
        }

        /// <summary>Helper for getting <see cref="IsDragLeavedProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsDragLeavedProperty"/> from.</param>
        /// <returns>IsDragLeaved property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        internal static bool GetIsDragLeaved (DependencyObject element)
        {
            return (bool)element.GetValue(IsDragLeavedProperty);
        }

        /// <summary>Helper for getting <see cref="IsDragOverProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsDragOverProperty"/> from.</param>
        /// <returns>IsDragOver property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        internal static bool GetIsDragOver (DependencyObject element)
        {
            return (bool)element.GetValue(IsDragOverProperty);
        }

        /// <summary>Helper for setting <see cref="IsDragLeavedProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="IsDragLeavedProperty"/> on.</param>
        /// <param name="value">IsDragLeaved property value.</param>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        internal static void SetIsDragLeaved (DependencyObject element, bool value)
        {
            element.SetValue(IsDragLeavedProperty, value);
        }

        /// <summary>Helper for setting <see cref="IsDragOverProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="IsDragOverProperty"/> on.</param>
        /// <param name="value">IsDragOver property value.</param>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        internal static void SetIsDragOver (DependencyObject element, bool value)
        {
            element.SetValue(IsDragOverProperty, value);
        }

        internal static ItemsPanelTemplate? TryGetDragAdornerItemsPanel (UIElement source, UIElement sender)
        {
            var itemsPanel = source is not null ? GetDragAdornerItemsPanel(source) : null;
            if (itemsPanel is null && sender is not null)
            {
                itemsPanel = GetDragAdornerItemsPanel(sender);
            }

            return itemsPanel;
        }

        internal static DataTemplate? TryGetDragAdornerMultiItemTemplate (UIElement source, UIElement sender)
        {
            var template = source is not null ? GetDragAdornerMultiItemTemplate(source) : null;
            if (template is null && sender is not null)
            {
                template = GetDragAdornerMultiItemTemplate(sender);
            }

            return template;
        }

        internal static DataTemplateSelector? TryGetDragAdornerMultiItemTemplateSelector (UIElement source, UIElement sender)
        {
            var templateSelector = source is not null ? GetDragAdornerMultiItemTemplateSelector(source) : null;
            if (templateSelector is null && sender is not null)
            {
                templateSelector = GetDragAdornerMultiItemTemplateSelector(sender);
            }

            return templateSelector;
        }

        internal static DataTemplate? TryGetDragAdornerTemplate (UIElement source, UIElement sender)
        {
            var template = source is not null ? GetDragAdornerTemplate(source) : null;
            if (template is null && sender is not null)
            {
                template = GetDragAdornerTemplate(sender);
            }

            return template;
        }

        internal static DataTemplateSelector? TryGetDragAdornerTemplateSelector (UIElement source, UIElement sender)
        {
            var templateSelector = source is not null ? GetDragAdornerTemplateSelector(source) : null;
            if (templateSelector is null && sender is not null)
            {
                templateSelector = GetDragAdornerTemplateSelector(sender);
            }

            return templateSelector;
        }

        internal static IDragPreviewItemsSorter? TryGetDragPreviewItemsSorter (IDragInfo dragInfo, UIElement sender)
        {
            var itemsSorter = dragInfo?.VisualSource != null ? GetDragPreviewItemsSorter(dragInfo.VisualSource) : null;
            if (itemsSorter is null && sender != null)
            {
                itemsSorter = GetDragPreviewItemsSorter(sender);
            }

            return itemsSorter;
        }

        internal static int TryGetDragPreviewMaxItemsCount (IDragInfo dragInfo, UIElement sender)
        {
            var itemsCount = dragInfo?.VisualSource != null ? GetDragPreviewMaxItemsCount(dragInfo.VisualSource) : -1;
            if (itemsCount < 0 && sender != null)
            {
                itemsCount = GetDragPreviewMaxItemsCount(sender);
            }

            return itemsCount < 0 || itemsCount >= int.MaxValue ? 10 : itemsCount;
        }

        internal static ItemsPanelTemplate? TryGetDropAdornerItemsPanel (UIElement source, UIElement sender)
        {
            var itemsPanel = source is not null ? GetDropAdornerItemsPanel(source) : null;
            if (itemsPanel is null && sender is not null)
            {
                itemsPanel = GetDropAdornerItemsPanel(sender);
            }

            return itemsPanel;
        }

        internal static DataTemplate? TryGetDropAdornerMultiItemTemplate (UIElement source, UIElement sender)
        {
            var template = source is not null ? GetDropAdornerMultiItemTemplate(source) : null;
            if (template is null && sender is not null)
            {
                template = GetDropAdornerMultiItemTemplate(sender);
            }

            return template;
        }

        internal static DataTemplateSelector? TryGetDropAdornerMultiItemTemplateSelector (UIElement source, UIElement sender)
        {
            var templateSelector = source is not null ? GetDropAdornerMultiItemTemplateSelector(source) : null;
            if (templateSelector is null && sender is not null)
            {
                templateSelector = GetDropAdornerMultiItemTemplateSelector(sender);
            }

            return templateSelector;
        }

        internal static DataTemplate? TryGetDropAdornerTemplate (UIElement source, UIElement sender)
        {
            var template = source is not null ? GetDropAdornerTemplate(source) : null;
            if (template is null && sender is not null)
            {
                template = GetDropAdornerTemplate(sender);
            }

            return template;
        }

        internal static DataTemplateSelector? TryGetDropAdornerTemplateSelector (UIElement source, UIElement sender)
        {
            var templateSelector = source is not null ? GetDropAdornerTemplateSelector(source) : null;
            if (templateSelector is null && sender is not null)
            {
                templateSelector = GetDropAdornerTemplateSelector(sender);
            }

            return templateSelector;
        }

        private static DataTemplate? CreateDefaultEffectDataTemplate (UIElement target, BitmapImage effectIcon, string effectText, string destinationText)
        {
            if (!GetUseDefaultEffectDataTemplate(target))
            {
                return null;
            }

            var fontSize = SystemFonts.MessageFontSize; // before 11d

            // The icon
            var imageFactory = new FrameworkElementFactory(typeof(Image));
            imageFactory.SetValue(Image.SourceProperty, effectIcon);
            imageFactory.SetValue(FrameworkElement.HeightProperty, 12d);
            imageFactory.SetValue(FrameworkElement.WidthProperty, 12d);

            // Only the icon for no effect
            if (Equals(effectIcon, IconFactory.EffectNone))
            {
                return new DataTemplate { VisualTree = imageFactory };
            }

            // Some margin for the icon
            imageFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 3, 0));
            imageFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Add effect text
            var effectTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            effectTextBlockFactory.SetValue(TextBlock.TextProperty, effectText);
            effectTextBlockFactory.SetValue(TextBlock.FontSizeProperty, fontSize);
            effectTextBlockFactory.SetValue(TextBlock.ForegroundProperty, Brushes.Blue);
            effectTextBlockFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Add destination text
            var destinationTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            destinationTextBlockFactory.SetValue(TextBlock.TextProperty, destinationText);
            destinationTextBlockFactory.SetValue(TextBlock.FontSizeProperty, fontSize);
            destinationTextBlockFactory.SetValue(TextBlock.ForegroundProperty, Brushes.DarkBlue);
            destinationTextBlockFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(3, 0, 0, 0));
            destinationTextBlockFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.DemiBold);
            destinationTextBlockFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Create containing panel
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            stackPanelFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(effectTextBlockFactory);
            stackPanelFactory.AppendChild(destinationTextBlockFactory);

            // Add border
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            var stopCollection = new GradientStopCollection
                                 {
                                     new GradientStop(Colors.White, 0.0),
                                     new GradientStop(Colors.AliceBlue, 1.0)
                                 };
            var gradientBrush = new LinearGradientBrush(stopCollection)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            borderFactory.SetValue(Panel.BackgroundProperty, gradientBrush);
            borderFactory.SetValue(Border.BorderBrushProperty, Brushes.DimGray);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(3));
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            borderFactory.SetValue(UIElement.SnapsToDevicePixelsProperty, true);
            borderFactory.SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
            borderFactory.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
            borderFactory.SetValue(TextOptions.TextHintingModeProperty, TextHintingMode.Fixed);
            borderFactory.AppendChild(stackPanelFactory);

            // Finally add content to template
            return new DataTemplate { VisualTree = borderFactory };
        }

        private static void DoDragSourceMove (object sender, Func<IInputElement, Point> getPosition)
        {
            var dragInfo = _dragInfo;
            if (dragInfo != null && !_dragInProgress)
            {
                // the start from the source
                var dragStart = dragInfo.DragStartPosition;

                // prevent selection changing while drag operation
                dragInfo.VisualSource?.ReleaseMouseCapture();

                // only if the sender is the source control and the mouse point differs from an offset
                var position = getPosition((IInputElement)sender);
                if (dragInfo.VisualSource == sender
                    && (Math.Abs(position.X - dragStart.X) > GetMinimumHorizontalDragDistance(dragInfo.VisualSource) ||
                        Math.Abs(position.Y - dragStart.Y) > GetMinimumVerticalDragDistance(dragInfo.VisualSource)))
                {
                    dragInfo.RefreshSelectedItems(sender);

                    var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);
                    if (dragHandler.CanStartDrag(dragInfo))
                    {
                        dragHandler.StartDrag(dragInfo);

                        if (dragInfo.Effects != DragDropEffects.None)
                        {
                            var dataObject = dragInfo.DataObject;

                            if (dataObject == null)
                            {
                                if (dragInfo.Data == null)
                                {
                                    // it's bad if the Data is null, cause the DataObject constructor will raise an ArgumentNullException
                                    _dragInfo = null; // maybe not necessary or should not set here to null
                                    return;
                                }

                                dataObject = new DataObject(dragInfo.DataFormat.Name, dragInfo.Data);
                            }

                            try
                            {
                                _dragInProgress = true;

                                if (DragDropPreview is null)
                                {
                                    DragDropPreview = GetDragDropPreview(dragInfo, null, sender as UIElement);
                                    DragDropPreview?.Move(getPosition(DragDropPreview.PlacementTarget));
                                }

                                MouseHelper.HookMouseMove(point =>
                                    {
                                        if (DragDropPreview?.PlacementTarget != null)
                                        {
                                            DragDropPreview.Move(DragDropPreview.PlacementTarget.PointFromScreen(point));
                                        }

                                        if (DragDropEffectPreview?.PlacementTarget != null)
                                        {
                                            DragDropEffectPreview.Move(DragDropEffectPreview.PlacementTarget.PointFromScreen(point));
                                        }
                                    });

                                var dragDropHandler = dragInfo.DragDropHandler ?? System.Windows.DragDrop.DoDragDrop;
                                var dragDropEffects = dragDropHandler(dragInfo.VisualSource, dataObject, dragInfo.Effects);
                                if (dragDropEffects == DragDropEffects.None)
                                {
                                    dragHandler.DragCancelled();
                                }

                                dragHandler.DragDropOperationFinished(dragDropEffects, dragInfo);
                            }
                            catch (Exception ex)
                            {
                                if (!dragHandler.TryCatchOccurredException(ex))
                                {
                                    throw;
                                }
                            }
                            finally
                            {
                                MouseHelper.UnHook();
                                _dragInProgress = false;
                                _dragInfo = null;
                            }
                        }
                    }
                }
            }
        }

        private static void DoMouseButtonDown (object sender, MouseButtonEventArgs e)
        {
            _dragInfo = null;

            // Ignore the click if clickCount != 1 or the user has clicked on a scrollbar.
            var elementPosition = e.GetPosition((IInputElement)sender);
            if (e.ClickCount != 1
                || (sender as UIElement).IsDragSourceIgnored()
                || (e.Source as UIElement).IsDragSourceIgnored()
                || (e.OriginalSource as UIElement).IsDragSourceIgnored()
                || GetHitTestResult(sender, elementPosition)
                || HitTestUtilities.IsNotPartOfSender(sender, e))
            {
                return;
            }

            var infoBuilder = TryGetDragInfoBuilder(sender as DependencyObject);
            var dragInfo = infoBuilder?.CreateDragInfo(sender, e.OriginalSource, e.ChangedButton, item => e.GetPosition(item))
                           ?? new DragInfo(sender, e.OriginalSource, e.ChangedButton, item => e.GetPosition(item));

            DragSourceDown(sender, dragInfo, e, elementPosition);
        }

        private static void DragSourceDown (object sender, DragInfo dragInfo, InputEventArgs e, Point elementPosition)
        {
            if (dragInfo.VisualSource is ItemsControl control && control.CanSelectMultipleItems())
            {
                control.Focus();
            }

            if (dragInfo.VisualSourceItem == null)
            {
                return;
            }

            var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);
            if (!dragHandler.CanStartDrag(dragInfo))
            {
                return;
            }

            // If the sender is a list box that allows multiple selections, ensure that clicking on an
            // already selected item does not change the selection, otherwise dragging multiple items
            // is made impossible.
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0
                //&& (Keyboard.Modifiers & ModifierKeys.Control) == 0 // #432
                && dragInfo.VisualSourceItem != null
                && sender is ItemsControl itemsControl
                && itemsControl.CanSelectMultipleItems())
            {
                var selectedItems = itemsControl.GetSelectedItems().OfType<object>().ToList();
                if (selectedItems.Count > 1 && selectedItems.Contains(dragInfo.SourceItem))
                {
                    if (!HitTestUtilities.HitTest4Type<ToggleButton>(sender, elementPosition))
                    {
                        _clickSupressItem = dragInfo.SourceItem;
                        e.Handled = true;
                    }
                }
            }

            _dragInfo = dragInfo;
        }

        private static void DragSourceOnGiveFeedback (object sender, GiveFeedbackEventArgs e)
        {
            if (DragDropEffectPreview != null)
            {
                e.UseDefaultCursors = false;
                e.Handled = true;
                if (Mouse.OverrideCursor != Cursors.Arrow)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
                if (Mouse.OverrideCursor != null)
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private static void DragSourceOnMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonDown(sender, e);
        }

        private static void DragSourceOnMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            DragSourceUp(sender, e.GetPosition((IInputElement)sender));
        }

        private static void DragSourceOnMouseMove (object sender, MouseEventArgs e)
        {
            if (_dragInfo != null && !_dragInProgress)
            {
                if (_dragInfo.MouseButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
                {
                    _dragInfo = null;
                    return;
                }

                if (GetCanDragWithMouseRightButton(_dragInfo.VisualSource)
                    && _dragInfo.MouseButton == MouseButton.Right
                    && e.RightButton == MouseButtonState.Released)
                {
                    _dragInfo = null;
                    return;
                }

                DoDragSourceMove(sender, element => e.GetPosition(element));
            }
        }

        private static void DragSourceOnMouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonDown(sender, e);
        }

        private static void DragSourceOnMouseRightButtonUp (object sender, MouseButtonEventArgs e)
        {
            DragSourceUp(sender, e.GetPosition((IInputElement)sender));
        }

        private static void DragSourceOnQueryContinueDrag (object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Cancel || e.EscapePressed || e.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton) == e.KeyStates.HasFlag(DragDropKeyStates.RightMouseButton))
            {
                DragDropPreview = null;
                DragDropEffectPreview = null;
                DropTargetAdorner = null;
                Mouse.OverrideCursor = null;
            }
        }

        private static void DragSourceOnTouchDown (object sender, TouchEventArgs e)
        {
            _dragInfo = null;

            // Ignore the click if clickCount != 1 or the user has clicked on a scrollbar.
            var elementPosition = e.GetTouchPoint((IInputElement)sender).Position;
            if ((sender as UIElement).IsDragSourceIgnored()
                || (e.Source as UIElement).IsDragSourceIgnored()
                || (e.OriginalSource as UIElement).IsDragSourceIgnored()
                || GetHitTestResult(sender, elementPosition)
                || HitTestUtilities.IsNotPartOfSender(sender, e))
            {
                return;
            }

            var infoBuilder = TryGetDragInfoBuilder(sender as DependencyObject);
            var dragInfo = infoBuilder?.CreateDragInfo(sender, e.OriginalSource, MouseButton.Left, item => e.GetTouchPoint(item).Position)
                           ?? new DragInfo(sender, e.OriginalSource, MouseButton.Left, item => e.GetTouchPoint(item).Position);

            DragSourceDown(sender, dragInfo, e, elementPosition);
        }

        private static void DragSourceOnTouchMove (object sender, TouchEventArgs e)
        {
            if (_dragInfo != null && !_dragInProgress)
            {
                // do nothing if mouse left/right button is released or the pointer is captured
                if (_dragInfo.MouseButton == MouseButton.Left && !e.TouchDevice.IsActive)
                {
                    _dragInfo = null;
                    return;
                }

                DoDragSourceMove(sender, element => e.GetTouchPoint(element).Position);
            }
        }

        private static void DragSourceOnTouchUp (object sender, TouchEventArgs e)
        {
            DragSourceUp(sender, e.GetTouchPoint((IInputElement)sender).Position);
        }

        private static void DragSourceUp (object sender, Point elementPosition)
        {
            if (HitTestUtilities.HitTest4Type<ToggleButton>(sender, elementPosition))
            {
                return;
            }

            var dragInfo = _dragInfo;

            // If we prevented the control's default selection handling in DragSource_PreviewMouseLeftButtonDown
            // by setting 'e.Handled = true' and a drag was not initiated, manually set the selection here.
            if (dragInfo?.VisualSource is ItemsControl itemsControl && _clickSupressItem != null && _clickSupressItem == dragInfo.SourceItem)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) != 0 || itemsControl is ListBox listBox && listBox.SelectionMode == SelectionMode.Multiple)
                {
                    itemsControl.SetItemSelected(dragInfo.SourceItem, false);
                }
                else if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
                {
                    itemsControl.SetSelectedItem(dragInfo.SourceItem);

                    if (sender != itemsControl && sender is ItemsControl ancestorItemsControl)
                    {
                        var ancestorItemContainer = ancestorItemsControl.ContainerFromElement(itemsControl);

                        if (ancestorItemContainer != null)
                        {
                            var ancestorItem = ancestorItemsControl.ItemContainerGenerator.ItemFromContainer(ancestorItemContainer);

                            if (ancestorItem != null)
                            {
                                ancestorItemsControl.SetSelectedItem(ancestorItem);
                            }
                        }
                    }
                }
            }

            _dragInfo = null;
            _clickSupressItem = null;
        }

        private static void DropTargetOnDragEnter (object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Bubbled, GetIsDragLeaved(sender as DependencyObject));
        }

        private static void DropTargetOnDragLeave (object sender, DragEventArgs e)
        {
            SetIsDragOver(sender as DependencyObject, false);
            DropTargetAdorner = null;

            (sender as UIElement)?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (GetIsDragOver(sender as DependencyObject) == false && GetIsDragLeaved(sender as DependencyObject) == false)
                    {
                        OnRealTargetDragLeave(sender, e);
                    }
                }));
        }

        private static void DropTargetOnDragOver (object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Bubbled, false);
        }

        private static void DropTargetOnDragOver (object sender, DragEventArgs e, EventType eventType, bool isDragEnter)
        {
            SetIsDragOver(sender as DependencyObject, true);
            SetIsDragLeaved(sender as DependencyObject, false);

            var elementPosition = e.GetPosition((IInputElement)sender);

            var dragInfo = _dragInfo;
            var dropInfoBuilder = TryGetDropInfoBuilder(sender as DependencyObject);
            var dropInfo = dropInfoBuilder?.CreateDropInfo(sender, e, dragInfo, eventType) ?? new DropInfo(sender, e, dragInfo, eventType);
            var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
            var itemsControl = dropInfo.VisualTarget;

            if (isDragEnter)
            {
                dropHandler.DragEnter(dropInfo);
            }

            dropHandler.DragOver(dropInfo);

            if (dragInfo is not null)
            {
                if (DragDropPreview is null)
                {
                    DragDropPreview = GetDragDropPreview(dragInfo, dropInfo.VisualTarget, sender as UIElement);
                    DragDropPreview?.Move(e.GetPosition(DragDropPreview.PlacementTarget));
                }

                DragDropPreview?.UpdatePreviewPresenter(dragInfo, dropInfo.VisualTarget, sender as UIElement);
            }

            Scroll(dropInfo, e);

            if (HitTestUtilities.HitTest4Type<ScrollBar>(sender, elementPosition)
                || HitTestUtilities.HitTest4GridViewColumnHeader(sender, elementPosition)
                || HitTestUtilities.HitTest4DataGridTypesOnDragOver(sender, elementPosition))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            // If the target is an ItemsControl then update the drop target adorner.
            if (itemsControl != null)
            {
                // Display the adorner in the control's ItemsPresenter. If there is no
                // ItemsPresenter provided by the style, try getting hold of a
                // ScrollContentPresenter and using that.
                UIElement adornedElement;
                if (itemsControl is TabControl)
                {
                    adornedElement = itemsControl.GetVisualDescendent<TabPanel>();
                }
                else if (itemsControl is DataGrid || (itemsControl as ListView)?.View is GridView)
                {
                    adornedElement = itemsControl.GetVisualDescendent<ScrollContentPresenter>() as UIElement ?? itemsControl.GetVisualDescendent<ItemsPresenter>() as UIElement ?? itemsControl;
                }
                else
                {
                    adornedElement = itemsControl.GetVisualDescendent<ItemsPresenter>() as UIElement ?? itemsControl.GetVisualDescendent<ScrollContentPresenter>() as UIElement ?? itemsControl;
                }

                if (adornedElement != null)
                {
                    if (dropInfo.DropTargetAdorner == null)
                    {
                        DropTargetAdorner = null;
                    }
                    else if (!dropInfo.DropTargetAdorner.IsInstanceOfType(DropTargetAdorner))
                    {
                        DropTargetAdorner = DropTargetAdorner.Create(dropInfo.DropTargetAdorner, adornedElement, dropInfo);
                    }

                    var adorner = DropTargetAdorner;
                    if (adorner != null)
                    {
                        var adornerPen = GetDropTargetAdornerPen(dropInfo.VisualTarget);
                        if (adornerPen != null)
                        {
                            adorner.Pen = adornerPen;
                        }
                        else
                        {
                            var adornerBrush = GetDropTargetAdornerBrush(dropInfo.VisualTarget);
                            if (adornerBrush != null)
                            {
                                adorner.Pen.SetCurrentValue(Pen.BrushProperty, adornerBrush);
                            }
                        }

                        adorner.DropInfo = dropInfo;
                        adorner.InvalidateVisual();
                    }
                }
            }

            // Set the drag effect adorner if there is one
            if (dragInfo != null)
            {
                if (DragDropEffectPreview is null)
                {
                    DragDropEffectPreview = GetDragDropEffectPreview(dropInfo, sender as UIElement);
                    DragDropEffectPreview?.Move(e.GetPosition(DragDropEffectPreview.PlacementTarget));
                }
                else if (DragDropEffectPreview.Effects != dropInfo.Effects || DragDropEffectPreview.EffectText != dropInfo.EffectText || DragDropEffectPreview.DestinationText != dropInfo.DestinationText)
                {
                    DragDropEffectPreview.Effects = dropInfo.Effects;
                    DragDropEffectPreview.EffectText = dropInfo.EffectText;
                    DragDropEffectPreview.DestinationText = dropInfo.DestinationText;

                    var template = GetDragDropEffectTemplate(dragInfo.VisualSource, dropInfo);
                    if (template is null)
                    {
                        DragDropEffectPreview = null;
                    }
                    else
                    {
                        ((ContentPresenter)DragDropEffectPreview.Child).SetCurrentValue(ContentPresenter.ContentTemplateProperty, template);
                    }
                }
            }

            e.Effects = dropInfo.Effects;
            e.Handled = !dropInfo.NotHandled;

            if (!dropInfo.IsSameDragDropContextAsSource)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private static void DropTargetOnDrop (object sender, DragEventArgs e)
        {
            DropTargetOnDrop(sender, e, EventType.Bubbled);
        }

        private static void DropTargetOnDrop (object sender, DragEventArgs e, EventType eventType)
        {
            var dragInfo = _dragInfo;
            var dropInfoBuilder = TryGetDropInfoBuilder(sender as DependencyObject);
            var dropInfo = dropInfoBuilder?.CreateDropInfo(sender, e, dragInfo, eventType) ?? new DropInfo(sender, e, dragInfo, eventType);
            var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
            var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);
            var itemsSorter = TryGetDropTargetItemsSorter(dropInfo, sender as UIElement);

            DragDropPreview = null;
            DragDropEffectPreview = null;
            DropTargetAdorner = null;

            dropHandler.DragOver(dropInfo);

            if (itemsSorter != null && dropInfo.Data is IEnumerable enumerable and not string)
            {
                dropInfo.Data = itemsSorter.SortDropTargetItems(enumerable);
            }

            dropHandler.Drop(dropInfo);
            dragHandler.Dropped(dropInfo);

            e.Effects = dropInfo.Effects;
            e.Handled = !dropInfo.NotHandled;

            Mouse.OverrideCursor = null;
            SetIsDragLeaved(sender as DependencyObject, true);
        }

        private static void DropTargetOnGiveFeedback (object sender, GiveFeedbackEventArgs e)
        {
            if (DragDropEffectPreview != null)
            {
                e.UseDefaultCursors = false;
                e.Handled = true;
                if (Mouse.OverrideCursor != Cursors.Arrow)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
                if (Mouse.OverrideCursor != null)
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private static void DropTargetOnPreviewDragEnter (object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Tunneled, GetIsDragLeaved(sender as DependencyObject));
        }

        private static void DropTargetOnPreviewDragOver (object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Tunneled, false);
        }

        private static void DropTargetOnPreviewDrop (object sender, DragEventArgs e)
        {
            DropTargetOnDrop(sender, e, EventType.Tunneled);
        }

        private static DragDropEffectPreview? GetDragDropEffectPreview (IDropInfo dropInfo, UIElement sender)
        {
            var dragInfo = dropInfo.DragInfo;
            var template = GetDragDropEffectTemplate(dragInfo.VisualSource, dropInfo);

            if (template != null)
            {
                var rootElement = TryGetRootElementFinder(sender).FindRoot(dropInfo.VisualTarget ?? dragInfo.VisualSource);

                var adornment = new ContentPresenter { Content = dragInfo.Data, ContentTemplate = template };

                var preview = new DragDropEffectPreview(rootElement, adornment, GetEffectAdornerTranslation(dragInfo.VisualSource), dropInfo.Effects, dropInfo.EffectText, dropInfo.DestinationText)
                {
                    IsOpen = true
                };

                return preview;
            }

            return null;
        }

        private static DataTemplate? GetDragDropEffectTemplate (UIElement target, IDropInfo dropInfo)
        {
            if (target is null)
            {
                return null;
            }

            var effectText = dropInfo.EffectText;
            var destinationText = dropInfo.DestinationText;

            return dropInfo.Effects switch
            {
                DragDropEffects.All => GetEffectAllAdornerTemplate(target), // TODO: Add default template for EffectAll
                DragDropEffects.Copy => GetEffectCopyAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectCopy, string.IsNullOrEmpty(effectText) ? "Copy to" : effectText, destinationText),
                DragDropEffects.Link => GetEffectLinkAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectLink, string.IsNullOrEmpty(effectText) ? "Link to" : effectText, destinationText),
                DragDropEffects.Move => GetEffectMoveAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectMove, string.IsNullOrEmpty(effectText) ? "Move to" : effectText, destinationText),
                DragDropEffects.None => GetEffectNoneAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectNone, string.IsNullOrEmpty(effectText) ? "None" : effectText, destinationText),
                DragDropEffects.Scroll => GetEffectScrollAdornerTemplate(target), // TODO: Add default template EffectScroll
                _ => null
            };
        }

        private static DragDropPreview? GetDragDropPreview (IDragInfo dragInfo, UIElement? visualTarget, UIElement sender)
        {
            var visualSource = dragInfo?.VisualSource;
            if (visualSource is null)
            {
                return null;
            }

            var hasDragDropPreview = DragDropPreview.HasDragDropPreview(dragInfo, visualTarget ?? visualSource, sender);
            if (hasDragDropPreview)
            {
                var rootElement = TryGetRootElementFinder(sender).FindRoot(visualTarget ?? visualSource);

                var preview = new DragDropPreview(rootElement, dragInfo, visualTarget ?? visualSource, sender);
                if (preview.Child != null)
                {
                    preview.IsOpen = true;
                    return preview;
                }
            }

            return null;
        }

        private static bool GetHitTestResult (object sender, Point elementPosition)
        {
            return sender is TabControl && !HitTestUtilities.HitTest4Type<TabPanel>(sender, elementPosition)
                   || HitTestUtilities.HitTest4Type<RangeBase>(sender, elementPosition)
                   || HitTestUtilities.HitTest4Type<TextBoxBase>(sender, elementPosition)
                   || HitTestUtilities.HitTest4Type<PasswordBox>(sender, elementPosition)
                   || HitTestUtilities.HitTest4Type<ComboBox>(sender, elementPosition)
                   || HitTestUtilities.HitTest4Type<MenuBase>(sender, elementPosition)
                   || HitTestUtilities.HitTest4GridViewColumnHeader(sender, elementPosition)
                   || HitTestUtilities.HitTest4DataGridTypes(sender, elementPosition);
        }

        private static void OnCanDragWithMouseRightButtonChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && d is UIElement uiElement)
            {
                uiElement.PreviewMouseRightButtonDown -= DragSourceOnMouseRightButtonDown;
                uiElement.PreviewMouseRightButtonUp -= DragSourceOnMouseRightButtonUp;

                if ((bool)e.NewValue)
                {
                    uiElement.PreviewMouseRightButtonDown += DragSourceOnMouseRightButtonDown;
                    uiElement.PreviewMouseRightButtonUp += DragSourceOnMouseRightButtonUp;
                }
            }
        }

        private static void OnDropEventTypeChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && d is UIElement uiElement)
            {
                if (!GetIsDropTarget(uiElement))
                {
                    return;
                }

                UnregisterDragDropEvents(uiElement, (EventType)e.OldValue);
                RegisterDragDropEvents(uiElement, (EventType)e.NewValue);
            }
        }

        private static void OnIsDragSourceChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && d is UIElement uiElement)
            {
                uiElement.PreviewMouseLeftButtonDown -= DragSourceOnMouseLeftButtonDown;
                uiElement.PreviewMouseLeftButtonUp -= DragSourceOnMouseLeftButtonUp;
                uiElement.PreviewMouseMove -= DragSourceOnMouseMove;

                uiElement.PreviewTouchDown -= DragSourceOnTouchDown;
                uiElement.PreviewTouchUp -= DragSourceOnTouchUp;
                uiElement.PreviewTouchMove -= DragSourceOnTouchMove;

                uiElement.QueryContinueDrag -= DragSourceOnQueryContinueDrag;
                uiElement.GiveFeedback -= DragSourceOnGiveFeedback;

                if ((bool)e.NewValue)
                {
                    uiElement.PreviewMouseLeftButtonDown += DragSourceOnMouseLeftButtonDown;
                    uiElement.PreviewMouseLeftButtonUp += DragSourceOnMouseLeftButtonUp;
                    uiElement.PreviewMouseMove += DragSourceOnMouseMove;

                    uiElement.PreviewTouchDown += DragSourceOnTouchDown;
                    uiElement.PreviewTouchUp += DragSourceOnTouchUp;
                    uiElement.PreviewTouchMove += DragSourceOnTouchMove;

                    uiElement.QueryContinueDrag += DragSourceOnQueryContinueDrag;
                    uiElement.GiveFeedback += DragSourceOnGiveFeedback;
                }
            }
        }

        private static void OnIsDropTargetChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && d is UIElement uiElement)
            {
                uiElement.AllowDrop = (bool)e.NewValue;

                UnregisterDragDropEvents(uiElement, GetDropEventType(d));

                if ((bool)e.NewValue)
                {
                    RegisterDragDropEvents(uiElement, GetDropEventType(d));
                }
            }
        }

        private static void OnRealTargetDragLeave (object sender, DragEventArgs e)
        {
            SetIsDragLeaved(sender as DependencyObject, true);

            var eventType = e.RoutedEvent?.RoutingStrategy switch
            {
                RoutingStrategy.Tunnel => EventType.Tunneled,
                RoutingStrategy.Bubble => EventType.Bubbled,
                _ => EventType.Auto
            };

            var dragInfo = _dragInfo;
            var dropInfoBuilder = TryGetDropInfoBuilder(sender as DependencyObject);
            var dropInfo = dropInfoBuilder?.CreateDropInfo(sender, e, dragInfo, eventType) ?? new DropInfo(sender, e, dragInfo, eventType);
            var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);

            dropHandler?.DragLeave(dropInfo);

            DragDropEffectPreview = null;
            DropTargetAdorner = null;
        }

        private static void RegisterDragDropEvents (UIElement uiElement, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Auto:
                    if (uiElement is ItemsControl)
                    {
                        // use normal events for ItemsControls
                        uiElement.DragEnter += DropTargetOnDragEnter;
                        uiElement.DragLeave += DropTargetOnDragLeave;
                        uiElement.DragOver += DropTargetOnDragOver;
                        uiElement.Drop += DropTargetOnDrop;
                        uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    }
                    else
                    {
                        // issue #85: try using preview events for all other elements than ItemsControls
                        uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                        uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                        uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                        uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                        uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    }

                    break;

                case EventType.Tunneled:
                    uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                    uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    break;

                case EventType.Bubbled:
                    uiElement.DragEnter += DropTargetOnDragEnter;
                    uiElement.DragLeave += DropTargetOnDragLeave;
                    uiElement.DragOver += DropTargetOnDragOver;
                    uiElement.Drop += DropTargetOnDrop;
                    uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    break;

                case EventType.TunneledBubbled:
                    uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                    uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    uiElement.DragEnter += DropTargetOnDragEnter;
                    uiElement.DragLeave += DropTargetOnDragLeave;
                    uiElement.DragOver += DropTargetOnDragOver;
                    uiElement.Drop += DropTargetOnDrop;
                    uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    break;

                default:
                    throw new ArgumentException("Unknown value for eventType: " + eventType.ToString(), nameof(eventType));
            }
        }

        private static void Scroll (IDropInfo dropInfo, DragEventArgs e)
        {
            if (dropInfo?.TargetScrollViewer is null)
            {
                return;
            }

            var scrollViewer = dropInfo.TargetScrollViewer;
            var scrollingMode = dropInfo.TargetScrollingMode;

            var position = e.GetPosition(scrollViewer);
            var scrollMargin = Math.Min(scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

            if (scrollingMode == ScrollingMode.Both || scrollingMode == ScrollingMode.HorizontalOnly)
            {
                if (position.X >= scrollViewer.ActualWidth - scrollMargin && scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)
                {
                    scrollViewer.LineRight();
                }
                else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0)
                {
                    scrollViewer.LineLeft();
                }
            }

            if (scrollingMode == ScrollingMode.Both || scrollingMode == ScrollingMode.VerticalOnly)
            {
                if (position.Y >= scrollViewer.ActualHeight - scrollMargin && scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)
                {
                    scrollViewer.LineDown();
                }
                else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0)
                {
                    scrollViewer.LineUp();
                }
            }
        }

        /// <summary>
        /// Gets the drag handler from the drag info or from the sender, if the drag info is null
        /// </summary>
        /// <param name="dragInfo">the drag info object</param>
        /// <param name="sender">the sender from an event, e.g. mouse down, mouse move</param>
        /// <returns></returns>
        private static IDragSource TryGetDragHandler (IDragInfo dragInfo, UIElement sender)
        {
            var dragHandler = (dragInfo?.VisualSource != null ? GetDragHandler(dragInfo.VisualSource) : null) ?? (sender != null ? GetDragHandler(sender) : null);

            return dragHandler ?? DefaultDragHandler;
        }

        /// <summary>
        /// Gets the drag info builder from the sender.
        /// </summary>
        /// <param name="sender">the sender from an event, e.g. drag over</param>
        /// <returns></returns>
        private static IDragInfoBuilder? TryGetDragInfoBuilder (DependencyObject sender)
        {
            return sender != null ? GetDragInfoBuilder(sender) : null;
        }

        /// <summary>
        /// Gets the drop handler from the drop info or from the sender, if the drop info is null
        /// </summary>
        /// <param name="dropInfo">the drop info object</param>
        /// <param name="sender">the sender from an event, e.g. drag over</param>
        /// <returns></returns>
        private static IDropTarget TryGetDropHandler (IDropInfo dropInfo, UIElement sender)
        {
            var dropHandler = (dropInfo?.VisualTarget != null ? GetDropHandler(dropInfo.VisualTarget) : null) ?? (sender != null ? GetDropHandler(sender) : null);

            return dropHandler ?? DefaultDropHandler;
        }

        /// <summary>
        /// Gets the drop info builder from the sender.
        /// </summary>
        /// <param name="sender">the sender from an event, e.g. drag over</param>
        /// <returns></returns>
        private static IDropInfoBuilder? TryGetDropInfoBuilder (DependencyObject sender)
        {
            return sender != null ? GetDropInfoBuilder(sender) : null;
        }

        private static IDropTargetItemsSorter? TryGetDropTargetItemsSorter (IDropInfo dropInfo, UIElement sender)
        {
            var itemsSorter = dropInfo?.VisualTarget != null ? GetDropTargetItemsSorter(dropInfo.VisualTarget) : null;
            if (itemsSorter is null && sender != null)
            {
                itemsSorter = GetDropTargetItemsSorter(sender);
            }

            return itemsSorter;
        }

        /// <summary>
        /// Gets the RootElementFinder from the sender or uses the default implementation, if it's null.
        /// </summary>
        /// <param name="sender">the sender from an event, e.g. drag over</param>
        /// <returns></returns>
        private static IRootElementFinder TryGetRootElementFinder (UIElement sender)
        {
            var rootElementFinder = sender != null ? GetRootElementFinder(sender) : null;

            return rootElementFinder ?? DefaultRootElementFinder;
        }

        private static void UnregisterDragDropEvents (UIElement uiElement, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Auto:
                    if (uiElement is ItemsControl)
                    {
                        // use normal events for ItemsControls
                        uiElement.DragEnter -= DropTargetOnDragEnter;
                        uiElement.DragLeave -= DropTargetOnDragLeave;
                        uiElement.DragOver -= DropTargetOnDragOver;
                        uiElement.Drop -= DropTargetOnDrop;
                        uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    }
                    else
                    {
                        // issue #85: try using preview events for all other elements than ItemsControls
                        uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                        uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                        uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                        uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                        uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    }

                    break;

                case EventType.Tunneled:
                    uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                    uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                case EventType.Bubbled:
                    uiElement.DragEnter -= DropTargetOnDragEnter;
                    uiElement.DragLeave -= DropTargetOnDragLeave;
                    uiElement.DragOver -= DropTargetOnDragOver;
                    uiElement.Drop -= DropTargetOnDrop;
                    uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                case EventType.TunneledBubbled:
                    uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                    uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    uiElement.DragEnter -= DropTargetOnDragEnter;
                    uiElement.DragLeave -= DropTargetOnDragLeave;
                    uiElement.DragOver -= DropTargetOnDragOver;
                    uiElement.Drop -= DropTargetOnDrop;
                    uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                default:
                    throw new ArgumentException("Unknown value for eventType: " + eventType.ToString(), nameof(eventType));
            }

            Mouse.OverrideCursor = null;
        }
    }
}