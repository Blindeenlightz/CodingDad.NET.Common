﻿
using CodingDad.NET.Common.DragDrop.Enums;
using CodingDad.NET.Common.DragDrop.Interfaces;
using CodingDad.NET.Common.DragDrop.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CodingDad.NET.Common.DragDrop.Drop
{
    [Flags]
    public enum RelativeInsertPosition
    {
        None = 0,
        BeforeTargetItem = 1,
        AfterTargetItem = 2,
        TargetItemCenter = 4
    }

    /// <summary>
    /// Holds information about a the target of a drag drop operation.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="DropInfo"/> class holds all of the framework's information about the current
    /// target of a drag. It is used by <see cref="IDropTarget.DragOver"/> method to determine whether
    /// the current drop target is valid, and by <see cref="IDropTarget.Drop"/> to perform the drop.
    /// </remarks>
    public class DropInfo : IDropInfo
    {
        private readonly ItemsControl itemParent;

        /// <summary>
        /// Initializes a new instance of the DropInfo class.
        /// </summary>
        /// <param name="sender">The sender of the drop event.</param>
        /// <param name="e">The drag event arguments.</param>
        /// <param name="dragInfo">Information about the drag source, if the drag came from within the framework.</param>
        /// <param name="eventType">The type of the underlying event (tunneled or bubbled).</param>
        public DropInfo (object sender, DragEventArgs e, IDragInfo dragInfo, EventType eventType)
        {
            DragInfo = dragInfo;
            KeyStates = e.KeyStates;
            EventType = eventType;
            var dataFormat = dragInfo?.DataFormat;
            Data = dataFormat != null && e.Data.GetDataPresent(dataFormat.Name) ? e.Data.GetData(dataFormat.Name) : e.Data;

            VisualTarget = sender as UIElement;
            // if there is no drop target, find another
            if (!VisualTarget.IsDropTarget())
            {
                // try to find next element
                var element = VisualTarget.TryGetNextAncestorDropTargetElement();
                if (element != null)
                {
                    VisualTarget = element;
                }
            }

            // try find ScrollViewer
            var dropTargetScrollViewer = DragDropMain.GetDropTargetScrollViewer(VisualTarget);
            if (dropTargetScrollViewer != null)
            {
                TargetScrollViewer = dropTargetScrollViewer;
            }
            else if (VisualTarget is TabControl)
            {
                var tabPanel = VisualTarget.GetVisualDescendent<TabPanel>();
                TargetScrollViewer = tabPanel?.GetVisualAncestor<ScrollViewer>();
            }
            else
            {
                TargetScrollViewer = VisualTarget?.GetVisualDescendent<ScrollViewer>();
            }

            TargetScrollingMode = VisualTarget != null ? DragDropMain.GetDropScrollingMode(VisualTarget) : ScrollingMode.Both;

            // visual target can be null, so give us a point...
            DropPosition = VisualTarget != null ? e.GetPosition(VisualTarget) : new Point();

            if (VisualTarget is TabControl)
            {
                if (!HitTestUtilities.HitTest4Type<TabPanel>(VisualTarget, DropPosition))
                {
                    return;
                }
            }

            if (VisualTarget is ItemsControl itemsControl)
            {
                //System.Diagnostics.Debug.WriteLine(">>> Name = {0}", itemsControl.Name);

                // get item under the mouse
                var item = itemsControl.GetItemContainerAt(DropPosition);
                var directlyOverItem = item != null;

                TargetGroup = itemsControl.FindGroup(DropPosition);
                VisualTargetOrientation = itemsControl.GetItemsPanelOrientation();
                VisualTargetFlowDirection = itemsControl.GetItemsPanelFlowDirection();

                if (item == null)
                {
                    // ok, no item found, so maybe we can found an item at top, left, right or bottom
                    item = itemsControl.GetItemContainerAt(DropPosition, VisualTargetOrientation);
                    directlyOverItem = DropPosition.DirectlyOverElement(item, itemsControl);
                }

                if (item == null && TargetGroup is { IsBottomLevel: true })
                {
                    var itemData = TargetGroup.Items.FirstOrDefault();
                    if (itemData != null)
                    {
                        item = itemsControl.ItemContainerGenerator.ContainerFromItem(itemData) as UIElement;
                        directlyOverItem = DropPosition.DirectlyOverElement(item, itemsControl);
                    }
                }

                if (item != null)
                {
                    itemParent = ItemsControl.ItemsControlFromItemContainer(item);
                    VisualTargetOrientation = itemParent.GetItemsPanelOrientation();
                    VisualTargetFlowDirection = itemParent.GetItemsPanelFlowDirection();

                    InsertIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                    TargetCollection = itemParent.ItemsSource ?? itemParent.Items;

                    var tvItem = item as TreeViewItem;

                    if (directlyOverItem || tvItem != null)
                    {
                        VisualTargetItem = item;
                        TargetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    }

                    var tvItemIsExpanded = tvItem is { HasHeader: true, HasItems: true, IsExpanded: true };
                    var itemRenderSize = tvItemIsExpanded ? tvItem.GetHeaderSize() : item.RenderSize;

                    if (VisualTargetOrientation == Orientation.Vertical)
                    {
                        var currentYPos = e.GetPosition(item).Y;
                        var targetHeight = itemRenderSize.Height;

                        var topGap = targetHeight * 0.25;
                        var bottomGap = targetHeight * 0.75;
                        if (currentYPos > targetHeight / 2)
                        {
                            if (tvItemIsExpanded && (currentYPos < topGap || currentYPos > bottomGap))
                            {
                                VisualTargetItem = tvItem.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
                                TargetItem = VisualTargetItem != null ? tvItem.ItemContainerGenerator.ItemFromContainer(VisualTargetItem) : null;
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = 0;
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                            else
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                        }
                        else
                        {
                            InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                        }

                        if (currentYPos > topGap && currentYPos < bottomGap)
                        {
                            if (tvItem != null)
                            {
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = TargetCollection != null ? TargetCollection.OfType<object>().Count() : 0;
                            }

                            InsertPosition |= RelativeInsertPosition.TargetItemCenter;
                        }

                        // System.Diagnostics.Debug.WriteLine($"==> DropInfo: pos={this.InsertPosition}, index={this.InsertIndex}, currentXPos={currentXPos}, Item={this.item}");
                    }
                    else
                    {
                        var currentXPos = e.GetPosition(item).X;
                        var targetWidth = itemRenderSize.Width;

                        if (VisualTargetFlowDirection == FlowDirection.RightToLeft)
                        {
                            if (currentXPos < targetWidth / 2)
                            {
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                            else
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                        }
                        else if (VisualTargetFlowDirection == FlowDirection.LeftToRight)
                        {
                            if (currentXPos > targetWidth / 2)
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                            else
                            {
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                        }

                        if (currentXPos > targetWidth * 0.25 && currentXPos < targetWidth * 0.75)
                        {
                            if (tvItem != null)
                            {
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = TargetCollection != null ? TargetCollection.OfType<object>().Count() : 0;
                            }

                            InsertPosition |= RelativeInsertPosition.TargetItemCenter;
                        }

                        // System.Diagnostics.Debug.WriteLine($"==> DropInfo: pos={this.InsertPosition}, index={this.InsertIndex}, currentXPos={currentXPos}, Item={this.item}");
                    }
                }
                else
                {
                    TargetCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                    InsertIndex = itemsControl.Items.Count;
                    //System.Diagnostics.Debug.WriteLine("==> DropInfo: pos={0}, item=NULL, idx={1}", this.InsertPosition, this.InsertIndex);
                }
            }
            else
            {
                VisualTargetItem = VisualTarget;
            }
        }

        /// <inheritdoc />
        public object Data { get; set; }

        /// <inheritdoc />
        public string DestinationText { get; set; }

        /// <inheritdoc />
        public IDragInfo DragInfo { get; protected set; }

        /// <inheritdoc />
        public Point DropPosition { get; protected set; }

        /// <inheritdoc />
        public Type DropTargetAdorner { get; set; }

        /// <inheritdoc />
        public DragDropEffects Effects { get; set; }

        /// <inheritdoc />
        public string EffectText { get; set; }

        /// <inheritdoc />
        public EventType EventType { get; }

        /// <inheritdoc />
        public int InsertIndex { get; protected set; }

        /// <inheritdoc />
        public RelativeInsertPosition InsertPosition { get; protected set; }

        /// <inheritdoc />
        public bool IsSameDragDropContextAsSource
        {
            get
            {
                // Check if DragInfo stuff exists
                if (DragInfo?.VisualSource is null)
                {
                    return true;
                }

                // A target should be exists
                if (VisualTarget is null)
                {
                    return true;
                }

                // Source element has a drag context constraint, we need to check the target property matches.
                var sourceContext = DragDropMain.GetDragDropContext(DragInfo.VisualSource);
                var targetContext = DragDropMain.GetDragDropContext(VisualTarget);

                return string.Equals(sourceContext, targetContext)
                       || string.IsNullOrEmpty(targetContext);
            }
        }

        /// <inheritdoc />
        public DragDropKeyStates KeyStates { get; protected set; }

        /// <inheritdoc />
        public bool NotHandled { get; set; }

        /// <inheritdoc />
        public IEnumerable TargetCollection { get; protected set; }

        /// <inheritdoc />
        public CollectionViewGroup TargetGroup { get; protected set; }

        /// <inheritdoc />
        public object TargetItem { get; protected set; }

        /// <summary>
        /// Gets or Sets the ScrollingMode for the drop action.
        /// </summary>
        public ScrollingMode TargetScrollingMode { get; set; }

        /// <summary>
        /// Gets the ScrollViewer control for the visual target.
        /// </summary>
        public ScrollViewer TargetScrollViewer { get; protected set; }

        /// <inheritdoc />
        public int UnfilteredInsertIndex
        {
            get
            {
                var insertIndex = InsertIndex;
                if (itemParent is null)
                {
                    return insertIndex;
                }

                var itemSourceAsList = itemParent.ItemsSource.TryGetList();
                if (itemSourceAsList != null && itemParent.Items != null && itemParent.Items.Count != itemSourceAsList.Count)
                {
                    if (insertIndex >= 0 && insertIndex < itemParent.Items.Count)
                    {
                        var indexOf = itemSourceAsList.IndexOf(itemParent.Items [insertIndex]);
                        if (indexOf >= 0)
                        {
                            return indexOf;
                        }
                    }
                    else if (itemParent.Items.Count > 0 && insertIndex == itemParent.Items.Count)
                    {
                        var indexOf = itemSourceAsList.IndexOf(itemParent.Items [insertIndex - 1]);
                        if (indexOf >= 0)
                        {
                            return indexOf + 1;
                        }
                    }
                }

                return insertIndex;
            }
        }

        /// <inheritdoc />
        public UIElement VisualTarget { get; protected set; }

        /// <inheritdoc />
        public FlowDirection VisualTargetFlowDirection { get; protected set; }

        /// <inheritdoc />
        public UIElement VisualTargetItem { get; protected set; }

        /// <inheritdoc />
        public Orientation VisualTargetOrientation { get; protected set; }
    }
}