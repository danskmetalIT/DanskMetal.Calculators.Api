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

            // Severance pay dates
            // not on danish salaried employees act
            // >= 3 år = mulighed for fratrædelsesgodtgørelse.
            DateOnly severancePay = input.ContractStartDate.AddYears(3);
            string severanceText = "Der kan være krav på en fratrædelsesgodtgørelse. i henhold til § 38.Stk. 11. kontakt din lokale dansk metal afdeling";
            // on danish salaried employees act
            DateOnly severancePayS1 = input.ContractStartDate.AddYears(3);
            string severanceTextS1 = "Der kan være krav på en fratrædelsesgodtgørelse. i henhold til § 38.Stk. 11. kontakt din lokale dansk metal afdeling";
            DateOnly severancePayS2 = input.ContractStartDate.AddYears(12);
            string severanceTextS2 = "Der kan være krav på en fratrædelsesgodtgørelse. Enten FUL §2a godtgørelse eller § 38. Stk. 11 i henhold til billag 5. kontakt din lokale dansk metal afdeling";

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

            //// If the employee is terminated by the employee, and is NOT on the Danish Salaried Employees Act
            if (input.TerminatingParty == 1 /* Terminated by employee */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */)
            {
                if (input.ContractTerminatedDate <= p1mEnd && input.ContractTerminatedDate >= p1mStart)
                {
                    returnState = "OK P1M";
                    noticePeriodStr = "0 dages varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "N/A";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
                else if (input.ContractTerminatedDate <= p2mEnd && input.ContractTerminatedDate >= p2mStart)
                {
                    returnState = "OK P2M";
                    noticePeriodStr = "7 dages varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "N/A";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
                else if (input.ContractTerminatedDate <= p3mEnd && input.ContractTerminatedDate >= p3mStart)
                {
                    returnState = "OK P3M";
                    noticePeriodStr = "14 dages varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "N/A";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
                else if (input.ContractTerminatedDate <= p4mEnd && input.ContractTerminatedDate >= p4mStart)
                {
                    returnState = "OK P4M";
                    noticePeriodStr = "21 dages varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "N/A";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
                else if (input.ContractTerminatedDate <= p6mEnd && input.ContractTerminatedDate >= p6mStart)
                {
                    returnState = "OK P6M";
                    noticePeriodStr = "28 dages varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "N/A";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
            }

            if (input.SalariedEmployee == 1 || input.SalariedEmployee == 2 /* On Danish Salaried Employees Act */
                && input.TerminatingParty == 0)
            {
                // If the employee is terminated by the company, and is on the Danish Salaried Employees Act
                // Opsigelsesvarsel:
                // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
                DateOnly p1fStart = input.ContractStartDate;
                DateOnly p1fEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);

                if (input.ContractTerminatedDate <= p1fEnd)
                {
                    returnState = "OK P1f";
                    noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }

                // 3 måneder - 14 dage til 5 måneder Løbende - måned + 1 måned.
                DateOnly p2fStart = input.ContractStartDate.AddMonths(3).AddDays(-14);
                DateOnly p2fEnd = input.ContractStartDate.AddMonths(5).AddDays(-1);

                if (input.ContractTerminatedDate <= p2fEnd && input.ContractTerminatedDate >= p2fStart)
                {
                    returnState = "OK P2f";
                    noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }

                // 5 måneder til 2 år og 9 måneder - Løbende måned + 3 måneder.
                DateOnly p3fStart = input.ContractStartDate.AddMonths(5);
                DateOnly p3fEnd = input.ContractStartDate.AddYears(2).AddMonths(9).AddDays(-1);

                if (input.ContractTerminatedDate <= p3fEnd && input.ContractTerminatedDate >= p3fStart)
                {
                    returnState = "OK P3f";
                    noticePeriodStr = "Løbende måned plus 3 måneds varsel";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }

                // 2 år og 9 måneder til 5 år og 8 måneder - Løbende måned + 4 måneder.
                DateOnly p4fStart = input.ContractStartDate.AddYears(2).AddMonths(9);
                DateOnly p4fEnd = input.ContractStartDate.AddYears(5).AddMonths(8).AddDays(-1);

                if (input.ContractTerminatedDate <= p4fEnd && input.ContractTerminatedDate >= p4fStart)
                {
                    returnState = "OK P4f";
                    noticePeriodStr = "Løbende måned plus 4 måneds varsel";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }

                // 5 år og 8 måneder til 8 år og 7 måneder - Løbende måned + 5 måneder. 
                DateOnly p5fStart = input.ContractStartDate.AddYears(5).AddMonths(8);
                DateOnly p5fEnd = input.ContractStartDate.AddYears(8).AddMonths(7).AddDays(-1);

                if (input.ContractTerminatedDate <= p5fEnd && input.ContractTerminatedDate >= p5fStart)
                {
                    returnState = "OK P5f";
                    noticePeriodStr = "Løbende måned plus 5 måneds varsel";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }

                // Ansat i mere end 8 år og 7 måneder - Løbende måned + 6 måneder. (103+=m)
                DateOnly p6fStart = input.ContractStartDate.AddYears(8).AddMonths(7);
                DateOnly p6fEnd = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future.

                if (input.ContractTerminatedDate <= p6fEnd && input.ContractTerminatedDate >= p6fStart)
                {
                    returnState = "OK P6f";
                    noticePeriodStr = "Løbende måned plus 6 måneds varsel";
                    severancePayStr = "N/A";
                    if (input.ContractTerminatedDate >= severancePayS1 && input.ContractTerminatedDate < severancePayS2) { severancePayStr = severanceTextS1; } else if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
            }

            if (input.SalariedEmployee == 1 || input.SalariedEmployee == 2 /* On Danish Salaried Employees Act */
                && input.TerminatingParty == 1)
            {
                // If the employee is terminated by the employee, and is on the Danish Salaried Employees Act
                // 14 dages varsel som skal ligge indenfor den periode. (3m - 14 dage)
                DateOnly p1fmStart = input.ContractStartDate;
                DateOnly p1fmEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);

                if (input.ContractTerminatedDate <= p1fmEnd)
                {
                    returnState = "OK P1f";
                    noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }


                // Opsigelsesvarsel:
                // 3 måneder+ - Løbende måned + 1 måned. 
                DateOnly p2fmStart = input.ContractStartDate.AddMonths(3).AddDays(-14);
                DateOnly p2fmEnd = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future

                if (input.ContractTerminatedDate <= p2fmEnd && input.ContractTerminatedDate >= p2fmStart)
                {
                    returnState = "OK P2f";
                    noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                    severancePayStr = "N/A";
                    extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse.";
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
            }

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
