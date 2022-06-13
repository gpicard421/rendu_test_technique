using System.Globalization;
using System.Linq;
namespace CurrencyConverter
{
    internal class ExchangeRate
    {
        internal string FromCurrency { get; set; }
        internal string ToCurrency { get; set; }
        internal decimal Rate { get; set; }
    }

    internal class ExchangeRates : List<ExchangeRate>
    {
        internal ExchangeRates(string[] parameters)
        {
            foreach(string parameter in parameters)
            {
                string[] splited = parameter.Split(';');
                if (splited.Length != 3)
                    throw new ArgumentException("currency exchange rate parameter format should be like XXX;XXX;0.0000");
                else
                {
                    this.Add(new ExchangeRate()
                    {
                        FromCurrency = splited[0],
                        ToCurrency = splited[1],
                        Rate = decimal.Parse(splited[2], CultureInfo.InvariantCulture)
                    });
                }
            }
        }
    }
}
