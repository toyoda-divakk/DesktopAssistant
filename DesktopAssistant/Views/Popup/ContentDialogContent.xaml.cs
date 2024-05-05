using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace DesktopAssistant.Views.Popup;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ContentDialogContent : Page
{
    private readonly string _text1;
    private readonly string _text2;
    public ContentDialogContent(string text1, string text2)
    {
        InitializeComponent();
        Text_Content1.Text = text1;
        Text_Content2.Text = text2;

        _text1 = text1;  // 外からアクセスできるようにする
        _text2 = text2;
    }

    public ContentDialogContent CopyContent()
    {
        return new ContentDialogContent(_text1, _text2);
    }
}
