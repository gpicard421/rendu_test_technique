using CurrencyConverter;
internal class Program
{
    static void Main(string[] args)
    {
        string[] lines;
        try
        {
            if (args.Length != 1)
                throw new ArgumentException("Should have only one parameter : parameter file path");
            lines = File.ReadAllLines(args[0]);
            int parametersArrayLength = Convert.ToInt32(lines[1]);
            if (lines.Length - 2 != parametersArrayLength)
                Console.WriteLine($"Should have {lines[1]} parameters set but got {lines.Length}");
            else
            {
                string[] parametersToProcess = lines[0].Split(';');

                if (parametersToProcess.Length != 3)
                    throw new ArgumentException("parameters to process should be like XXX;number;XXX");

                string[] exchangeRatesParameters = new string[parametersArrayLength];
                Array.Copy(lines, 2, exchangeRatesParameters, 0, parametersArrayLength);
                ExchangeRates rates = new(exchangeRatesParameters);

                string fromCurrency = parametersToProcess[0];

                int amount = Convert.ToInt32(parametersToProcess[1]);
                if (amount < 0)
                    throw new ArgumentException("amount to calculate can't be inferior to 0");

                string toCurrency = parametersToProcess[2];

                string[] shortestPath = Helpers.GetShortestPathForConversion(fromCurrency, toCurrency, rates);
                if (shortestPath == null || !shortestPath.Any())
                    throw new Exception("no path found for this currency conversion");

                decimal result = ConvertCurrency(amount, rates, shortestPath);
                Console.WriteLine(result);
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }        
    }

    private static decimal ConvertCurrency(int amount, ExchangeRates rates, string[] shortestPath)
    {
        bool isInvertedRate = false;
        decimal result = 0;
        for(int i = 0; i < shortestPath.Length -1; i++)
        {
            ExchangeRate rate = rates.FirstOrDefault(rate => rate.FromCurrency == shortestPath[i] && rate.ToCurrency == shortestPath[i + 1]);
            if (rate == null)
            {
                isInvertedRate = true;
                rate = rates.FirstOrDefault(rate => rate.FromCurrency == shortestPath[i + 1] && rate.ToCurrency == shortestPath[i]);
            }
            else
                isInvertedRate = false;
            
            result = Math.Round((result == 0 ? amount : result) * (isInvertedRate ? (Math.Round(1 / rate.Rate, 4)) : rate.Rate),4);
        }
        return decimal.Round(result);
    }

}
