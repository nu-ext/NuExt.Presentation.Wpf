using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace Presentation.Wpf.Controls;

public static class VirtualizingStackPanelExtensions
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> s_bringIndexIntoViewMethods = new();

    static VirtualizingStackPanelExtensions()
    {

    }

    /// <summary>
    /// Publically expose BringIndexIntoView.
    /// </summary>
    public static void BringIntoView(this VirtualizingStackPanel virtualizingPanel, int index)
    {
        ArgumentNullException.ThrowIfNull(virtualizingPanel);

        var method = s_bringIndexIntoViewMethods.GetOrAdd(virtualizingPanel.GetType(),
            static type => type.GetMethod("BringIndexIntoView", BindingFlags.NonPublic | BindingFlags.Instance)
                           ?? throw new MissingMethodException(type.FullName, "BringIndexIntoView"));

        //virtualizingPanel.BringIndexIntoView(index);
        method.Invoke(virtualizingPanel, [index]);//TODO to delegate
    }
}