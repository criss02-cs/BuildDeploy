using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Models;

namespace BuildDeploy.Utils;
#if WINDOWS
using BuildDeploy.Platforms.Windows;
#endif


public class CursorBehavior
{
    public static readonly BindableProperty CursorProperty = BindableProperty.CreateAttached("Cursor", typeof(CursorIcon), typeof(CursorBehavior), CursorIcon.Arrow, propertyChanged: CursorChanged);

    private static void CursorChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is VisualElement visualElement)
        {
            visualElement.SetCursor((CursorIcon)newvalue, Application.Current?.MainPage?.Handler?.MauiContext);
        }
    }

    public static CursorIcon GetCursor(BindableObject view) => (CursorIcon)view.GetValue(CursorProperty);

    public static void SetCursor(BindableObject view, CursorIcon value) => view.SetValue(CursorProperty, value);
}