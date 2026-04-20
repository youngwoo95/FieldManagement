using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FieldManagement.Views;

namespace FieldManagement;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static readonly Uri DarkThemeUri = new("Themes/DarkTheme.xaml", UriKind.Relative);
    private static readonly Uri LightThemeUri = new("Themes/LightTheme.xaml", UriKind.Relative);

    private bool _isDarkTheme;

    public MainWindow()
    {
        InitializeComponent();
        _isDarkTheme = IsDarkThemeActive();
        MainContent.Content = new MainBoardView();
    }
    
    private void MenuRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is not RadioButton radio)
            return;

        string? menu = radio.Tag?.ToString();

        UserControl view = menu switch
        {
            "Home" => new MainBoardView(), /* MainBoardView.xaml */
            "Input" => new InputView(), /* InputView.xaml */
            "Workers" => new WorkerView(), /* LogView.xaml */
            "Data" => new DataView(), /* DataView.xaml */
            "Settings" => new SettingsView(), /* SettingsView.xaml */
            _ => new MainBoardView()
        };

        MainContent.Content = view;
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyTheme(!_isDarkTheme);
    }

    private bool IsDarkThemeActive()
    {
        var appResources = Application.Current.Resources;
        return appResources.MergedDictionaries.Any(d =>
            d.Source is not null &&
            d.Source.OriginalString.Contains("DarkTheme.xaml", StringComparison.OrdinalIgnoreCase));
    }

    private void ApplyTheme(bool useDarkTheme)
    {
        var appResources = Application.Current.Resources;
        var existingThemeDictionaries = appResources.MergedDictionaries
            .Where(d => d.Source is not null &&
                        (d.Source.OriginalString.Contains("DarkTheme.xaml", StringComparison.OrdinalIgnoreCase) ||
                         d.Source.OriginalString.Contains("LightTheme.xaml", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var dictionary in existingThemeDictionaries)
        {
            appResources.MergedDictionaries.Remove(dictionary);
        }

        appResources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = useDarkTheme ? DarkThemeUri : LightThemeUri
        });

        _isDarkTheme = useDarkTheme;
    }
}
