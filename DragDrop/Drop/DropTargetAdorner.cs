﻿using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CodingDad.DragAndDrop
{
    public abstract class DropTargetAdorner : Adorner
    {
        private readonly AdornerLayer m_AdornerLayer;

        public DropTargetAdorner (UIElement adornedElement, IDropInfo dropInfo)
                    : base(adornedElement)
        {
            DropInfo = dropInfo;
            IsHitTestVisible = false;
            AllowDrop = false;
            SnapsToDevicePixels = true;
            m_AdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            m_AdornerLayer.Add(this);
        }

        public IDropInfo DropInfo { get; set; }

        /// <summary>
        /// Gets or Sets the pen which can be used for the render process.
        /// </summary>
        public Pen Pen { get; set; } = new Pen(Brushes.Gray, 2);

        public void Detatch ()
        {
            m_AdornerLayer.Remove(this);
        }

        internal static DropTargetAdorner? Create (Type type, UIElement adornedElement, IDropInfo dropInfo)
        {
            if (!typeof(DropTargetAdorner).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The requested adorner class does not derive from DropTargetAdorner.");
            }
            return type.GetConstructor(new [] { typeof(UIElement), typeof(DropInfo) })?.Invoke(new object [] { adornedElement, dropInfo }) as DropTargetAdorner;
        }
    }
}