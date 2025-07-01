using DanskMetal.Calculators.Api.Models;

namespace DanskMetal.Calculators.Api.Services
{
    public static class SeverancePayCalculator
    {
        

        /* Calculator service for SeverancePay */
        // This calculator returns the following
        // returnState = String. Like "OK" or "Error in calculation"
        // chart1 to chart4 = decimal values. chart1 = first month. chart2 = second month. chart3 = third month. chart4 = fourth month.
        // AND YES! chart1 will always return 0. That is intended behavior.

        public static SeverancePayResult Calculate(decimal salaryPerHour, decimal workingHours, decimal fritvalgsPercent, decimal unemploymentMonthlyRate)
        {

            decimal minSeverancePay = 2500; // Fixed Value from the Collective agreement
            decimal maxSeverancePay = 15000; // Fixed Value from the Collective agreement
            decimal deductionRate = 15.00m; // Fixed Value from the Collective agreement

            string returnState = "OK";
            decimal chart1 = 0.00m, chart2 = 0.00m, chart3 = 0.00m, chart4 = 0.00m;

            try
            {
                if (salaryPerHour <= 0 || workingHours <= 0 || fritvalgsPercent < 0)
                    throw new ArgumentException("Invalid input values.");

                // Calculate the Salary part
                decimal salaryResult = salaryPerHour * (160.33m * (workingHours / 37)); // Calculate the salary based upon the reported weekly hours
                fritvalgsPercent = fritvalgsPercent / 100; // Changing the number from frontend to a value that is valid for a percentage. ex. 9.00 will become 0.09 that is equal to 9%.
                salaryResult += salaryResult * fritvalgsPercent; // Adding the value of the fritvalgspercent to the result and adding 
                deductionRate = deductionRate / 100; // Changing the number from backend to a value that is valid for a percentage. ex. 15.00 will become 0.15 that is equal to 15%.
                salaryResult -= salaryResult * deductionRate; // Subtracting the value based on the deduction rate
                salaryResult = Math.Round(salaryResult, 2); // Rounds value to two decimals only.

                // Calculate the unemployment part
                decimal unemployedResult = (unemploymentMonthlyRate / 160.33m) * (160.33m * (workingHours / 37)); // Calculate the unemployed rate based upon the reported weekly hours
                unemployedResult = Math.Round(unemployedResult, 2); // Rounds value to two decimals only.

                // Calculate severance result
                decimal severanceResult = salaryResult - unemployedResult; // Calculate the difference between the two
                severanceResult = Math.Round(severanceResult, 2); // Rounds value to two decimals only.


                // Apply min and max logic
                // Check if severanceResult is under or above the min and max values from backend
                if (severanceResult < minSeverancePay)
                {
                    severanceResult = minSeverancePay;
                }
                else if (severanceResult > maxSeverancePay)
                {
                    severanceResult = maxSeverancePay;
                }

                // Calculates the the two last columns
                chart2 = severanceResult;
                chart3 = Math.Round(chart2 * 2, 2);
                chart4 = Math.Round(chart2 * 3, 2);
            }
            catch
            {
                returnState = "Error in calculation";
            }

            return new SeverancePayResult(returnState, chart1, chart2, chart3, chart4);
        }
    }
}
