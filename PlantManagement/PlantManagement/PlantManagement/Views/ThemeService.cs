using System.Windows;

namespace PlantManagement.Views;

public class ThemeService
{
    private static readonly Uri _darkThemeUri = new("Comm/Themes/DarkTheme.xaml", UriKind.Relative);
    private static readonly Uri _lightThemeUri = new("comm/Themes/LightTheme.xaml", UriKind.Relative);

    public void ApplyTheme(bool useDarkMode)
    {
        var appResources = Application.Current.Resources;

        var themeResoure = appResources.MergedDictionaries
            .Where(d => d.Source is not null &&
                        (d.Source.OriginalString.Contains("DarkTheme.xaml", StringComparison.OrdinalIgnoreCase) ||
                         d.Source.OriginalString.Contains("LightTheme.xaml", StringComparison.OrdinalIgnoreCase)))
            .ToList();
        
        foreach (var item in themeResoure)
        {
            appResources.MergedDictionaries.Remove(item);
        }
        
        appResources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = useDarkMode ? _darkThemeUri : _lightThemeUri
        });
    }
    
}