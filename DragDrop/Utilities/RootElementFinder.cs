using CodingDad.NET.Common.DragDrop.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace CodingDad.DragAndDrop
{
    public class RootElementFinder : IRootElementFinder
    {
        public UIElement FindRoot (DependencyObject visual)
        {
            var parentWindow = Window.GetWindow(visual);
            var rootElement = parentWindow != null ? parentWindow.Content as UIElement : null;
            if (rootElement == null)
            {
                if (Application.Current != null && Application.Current.MainWindow != null)
                {
                    rootElement = Application.Current.MainWindow.Content as UIElement;
                }
                if (rootElement == null)
                {
                    rootElement = visual.GetVisualAncestor<Page>() ?? visual.GetVisualAncestor<UserControl>() as UIElement;
                }
            }

            return rootElement;
        }
    }
}