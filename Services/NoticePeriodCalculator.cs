using DanskMetal.Calculators.Api.Models;

namespace DanskMetal.Calculators.Api.Services
{


    public static class NoticePeriodCalculator
    {        

        public static NoticePeriodResult Calculate(NoticePeriodInput input)
        {
            string returnState = "OK";
            string noticePeriodStr = "N/A";
            string severencePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            /* Update list and values informations on both this class and NoticePeriodInput */
            /* Selected collective agreement list and values */
            // 1 = Industriens Overenskomst
            // 2 = Industriens FunktionærOverenskomst
            // 3 = Metal-Transportoverenskomsten
            // 4 = Overenskomsten for faglærte
            // 5 = Industri- og VVS-Overenskomsten
            // 6 = DI Byggeri
            // 7 = DM Arbejdsgiver
            // 8 = VVS - Overenskomsten
            // 9 = ABAF

            /* TerminatingParty list and values */
            // 0 = Terminated by Employer
            // 1 = Terminated by Employee

            /* SalariedEmployee list and values */
            // 0 = NOT on Danish Salaried Employees Act
            // 1 = IS on Danish Salaried Employees Act
            // 2 = IS on Danish Salaried Employees Act, but not by law, the Employee is on by the collective agreement and their contract.
            
            try
            {
                if (input.SelectedCollectiveAgreement != 0 && input.SelectedCollectiveAgreement! > 9)
                    throw new ArgumentException("Invalid input values.");

                switch (input.SelectedCollectiveAgreement)
                {
                    case 1: // Industriens Overenskomst
                        // Calling calculator
                        return CalculateIndustriensOverenskomst(input);

                    case 2: // Industriens FunktionærOverenskomst

                        returnState = "OK - Industriens FunktionærOverenskomst";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 3: // Metal-Transportoverenskomsten

                        returnState = "OK - Metal-Transportoverenskomsten";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 4: // Overenskomsten for faglærte

                        returnState = "OK - Overenskomsten for faglærte";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 5: // Industri- og VVS-Overenskomsten

                        returnState = "OK - Industri- og VVS-Overenskomsten";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 6: // DI Byggeri

                        returnState = "OK - DI Byggeri";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 7: // DM Arbejdsgiver

                        returnState = "OK - DM Arbejdsgiver";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 8: // VVS - Overenskomsten

                        returnState = "OK - VVS - Overenskomsten";
                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                    case 9: // ABAF

                        returnState = "OK - ABAF";


                        noticePeriodStr = "";
                        severencePayStr = "";
                        extraDisplayedInfoStr = "";
                        break;
                }

            }
            catch
            {
                returnState = "Error in selecting collective agreement";
            }

            return new NoticePeriodResult(returnState, noticePeriodStr, severencePayStr, extraDisplayedInfoStr);
        }

