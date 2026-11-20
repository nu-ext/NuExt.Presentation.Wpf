using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Presentation.Wpf.Diagnostics;

/// <summary>
/// A custom <see cref="TraceListener"/> that captures binding errors and displays them in a message box.
/// This listener is particularly useful for debugging purposes and identifying binding issues in WPF applications.
/// </summary>
/// <example>
/// To use the <see cref="BindingErrorTraceListener"/>, add the following code to the startup of your WPF application,
/// typically in the `App.xaml.cs` file or in the initialization logic of your main window:
/// <code>
/// public partial class App : Application
/// {
///     protected override void OnStartup(StartupEventArgs e)
///     {
///         base.OnStartup(e);
///
///         // Enable tracing for data binding errors
///         PresentationTraceSources.Refresh();
///         PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
///
///         // Ignore the transient errors produced by third-party controls while the visual tree is re-templated
///         PresentationTraceSources.DataBindingSource.Listeners.Add(
///             new BindingErrorTraceListener("AncestorType='AvalonDock.ToggleDockingManager'"));
///     }
/// }
/// </code>
/// </example>
public sealed class BindingErrorTraceListener : TraceListener
{
    private readonly StringBuilder _messageBuilder = new();
    private readonly string[] _ignoredFragments;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of the <see cref="BindingErrorTraceListener"/> class.
    /// </summary>
    /// <param name="ignoredFragments">
    /// Fragments of messages that must not be reported. A message is ignored when it contains any of the
    /// specified fragments (ordinal comparison), which is handy for the transient binding errors produced by
    /// third-party controls while the visual tree is being re-templated. <see langword="null"/>, empty and
    /// whitespace fragments are skipped, duplicates are removed preserving the first occurrence.
    /// </param>
    public BindingErrorTraceListener(params ReadOnlySpan<string> ignoredFragments)
    {
        _ignoredFragments = CreateIgnoredFragments(ignoredFragments);
    }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="BindingErrorTraceListener"/> class.
    /// </summary>
    /// <param name="ignoredFragments">
    /// Fragments of messages that must not be reported. A message is ignored when it contains any of the
    /// specified fragments (ordinal comparison), which is handy for the transient binding errors produced by
    /// third-party controls while the visual tree is being re-templated. <see langword="null"/>, empty and
    /// whitespace fragments are skipped, duplicates are removed preserving the first occurrence.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ignoredFragments"/> is <see langword="null"/>.</exception>
    public BindingErrorTraceListener(params string[] ignoredFragments)
    {
        _ignoredFragments = CreateIgnoredFragments(
            ignoredFragments ?? throw new ArgumentNullException(nameof(ignoredFragments)));
    }
#endif

    /// <inheritdoc/>
    public override void Write(string? message)
    {
        _messageBuilder.Append(message);
    }

    /// <inheritdoc/>
    public override void WriteLine(string? message)
    {
        Write(message);

        string text = _messageBuilder.ToString();
        _messageBuilder.Clear();

        if (IsIgnored(text))
        {
            return;
        }

        MessageBox.Show(text, "Binding error", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    private static string[] CreateIgnoredFragments(ReadOnlySpan<string> fragments)
    {
        if (fragments.IsEmpty)
        {
            return [];
        }

        var result = new List<string>(fragments.Length);
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (string fragment in fragments)
        {
            if (!string.IsNullOrWhiteSpace(fragment) && seen.Add(fragment))
            {
                result.Add(fragment);
            }
        }

        return [.. result];
    }
#else
    private static string[] CreateIgnoredFragments(string[] fragments)
    {
        if (fragments.Length == 0)
        {
            return [];
        }

        var result = new List<string>(fragments.Length);
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (string fragment in fragments)
        {
            if (!string.IsNullOrWhiteSpace(fragment) && seen.Add(fragment))
            {
                result.Add(fragment);
            }
        }

        return [.. result];
    }
#endif

    private bool IsIgnored(string message)
    {
        foreach (string fragment in _ignoredFragments)
        {
            if (message.Contains(fragment, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
