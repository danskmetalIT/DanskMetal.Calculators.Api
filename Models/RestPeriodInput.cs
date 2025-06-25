namespace DanskMetal.Calculators.Api.Models
{
    public class RestPeriodInput
    {
        public int IsEightHourRules { get; set; } // 1 = 8 hours rules. 2 = 11 hours rules
        // Selected tiles
        public int T1 { get; set; } // Tile 1 representing one hour. 1 = selected. 2 = not selected.
        public int T2 { get; set; } // Tile 2 representing one hour. 1 = selected. 2 = not selected.
        public int T3 { get; set; } // Tile 3 representing one hour. 1 = selected. 2 = not selected.
        public int T4 { get; set; } // Tile 4 representing one hour. 1 = selected. 2 = not selected.
        public int T5 { get; set; } // Tile 5 representing one hour. 1 = selected. 2 = not selected.
        public int T6 { get; set; } // Tile 6 representing one hour. 1 = selected. 2 = not selected.
        public int T7 { get; set; } // Tile 7 representing one hour. 1 = selected. 2 = not selected.
        public int T8 { get; set; } // Tile 8 representing one hour. 1 = selected. 2 = not selected.
        public int T9 { get; set; } // Tile 9 representing one hour. 1 = selected. 2 = not selected.
        public int T10 { get; set; } // Tile 10 representing one hour. 1 = selected. 2 = not selected.
        public int T11 { get; set; } // Tile 11 representing one hour. 1 = selected. 2 = not selected.
        public int T12 { get; set; } // Tile 12 representing one hour. 1 = selected. 2 = not selected.
        public int T13 { get; set; } // Tile 13 representing one hour. 1 = selected. 2 = not selected.
        public int T14 { get; set; } // Tile 14 representing one hour. 1 = selected. 2 = not selected.
        public int T15 { get; set; } // Tile 15 representing one hour. 1 = selected. 2 = not selected.
        public int T16 { get; set; } // Tile 16 representing one hour. 1 = selected. 2 = not selected.
        public int T17 { get; set; } // Tile 17 representing one hour. 1 = selected. 2 = not selected.
        public int T18 { get; set; } // Tile 18 representing one hour. 1 = selected. 2 = not selected.
        public int T19 { get; set; } // Tile 19 representing one hour. 1 = selected. 2 = not selected.
        public int T20 { get; set; } // Tile 20 representing one hour. 1 = selected. 2 = not selected.
        public int T21 { get; set; } // Tile 21 representing one hour. 1 = selected. 2 = not selected.
        public int T22 { get; set; } // Tile 22 representing one hour. 1 = selected. 2 = not selected.
        public int T23 { get; set; } // Tile 23 representing one hour. 1 = selected. 2 = not selected.
        public int T24 { get; set; } // Tile 24 representing one hour. 1 = selected. 2 = not selected.

        // Examples
        public decimal SalaryPerHour { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal FritvalgsPercent { get; set; }
        public decimal UnemploymentMonthlyRate { get; set; }
    }
}
