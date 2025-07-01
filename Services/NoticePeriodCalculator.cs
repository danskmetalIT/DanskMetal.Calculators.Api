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
                        // Calling calculator
                        return CalculateIndustriensFunktionaerOvenskomst(input);
                    case 3: // Metal-Transportoverenskomsten
                        // Calling calculator
                        return CalculateMetalTransportoverenskomsten(input);
                    case 4: // Overenskomsten for faglærte
                        // Calling calculator
                        return CalculateOverenskomstenForFaglaerte(input);
                    case 5: // Industri- og VVS-Overenskomsten
                        // Calling calculator
                        return CalculateIndustriOgVVSOverenskomsten(input);
                    case 6: // DI Byggeri
                        // Calling calulator
                        return CalculateDanskByggeri(input);
                    case 7: // DM Arbejdsgiver
                        // Calling calculator
                        return CalculateDMArbejdsgiver(input);
                    case 8: // VVS - Overenskomsten
                        //Calling calculator
                        return CalculateVVSoverenskomst(input);
                    case 9: // ABAF
                        // Calling calculator
                        return CalculateABAF(input);
                }

            }
            catch
            {
                returnState = "Error in selecting collective agreement calculator";
            }

            return new NoticePeriodResult(returnState, noticePeriodStr, severencePayStr, extraDisplayedInfoStr);
        }

        private static NoticePeriodResult CalculateIndustriensOverenskomst(NoticePeriodInput input)
        {
            
            string returnState = "Error CalculateIndustriensOverenskomst";
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
            string severanceTextS1 = "N/A";
            if (input.SalariedEmployee == 2) { severanceTextS1 = "Der kan være krav på en fratrædelsesgodtgørelse. i henhold til § 38.Stk. 11. kontakt din lokale dansk metal afdeling"; }
            DateOnly severancePayS2 = input.ContractStartDate.AddYears(12);
            string severanceTextS2 = "Der kan være krav på en fratrædelsesgodtgørelse. Enten FUL §2a godtgørelse eller § 38. Stk. 11 i henhold til bilag 5. kontakt din lokale dansk metal afdeling";

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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; }
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
                    extraDisplayedInfoStr = "N/A";
                    if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = "Bilag 5 Opsigelsesvarslernes længde kan ikke blive kortere end de i henhold til Industriens Overenskomst opnåede ved overgang til funktionærlignende ansættelse."; } 
                    return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                }
            }

            // Return answer to API stack Fallback
            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    
        private static NoticePeriodResult CalculateIndustriensFunktionaerOvenskomst(NoticePeriodInput input)
        {
            string returnState = "Error CalculateIndustriensFunktionaerOvenskomst";
            string noticePeriodStr = "";
            string severancePayStr = "";
            string extraDisplayedInfoStr = "";

            /* start calculation */
            // Vaiables
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // Severance pay dates
            DateOnly severancePayS2 = input.ContractStartDate.AddYears(12);
            string severanceTextS2 = "Der kan være krav på en fratrædelsesgodtgørelse. Enten FUL §2a godtgørelse eller § 38. Stk. 11 i henhold til billag 5. kontakt din lokale dansk metal afdeling";

            // § 15 Opsigelsesvarsler
            // Stk. 1
            // For medarbejdere, der er funktionærer, henvises til Funktionærloven.

            // Opsigelse fra virksomhedens side funktionær og funktionær lign.
            // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
            DateOnly p1Start = input.ContractStartDate;
            DateOnly p1End = input.ContractStartDate.AddMonths(3).AddDays(-15);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p1End)
            {
                returnState = "OK Funk P1";
                noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // 3 måneder - 14 dage til 5 måneder Løbende - måned + 1 måned. (5m - 1 dag)
            DateOnly p2Start = input.ContractStartDate.AddMonths(3).AddDays(-14);
            DateOnly p2End = input.ContractStartDate.AddMonths(5).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
            {
                returnState = "OK Funk P2";
                noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // 5 måneder til 2 år og 9 måneder - Løbende måned + 3 måneder. (33m - 1 dag)
            DateOnly p3Start = input.ContractStartDate.AddMonths(5);
            DateOnly p3End = input.ContractStartDate.AddYears(2).AddMonths(9).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
            {
                returnState = "OK Funk P3";
                noticePeriodStr = "Løbende måned plus 3 måneders varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // 2 år og 9 måneder til 5 år og 8 måneder - Løbende måned + 4 måneder. (68m - 1 dag)
            DateOnly p4Start = input.ContractStartDate.AddYears(2).AddMonths(9);
            DateOnly p4End = input.ContractStartDate.AddYears(5).AddMonths(8).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
            {
                returnState = "OK Funk P4";
                noticePeriodStr = "Løbende måned plus 4 måneders varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // 5 år og 8 måneder til 8 år og 7 måneder - Løbende måned + 5 måneder. (103m - 1 dag)
            DateOnly p5Start = input.ContractStartDate.AddYears(5).AddMonths(8);
            DateOnly p5End = input.ContractStartDate.AddYears(8).AddMonths(7).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
            {
                returnState = "OK Funk P5";
                noticePeriodStr = "Løbende måned plus 5 måneders varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Ansat i mere end 8 år og 7 måneder - Løbende måned + 6 måneder. (103+=m)
            DateOnly p6Start = input.ContractStartDate.AddYears(8).AddMonths(7);
            DateOnly p6End = input.ContractStartDate.AddYears(5000); // Way out in the future.
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee != 0 /* On on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
            {
                returnState = "OK Funk P6";
                noticePeriodStr = "Løbende måned plus 6 måneders varsel";
                severancePayStr = "N/A";
                if (input.ContractTerminatedDate >= severancePayS2) { severancePayStr = severanceTextS2; }
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // Opsigelse fra medarbejderens side funktionær og funktionær lign.
            // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
            DateOnly p1mStart = input.ContractStartDate;
            DateOnly p1mEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);
            if (input.TerminatingParty == 1 /* Terminated by employee */ && input.SalariedEmployee != 0 /* On Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p1mEnd)
            {
                returnState = "OK Funk M P1";
                noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                severancePayStr = "N/A";
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // 3 måneder+ - Løbende måned + 1 måned.
            DateOnly p2mStart = input.ContractStartDate;
            DateOnly p2mEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);
            if (input.TerminatingParty == 1 /* Terminated by employee */ && input.SalariedEmployee != 0 /* On Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p2mEnd && input.ContractTerminatedDate >= p2mStart)
            {
                returnState = "OK Funk M P2";
                noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                severancePayStr = "N/A";
                extraDisplayedInfoStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // Set Special note for persons "Not on Danish Salaried Employees Act"
            extraDisplayedInfoStr = "§ 15 Opsigelsesvarsler\r\nStk. 3\r\nI tilfælde, hvor en timelønnet medarbejder bliver funktionær på samme virksomhed, bevarer medarbejderen det opsigelsesvarsel, der var gældende på overflytningstidspunktet, indtil den pågældende i henhold til Funktionærloven opnår mindst samme opsigelsesvarsel.";

            // § 15 Opsigelsesvarsler
            // Stk. 2
            // For medarbejdere, der ikke er omfattet af Funktionærloven, gælder
            // følgende opsigelsesvarsler, jf. § 1, stk. 3:
            // I de første 3 måneder efter ansættelsen kan opsigelse fra begge sider
            // ske uden varsel, således at fratræden sker ved normal arbejdstids ophør den pågældende dag.
            // Fra medarbejderside:
            DateOnly p1nfStart = input.ContractStartDate;
            DateOnly p1nfEnd = input.ContractStartDate.AddMonths(3).AddDays(-1);
            if (input.TerminatingParty == 1 /* Terminated by employee */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p1nfEnd)
            {
                returnState = "OK NOT Funk P1";
                noticePeriodStr = "0 dages varsel, med mindre andet er aftalt lokalt";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Efter 3 måneders uafbrudt beskæftigelse:
            DateOnly p2nfmStart = input.ContractStartDate;
            DateOnly p2nfmEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);
            if (input.TerminatingParty == 1 /* Terminated by employee */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p2nfmEnd && input.ContractTerminatedDate >= p2nfmStart)
            {
                returnState = "OK NOT Funk M P2";
                noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }

            // Fra arbejdsgiverside:
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p1nfEnd)
            {
                returnState = "OK NOT Funk P1";
                noticePeriodStr = "0 dages varsel, med mindre andet er aftalt lokalt";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Efter 3 måneders uafbrudt beskæftigelse:
            DateOnly p2nfStart = input.ContractStartDate.AddMonths(3);
            DateOnly p2nfEnd = input.ContractStartDate.AddYears(2).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p2nfEnd && input.ContractTerminatedDate >= p2nfStart)
            {
                returnState = "OK NOT Funk P2";
                noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Efter 2 års uafbrudt beskæftigelse:
            DateOnly p3nfStart = input.ContractStartDate.AddYears(2);
            DateOnly p3nfEnd = input.ContractStartDate.AddYears(3).AddDays(-1);
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p3nfEnd && input.ContractTerminatedDate >= p3nfStart)
            {
                returnState = "OK NOT Funk P3";
                noticePeriodStr = "Løbende måned plus 2 måneders varsel";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Efter 3 års uafbrudt beskæftigelse:
            DateOnly p4nfStart = input.ContractStartDate.AddYears(3);
            DateOnly p4nfEnd = input.ContractStartDate.AddYears(5000); // Way out in the future.
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */
                && input.ContractTerminatedDate <= p4nfEnd && input.ContractTerminatedDate >= p4nfStart)
            {
                returnState = "OK NOT Funk P4";
                noticePeriodStr = "Løbende måned plus 3 måneders varsel";
                severancePayStr = "N/A";
                return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
            }
            // Return answer to API stack
            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    
        private static NoticePeriodResult CalculateMetalTransportoverenskomsten(NoticePeriodInput input)
        {

            // Same as Industriens overenskomst
            // I Herefor call Industriens overenskomst instead of recreating it again
            var result = CalculateIndustriensOverenskomst(input);
            string returnState = "Error CalculateMetalTransportoverenskomsten";
            //string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            if (input.SalariedEmployee != 0 /* On Danish Salaried Employees Act */)
            {
                extraDisplayedInfoStr = "Bilag 8\r\nParterne er enige om, at opsigelsesvarslernes længde ikke kan være kortere end de i henhold til overenskomsten opnåede ved overgang til funktionærlignende ansættelse.";
            }

            // SeverancePay
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(3);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse. § 10. Stk. 7. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 1 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en 2a godtgørelse i henhold til bilag 8 Opsigelse. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 1 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en 2a godtgørelse. Kontakt din afdeling";
                }
            }

            return new NoticePeriodResult(returnState, result.NoticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    
        private static NoticePeriodResult CalculateOverenskomstenForFaglaerte(NoticePeriodInput input)
        {
            string returnState = "Error CalculateOverenskomstenForFaglaerte";
            string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            // Severance pay dates
            // not on danish salaried employees act
            DateOnly severancePay = input.ContractStartDate.AddYears(3);
            string severanceText = "Der kan være krav på en fratrædelsesgodtgørelse. § 13. Stk. 7. kontakt din lokale dansk metal afdeling";
            // on danish salaried employees act
            DateOnly severancePayS1 = input.ContractStartDate.AddYears(3);
            string severanceTextS1 = "Der kan være krav på en 2a godtgørelse i henhold til funktionærloven. kontakt din lokale dansk metal afdeling";

            // Calculating age
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // SalariedEmployee == 0
            // Det er ancienniteten på opsigelsestidspunktet, der er afgørende for de under a) og b) anførte opsigelsesvarsler.
            if (input.SalariedEmployee == 0)
            {
                if (input.TerminatingParty == 0)
                {
                    // Fra arbejdsgiverens side
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK P1";
                        noticePeriodStr = "0 dages varsel, medmindre andet er aftalt i kontrakten";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 9 måneders beskæftigelse 21 dage
                    DateOnly p2Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p2End = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK P2";
                        noticePeriodStr = "21 dages varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 3 års beskæftigelse 49 dage
                    DateOnly p3Start = input.ContractStartDate.AddYears(3);
                    DateOnly p3End = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
                    {
                        returnState = "OK P3";
                        noticePeriodStr = "49 dages varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 6 års beskæftigelse 70 dage
                    DateOnly p4Start = input.ContractStartDate.AddYears(6);
                    DateOnly p4End = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future.
                    if (age >= 50)
                    {
                        p4End = input.ContractStartDate.AddYears(9).AddDays(-1);
                    }
                    if (input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
                    {
                        returnState = "OK P4";
                        noticePeriodStr = "70 dages varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Medarbejdere, der er fyldt 50 år
                    // Efter 9 års beskæftigelse 90 dage
                    DateOnly p5Start;
                    DateOnly p5End;
                    if (age >= 50)
                    {
                        p5Start = input.ContractStartDate.AddYears(9);
                        p5End = input.ContractStartDate.AddYears(12).AddDays(-1);
                        if (input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
                        {
                            returnState = "OK P5";
                            noticePeriodStr = "90 dages varsel";
                            severancePayStr = "N/A";
                            if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                            extraDisplayedInfoStr = "N/A";
                            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                        }
                    }

                    // Efter 12 års beskæftigelse 120 dage
                    DateOnly p6Start;
                    DateOnly p6End;
                    if (age >= 50)
                    {
                        p6Start = input.ContractStartDate.AddYears(12);
                        p6End = input.ContractStartDate.AddYears(5000).AddDays(-1);
                        if (input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
                        {
                            returnState = "OK P6";
                            noticePeriodStr = "120 dages varsel";
                            severancePayStr = "N/A";
                            if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                            extraDisplayedInfoStr = "N/A";
                            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                        }
                    }
                }

                if (input.TerminatingParty == 1)
                {
                    // Fra medarbejderens side
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK M P1";
                        noticePeriodStr = "0 dages varsel, medmindre andet er aftalt i kontrakten";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 9 måneders beskæftigelse 7 dage
                    DateOnly p2Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p2End = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK M P2";
                        noticePeriodStr = "7 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 3 års beskæftigelse 14 dage
                    DateOnly p3Start = input.ContractStartDate.AddYears(3);
                    DateOnly p3End = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
                    {
                        returnState = "OK M P3";
                        noticePeriodStr = "14 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 års beskæftigelse 21 dage
                    DateOnly p4Start = input.ContractStartDate.AddYears(6);
                    DateOnly p4End = input.ContractStartDate.AddYears(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
                    {
                        returnState = "OK M P4";
                        noticePeriodStr = "21 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 9 års beskæftigelse 28 dage
                    DateOnly p5Start = input.ContractStartDate.AddYears(9);
                    DateOnly p5End = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future.
                    if (input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
                    {
                        returnState = "OK M P5";
                        noticePeriodStr = "28 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }

            }

            // SalariedEmployee != 0
            if (input.SalariedEmployee != 0)
            {
                extraDisplayedInfoStr = "Bilag 10\r\nParterne er enige om, at opsigelsesvarslernes længde ikke kan være kortere end de i henhold til overenskomsten opnåede ved overgang til funktionærlignende ansættelse.";

                if (input.TerminatingParty == 0)
                {
                    // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(3).AddDays(-15);
                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK Funk P1";
                        noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // 3 måneder - 14 dage til 5 måneder Løbende - måned + 1 måned. (5m - 1 dag)
                    DateOnly p2Start = input.ContractStartDate.AddMonths(3).AddDays(-14);
                    DateOnly p2End = input.ContractStartDate.AddMonths(5).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK Funk P2";
                        noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // 5 måneder til 2 år og 9 måneder - Løbende måned + 3 måneder. (33m - 1 dag)
                    DateOnly p3Start = input.ContractStartDate.AddMonths(5);
                    DateOnly p3End = input.ContractStartDate.AddYears(2).AddMonths(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
                    {
                        returnState = "OK Funk P3";
                        noticePeriodStr = "Løbende måned plus 3 måneders varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // 2 år og 9 måneder til 5 år og 8 måneder - Løbende måned + 4 måneder. (68m - 1 dag)
                    DateOnly p4Start = input.ContractStartDate.AddYears(2).AddMonths(9);
                    DateOnly p4End = input.ContractStartDate.AddYears(5).AddMonths(8).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
                    {
                        returnState = "OK Funk P4";
                        noticePeriodStr = "Løbende måned plus 4 måneders varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // 5 år og 8 måneder til 8 år og 7 måneder - Løbende måned + 5 måneder. (103m - 1 dag)
                    DateOnly p5Start = input.ContractStartDate.AddYears(5).AddMonths(8);
                    DateOnly p5End = input.ContractStartDate.AddYears(8).AddMonths(7).AddDays(-1);
                    if (input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
                    {
                        returnState = "OK Funk P5";
                        noticePeriodStr = "Løbende måned plus 5 måneders varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Ansat i mere end 8 år og 7 måneder - Løbende måned + 6 måneder. (103+=m)
                    DateOnly p6Start = input.ContractStartDate.AddYears(8).AddMonths(7);
                    DateOnly p6End = input.ContractStartDate.AddYears(5000); // Way out in the future.
                    if (input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
                    {
                        returnState = "OK Funk P6";
                        noticePeriodStr = "Løbende måned plus 6 måneders varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
  
                }

                if (input.TerminatingParty == 1)
                {
                    // Opsigelse fra medarbejderens side funktionær og funktionær lign.
                    // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
                    DateOnly p1mStart = input.ContractStartDate;
                    DateOnly p1mEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);
                    if (input.ContractTerminatedDate <= p1mEnd)
                    {
                        returnState = "OK Funk M P1";
                        noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                        severancePayStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // 3 måneder+ - Løbende måned + 1 måned.
                    DateOnly p2mStart = input.ContractStartDate;
                    DateOnly p2mEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);
                    if (input.ContractTerminatedDate <= p2mEnd && input.ContractTerminatedDate >= p2mStart)
                    {
                        returnState = "OK Funk M P2";
                        noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                        severancePayStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }

            }
            
            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }

        private static NoticePeriodResult CalculateIndustriOgVVSOverenskomsten(NoticePeriodInput input)
        {
            // Same as Industriens overenskomst
            // I Herefor call Industriens overenskomst instead of recreating it again
            var result = CalculateIndustriensOverenskomst(input);
            string returnState = "Error CalculateIndustriOgVVSOverenskomsten";
            //string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            // SeverancePay
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(3);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse. § 37. Stk. 8. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 1 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse en 2a godtgørelse. OK Bilag 13 Stk. 3: Opsigelse. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 2 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse en 2a godtgørelse. OK Bilag 13 Stk. 3: Opsigelse. Kontakt din afdeling";
                }
            }

            return new NoticePeriodResult(returnState, result.NoticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    
        private static NoticePeriodResult CalculateDanskByggeri(NoticePeriodInput input)
        {
            string returnState = "Error CalculateDanskByggeri";
            string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            // calculate age
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // SalariedEmployee == 0
            if (input.SalariedEmployee == 0)
            {
                if (input.TerminatingParty == 0)
                {
                    // Fra virksomhedens side:
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(6).AddDays(-1);

                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK P1";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 måneders beskæftigelse .......................14 dage
                    DateOnly p2Start = input.ContractStartDate.AddMonths(6);
                    DateOnly p2End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK P2";
                        noticePeriodStr = "14 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 9 måneders beskæftigelse .......................21 dage
                    DateOnly p3Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p3End = input.ContractStartDate.AddYears(2).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
                    {
                        returnState = "OK P3";
                        noticePeriodStr = "21 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 2 års beskæftigelse.............................28 dage
                    DateOnly p4Start = input.ContractStartDate.AddYears(2);
                    DateOnly p4End = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
                    {
                        returnState = "OK P4";
                        noticePeriodStr = "28 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 3 års beskæftigelse.............................56 dage
                    DateOnly p5Start = input.ContractStartDate.AddYears(3);
                    DateOnly p5End = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
                    {
                        returnState = "OK P5";
                        noticePeriodStr = "56 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 års beskæftigelse.............................70 dage
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
                    if (input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
                    {
                        returnState = "OK P6";
                        noticePeriodStr = "70 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Medarbejdere, der er fyldt 50 år:
                    // Efter 9 års beskæftigelse.............................90 dage
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
                    if (input.ContractTerminatedDate <= p7End && input.ContractTerminatedDate >= p7Start && age >= 50)
                    {
                        returnState = "OK P7";
                        noticePeriodStr = "90 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                }
                if (input.TerminatingParty == 1)
                {
                    // Fra medarbejderens side:
                    DateOnly p1mStart = input.ContractStartDate;
                    DateOnly p1mEnd = input.ContractStartDate.AddMonths(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p1mEnd && input.ContractTerminatedDate >= p1mStart)
                    {
                        returnState = "OK P1M";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 6 måneders beskæftigelse ........................7 dage
                    DateOnly p2mStart = input.ContractStartDate.AddMonths(6);
                    DateOnly p2mEnd = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2mEnd && input.ContractTerminatedDate >= p2mStart)
                    {
                        returnState = "OK P2M";
                        noticePeriodStr = "7 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 3 års beskæftigelse.............................14 dage
                    DateOnly p3mStart = input.ContractStartDate.AddYears(3);
                    DateOnly p3mEnd = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3mEnd && input.ContractTerminatedDate >= p3mStart)
                    {
                        returnState = "OK P3M";
                        noticePeriodStr = "14 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Efter 6 års beskæftigelse ............................21 dage
                    DateOnly p4mStart = input.ContractStartDate.AddYears(6);
                    DateOnly p4mEnd = input.ContractStartDate.AddYears(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4mEnd && input.ContractTerminatedDate >= p4mStart)
                    {
                        returnState = "OK P4M";
                        noticePeriodStr = "21 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                        // Efter 9 års beskæftigelse.............................28 dage
                    DateOnly p6mStart = input.ContractStartDate.AddYears(9);
                    DateOnly p6mEnd = input.ContractStartDate.AddYears(4000).AddDays(-1); // Way out in the future.;
                    if (input.ContractTerminatedDate <= p6mEnd && input.ContractTerminatedDate >= p6mStart)
                    {
                        returnState = "OK P6M";
                        noticePeriodStr = "28 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = "N/A";
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Det er ancienniteten på opsigelsestidspunktet, der er afgørende
                    // for de anførte opsigelsesvarsler.
                }
            }

            if (input.SalariedEmployee == 1)
            {
                extraDisplayedInfoStr = "§ 3 Ansættelse på funktionærlignende vilkår\r\nStk 8. I tilfælde af opsigelse regnes opsigelsesvarslets længde for begge parter efter reglerne i funktionærlovens § 2.\r\nParterne er enige om, at opsigelsesvarslernes længde ikke kan blive kortere end de i henhold til overenskomsten opnåede ved overgang til funktionærlignende ansættelse.";
                DateOnly severancePayS1 = input.ContractStartDate.AddYears(12);
                string severanceTextS1 = "Der kan være krav på en fratrædelsesgodtgørelse. En §2a godtgørelse i henhold til funktionærloven. Kontakt din afdeling";
                
                if (input.TerminatingParty == 0)
                {
                    // startdato til 3 måneder - 14 dage - 14 dages opsigelse eller samme som nedenstående hvis ingen prøveperiode.
                    DateOnly p1fStart = input.ContractStartDate;
                    DateOnly p1fEnd = input.ContractStartDate.AddMonths(3).AddDays(-15);

                    if (input.ContractTerminatedDate <= p1fEnd)
                    {
                        returnState = "OK P1f";
                        noticePeriodStr = "14 dages varsel hvis aftalt prøvetid, ellers løbende måned plus 1 måned";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
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
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
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
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
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
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
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
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
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
                        if (input.ContractTerminatedDate >= severancePayS1) { severancePayStr = severanceTextS1; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                }
                if (input.TerminatingParty == 1)
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
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }
            }

            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
        
        private static NoticePeriodResult CalculateDMArbejdsgiver(NoticePeriodInput input)
        {
            string returnState = "Error CalculateDMArbejdsgiver";
            string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            // SeverancePay
            DateOnly severancePay = input.ContractStartDate.AddYears(3);
            string severanceText = "Der kan være krav på en fratrædelsesgodtgørelse. § 15. Fratrædelsesgodtgørelse. Kontakt din afdeling."; // After 3 years. SalariedEmployee == 0
            DateOnly severancePay1 = input.ContractStartDate.AddYears(12);
            string severanceText1 = "Der kan være krav på FUL §2a godtgørelse. Kontakt din afdeling."; // After 12 years. SalariedEmployee == 1
            DateOnly severancePay2 = input.ContractStartDate.AddYears(12);
            string severanceText2 = "Der kan være krav på en FUL §2a godtgørelse i henhold til § 16 - Funktionærlignende ansættelse Stk. 9. Kontakt din afdeling"; // After 12 years. SalariedEmployee == 2


            string extraDisplayedInfoStr1 = "§ 14 – Opsigelsesregler\r\nStk. 1 Litra a. Inden for de første 6 måneder er ingen af parterne forpligtede til at afgive noget varsel i forbindelse med en afbrydelse af ansættelsesforholdet, idet dog sidste arbejdsdag altid skal være en fredag eller ugens sidste arbejdsdag.";
            string extraDisplayedInfoStr2 = "§ 14 – Opsigelsesregler\r\nStk. 1 Litra c. Opsigelsen skal være meddelt senest dagen før, varslet skal begynde at løbe – dvs. torsdag i en uge til fratræden en fredag. Feriefridage kan ikke indgå i varslet.";
            // SalariedEmployee == 2
            string extraDisplayedInfoStr3 = "§ 16 – Funktionærlignende ansættelse\r\nStk. 6 – Anciennitet og opsigelse\r\na. Anciennitet ved ansættelse på funktionærlignende vilkår regnes fra den \r\n1. i den måned, hvor aftalen træder i kraft.\r\nb. I tilfælde af opsigelse regnes opsigelsesvarslets længde efter reglerne i \r\nfunktionærlovens § 2. Parterne er enige om, at opsigelsesvarslets længde \r\nikke kan blive kortere end de i henhold til overenskomstens opnåede ved \r\novergang til funktionærlignende ansættelse.\r\n";

            // age
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // Text from the collective agreement
            // § 14 – Opsigelsesregler
            // Stk. 1 – Opsigelsesvarsler a.
            // Inden for de første 6 måneder er ingen af parterne forpligtede til at
            // afgive noget varsel i forbindelse med en afbrydelse af ansættelsesforholdet,
            // idet dog sidste arbejdsdag altid skal være en fredag eller ugens sidste arbejdsdag.

            // Stk. 1 – Opsigelsesvarsler b.
            // Opsigelse skal altid ske til udløbet af en uge.

            // Stk. 1 – Opsigelsesvarsler c.
            // Opsigelsen skal være meddelt senest dagen før, varslet skal begynde at
            // løbe – dvs.torsdag i en uge til fratræden en fredag. Feriefridage kan ikke
            // indgå i varslet.

            if (input.SalariedEmployee == 0)
            {
                if (input.TerminatingParty == 0)
                {
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(6).AddDays(-1);

                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK P1";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 måneders beskæftigelse 2 uger
                    DateOnly p2Start = input.ContractStartDate.AddMonths(6);
                    DateOnly p2End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK P2";
                        noticePeriodStr = "2 ugers varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 9 måneders beskæftigelse 3 uger
                    DateOnly p3Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p3End = input.ContractStartDate.AddYears(2).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3End && input.ContractTerminatedDate >= p3Start)
                    {
                        returnState = "OK P3";
                        noticePeriodStr = "3 ugers varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 2 års beskæftigelse 4 uger
                    DateOnly p4Start = input.ContractStartDate.AddYears(2);
                    DateOnly p4End = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4End && input.ContractTerminatedDate >= p4Start)
                    {
                        returnState = "OK P4";
                        noticePeriodStr = "4 ugers varsel";
                        severancePayStr = "N/A";
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 3 års beskæftigelse 8 uger
                    DateOnly p5Start = input.ContractStartDate.AddYears(3);
                    DateOnly p5End = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p5End && input.ContractTerminatedDate >= p5Start)
                    {
                        returnState = "OK P5";
                        noticePeriodStr = "8 ugers varsel";
                        severancePayStr = severanceText;
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 års beskæftigelse 10 uger
                    DateOnly p6Start = input.ContractStartDate.AddYears(6);
                    DateOnly p6End;
                    if (age >= 50) { p6End = input.ContractStartDate.AddYears(9).AddDays(-1); } else { p6End = input.ContractStartDate.AddYears(5000).AddDays(-1); } // Way out in the future
                    
                    if (input.ContractTerminatedDate <= p6End && input.ContractTerminatedDate >= p6Start)
                    {
                        returnState = "OK P6";
                        noticePeriodStr = "10 ugers varsel";
                        severancePayStr = severanceText;
                        if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    
                    // Medarbejdere, der er fyldt 50 år og har mindst
                    // 9 års anciennitet på virksomheden 3 mdr.
                    if (age >= 50)
                        {
                        DateOnly p7Start = input.ContractStartDate.AddYears(9);
                        DateOnly p7End = input.ContractStartDate.AddYears(12).AddDays(-1);
                        if (input.ContractTerminatedDate <= p7End && input.ContractTerminatedDate >= p7Start)
                        {
                             returnState = "OK P7";
                             noticePeriodStr = "3 måneders varsel";
                             severancePayStr = severanceText;
                             if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                             extraDisplayedInfoStr = extraDisplayedInfoStr2;
                             return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                        }
                    }

                    // Medarbejdere, der er fyldt 50 år og har mindst
                    // 12 års anciennitet på virksomheden 4 mdr.
                    if (age >= 50)
                    {
                        DateOnly p8Start = input.ContractStartDate.AddYears(12);
                        DateOnly p8End = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future
                        if (input.ContractTerminatedDate <= p8End && input.ContractTerminatedDate >= p8Start)
                        {
                            returnState = "OK P8";
                            noticePeriodStr = "4 måneders varsel";
                            severancePayStr = severanceText;
                            if (input.ContractTerminatedDate >= severancePay) { severancePayStr = severanceText; }
                            extraDisplayedInfoStr = extraDisplayedInfoStr2;
                            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                        }
                    }
                }

                if (input.TerminatingParty == 1)
                {
                    // Fra medarbejderens side
                    DateOnly p1mStart = input.ContractStartDate;
                    DateOnly p1mEnd = input.ContractStartDate.AddMonths(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p1mEnd && input.ContractTerminatedDate >= p1mStart)
                    {
                        returnState = "OK P1M";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 måneders beskæftigelse 1 uge
                    DateOnly p2mStart = input.ContractStartDate.AddMonths(6);
                    DateOnly p2mEnd = input.ContractStartDate.AddYears(3).AddDays(-1);
                    if (input.ContractTerminatedDate <= p2mEnd && input.ContractTerminatedDate >= p2mStart)
                    {
                        returnState = "OK P2M";
                        noticePeriodStr = "1 uges varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 3 års beskæftigelse 2 uger
                    DateOnly p3mStart = input.ContractStartDate.AddYears(3);
                    DateOnly p3mEnd = input.ContractStartDate.AddYears(6).AddDays(-1);
                    if (input.ContractTerminatedDate <= p3mEnd && input.ContractTerminatedDate >= p3mStart)
                    {
                        returnState = "OK P3M";
                        noticePeriodStr = "2 ugers varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 6 års beskæftigelse 3 uger
                    DateOnly p4mStart = input.ContractStartDate.AddYears(6);
                    DateOnly p4mEnd = input.ContractStartDate.AddYears(12).AddDays(-1);
                    if (input.ContractTerminatedDate <= p4mEnd && input.ContractTerminatedDate >= p4mStart)
                    {
                        returnState = "OK P4M";
                        noticePeriodStr = "3 ugers varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                    // Efter 12 års beskæftigelse 4 uger
                    DateOnly p6mStart = input.ContractStartDate.AddYears(12);
                    DateOnly p6mEnd = input.ContractStartDate.AddYears(4000).AddDays(-1); // Way out in the future.;
                    if (input.ContractTerminatedDate <= p6mEnd && input.ContractTerminatedDate >= p6mStart)
                    {
                        returnState = "OK P6M";
                        noticePeriodStr = "4 ugers varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr2;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                }

            }
            
            if (input.SalariedEmployee != 0)
            {
                if (input.TerminatingParty == 0)
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
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // 3 måneder - 14 dage til 5 måneder Løbende - måned + 1 måned.
                    DateOnly p2fStart = input.ContractStartDate.AddMonths(3).AddDays(-14);
                    DateOnly p2fEnd = input.ContractStartDate.AddMonths(5).AddDays(-1);

                    if (input.ContractTerminatedDate <= p2fEnd && input.ContractTerminatedDate >= p2fStart)
                    {
                        returnState = "OK P2f";
                        noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // 5 måneder til 2 år og 9 måneder - Løbende måned + 3 måneder.
                    DateOnly p3fStart = input.ContractStartDate.AddMonths(5);
                    DateOnly p3fEnd = input.ContractStartDate.AddYears(2).AddMonths(9).AddDays(-1);

                    if (input.ContractTerminatedDate <= p3fEnd && input.ContractTerminatedDate >= p3fStart)
                    {
                        returnState = "OK P3f";
                        noticePeriodStr = "Løbende måned plus 3 måneds varsel";
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // 2 år og 9 måneder til 5 år og 8 måneder - Løbende måned + 4 måneder.
                    DateOnly p4fStart = input.ContractStartDate.AddYears(2).AddMonths(9);
                    DateOnly p4fEnd = input.ContractStartDate.AddYears(5).AddMonths(8).AddDays(-1);

                    if (input.ContractTerminatedDate <= p4fEnd && input.ContractTerminatedDate >= p4fStart)
                    {
                        returnState = "OK P4f";
                        noticePeriodStr = "Løbende måned plus 4 måneds varsel";
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // 5 år og 8 måneder til 8 år og 7 måneder - Løbende måned + 5 måneder. 
                    DateOnly p5fStart = input.ContractStartDate.AddYears(5).AddMonths(8);
                    DateOnly p5fEnd = input.ContractStartDate.AddYears(8).AddMonths(7).AddDays(-1);

                    if (input.ContractTerminatedDate <= p5fEnd && input.ContractTerminatedDate >= p5fStart)
                    {
                        returnState = "OK P5f";
                        noticePeriodStr = "Løbende måned plus 5 måneds varsel";
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // Ansat i mere end 8 år og 7 måneder - Løbende måned + 6 måneder. (103+=m)
                    DateOnly p6fStart = input.ContractStartDate.AddYears(8).AddMonths(7);
                    DateOnly p6fEnd = input.ContractStartDate.AddYears(5000).AddDays(-1); // Way out in the future.

                    if (input.ContractTerminatedDate <= p6fEnd && input.ContractTerminatedDate >= p6fStart)
                    {
                        returnState = "OK P6f";
                        noticePeriodStr = "Løbende måned plus 6 måneds varsel";
                        if (input.ContractTerminatedDate >= severancePay1 && input.SalariedEmployee == 1) { severancePayStr = severanceText1; }
                        else if (input.ContractTerminatedDate >= severancePay2 && input.SalariedEmployee == 2) { severancePayStr = severanceText2; } else { severancePayStr = "N/A"; }
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }

                if (input.TerminatingParty == 1)
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
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
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
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr3; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }
            }

            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }

        private static NoticePeriodResult CalculateVVSoverenskomst(NoticePeriodInput input)
        {
            string returnState = "Error CalculateVVSoverenskomst";
            string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            string extraDisplayedInfoStr1 = "Stk. 5: Fratrædelse ved arbejdsugens afslutning\r\nFratrædelse kan kun finde sted ved arbejdsugens afslutning.";
            string extraDisplayedInfoStr2 = "Punkt 25 - Funktionærlignende ansættelse\r\nStk. 5: Anciennitet\r\nAnciennitet ved ansættelse på funktionærlignende vilkår regnes fra den \r\n1. i den måned, hvor aftalen træder i kraft."; // Salaried == 2

            // SeverancePay
            DateOnly severancePay1 = input.ContractStartDate.AddYears(12);
            string severancePayStr1 = "Der kan være krav på en fratrædelsesgodtgørelse. En §2a godtgørelse i henhold til funktionærloven. Kontakt din afdeling";

            if (input.SalariedEmployee == 0) 
            {
                if (input.TerminatingParty == 0)
                {
                    // For medarbejdere, der uden anden afbrydelse har været beskæftiget på
                    // samme virksomhed i mindst 9 måneder, gælder følgende opsigelsesvarsel:
                    // Fra virksomhedside 10 arbejdsdage
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    DateOnly p2Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p2End = input.ContractStartDate.AddMonths(5000).AddDays(-1); // Way out in the future

                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK P1";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK P2";
                        noticePeriodStr = "10 arbejdsdage";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }

                if (input.TerminatingParty == 1)
                {
                    // For medarbejdere, der uden anden afbrydelse har været beskæftiget på
                    // samme virksomhed i mindst 9 måneder, gælder følgende opsigelsesvarsel:
                    // Fra medarbejderside 5 arbejdsdage
                    DateOnly p1Start = input.ContractStartDate;
                    DateOnly p1End = input.ContractStartDate.AddMonths(9).AddDays(-1);
                    DateOnly p2Start = input.ContractStartDate.AddMonths(9);
                    DateOnly p2End = input.ContractStartDate.AddMonths(5000).AddDays(-1); // Way out in the future

                    if (input.ContractTerminatedDate <= p1End)
                    {
                        returnState = "OK P1";
                        noticePeriodStr = "0 dages varsel";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    if (input.ContractTerminatedDate <= p2End && input.ContractTerminatedDate >= p2Start)
                    {
                        returnState = "OK P2";
                        noticePeriodStr = "5 arbejdsdage";
                        severancePayStr = "N/A";
                        extraDisplayedInfoStr = extraDisplayedInfoStr1;
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                }
            }

            if (input.SalariedEmployee != 0)
            {
                if (input.TerminatingParty == 0)
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
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }

                    // 3 måneder - 14 dage til 5 måneder Løbende - måned + 1 måned.
                    DateOnly p2fStart = input.ContractStartDate.AddMonths(3).AddDays(-14);
                    DateOnly p2fEnd = input.ContractStartDate.AddMonths(5).AddDays(-1);

                    if (input.ContractTerminatedDate <= p2fEnd && input.ContractTerminatedDate >= p2fStart)
                    {
                        returnState = "OK P2f";
                        noticePeriodStr = "Løbende måned plus 1 måneds varsel";
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
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
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
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
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
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
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
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
                        if (input.SalariedEmployee == 1 && input.ContractTerminatedDate >= severancePay1) { severancePayStr = severancePayStr1; }
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
                }
            
                

                if (input.TerminatingParty == 1)
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
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
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
                        extraDisplayedInfoStr = "N/A";
                        if (input.SalariedEmployee == 2) { extraDisplayedInfoStr = extraDisplayedInfoStr2; }
                        return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
                    }
            
                }
            }
            // Return fallback if it fails to find a match
            return new NoticePeriodResult(returnState, noticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
        private static NoticePeriodResult CalculateABAF(NoticePeriodInput input)
        {
            // Same as Industriens overenskomst
            // I Herefor call Industriens overenskomst instead of recreating it again
            var result = CalculateIndustriensOverenskomst(input);
            string returnState = "Error CalculateABAF";
            //string noticePeriodStr = "N/A";
            string severancePayStr = "N/A";
            string extraDisplayedInfoStr = "N/A";

            // SeverancePay
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 0 /* NOT on Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(3);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse. § 21. G. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 1 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse en 2a godtgørelse. Kontakt din afdeling";
                }
            }
            if (input.TerminatingParty == 0 /* Terminated by Employer */ && input.SalariedEmployee == 2 /* On Danish Salaried Employees Act */)
            {
                DateOnly p1Start = input.ContractStartDate.AddYears(12);
                if (input.ContractTerminatedDate >= p1Start)
                {
                    severancePayStr = "Der kan være krav på en fratrædelsesgodtgørelse en 2a godtgørelse. Kontakt din afdeling";
                }
            }


            return new NoticePeriodResult(returnState, result.NoticePeriodStr, severancePayStr, extraDisplayedInfoStr);
        }
    }
}
