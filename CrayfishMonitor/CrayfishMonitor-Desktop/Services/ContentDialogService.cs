using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrayfishMonitor_Desktop.Services
{
    public static class ContentDialogService
    {
        public static async Task ShowAsync(Page page, string title, string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                CloseButtonText = "閉じる",
                XamlRoot = page.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        public static async Task ShowScrollAsync(Page page, string title, string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = new ScrollViewer()
                {
                    Content = new TextBlock() { Text = content }
                },
                CloseButtonText = "閉じる",
                XamlRoot = page.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        public static async Task ShowExceptionAsync(Page page, Exception exception)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "エラー",
                Content = exception.Message,
                CloseButtonText = "閉じる",
                XamlRoot = page.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
