using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Threading;


namespace FuelCalculator;

public partial class MainPage : ContentPage
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    List<string> countries;

    public MainPage()
    {
        InitializeComponent();
        Model.GetDataAsync();
        GetCurrentLocation();
        assignPickers();
        var res = Model.LoadState();
        LoadAppState(res.Item1, res.Item2, res.Item3, res.Item4, res.Item5, res.Item6);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Model.SaveState(countryPicker.SelectedItem.ToString(), kilometersEntry.Text, usageEntry.Text, fuelPicker.SelectedItem.ToString(), peopleEntry.Text, convertSwitch.IsToggled);
    }

    private async Task LoadResultPageAsync()
    {
        await Navigation.PushAsync(new Results(countryPicker.SelectedItem.ToString(), kilometersEntry.Text, usageEntry.Text, fuelPicker.SelectedItem.ToString(), peopleEntry.Text, convertSwitch.IsToggled)); ;
    }
    private void assignPickers()
    {
        countries = Properties.Resources.countries.Replace("\r", string.Empty).Split('\n').ToList();
        foreach (var country in countries)
        {
            countryPicker.Items.Add(country);
        }
        fuelPicker.Items.Add("Gasoline");
        fuelPicker.Items.Add("Diesel");
        fuelPicker.Items.Add("LPG");
    }


    private void clearButton_Clicked(object sender, EventArgs e)
    {
        kilometersEntry.Text = "";
        usageEntry.Text = "";
        peopleEntry.Text = "";
        convertSwitch.IsToggled = false;
    }

    private void calculateButton_Clicked(object sender, EventArgs e)
    {
        if (Model.dataReady)
        {
            LoadResultPageAsync();
        }
        else
        {
            DisplayAlert("Collecting data about fuel prices!", "Please wait few seconds and try again", "Ok");
        }
    }

    private void localizationButton_Clicked(object sender, EventArgs e)
    {
        if (Model.countryName != null)
        {
            for (int index = 0; index < countries.Count(); index++)
            {
                if (String.Equals(countries[index], Model.countryName))
                {
                    countryPicker.SelectedIndex = index;
                    break;
                }
            }
        }
        else
        {
            DisplayAlert("Geolocation error!", "Please wait few seconds and try again", "Ok");
        }
    }

    public async Task AskForLocation()
    {
        PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                Task.Run(() => Model.GetCountryNameAsync(location.Latitude, location.Longitude).Wait());
            }

        }
        catch (Exception ex)
        {
            DisplayAlert("Geolocation error!", "Cannot find your current location!\nPerhabs you need to grant location permission in your system settings.", "Ok");
            localizationButton.IsEnabled = false;

        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }

    private void LoadAppState(string country, string kilometersS, string fuelUsageS, string fuelType, string peopleS, bool convertToPLN)
    {
        if (country == "none")
            countryPicker.SelectedIndex = 0;
        else
        {
            for (int index = 0; index < countries.Count(); index++)
            {
                if (String.Equals(countries[index], country))
                {
                    countryPicker.SelectedIndex = index;
                    break;
                }
            }
        }
        kilometersEntry.Text = kilometersS;
        usageEntry.Text = fuelUsageS;
        if (fuelType == "Gasoline")
            fuelPicker.SelectedIndex = 0;
        else if (fuelType == "Diesel")
            fuelPicker.SelectedIndex = 1;
        else
            fuelPicker.SelectedIndex = 2;
        peopleEntry.Text = peopleS;
        if(convertToPLN)
            convertSwitch.IsToggled = true;
        else
            convertSwitch.IsToggled = false;
    }
}

