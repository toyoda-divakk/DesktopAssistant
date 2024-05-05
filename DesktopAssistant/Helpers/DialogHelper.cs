using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopAssistant.Views.Popup;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.SemanticKernel;

namespace DesktopAssistant.Helpers;

/// <summary>
/// 適当だがダイアログのヘルパー
/// </summary>
public class DialogHelper
{
    /// <summary>
    /// HAL研みたいなダイアログを表示する
    /// </summary>
    /// <param name="window"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static async Task<bool> ShowDeleteDialog(Window window, string title, ContentDialogContent content)
    {
        var dialog1 = new ContentDialog
        {
            XamlRoot = window.Content.XamlRoot,
            Title = title,
            PrimaryButtonText = "Button_Delete".GetLocalized(),
            CloseButtonText = "Button_Cancel".GetLocalized(),
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };
        var result1 = await dialog1.ShowAsync();
        if (result1 == ContentDialogResult.Primary)
        {
            var dialog2 = new ContentDialog
            {
                XamlRoot = window.Content.XamlRoot,
                Title = "Message_ConfirmDelete1".GetLocalized(),
                PrimaryButtonText = "Button_Delete".GetLocalized(),
                CloseButtonText = "Button_Cancel".GetLocalized(),
                DefaultButton = ContentDialogButton.Primary,
                Content = content.CopyContent()
            };
            var result2 = await dialog2.ShowAsync();
            if (result2 == ContentDialogResult.Primary)
            {
                var dialog3 = new ContentDialog
                {
                    XamlRoot = window.Content.XamlRoot,
                    Title = "Message_ConfirmDelete2".GetLocalized(),
                    PrimaryButtonText = "Button_NoRegrets".GetLocalized(),
                    CloseButtonText = "Button_Cancel".GetLocalized(),
                    DefaultButton = ContentDialogButton.Primary,
                    Content = content.CopyContent()
                };
                var result3 = await dialog3.ShowAsync();
                if (result3 == ContentDialogResult.Primary)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
