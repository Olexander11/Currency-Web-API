using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication001.Models.Fixer_io
{
    public static class CurrencyDBMethod
    {
        public static void DBUpdate(DateTime date)
        {
            for (int i = 0; i < 10; i++)
            {
                DateTime someDaysAgo = date.AddDays(-i);

                using (CurrecyContext context = new CurrecyContext())
                {
                    string dbDateFrmat = someDaysAgo.ToString("yyyy-MM-dd");
                    var result = context.Currencies.FirstOrDefault(rec => rec.Date == dbDateFrmat);
                    if (result != null) continue;

                }

                RestToDB(someDaysAgo);


            }
        }

        public static void RestToDB(DateTime someDaysAgo)
        {
            var client = new RestClient(@"http://data.fixer.io/api/");
            string key = "96067c8c5fb4cd025c5edec854c44316";
            var request = new RestRequest(MakeResponceFixer_io(key, someDaysAgo), Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);
            if (queryResult.StatusCode == HttpStatusCode.OK)

            {
                string rawResponse = queryResult.Content;
                JObject jsonResponce = JObject.Parse(rawResponse);
                //using (StreamWriter file = File.CreateText(@"C:\1\currency.json"))
                //using (JsonTextWriter writer = new JsonTextWriter(file))
                //{
                //    jsonResponce.WriteTo(writer);
                //}


                using (CurrecyContext db = new CurrecyContext())
                {
                    Currency currencyRate = JsonConvert.DeserializeObject<Currency>(rawResponse);

                    var dt = currencyRate.Date;
                    Console.WriteLine("Add infofmation for date -- " + dt);

                    db.Currencies.Add(currencyRate);
                    db.SaveChanges();
                }
            }

        }

        public static string MakeResponceFixer_io(string key, DateTime date)
        {
            string responce = "";
            responce += date.ToString("yyyy-MM-dd");
            responce += "?access_key=" + key;
            //responce += "&base=" + BaseCurrency.ToString();
            //responce += "&symbols=" + ArrayToSCV(SymbolsCurrency);
            return responce;

        }
    }
}
