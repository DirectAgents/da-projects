using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Currency
    {
        public static Currency ByName(string name, AccountingBackupEntities model, bool create)
        {
            return model.Currencies.FirstOrDefault(c => c.Name == name) ?? (create ? Create(name) : null);
        }

        private static Currency Create(string name)
        {
            return new Currency { Name = name, Culture = CultureMap[name] };
        }

        static Dictionary<string, string> CultureMap = new Dictionary<string, string>
        {
            {"USD", "en-us"},
            {"GBP", "en-gb"},
            {"EUR", "de-de"},
            {"AUD", "en-AU"},
            {"CAD", "en-us"}
        };

        public static string Format(string currencyName, decimal amount)
        {
            string result = string.Format(CultureInfo.CreateSpecificCulture(CultureMap[currencyName]), "{0:C}", amount);

            return result;
        }

        public static decimal Convert(int fromCurrencyId, int toCurrencyId, DateTime date, decimal amount, List<Rate> orderedRates)
        {
            if (fromCurrencyId == toCurrencyId)
            {
                return amount;
            }

            decimal result;

            int month = date.Month;

            int year = date.Year;

            var fromRate = GetRate(fromCurrencyId, orderedRates, month, year);

            if (fromRate == null)
            {
                month--;

                if (month == 0)
                {
                    month = 12;
                    year--;
                }

                fromRate = GetRate(fromCurrencyId, orderedRates, month, year);

                if (fromRate == null)
                {
                    throw new Exception("no rate for month=" + month + " year=" + year + " fromCurrencyId=" + fromCurrencyId);
                }
            }

            var toRate = GetRate(toCurrencyId, orderedRates, month, year);

            decimal conversion = fromRate.ToUSD / toRate.ToUSD;

            result = amount * conversion;

            Log.Write(string.Format("Converting {0:N2} {1} to {2} for a transaction on {3:d} using rates from {4:d} [({0:N2} {1}) * ({5} {1} / 1 USD) / ({6} {2} / 1 USD) = {7} {2}]",
                amount,
                ById(fromCurrencyId).Name,
                ById(toCurrencyId).Name,
                date,
                toRate.Period.BeginDate,
                fromRate.ToUSD,
                toRate.ToUSD,
                result)
            );

            return result;
        }

        private static Rate GetRate(int currencyId, List<Rate> ratesOrderedByDate, int month, int year)
        {
            var rate = ratesOrderedByDate.FirstOrDefault(
                c => c.Period.BeginDate.Month == month && 
                    c.Period.BeginDate.Year == year && 
                    c.Currency.Id == currencyId);

            return rate;
        }

        static Lazy<Dictionary<int, Currency>> CurrenciesById = new Lazy<Dictionary<int, Currency>>(() =>
        {
            using (var model = new AccountingBackupEntities())
            {
                return model.Currencies.ToDictionary(c => c.Id);
            }
        });

        static Currency ById(int id)
        {
            return CurrenciesById.Value[id];
        }

        static Lazy<Dictionary<string, Currency>> CurrenciesByName = new Lazy<Dictionary<string, Currency>>(() =>
        {
            using (var model = new AccountingBackupEntities())
            {
                return model.Currencies.ToDictionary(c => c.Name);
            }
        });

        static Currency ById(string name)
        {
            return CurrenciesByName.Value[name];
        }
    }
}
