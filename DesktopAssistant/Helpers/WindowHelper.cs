//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
// https://github.com/microsoft/WinUI-Gallery/blob/main/WinUIGallery/Helper/WindowHelper.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Windows.Storage;
using WinRT.Interop;
using Microsoft.UI.Windowing;

namespace DesktopAssistant.Helpers;
// Helper class to allow the app to find the Window that contains an
// arbitrary UIElement (GetWindowForElement).  To do this, we keep track
// of all active Windows.  The app code must call WindowHelper.CreateWindow
// rather than "new Window" so we can keep track of all the relevant
// windows.  In the future, we would like to support this in platform APIs.
// 任意のUIElementを含むWindowをアプリが見つけられるようにするためのヘルパークラスです（GetWindowForElement）。
// そのために、すべてのアクティブなウィンドウを追跡する。アプリのコードは「新しいウィンドウ」ではなく、WindowHelper.CreateWindowを呼び出す必要があります。
// 将来的には、プラットフォームAPIでこれをサポートしたい。
public class WindowHelper
{
    /// <summary>
    /// Windowを作成し、追跡を行う
    /// </summary>
    /// <returns></returns>
    public static Window CreateWindow()
    {
        var newWindow = new Window
        {
            SystemBackdrop = new MicaBackdrop()
        };
        TrackWindow(newWindow);
        return newWindow;
    }

    /// <summary>
    /// 作成したWindowに対して追跡を行う
    /// </summary>
    /// <param name="window">作成したWindow</param>
    public static void TrackWindow(Window window)
    {
        window.Closed += (sender, args) => {
            _activeWindows.Remove(window);
        };
        _activeWindows.Add(window);
    }

    /// <summary>
    /// WindowからAppWindowを取得する
    /// </summary>
    /// <param name="window"></param>
    /// <returns></returns>
    public static AppWindow GetAppWindow(Window window)
    {
        var hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    /// <summary>
    /// アプリが任意のUIElementを含むウィンドウを見つける
    /// これを行うために、すべてのアクティブなWindowsを追跡します。
    /// </summary>
    /// <param name="element">対象のUIElement</param>
    /// <returns></returns>
    public static Window? GetWindowForElement(UIElement element)
    {
        if (element.XamlRoot != null)
        {
            foreach (var window in _activeWindows)
            {
                // XamlRootが一致するWindowが探しているWindowということになる
                if (element.XamlRoot == window.Content.XamlRoot)
                {
                    return window;
                }
            }
        }
        return null;
    }
    // 要素の dpi を取得する
    public static double GetRasterizationScaleForElement(UIElement element)
    {
        if (element.XamlRoot != null)
        {
            foreach (var window in _activeWindows)
            {
                if (element.XamlRoot == window.Content.XamlRoot)
                {
                    return element.XamlRoot.RasterizationScale;
                }
            }
        }
        return 0.0;
    }

    /// <summary>
    /// 現在アクティブなウィンドウのリスト
    /// </summary>
    public static List<Window> ActiveWindows => _activeWindows;

    private static readonly List<Window> _activeWindows = new();

    public static StorageFolder GetAppLocalFolder()
    {
        StorageFolder localFolder;
        if (!NativeHelper.IsAppPackaged)
        {
            localFolder = Task.Run(async () => await StorageFolder.GetFolderFromPathAsync(System.AppContext.BaseDirectory)).Result;
        }
        else
        {
            localFolder = ApplicationData.Current.LocalFolder;
        }
        return localFolder;
    }
}
