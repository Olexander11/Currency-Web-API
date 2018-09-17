using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication001.Models.Fixer_io;

namespace WebApplication001.Services
{
    public class DBUpdateFixer : IDBUpdate
    {
        public void Update(DateTime date)
        {
            CurrencyDBMethod.DBUpdate(date);
        }
    }
}
