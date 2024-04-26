using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BuildDeploy.WinUI;
public class WinUiWindow : Window
{
    static WinUiWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WinUiWindow), new FrameworkPropertyMetadata(typeof(WinUiWindow)));
    }

    public void ShowAlert(string text, string cancelText = "OK")
    {
        if (Template.FindName("Alert", this) is not Modal modal) return;
        if (Template.FindName("AlertText", this) is TextBlock textBlock) textBlock.Text = text;
        if (Template.FindName("AlertButton", this) is Button button)
        {
            button.Content = cancelText;
            button.Click += (s, e) => modal.IsOpen = false;
        }
        modal.IsOpen = true;
    }


    public Task<bool> ShowConfirmAsync(string title, string text, string confirmText = "Yes", string cancelText = "No")
    {
        if (Template.FindName("ConfirmAlert", this) is not Modal modal) throw new Exception("Non è presente una modale");
        var task = new TaskCompletionSource<bool>();
        if (Template.FindName("ConfirmTitle", this) is TextBlock titleBlock) titleBlock.Text = title;
        if (Template.FindName("ConfirmText", this) is TextBlock textBlock) textBlock.Text = text;
        if (Template.FindName("ConfirmButton", this) is Button confirmButton)
        {
            confirmButton.Content = confirmText;
            EventHandler callback = null;
            callback += (s, e) =>
            {
                modal.IsOpen = false;
                confirmButton.Click -= callback;
                task.TrySetResult(true);
            };
            confirmButton.Click += callback;
        }
        if (Template.FindName("CancelButton", this) is Button cancelButton)
        {
            cancelButton.Content = cancelText;
            EventHandler callback = null;
            callback += (s, e) =>
            {
                modal.IsOpen = false;
                cancelButton.Click -= callback;
                task.TrySetResult(false);
            };
            cancelButton.Click += callback;
        }

        modal.IsOpen = true;
        return task.Task;
    }

    private void CloseConfirmDialog(object? sender, EventArgs e)
    {

    }

}

public class ResulModalEventArgs : EventArgs
{
    public bool Result { get; set; }
}
