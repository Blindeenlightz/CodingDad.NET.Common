﻿using CodingDad.NET.Common.DragDrop.Drag;
using CodingDad.NET.Common.DragDrop.Interfaces;
using System;
using System.Windows;

namespace CodingDad.NET.Common.DragDrop.Interfaces
{
    /// <summary>
    /// Interface implemented by Drag Handlers.
    /// </summary>
    public interface IDragSource
    {
        /// <summary>
        /// Determines whether this instance [can start drag] the specified drag information.
        /// With this action it's possible to check if the drag and drop operation is allowed to start
        /// e.g. check for a UIElement inside a list view item, that should not start a drag and drop operation
        /// </summary>
        /// <param name="dragInfo">Object which contains several drag information.</param>
        /// <returns><c>True</c> if the drag and drop operation is allowed.</returns>
        bool CanStartDrag (IDragInfo dragInfo);

        /// <summary>
        /// Notifies the drag handler that a drag has been aborted.
        /// </summary>
        void DragCancelled ();

        /// <summary>
        /// Notifies the drag handler that a drag and drop operation has finished.
        /// </summary>
        /// <param name="operationResult">The operation result.</param>
        /// <param name="dragInfo">Object which contains several drag information.</param>
        void DragDropOperationFinished (DragDropEffects operationResult, IDragInfo dragInfo);

        /// <summary>
        /// Notifies the drag handler that a drop has occurred.
        /// </summary>
        /// <param name="dropInfo">Object which contains several drop information.</param>
        void Dropped (IDropInfo dropInfo);

        /// <summary>
        /// Queries whether a drag can be started.
        /// </summary>
        /// <param name="dragInfo">Object which contains several drag information.</param>
        /// <remarks>
        /// To allow a drag to be started, the <see cref="DragInfo.Effects" /> property on <paramref name="dragInfo" />
        /// should be set to a value other than <see cref="DragDropEffects.None" />.
        /// </remarks>
        void StartDrag (IDragInfo dragInfo);

        /// <summary>
        /// Notifies that an exception has occurred upon dragging.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>
        /// Boolean indicating whether the exception is handled in the drag handler.
        /// False will rethrow the exception.
        /// </returns>
        bool TryCatchOccurredException (Exception exception);
    }
}