        private static NoticePeriodResult CalculateIndustriensOverenskomst(NoticePeriodInput input)
        {
            
            string returnState = "OK - Industriens Overenskomst";
            string noticePeriodStr = "";
            string severancePayStr = "";
            string extraDisplayedInfoStr = "";

            /* start calculation */
            // Vaiables
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // Fratrædelsesgodtgørelse
            // >= 3 år = mulighed for fratrædelsesgodtgørelse.
            DateOnly severancePay = input.ContractStartDate.AddYears(3);
            string severanceText = "Der kan være krav på en fratrædelsesgodtgørelse. i henhold til § 38.Stk. 11. kontakt din lokale dansk metal afdeling";

            /* Text from the collective agreement */
            // § 38 Opsigelsesregler
            // Ophævelse fra virksomhedens side
            // p1 < 6 måneder = Ingen varsel - tjek eventuelt om de har aftalt yderelige i kontrakten
            DateOnly p1Start = input.ContractStartDate;
            DateOnly p1End = input.ContractStartDate.AddMonths(6).AddDays(-1);
            
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p1End)
            {
                returnState = "OK P1";
                noticePeriodStr = "0 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // p2 >= 6 måneder og < 9 måneder = 14 dages varsel
            DateOnly p2Start = input.ContractStartDate.AddMonths(6);
            DateOnly p2End = input.ContractStartDate.AddMonths(9).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
            {
                returnState = "OK P2";
                noticePeriodStr = "14 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // p3 >= 9 måneder og < 2 år = 21 dages varsel
            DateOnly p3Start = input.ContractStartDate.AddMonths(9);
            DateOnly p3End = input.ContractStartDate.AddYears(2).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
            {
                returnState = "OK P3";
                noticePeriodStr = "21 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // p4 >= 2 år og < 3 år = 28 dages varsel
            DateOnly p4Start = input.ContractStartDate.AddYears(2);
            DateOnly p4End = input.ContractStartDate.AddYears(3).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
            {
                returnState = "OK P4";
                noticePeriodStr = "28 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // p5 >= 3 år og < 6 år = 56 dages varsel
            DateOnly p5Start = input.ContractStartDate.AddYears(3);
            DateOnly p5End = input.ContractStartDate.AddYears(6).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
            {
                returnState = "OK P5";
                noticePeriodStr = "56 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // p6 >= 6 år = 70 dages varsel
            DateOnly p6Start = input.ContractStartDate.AddYears(6);
            DateOnly p6End; // populated in the if statement
            if (age >= 50)
            {
                p6End = input.ContractStartDate.AddYears(9).AddDays(-1);
            } 
            else
            {
                p6End = input.ContractStartDate.AddYears(4000).AddDays(-1); // Way out in the future.
            }
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
            {
                returnState = "OK P6";
                noticePeriodStr = "70 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Medarbejdere på eller over 50 år
            // p7 >= 9 år < 12 år = 90 dages varsel
            DateOnly p7Start; // populated in the if statement
            DateOnly p7End; // populated in the if statement
            if (age >= 50)
            {
                p7Start = input.ContractStartDate.AddYears(9);
                p7End = input.ContractStartDate.AddYears(12).AddDays(-1);
            }
            else
            {
                p7Start = input.ContractStartDate.AddYears(5000);
                p7End = input.ContractStartDate.AddYears(5000);
            }
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p7End && input.ContractTerminatedDate >= p7Start && age >= 50)
            {
                returnState = "OK P7";
                noticePeriodStr = "90 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // p8 >= 12 år = 120 dages varsel
            DateOnly p8Start; // populated in the if statement
            DateOnly p8End; // populated in the if statement
            if (age >= 50)
            {
                p8Start = input.ContractStartDate.AddYears(12);
                p8End = input.ContractStartDate.AddYears(5000).AddDays(-1);
            }
            else
            {
                p8Start = input.ContractStartDate.AddYears(5000);
                p8End = input.ContractStartDate.AddYears(5000);
            }
            if (input.TerminatingParty == 0 /* Terminated by company */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p8End && input.ContractTerminatedDate >= p8Start && age >= 50)
            {
                returnState = "OK P8";
                noticePeriodStr = "120 dages varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // Ophævelse fra medarbejderens side
            // < 6 måneder = Ingen varsel - tjek eventuelt om de har aftalt yderelige i kontrakten
            DateOnly p1mStart = input.ContractStartDate;
            DateOnly p1mEnd = input.ContractStartDate.AddMonths(6).AddDays(-1);
            // >= 6 måneder og < 3 år = 7 dages varsel
            DateOnly p2mStart = input.ContractStartDate.AddMonths(6);
            DateOnly p2mEnd = input.ContractStartDate.AddYears(3).AddDays(-1);
            // >= 3 år og < 6 år = 14 dages varsel
            DateOnly p3mStart = input.ContractStartDate.AddYears(3);
            DateOnly p3mEnd = input.ContractStartDate.AddYears(6).AddDays(-1);
            // >= 6 år og < 9 år = 21 dages varsel
            DateOnly p4mStart = input.ContractStartDate.AddYears(6);
            DateOnly p4mEnd = input.ContractStartDate.AddYears(9).AddDays(-1);
            // >= 9 år = 28 dages varsel
            DateOnly p6mStart = input.ContractStartDate.AddYears(9);
            DateOnly p6mEnd = input.ContractStartDate.AddYears(4000).AddDays(-1); // Way out in the future.;

            // cD = contractDuration
            var cDYears = input.ContractTerminatedDate.Year - input.ContractStartDate.Year; 
            var cDMonths = input.ContractTerminatedDate.Month - input.ContractStartDate.Month;
            var cDDays = input.ContractTerminatedDate.Day - input.ContractStartDate.Day;

                // Test output
                returnState = age + " years old";

            // Return answer to API stack
            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    }
}
