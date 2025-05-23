﻿
using CodingDad.NET.Common.DragDrop.Interfaces;
using CodingDad.NET.Common.DragDrop.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CodingDad.NET.Common.DragDrop.Drag
{
    /// <summary>
    /// Holds information about a the source of a drag drop operation.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="DragInfo"/> class holds all of the framework's information about the source
    /// of a drag. It is used by <see cref="IDragSource.StartDrag"/> to determine whether a drag
    /// can start, and what the dragged data should be.
    /// </remarks>
    public class DragInfo : IDragInfo
    {
        /// <summary>
        /// Initializes a new instance of the DragInfo class.
        /// </summary>
        /// <param name="sender">The sender of the input event that initiated the drag operation.</param>
        /// <param name="originalSource">The original source of the input event.</param>
        /// <param name="mouseButton">The mouse button which was used for the drag operation.</param>
        /// <param name="getPosition">A function of the input event which is used to get drag position points.</param>
        public DragInfo (object sender, object originalSource, MouseButton mouseButton, Func<IInputElement, Point> getPosition)
        {
            MouseButton = mouseButton;
            Effects = DragDropEffects.None;
            VisualSource = sender as UIElement;
            DragStartPosition = getPosition(VisualSource);
            DragDropCopyKeyState = DragDropMain.GetDragDropCopyKeyState(VisualSource);

            var dataFormat = DragDropMain.GetDataFormat(VisualSource);
            if (dataFormat != null)
            {
                DataFormat = dataFormat;
            }

            var sourceElement = originalSource as UIElement;
            // If we can't cast object as a UIElement it might be a FrameworkContentElement, if so try and use its parent.
            if (sourceElement == null && originalSource is FrameworkContentElement frameworkContentElement)
            {
                sourceElement = frameworkContentElement.Parent as UIElement;
            }

            if (sender is ItemsControl itemsControl)
            {
                SourceGroup = itemsControl.FindGroup(DragStartPosition);
                VisualSourceFlowDirection = itemsControl.GetItemsPanelFlowDirection();

                UIElement? item = null;
                if (sourceElement != null)
                {
                    item = itemsControl.GetItemContainer(sourceElement);
                }

                if (item == null)
                {
                    var itemPosition = DragStartPosition;

                    if (DragDropMain.GetDragDirectlySelectedOnly(VisualSource))
                    {
                        item = itemsControl.GetItemContainerAt(itemPosition);
                    }
                    else
                    {
                        item = itemsControl.GetItemContainerAt(itemPosition, itemsControl.GetItemsPanelOrientation());

                        if (item.IsDragSourceIgnored())
                        {
                            item = null;
                        }
                    }
                }

                if (item != null)
                {
                    // Remember the relative position of the item being dragged
                    PositionInDraggedItem = getPosition(item);

                    var itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                    if (itemParent != null)
                    {
                        SourceCollection = itemParent.ItemsSource ?? itemParent.Items;
                        if (itemParent != itemsControl)
                        {
                            if (item is TreeViewItem tvItem)
                            {
                                var tv = tvItem.GetVisualAncestor<TreeView>();
                                if (tv != null && tv != itemsControl && !tv.IsDragSource())
                                {
                                    return;
                                }
                            }
                            else if (itemsControl.ItemContainerGenerator.IndexFromContainer(itemParent) < 0 && !itemParent.IsDragSource())
                            {
                                return;
                            }
                        }

                        SourceIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                        SourceItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    }
                    else
                    {
                        SourceIndex = -1;
                    }

                    var selectedItems = itemsControl.GetSelectedItems().OfType<object>().Where(i => i != CollectionView.NewItemPlaceholder).ToList();
                    SourceItems = selectedItems;

                    // Some controls (I'm looking at you TreeView!) haven't updated their
                    // SelectedItem by this point. Check to see if there 1 or less item in
                    // the SourceItems collection, and if so, override the control's SelectedItems with the clicked item.
                    //
                    // The control has still the old selected items at the mouse down event, so we should check this and give only the real selected item to the user.
                    if (selectedItems.Count <= 1 || SourceItem != null && !selectedItems.Contains(SourceItem))
                    {
                        SourceItems = Enumerable.Repeat(SourceItem, 1);
                    }

                    VisualSourceItem = item;
                }
                else
                {
                    SourceCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                }
            }
            else
            {
                SourceItem = (sourceElement as FrameworkElement)?.DataContext ?? (sender as FrameworkElement)?.DataContext;
                if (SourceItem != null)
                {
                    SourceItems = Enumerable.Repeat(SourceItem, 1);
                }

                VisualSourceItem = sourceElement;
                PositionInDraggedItem = sourceElement != null ? getPosition(sourceElement) : DragStartPosition;
            }

            SourceItems ??= Enumerable.Empty<object>();
        }

        /// <inheritdoc />
        public object Data { get; set; }

        /// <inheritdoc />
        public DataFormat DataFormat { get; set; } = DragDropMain.DataFormat;

        /// <inheritdoc />
        public object DataObject { get; set; }

        /// <inheritdoc />
        public DragDropKeyStates DragDropCopyKeyState { get; protected set; }

        /// <inheritdoc />
        public Func<DependencyObject, object, DragDropEffects, DragDropEffects> DragDropHandler { get; set; } = System.Windows.DragDrop.DoDragDrop;

        /// <inheritdoc />
        public Point DragStartPosition { get; protected set; }

        /// <inheritdoc />
        public DragDropEffects Effects { get; set; }

        /// <inheritdoc />
        public MouseButton MouseButton { get; protected set; }

        /// <inheritdoc />
        public Point PositionInDraggedItem { get; protected set; }

        /// <inheritdoc />
        public IEnumerable SourceCollection { get; protected set; }

        /// <inheritdoc />
        public CollectionViewGroup SourceGroup { get; protected set; }

        /// <inheritdoc />
        public int SourceIndex { get; protected set; }

        /// <inheritdoc />
        public object SourceItem { get; protected set; }

        /// <inheritdoc />
        public IEnumerable SourceItems { get; protected set; }

        /// <inheritdoc />
        public UIElement VisualSource { get; protected set; }

        /// <inheritdoc />
        public FlowDirection VisualSourceFlowDirection { get; protected set; }

        /// <inheritdoc />
        public UIElement VisualSourceItem { get; protected set; }

        internal void RefreshSelectedItems (object sender)
        {
            if (sender is not ItemsControl itemsControl)
            {
                return;
            }

            var selectedItems = itemsControl.GetSelectedItems().OfType<object>().Where(i => i != CollectionView.NewItemPlaceholder).ToList();
            SourceItems = selectedItems;

            // Some controls (I'm looking at you TreeView!) haven't updated their
            // SelectedItem by this point. Check to see if there 1 or less item in
            // the SourceItems collection, and if so, override the control's SelectedItems with the clicked item.
            //
            // The control has still the old selected items at the mouse down event, so we should check this and give only the real selected item to the user.
            if (selectedItems.Count <= 1 || SourceItem != null && !selectedItems.Contains(SourceItem))
            {
                SourceItems = Enumerable.Repeat(SourceItem, 1);
            }
        }
    }
}