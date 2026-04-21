using System;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

namespace FieldManagement.Behaviors;

public static class WebView2Behavior
{
    private static readonly Uri BlankUri = new("about:blank");

    public static readonly DependencyProperty SafeSourceProperty =
        DependencyProperty.RegisterAttached(
            "SafeSource",
            typeof(Uri),
            typeof(WebView2Behavior),
            new PropertyMetadata(null, OnSafeSourceChanged));

    public static void SetSafeSource(DependencyObject element, Uri? value)
    {
        element.SetValue(SafeSourceProperty, value);
    }

    public static Uri? GetSafeSource(DependencyObject element)
    {
        return (Uri?)element.GetValue(SafeSourceProperty);
    }

    private static void OnSafeSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not WebView2 webView)
        {
            return;
        }

        var newSource = e.NewValue as Uri ?? BlankUri;
        if (!Equals(webView.Source, newSource))
        {
            webView.Source = newSource;
        }
    }
}
