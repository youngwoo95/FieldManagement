using System.Windows;

namespace FieldManagement.Services;

public class ThemeService
{
    private static readonly Uri DarkThemeUri = new("Themes/DarkTheme.xaml", UriKind.Relative);
    private static readonly Uri LightThemeUri = new("Themes/LightTheme.xaml", UriKind.Relative);

    public bool IsDarkThemeActive()
    {
        var appResources = Application.Current.Resources;

        return appResources.MergedDictionaries.Any(d =>
            d.Source is not null &&
            d.Source.OriginalString.Contains("DarkTheme.xaml", StringComparison.OrdinalIgnoreCase));
    }

    public void ApplyTheme(bool useDarkTheme)
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
    }
}