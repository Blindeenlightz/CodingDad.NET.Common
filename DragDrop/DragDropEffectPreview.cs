using CodingDad.DragDrop;
using System.Windows;

namespace CodingDad.DragAndDrop
{
    internal class DragDropEffectPreview : DragDropPreview
    {
        public DragDropEffectPreview (UIElement rootElement, UIElement previewElement, Point translation, DragDropEffects effects, string effectText, string destinationText)
            : base(rootElement, previewElement, translation, default)
        {
            Effects = effects;
            EffectText = effectText;
            DestinationText = destinationText;
        }

        public string DestinationText { get; set; }
        public DragDropEffects Effects { get; set; }

        public string EffectText { get; set; }
    }
}