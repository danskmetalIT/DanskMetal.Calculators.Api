using DanskMetal.Calculators.Api.Models;

namespace DanskMetal.Calculators.Api.Services
{
    public static class RestPeriodCalculator
    {
        // Variables area

        public static RestPeriodResult Calculate(int IsEightHourRules, int T1, int T2, int T3, int T4, int T5, int T6, int T7, int T8, int T9, int T10, int T11, int T12, int T13, int T14, int T15, int T16, int T17, int T18, int T19, int T20, int T21, int T22, int T23, int T24)
        {
            string returnState = "OK";
            string returnText = "Not calculated yet";
            int minRestPeriod = 11; // hvileTidTvunget
            var intArb = 1; // colorArb
            var intRest = 0; // colorFri
            int restPeriodCount = 0; // hvileTidCount
            int restPeriodHighest = 0; // hvileTidHighest

            /* Calculates restPeriodCount */
            int[] cells = new int[]
            {
                T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24
            };

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == intRest)
                {
                    restPeriodCount++;
                    if (restPeriodCount > restPeriodHighest)
                    {
                        restPeriodHighest++;
                    }
                }
                else if (cells[i] == intArb)
                {
                    restPeriodCount = 0;
                }
            }

            if (restPeriodHighest < 11)
            {
                minRestPeriod -= restPeriodHighest;
                minRestPeriod += 11;
            }

            /* Calculation end */

            if (IsEightHourRules == 1)
            {
                // Calculations with the 8 hours rules
                if (restPeriodHighest < 8)
                {
                    returnText = "8 timers reglen er ikke overholdt, da der kun har været " + restPeriodHighest.ToString() + " timers sammenhængede hvile";
                }
                else
                {
                    returnText = "8 timers reglen er overholdt, da der har været " + restPeriodHighest.ToString() + " timers sammenhængede hvile";
                }

            }
            else
            {
                // Calculations with the 11 hours rules
                if (restPeriodHighest < 11)
                {
                    returnText = "11 timers reglen er ikke overholdt, da der kun har været " + restPeriodHighest.ToString() + " timers sammenhængede hvile";
                }
                else
                {
                    returnText = "11 timers reglen er overholdt, da der har været " + restPeriodHighest.ToString() + " timers sammenhængede hvile";
                }
            }

            /* Sets next 24 tiles based on minRestPeriod's value */
            int T25 = 0, T26 = 0, T27 = 0, T28 = 0, T29 = 0, T30 = 0, T31 = 0, T32 = 0, T33 = 0, T34 = 0, T35 = 0, T36 = 0, T37 = 0, T38 = 0, T39 = 0, T40 = 0, T41 = 0, T42 = 0, T43 = 0, T44 = 0, T45 = 0, T46 = 0, T47 = 0, T48 = 0;

            if (minRestPeriod > 11)
            {
                T25 = 1; // 1
                T26 = 1; // 2
                T27 = 1; // 3
                T28 = 1; // 4
                T29 = 1; // 5
                T30 = 1; // 6
                T31 = 1; // 7
                T32 = 1; // 8
                T33 = 1; // 9
                T34 = 1; // 10
                T35 = 1; // 11
                if (minRestPeriod > 11)
                {
                    if (minRestPeriod > 11)
                    {
                        T36 = 1; // 12
                    }
                    if (minRestPeriod > 12)
                    {
                        T37 = 1; // 13
                    }
                    if (minRestPeriod > 13)
                    {
                        T38 = 1; // 14
                    }
                    if (minRestPeriod > 14)
                    {
                        T39 = 1; // 15
                    }
                    if (minRestPeriod > 15)
                    {
                        T40 = 1; // 16
                    }
                }
                // 
                if (minRestPeriod > 16)
                {
                    if (minRestPeriod > 16)
                    {
                        T41 = 1; // 17
                    }
                    if (minRestPeriod > 17)
                    {
                        T42 = 1; // 18
                    }
                    if (minRestPeriod > 18)
                    {
                        T43 = 1; // 19
                    }
                    if (minRestPeriod > 19)
                    {
                        T44 = 1; // 20
                    }
                    if (minRestPeriod > 20)
                    {
                        T45 = 1; // 21
                    }
                    if (minRestPeriod > 21)
                    {
                        T46 = 1; // 22
                    }
                    if (minRestPeriod > 22)
                    {
                        T47 = 1; // 23
                    }
                    if (minRestPeriod > 23)
                    {
                        T48 = 1; // 24
                    }
                }

            }

            return new RestPeriodResult(returnState, returnText, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48);
        }
    }
}
