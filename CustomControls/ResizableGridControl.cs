﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodingDad.NET.Common.CustomControls
{
    /// <summary>
    /// A custom Grid control that supports dynamic row and column resizing.
    /// </summary>
    public class ResizableGridControl : Grid
    {
        private bool isColumnResizing = false;
        private bool isRowResizing = false;
        private GridLength lastColumnWidth;
        private Point lastMousePosition;
        private GridLength lastRowHeight;
        private int resizingColumnIndex = -1;
        private int resizingRowIndex = -1;

        static ResizableGridControl ()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizableGridControl), new FrameworkPropertyMetadata(typeof(ResizableGridControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableGridControl"/> class.
        /// </summary>
        public ResizableGridControl ()
        {
            Initialize();
        }

        /// <summary>
        /// Adds a new column to the grid.
        /// </summary>
        public void AddColumn ()
        {
            ColumnDefinition col = new();
            ColumnDefinitions.Add(col);
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        public void AddRow ()
        {
            RowDefinition row = new();
            RowDefinitions.Add(row);
        }

        /// <summary>
        /// Removes a column at the specified index from the grid.
        /// </summary>
        /// <param name="index">The index of the column to remove.</param>
        public void RemoveColumn (int index)
        {
            if (index >= 0 && index < ColumnDefinitions.Count)
            {
                ColumnDefinitions.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes a row at the specified index from the grid.
        /// </summary>
        /// <param name="index">The index of the row to remove.</param>
        public void RemoveRow (int index)
        {
            if (index >= 0 && index < RowDefinitions.Count)
            {
                RowDefinitions.RemoveAt(index);
            }
        }

        /// <summary>
        /// Handles the MouseDown event, initiating row or column resizing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> containing event data.</param>
        private void GridControl_MouseDown (object sender, MouseButtonEventArgs e)
        {
            double threshold = 5.0; // Threshold for how close to the edge we should be to start resizing
            lastMousePosition = e.GetPosition(this);

            // Identify which column should be resized
            double left = 0.0;
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                double right = left + ColumnDefinitions [i].ActualWidth;
                if (Math.Abs(left - lastMousePosition.X) <= threshold)
                {
                    isColumnResizing = true;
                    resizingColumnIndex = i - 1; // Resizing the column to the left of the edge
                    lastColumnWidth = ColumnDefinitions [resizingColumnIndex].Width;
                    return;
                }
                else if (Math.Abs(right - lastMousePosition.X) <= threshold)
                {
                    isColumnResizing = true;
                    resizingColumnIndex = i; // Resizing the current column
                    lastColumnWidth = ColumnDefinitions [resizingColumnIndex].Width;
                    return;
                }
                left = right;
            }

            // We don't need to check rows if a column is found to be resizing
            if (!isColumnResizing)
            {
                // Identify which row should be resized
                double top = 0.0;
                for (int i = 0; i < RowDefinitions.Count; i++)
                {
                    double bottom = top + RowDefinitions [i].ActualHeight;
                    if (Math.Abs(top - lastMousePosition.Y) <= threshold)
                    {
                        isRowResizing = true;
                        resizingRowIndex = i - 1; // Resizing the row above the edge
                        lastRowHeight = RowDefinitions [resizingRowIndex].Height;
                        return;
                    }
                    else if (Math.Abs(bottom - lastMousePosition.Y) <= threshold)
                    {
                        isRowResizing = true;
                        resizingRowIndex = i; // Resizing the current row
                        lastRowHeight = RowDefinitions [resizingRowIndex].Height;
                        return;
                    }
                    top = bottom;
                }
            }
        }

        /// <summary>
        /// Handles the MouseLeave event, cancelling row or column resizing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> containing event data.</param>
        private void GridControl_MouseLeave (object sender, MouseEventArgs e)
        {
            // We've left the grid, so cancel resizing
            // Set the selected row or column back to its original size
            if (isColumnResizing && resizingColumnIndex >= 0)
            {
                var column = ColumnDefinitions [resizingColumnIndex];
                column.Width = lastColumnWidth;
            }

            if (isRowResizing && resizingRowIndex >= 0)
            {
                var row = RowDefinitions [resizingRowIndex];
                row.Height = lastRowHeight;
            }

            ResetResizingFlags();
        }

        /// <summary>
        /// Handles the MouseMove event, performing row or column resizing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> containing event data.</param>
        private void GridControl_MouseMove (object sender, MouseEventArgs e)
        {
            if (isColumnResizing && resizingColumnIndex >= 0)
            {
                Point currentPosition = e.GetPosition(this);
                double deltaX = currentPosition.X - lastMousePosition.X;

                var column = ColumnDefinitions [resizingColumnIndex];
                column.Width = new GridLength(column.ActualWidth + deltaX);

                lastMousePosition = currentPosition;
            }

            if (isRowResizing && resizingRowIndex >= 0)
            {
                Point currentPosition = e.GetPosition(this);
                double deltaY = currentPosition.Y - lastMousePosition.Y;

                var row = RowDefinitions [resizingRowIndex];
                row.Height = new GridLength(row.ActualHeight + deltaY);

                lastMousePosition = currentPosition;
            }
        }

        /// <summary>
        /// Handles the MouseUp event, finalizing row or column resizing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> containing event data.</param>
        private void GridControl_MouseUp (object sender, MouseButtonEventArgs e)
        {
            ResetResizingFlags();
        }

        /// <summary>
        /// Initializes the control with default rows and columns and attaches event handlers.
        /// </summary>
        private void Initialize ()
        {
            // Initialize grid with default rows and columns
            RowDefinition row = new();
            RowDefinitions.Add(row);
            ColumnDefinition col = new();
            ColumnDefinitions.Add(col);

            MouseDown += GridControl_MouseDown;
            MouseMove += GridControl_MouseMove;
            MouseUp += GridControl_MouseUp;
            MouseLeave += GridControl_MouseLeave;
        }

        /// <summary>
        /// Resets the resizing flags and indices.
        /// </summary>
        private void ResetResizingFlags ()
        {
            isColumnResizing = false;
            isRowResizing = false;
            resizingColumnIndex = -1;
            resizingRowIndex = -1;
        }
    }
}
