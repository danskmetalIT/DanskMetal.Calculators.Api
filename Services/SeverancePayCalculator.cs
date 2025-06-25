using DanskMetal.Calculators.Api.Models;

namespace DanskMetal.Calculators.Api.Services
{
    public static class SeverancePayCalculator
    {
        const decimal minSeverancePay = 2500; // Fixed Value from the Collective agreement
        const decimal maxSeverancePay = 15000; // Fixed Value from the Collective agreement
        const decimal deductionRate = 15.00m; // Fixed Value from the Collective agreement

        /* Calculator service for SeverancePay */
        // This calculator returns the following
        // returnState = String. Like "OK" or "Error in calculation"
        // chart1 to chart4 = decimal values. chart1 = first month. chart2 = second month. chart3 = third month. chart4 = fourth month.
        // AND YES! chart1 will always return 0. That is intended behavior.

        public static SeverancePayResult Calculate(decimal salaryPerHour, decimal workingHours, decimal fritvalgsPercent, decimal unemploymentMonthlyRate)
        {
            string returnState = "OK";
            decimal chart1 = 0.00m, chart2 = 0.00m, chart3 = 0.00m, chart4 = 0.00m;

            try
            {
                if (salaryPerHour <= 0 || workingHours <= 0 || fritvalgsPercent < 0)
                    throw new ArgumentException("Invalid input values.");

                decimal salaryResult = salaryPerHour * (160.33m * (workingHours / 37));
                fritvalgsPercent /= 100;
                salaryResult += salaryResult * fritvalgsPercent;
                salaryResult -= salaryResult * (deductionRate / 100);
                salaryResult = Math.Round(salaryResult, 2);

                decimal unemployedResult = (unemploymentMonthlyRate / 160.33m) * (160.33m * (workingHours / 37));
                unemployedResult = Math.Round(unemployedResult, 2);

                decimal severanceResult = Math.Round(salaryResult - unemployedResult, 2);
                if (severanceResult < minSeverancePay)
                    severanceResult = minSeverancePay;
                else if (severanceResult > maxSeverancePay)
                    severanceResult = maxSeverancePay;

                chart2 = severanceResult;
                chart3 = Math.Round(severanceResult * 2, 2);
                chart4 = Math.Round(severanceResult * 3, 2);
            }
            catch
            {
                returnState = "Error in calculation";
            }

            return new SeverancePayResult(returnState, chart1, chart2, chart3, chart4);
        }
    }
}
