using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class FacilityViewModel : BaseViewModel
{
    private readonly IFacilityDialogService _facilityDialogService;
    private readonly ObservableCollection<FacilityModel> _allFacilities = new();

    private string _nameKeyword = string.Empty;
    public string NameKeyword
    {
        get => _nameKeyword;
        set
        {
            _nameKeyword = value;
            OnPropertyChanged();
        }
    }

    private string _makerKeyword = string.Empty;
    public string MakerKeyword
    {
        get => _makerKeyword;
        set
        {
            _makerKeyword = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<FacilityModel> _facilityItem = new();
    public ObservableCollection<FacilityModel> FacilityItem
    {
        get => _facilityItem;
        set
        {
            _facilityItem = value;
            OnPropertyChanged();
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public FacilityViewModel(IFacilityDialogService facilityDialogService)
    {
        _facilityDialogService = facilityDialogService;

        SearchCommand = new RelayCommand(_ => Search());
        ResetCommand = new RelayCommand(_ => Reset());
        AddCommand = new RelayCommand(_ => Add());

        LoadFacility();
        Search();
    }

    private void LoadFacility()
    {
        _allFacilities.Clear();
        _allFacilities.Add(new FacilityModel
        {
            IsChecked = false,
            Name = "Driller Machine",
            Maker = "Kafo Korea",
            Purpose = "-"
        });
    }

    private void Search()
    {
        var name = NameKeyword.Trim();
        var maker = MakerKeyword.Trim();

        var filtered = _allFacilities.Where(x =>
            (string.IsNullOrWhiteSpace(name) || x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(maker) || x.Maker.Contains(maker, StringComparison.CurrentCultureIgnoreCase)));

        FacilityItem = new ObservableCollection<FacilityModel>(filtered);
    }

    private void Reset()
    {
        NameKeyword = string.Empty;
        MakerKeyword = string.Empty;
        Search();
    }

    private void Add()
    {
        var created = _facilityDialogService.ShowAddFacilityDialog();
        if (created is null)
            return;

        _allFacilities.Insert(0, created);
        Search();
    }
}
