namespace FuelCalculator;

public partial class Results : ContentPage
{
	public Results(string country,string kilometerS, string fuelUsageS, string fuelType,string peopleS, bool convertToPLN)
	{
		InitializeComponent();
		AssignData(country, kilometerS, fuelUsageS, fuelType, peopleS, convertToPLN);
	}

	private void AssignData(string country, string kilometerS, string fuelUsageS, string fuelType, string peopleS, bool convertToPLN)
	{
		var data = Model.CalculateResults(country, kilometerS, fuelUsageS, fuelType, peopleS, convertToPLN);
		if(data.Item1==-1)
		{
            fuelPriceLabel.Text = "Cannot find current price for this type of fuel!";
        }
		else
		{
			if(convertToPLN==false)
			{
                fuelPriceLabel.Text = "Current price for 1 liter: " + data.Item1.ToString() + "€";
                pricePer100KmLabel.Text = "Cost per 100km: " + data.Item2.ToString() + "€";
                travelCostLabel.Text = "Cost of travel: " + data.Item3.ToString() + "€";
                personCostLabel.Text = "Cost per 1 person: " + data.Item4.ToString() + "€";
            }
            
			else
			{
                fuelPriceLabel.Text = "Current price for 1 liter: " + data.Item1.ToString() + " PLN";
                pricePer100KmLabel.Text = "Cost per 100km: " + data.Item2.ToString() + " PLN";
                travelCostLabel.Text = "Cost of travel: " + data.Item3.ToString() + " PLN";
                personCostLabel.Text = "Cost per 1 person: " + data.Item4.ToString() + " PLN";
            }
        }
		
	}
}