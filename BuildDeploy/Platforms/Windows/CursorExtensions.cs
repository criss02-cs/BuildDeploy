using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Models;
using Microsoft.Maui.Platform;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;

namespace BuildDeploy.Platforms.Windows;
public static class CursorExtensions
{
    public static void SetCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? context)
    {
        ArgumentNullException.ThrowIfNull(visualElement, nameof(visualElement));
        UIElement view = visualElement.ToPlatform(context);
        view.PointerEntered += ViewOnPointerEntered;
        view.PointerExited += ViewOnPointerExited;
        void ViewOnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            view.ChangeCursor(InputCursor.CreateFromCoreCursor(new CoreCursor(GetCursor(CursorIcon.Arrow), 1)));
        }

        void ViewOnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            view.ChangeCursor(InputCursor.CreateFromCoreCursor(new CoreCursor(GetCursor(cursor), 1)));
        }
    }
    static void ChangeCursor(this UIElement uiElement, InputCursor cursor)
    {
        Type type = typeof(UIElement);
        type.InvokeMember("ProtectedCursor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, uiElement, new object[] { cursor });
    }

    static CoreCursorType GetCursor(CursorIcon cursor)
    {
        return cursor switch
        {
            CursorIcon.Hand => CoreCursorType.Hand,
            CursorIcon.IBeam => CoreCursorType.IBeam,
            CursorIcon.Cross => CoreCursorType.Cross,
            CursorIcon.Arrow => CoreCursorType.Arrow,
            CursorIcon.SizeAll => CoreCursorType.SizeAll,
            CursorIcon.Wait => CoreCursorType.Wait,
            _ => CoreCursorType.Arrow,
        };
    }
}
