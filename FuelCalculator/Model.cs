using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Environment;

namespace FuelCalculator
{
    public class FuelPrice
    {
        public string currency { get; set; }
        public string lpg { get; set; }
        public string diesel { get; set; }
        public string gasoline { get; set; }
        public string country { get; set; }
    }

    internal static class Model
    {
        public static string countryName = null;
        public static bool dataReady = false;
        static double euroPrice;
        public static List<FuelPrice> fuelPrices;
        static bool useApi = false;


        public static async Task GetDataAsync()
        {
            Task.Run(() => GetFuelPricesAsync().Wait());
            Task.Run(() => GetEuroPriceAsync().Wait());
            //GetEuroPrice();
            //GetFuelPrices();
            dataReady = true;
        }


        public static async Task GetCountryNameAsync(double lat, double lon)
        {
            string data;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.geoapify.com/v1/geocode/reverse?lat=" + lat.ToString().Replace(',', '.') + "&lon=" + lon.ToString().Replace(',', '.') + "&apiKey=69ca5d409978485897b434d18f15c167"),

            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body =  await response.Content.ReadAsStringAsync();
                data = body;
            }
            int startIndex = data.IndexOf("country\":\"");
            int endIndex = data.IndexOf(",\"country_code");
            data = data.Substring(startIndex + 10, endIndex - startIndex - 11);
            countryName =  data;

        }

