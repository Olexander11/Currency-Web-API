using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication001.Models;

namespace WebApplication001.Controllers
{
    public class ASPNetController : Controller
    {
        public SelectList DropDownList { get; set; }
        private CurrencyContext db;
        public ASPNetController(CurrencyContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {

            // ViewBag.curName = typeof(Rates).GetProperties().Select(f => f.Name).ToList();
            //ViewBag.baseCurr = "EUR";
            var vm = new MyViewModel();
            var cl = typeof(Rates).GetMethods().Where(f => (f.Name != "get_Id") && (f.Name.StartsWith("get_"))).Select(f => f.Name.Substring(4));
            vm.CurrencyList = new List<SelectListItem>();



            foreach (string el in cl)
            {
                vm.CurrencyList.Add(new SelectListItem { Text = el.ToString() });

            }
           
            return View(vm);
        }

        public IActionResult GetAction(string BaseCurr, string Curr01, string Curr02, string Curr03, string Curr04, string Curr05, string Curr06, string Curr07, string Curr08, string Curr09, string Curr10)
        {
            List<string> currList = new List<string>();
            if (Curr01 != null) currList.Add(Curr01);
            if (Curr02 != null) currList.Add(Curr02);
            if (Curr03 != null) currList.Add(Curr03);
            if (Curr04 != null) currList.Add(Curr04);
            if (Curr05 != null) currList.Add(Curr05);
            if (Curr06 != null) currList.Add(Curr06);
            if (Curr07 != null) currList.Add(Curr07);
            if (Curr08 != null) currList.Add(Curr08);
            if (Curr09 != null) currList.Add(Curr09);
            if (Curr10 != null) currList.Add(Curr10);
            currList.Add(BaseCurr);

            Object[,] tableArray = new Object[currList.Count() + 1, 11];
            tableArray[0, 0] = "Currency";
            
            for(int i=0; i<10; i++)
            {
              tableArray[0, i + 1] = DateTime.Now.Date.AddDays(i - 9).ToString("yyyy-MM-dd");
            }

            int ii = 0;
            var curData = db.Currencies;
            foreach (var d in curData)
                db.Entry(d).Navigation("Rates").Load();
            foreach(string c in currList)
            {
                tableArray[ii+1,0] = BaseCurr + "/" +c;
                MethodInfo mi = typeof(Rates).GetMethod("get_"+c);

                for (int j= 0; j<10; j++)
                {
                    
                   
                     
                    var it = curData.FirstOrDefault(x =>
                   
                        x.Date == (string)tableArray[0, j + 1]
                    
                    );
                    if (it != null)
                    {
                        var d = mi.Invoke(it.Rates, null);
                        tableArray[ii + 1, j + 1] = d;
                    }
                }

                ii++;
            }

            

            if (BaseCurr != "EUR")
            {

                var numRow = tableArray.GetLength(0);
                var numCol = tableArray.GetLength(1);
                for(int i = 1; i < numCol; i++)
                {
                    for (int j = 1; j < numRow-1; j++)
                    {
                       tableArray[j, i] =(float)tableArray[j, i]/ (float)tableArray[numRow-1, i];

                    }

                }
            }
            



                return View(tableArray);
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }
    }

   

    public class MyViewModel
    {
        public string Curr { get; set; }
        public List<SelectListItem> CurrencyList { set; get; }
    }

    

}
