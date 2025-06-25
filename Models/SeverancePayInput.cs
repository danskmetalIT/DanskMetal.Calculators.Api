namespace DanskMetal.Calculators.Api.Models
{
    public class SeverancePayInput
    {
        public decimal SalaryPerHour { get; set; } // The persons salary per hour. Decimal value
        public decimal WorkingHours { get; set; } // The persons weekly working hours. Decimal value "normally it's 37,00 And I would prefix it to 37,00 in the frontend if it was me. so they only have to change it, if 37,00 isnt the case"
        public decimal FritvalgsPercent { get; set; } // The persons fritvalgs percent from their collective agreement. Decimal value "It's changed in the code afterwards to a percentage, so in the frontend it's a decimal!"
        public decimal UnemploymentMonthlyRate { get; set; } // The danish monthly rate for unemployment. Decimal value "Currently = 21092"
    }
}