        static async Task GetFuelPricesAsync()
        {
            fuelPrices = new List<FuelPrice>();
            string data;
            if (useApi)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://gas-price.p.rapidapi.com/europeanCountries"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "API-Key" },
                        { "X-RapidAPI-Host", "gas-price.p.rapidapi.com" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    data = body;
                }
            }
            else
            {
                data = "{\"results\":[{\"currency\":\"euro\",\"lpg\":\"0,677\",\"diesel\":\"1,691\",\"gasoline\":\"1,644\",\"country\":\"Albania\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,419\",\"gasoline\":\"1,454\",\"country\":\"Andorra\"},{\"currency\":\"euro\",\"lpg\":\"0,510\",\"diesel\":\"0,922\",\"gasoline\":\"0,995\",\"country\":\"Armenia\"},{\"currency\":\"euro\",\"lpg\":\"1,320\",\"diesel\":\"1,661\",\"gasoline\":\"1,587\",\"country\":\"Austria\"},{\"currency\":\"euro\",\"lpg\":\"0,459\",\"diesel\":\"0,864\",\"gasoline\":\"0,864\",\"country\":\"Belarus\"},{\"currency\":\"euro\",\"lpg\":\"0,794\",\"diesel\":\"1,751\",\"gasoline\":\"1,752\",\"country\":\"Belgium\"},{\"currency\":\"euro\",\"lpg\":\"0,766\",\"diesel\":\"1,418\",\"gasoline\":\"1,368\",\"country\":\"Bosnia and Herzegovina\"},{\"currency\":\"euro\",\"lpg\":\"0,619\",\"diesel\":\"1,462\",\"gasoline\":\"1,324\",\"country\":\"Bulgaria\"},{\"currency\":\"euro\",\"lpg\":\"0,955\",\"diesel\":\"1,467\",\"gasoline\":\"1,432\",\"country\":\"Croatia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,556\",\"gasoline\":\"1,390\",\"country\":\"Cyprus\"},{\"currency\":\"euro\",\"lpg\":\"0,715\",\"diesel\":\"1,522\",\"gasoline\":\"1,579\",\"country\":\"Czech Republic\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,760\",\"gasoline\":\"1,934\",\"country\":\"Denmark\"},{\"currency\":\"euro\",\"lpg\":\"0,731\",\"diesel\":\"1,696\",\"gasoline\":\"1,726\",\"country\":\"Estonia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,966\",\"gasoline\":\"1,929\",\"country\":\"Finland\"},{\"currency\":\"euro\",\"lpg\":\"0,990\",\"diesel\":\"1,813\",\"gasoline\":\"1,879\",\"country\":\"France\"},{\"currency\":\"euro\",\"lpg\":\"0,647\",\"diesel\":\"1,330\",\"gasoline\":\"1,024\",\"country\":\"Georgia\"},{\"currency\":\"euro\",\"lpg\":\"1,089\",\"diesel\":\"1,788\",\"gasoline\":\"1,829\",\"country\":\"Germany\"},{\"currency\":\"euro\",\"lpg\":\"1,009\",\"diesel\":\"1,705\",\"gasoline\":\"1,894\",\"country\":\"Greece\"},{\"currency\":\"euro\",\"lpg\":\"0,966\",\"diesel\":\"1,632\",\"gasoline\":\"1,600\",\"country\":\"Hungary\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"2,123\",\"gasoline\":\"2,077\",\"country\":\"Iceland\"},{\"currency\":\"euro\",\"lpg\":\"0,890\",\"diesel\":\"1,659\",\"gasoline\":\"1,602\",\"country\":\"Ireland\"},{\"currency\":\"euro\",\"lpg\":\"0,813\",\"diesel\":\"1,819\",\"gasoline\":\"1,854\",\"country\":\"Italy\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"0,000\",\"gasoline\":\"0,000\",\"country\":\"Kosovo\"},{\"currency\":\"euro\",\"lpg\":\"0,782\",\"diesel\":\"1,616\",\"gasoline\":\"1,627\",\"country\":\"Latvia\"},{\"currency\":\"euro\",\"lpg\":\"0,587\",\"diesel\":\"1,579\",\"gasoline\":\"1,514\",\"country\":\"Lithuania\"},{\"currency\":\"euro\",\"lpg\":\"0,821\",\"diesel\":\"1,578\",\"gasoline\":\"1,561\",\"country\":\"Luxembourg\"},{\"currency\":\"euro\",\"lpg\":\"0,818\",\"diesel\":\"1,289\",\"gasoline\":\"1,313\",\"country\":\"North Macedonia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,208\",\"gasoline\":\"1,338\",\"country\":\"Malta\"},{\"currency\":\"euro\",\"lpg\":\"0,820\",\"diesel\":\"1,117\",\"gasoline\":\"1,243\",\"country\":\"Moldova\"},{\"currency\":\"euro\",\"lpg\":\"0,850\",\"diesel\":\"1,480\",\"gasoline\":\"1,550\",\"country\":\"Montenegro\"},{\"currency\":\"euro\",\"lpg\":\"0,979\",\"diesel\":\"1,712\",\"gasoline\":\"1,880\",\"country\":\"Netherlands\"},{\"currency\":\"euro\",\"lpg\":\"1,260\",\"diesel\":\"1,890\",\"gasoline\":\"2,032\",\"country\":\"Norway\"},{\"currency\":\"euro\",\"lpg\":\"0,673\",\"diesel\":\"1,513\",\"gasoline\":\"1,419\",\"country\":\"Poland\"},{\"currency\":\"euro\",\"lpg\":\"0,918\",\"diesel\":\"1,577\",\"gasoline\":\"1,697\",\"country\":\"Portugal\"},{\"currency\":\"euro\",\"lpg\":\"0,749\",\"diesel\":\"1,472\",\"gasoline\":\"1,333\",\"country\":\"Romania\"},{\"currency\":\"euro\",\"lpg\":\"0,250\",\"diesel\":\"0,725\",\"gasoline\":\"0,645\",\"country\":\"Russia\"},{\"currency\":\"euro\",\"lpg\":\"0,860\",\"diesel\":\"1,637\",\"gasoline\":\"1,455\",\"country\":\"Serbia\"},{\"currency\":\"euro\",\"lpg\":\"0,784\",\"diesel\":\"1,551\",\"gasoline\":\"1,557\",\"country\":\"Slovakia\"},{\"currency\":\"euro\",\"lpg\":\"0,932\",\"diesel\":\"1,485\",\"gasoline\":\"1,354\",\"country\":\"Slovenia\"},{\"currency\":\"euro\",\"lpg\":\"0,989\",\"diesel\":\"1,582\",\"gasoline\":\"1,625\",\"country\":\"Spain\"},{\"currency\":\"euro\",\"lpg\":\"1,257\",\"diesel\":\"2,062\",\"gasoline\":\"1,780\",\"country\":\"Sweden\"},{\"currency\":\"euro\",\"lpg\":\"0,951\",\"diesel\":\"2,033\",\"gasoline\":\"1,843\",\"country\":\"Switzerland\"},{\"currency\":\"euro\",\"lpg\":\"0,594\",\"diesel\":\"1,056\",\"gasoline\":\"1,023\",\"country\":\"Turkey\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"0,000\",\"gasoline\":\"0,000\",\"country\":\"U.S.A\"},{\"currency\":\"euro\",\"lpg\":\"0,614\",\"diesel\":\"1,277\",\"gasoline\":\"1,216\",\"country\":\"Ukraine\"},{\"currency\":\"euro\",\"lpg\":\"0,865\",\"diesel\":\"2,014\",\"gasoline\":\"1,670\",\"country\":\"United Kingdom\"}],\"success\":true}";
            }
            int startIndex = data.IndexOf('[');
            int endIndex = data.LastIndexOf(']');
            data = data.Substring(startIndex, endIndex - startIndex + 1);
            fuelPrices = JsonConvert.DeserializeObject<List<FuelPrice>>(data);
        }

        static void GetFuelPrices()
        {
            fuelPrices = new List<FuelPrice>();
            string data;
            if (useApi)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://gas-price.p.rapidapi.com/europeanCountries"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "API-Key" },
                        { "X-RapidAPI-Host", "gas-price.p.rapidapi.com" },
                    },
                };
                using (var response = client.Send(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = response.Content.ReadAsStream();
                    var tmp = new StreamReader(body).ReadToEnd();
                    data = tmp;
                }
            }
            else
            {
                data = "{\"results\":[{\"currency\":\"euro\",\"lpg\":\"0,677\",\"diesel\":\"1,691\",\"gasoline\":\"1,644\",\"country\":\"Albania\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,419\",\"gasoline\":\"1,454\",\"country\":\"Andorra\"},{\"currency\":\"euro\",\"lpg\":\"0,510\",\"diesel\":\"0,922\",\"gasoline\":\"0,995\",\"country\":\"Armenia\"},{\"currency\":\"euro\",\"lpg\":\"1,320\",\"diesel\":\"1,661\",\"gasoline\":\"1,587\",\"country\":\"Austria\"},{\"currency\":\"euro\",\"lpg\":\"0,459\",\"diesel\":\"0,864\",\"gasoline\":\"0,864\",\"country\":\"Belarus\"},{\"currency\":\"euro\",\"lpg\":\"0,794\",\"diesel\":\"1,751\",\"gasoline\":\"1,752\",\"country\":\"Belgium\"},{\"currency\":\"euro\",\"lpg\":\"0,766\",\"diesel\":\"1,418\",\"gasoline\":\"1,368\",\"country\":\"Bosnia and Herzegovina\"},{\"currency\":\"euro\",\"lpg\":\"0,619\",\"diesel\":\"1,462\",\"gasoline\":\"1,324\",\"country\":\"Bulgaria\"},{\"currency\":\"euro\",\"lpg\":\"0,955\",\"diesel\":\"1,467\",\"gasoline\":\"1,432\",\"country\":\"Croatia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,556\",\"gasoline\":\"1,390\",\"country\":\"Cyprus\"},{\"currency\":\"euro\",\"lpg\":\"0,715\",\"diesel\":\"1,522\",\"gasoline\":\"1,579\",\"country\":\"Czech Republic\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,760\",\"gasoline\":\"1,934\",\"country\":\"Denmark\"},{\"currency\":\"euro\",\"lpg\":\"0,731\",\"diesel\":\"1,696\",\"gasoline\":\"1,726\",\"country\":\"Estonia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,966\",\"gasoline\":\"1,929\",\"country\":\"Finland\"},{\"currency\":\"euro\",\"lpg\":\"0,990\",\"diesel\":\"1,813\",\"gasoline\":\"1,879\",\"country\":\"France\"},{\"currency\":\"euro\",\"lpg\":\"0,647\",\"diesel\":\"1,330\",\"gasoline\":\"1,024\",\"country\":\"Georgia\"},{\"currency\":\"euro\",\"lpg\":\"1,089\",\"diesel\":\"1,788\",\"gasoline\":\"1,829\",\"country\":\"Germany\"},{\"currency\":\"euro\",\"lpg\":\"1,009\",\"diesel\":\"1,705\",\"gasoline\":\"1,894\",\"country\":\"Greece\"},{\"currency\":\"euro\",\"lpg\":\"0,966\",\"diesel\":\"1,632\",\"gasoline\":\"1,600\",\"country\":\"Hungary\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"2,123\",\"gasoline\":\"2,077\",\"country\":\"Iceland\"},{\"currency\":\"euro\",\"lpg\":\"0,890\",\"diesel\":\"1,659\",\"gasoline\":\"1,602\",\"country\":\"Ireland\"},{\"currency\":\"euro\",\"lpg\":\"0,813\",\"diesel\":\"1,819\",\"gasoline\":\"1,854\",\"country\":\"Italy\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"0,000\",\"gasoline\":\"0,000\",\"country\":\"Kosovo\"},{\"currency\":\"euro\",\"lpg\":\"0,782\",\"diesel\":\"1,616\",\"gasoline\":\"1,627\",\"country\":\"Latvia\"},{\"currency\":\"euro\",\"lpg\":\"0,587\",\"diesel\":\"1,579\",\"gasoline\":\"1,514\",\"country\":\"Lithuania\"},{\"currency\":\"euro\",\"lpg\":\"0,821\",\"diesel\":\"1,578\",\"gasoline\":\"1,561\",\"country\":\"Luxembourg\"},{\"currency\":\"euro\",\"lpg\":\"0,818\",\"diesel\":\"1,289\",\"gasoline\":\"1,313\",\"country\":\"North Macedonia\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"1,208\",\"gasoline\":\"1,338\",\"country\":\"Malta\"},{\"currency\":\"euro\",\"lpg\":\"0,820\",\"diesel\":\"1,117\",\"gasoline\":\"1,243\",\"country\":\"Moldova\"},{\"currency\":\"euro\",\"lpg\":\"0,850\",\"diesel\":\"1,480\",\"gasoline\":\"1,550\",\"country\":\"Montenegro\"},{\"currency\":\"euro\",\"lpg\":\"0,979\",\"diesel\":\"1,712\",\"gasoline\":\"1,880\",\"country\":\"Netherlands\"},{\"currency\":\"euro\",\"lpg\":\"1,260\",\"diesel\":\"1,890\",\"gasoline\":\"2,032\",\"country\":\"Norway\"},{\"currency\":\"euro\",\"lpg\":\"0,673\",\"diesel\":\"1,513\",\"gasoline\":\"1,419\",\"country\":\"Poland\"},{\"currency\":\"euro\",\"lpg\":\"0,918\",\"diesel\":\"1,577\",\"gasoline\":\"1,697\",\"country\":\"Portugal\"},{\"currency\":\"euro\",\"lpg\":\"0,749\",\"diesel\":\"1,472\",\"gasoline\":\"1,333\",\"country\":\"Romania\"},{\"currency\":\"euro\",\"lpg\":\"0,250\",\"diesel\":\"0,725\",\"gasoline\":\"0,645\",\"country\":\"Russia\"},{\"currency\":\"euro\",\"lpg\":\"0,860\",\"diesel\":\"1,637\",\"gasoline\":\"1,455\",\"country\":\"Serbia\"},{\"currency\":\"euro\",\"lpg\":\"0,784\",\"diesel\":\"1,551\",\"gasoline\":\"1,557\",\"country\":\"Slovakia\"},{\"currency\":\"euro\",\"lpg\":\"0,932\",\"diesel\":\"1,485\",\"gasoline\":\"1,354\",\"country\":\"Slovenia\"},{\"currency\":\"euro\",\"lpg\":\"0,989\",\"diesel\":\"1,582\",\"gasoline\":\"1,625\",\"country\":\"Spain\"},{\"currency\":\"euro\",\"lpg\":\"1,257\",\"diesel\":\"2,062\",\"gasoline\":\"1,780\",\"country\":\"Sweden\"},{\"currency\":\"euro\",\"lpg\":\"0,951\",\"diesel\":\"2,033\",\"gasoline\":\"1,843\",\"country\":\"Switzerland\"},{\"currency\":\"euro\",\"lpg\":\"0,594\",\"diesel\":\"1,056\",\"gasoline\":\"1,023\",\"country\":\"Turkey\"},{\"currency\":\"euro\",\"lpg\":\"-\",\"diesel\":\"0,000\",\"gasoline\":\"0,000\",\"country\":\"U.S.A\"},{\"currency\":\"euro\",\"lpg\":\"0,614\",\"diesel\":\"1,277\",\"gasoline\":\"1,216\",\"country\":\"Ukraine\"},{\"currency\":\"euro\",\"lpg\":\"0,865\",\"diesel\":\"2,014\",\"gasoline\":\"1,670\",\"country\":\"United Kingdom\"}],\"success\":true}";
            }
            int startIndex = data.IndexOf('[');
            int endIndex = data.LastIndexOf(']');
            data = data.Substring(startIndex, endIndex - startIndex + 1);
            fuelPrices = JsonConvert.DeserializeObject<List<FuelPrice>>(data);
        }

        public static async Task GetEuroPriceAsync()
        {
            string data;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.nbp.pl/api/exchangerates/rates/a/eur/?format=json"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                data = body.ToString();
            }
            int startIndex = data.LastIndexOf(':');
            int endIndex = startIndex + 6;
            data = data.Substring(startIndex + 1, endIndex - startIndex);
            data = data.Replace('.', ',');
            euroPrice = Convert.ToDouble(data);
            dataReady = true;
        }
        static void GetEuroPrice()
        {
            string data;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.nbp.pl/api/exchangerates/rates/a/eur/?format=json"),
            };
            using (var response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStream();
                var tmp = new StreamReader(body).ReadToEnd();
                data = tmp;
            }
            int startIndex = data.LastIndexOf(':');
            int endIndex = startIndex + 6;
            data = data.Substring(startIndex + 1, endIndex - startIndex);
            data = data.Replace('.', ',');
            euroPrice = Convert.ToDouble(data);

        }

        public static (double, double, double, double) CalculateResults(string country, string kilometersS, string fuelUsageS, string fuelType, string peopleS, bool convertToPLN)
        {
            kilometersS = kilometersS.Replace('.', ',');
            fuelUsageS = fuelUsageS.Replace(".", ",");

            double kilometers = Convert.ToDouble(kilometersS);
            double fuelUsage = Convert.ToDouble(fuelUsageS);
            int people = Convert.ToInt32(peopleS);
            double currentFuelPrice = 0;
            foreach (var fuelPrice in fuelPrices)
            {
                if (String.Equals(fuelPrice.country, country))
                {
                    switch (fuelType)
                    {
                        case "Gasoline":
                            currentFuelPrice = Convert.ToDouble(fuelPrice.gasoline);
                            break;
                        case "Diesel":
                            currentFuelPrice = Convert.ToDouble(fuelPrice.diesel);
                            break;
                        case "LPG":
                            if (fuelPrice.lpg != "-")
                                currentFuelPrice = Convert.ToDouble(fuelPrice.lpg);
                            else
                                currentFuelPrice = 0;
                            break;
                        default:
                            currentFuelPrice = 0;
                            break;
                    }
                    break;
                }
            }
            if (currentFuelPrice <= 0)
                return (-1, -1, -1, -1);
            double travelCost = (kilometers / 100.0) * fuelUsage * currentFuelPrice;
            double costPer100Km = fuelUsage * currentFuelPrice;
            double personCost = travelCost / people;

            if (!convertToPLN)
            {
                return (Math.Round(currentFuelPrice, 2), Math.Round(costPer100Km, 2), Math.Round(travelCost, 2), Math.Round(personCost, 2));
            }
            else
            {
                return (Math.Round(currentFuelPrice * euroPrice, 2), Math.Round(costPer100Km * euroPrice, 2), Math.Round(travelCost * euroPrice, 2), Math.Round(personCost * euroPrice, 2));
            }

        }

        public static void SaveState(string country, string kilometersS, string fuelUsageS, string fuelType, string peopleS, bool convertToPLN)
        {
            string filePath = Path.Combine(GetFolderPath(SpecialFolder.UserProfile), "fuelCalculator.xml");
            XDocument xml = new XDocument(
                new XComment($"Saved {DateTime.Now.ToString()}"),
                new XElement("state",
                    new XElement("Country", country),
                    new XElement("Kilometers", kilometersS),
                    new XElement("FuelUsage", fuelUsageS),
                    new XElement("FuelType", fuelType),
                    new XElement("People", peopleS),
                    new XElement("ConvertToPLN", convertToPLN.ToString())
                    )
                );
            xml.Save(filePath);
        }
        public static (string,string,string,string,string,bool) LoadState()
        {
            try
            {
                string filePath = Path.Combine(GetFolderPath(SpecialFolder.UserProfile), "fuelCalculator.xml");
                XDocument xml = XDocument.Load(filePath);
                string country = xml.Root.Element("Country").Value;
                string kilometers = xml.Root.Element("Kilometers").Value;
                string fuelUsage = xml.Root.Element("FuelUsage").Value;
                string fuelType = xml.Root.Element("FuelType").Value;
                string people = xml.Root.Element("People").Value;
                string convertToPLN = xml.Root.Element("ConvertToPLN").Value;
                bool convert = convertToPLN == "True" ? true : false;
                return (country, kilometers, fuelUsage, fuelType, people, convert);
            }
            catch
            {
                return ("none", "100", "6", "Gasoline", "2", false);
            }
        }

    }
}
