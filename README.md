# NuExt.Presentation.Wpf

`NuExt.Presentation.Wpf` is a lightweight, **WPF-focused presentation library** providing XAML helpers, control extensions, data-binding utilities, diagnostics, and templating helpers. It enables clean, reusable presentation code across WPF applications with minimal dependencies.

[![NuGet](https://img.shields.io/nuget/v/NuExt.Presentation.Wpf.svg)](https://www.nuget.org/packages/NuExt.Presentation.Wpf)
[![Build](https://github.com/nu-ext/NuExt.Presentation.Wpf/actions/workflows/ci.yml/badge.svg)](https://github.com/nu-ext/NuExt.Presentation.Wpf/actions/workflows/ci.yml)
[![License](https://img.shields.io/github/license/nu-ext/NuExt.Presentation.Wpf?label=license)](https://github.com/nu-ext/NuExt.Presentation.Wpf/blob/main/LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/NuExt.Presentation.Wpf.svg)](https://www.nuget.org/packages/NuExt.Presentation.Wpf)

## Highlights

- **Control extensions** — `TreeViewItemHelper`, `ComboBoxExtensions`, `VirtualizingStackPanelExtensions`, `ControlExtensions` for common WPF control operations.
- **Binding helpers** — `BindingProxy` and `BindingProxy<T>` to pass DataContext across templates and containers without direct binding paths.
- **Framework extensions** — `FrameworkElementExtensions`, `DependencyObjectExtensions`, `WindowExtensions` for element and window operations.
- **Window placement** — `WindowPlacement` to save/restore window position, size, and state with minimal code.
- **Diagnostics** — `BindingErrorTraceListener` to capture and display WPF binding errors in debug builds.
- **Templating** — `ComboBoxItemTemplateSelector` and reusable XAML styles (TreeViewItem, etc.).
- **Zero heavy dependencies** — only depends on `NuExt.System` and WPF framework types.

### Compatibility

WPF on **.NET 8/9/10** and **.NET Framework 4.6.2+**. Platform-independent usage patterns suitable for any MVVM framework.

## Quick Start

### Binding across templates with BindingProxy

```xml
<Window.Resources>
    <local:BindingProxy x:Key="proxy" DataContext="{Binding SomeViewModel}" />
</Window.Resources>

<Grid DataContext="{Binding Source={StaticResource proxy}, Path=DataContext}">
    <!-- content can now access SomeViewModel directly -->
</Grid>
```

### Enable binding error diagnostics

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    PresentationTraceSources.Refresh();
    PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
    PresentationTraceSources.DataBindingSource.Listeners.Add(new BindingErrorTraceListener());
}
```

### Save/restore window placement

```csharp
var placement = WindowPlacement.GetPlacement(windowHandle);
// ... later, restore
WindowPlacement.SetPlacement(windowHandle, placement);
```

### Find TreeViewItem by data item

```csharp
var treeView = ...;
var item = treeView.GetTreeViewItem(myDataItem);
if (item != null) item.IsSelected = true;
```

## Why NuExt.Presentation.Wpf?

- **Reusable** — proven presentation patterns you can share across projects.
- **Minimal** — no bloat, no heavy frameworks; pure WPF and .NET goodness.
- **Fast** — efficient extensions with careful attention to performance.
- **Debuggable** — built-in diagnostics and clean, easy-to-follow code.

## Installation

Via [NuGet](https://www.nuget.org/):

```sh
dotnet add package NuExt.Presentation.Wpf
```

Or via Visual Studio:

1. Go to `Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...`.
2. Search for `NuExt.Presentation.Wpf`.
3. Click "Install".

## Ecosystem

- [NuExt.Minimal.Behaviors.Wpf](https://github.com/nu-ext/NuExt.Minimal.Behaviors.Wpf)
- [NuExt.Minimal.Mvvm.SourceGenerator](https://github.com/nu-ext/NuExt.Minimal.Mvvm.SourceGenerator)
- [NuExt.Minimal.Mvvm.Wpf](https://github.com/nu-ext/NuExt.Minimal.Mvvm.Wpf)
- [NuExt.Minimal.Mvvm.MahApps.Metro](https://github.com/nu-ext/NuExt.Minimal.Mvvm.MahApps.Metro)
- [NuExt.System](https://github.com/nu-ext/NuExt.System)
- [NuExt.System.Data](https://github.com/nu-ext/NuExt.System.Data)
- [NuExt.System.Data.SQLite](https://github.com/nu-ext/NuExt.System.Data.SQLite)

## Contributing

Issues and PRs are welcome. Keep changes minimal and performance-conscious.

## License

MIT. See LICENSE.