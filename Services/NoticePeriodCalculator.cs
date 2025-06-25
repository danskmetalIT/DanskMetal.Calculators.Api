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
            // Lav alle dine 500 linjers beregninger her
            string returnState = "OK - Industriens Overenskomst";
            string noticePeriodStr = "...";
            string severencePayStr = "...";
            string extraDisplayedInfoStr = "...";



            //extraDisplayedInfoStr = cDDays + " Days " + cDMonths + " Months " + cDYears + " Years";
            /* Text from the collective agreement */
            // § 38 Opsigelsesregler
            // Ophævelse fra virksomhedens side
            // p1 < 6 måneder = Ingen varsel - tjek eventuelt om de har aftalt yderelige i kontrakten
            // p2 >= 6 måneder og < 9 måneder = 14 dages varsel
            // p3 >= 9 måneder og < 2 år = 21 dages varsel
            // p4 >= 2 år og < 3 år = 28 dages varsel
            // p5 >= 3 år og < 6 år = 56 dages varsel
            // p6 >= 6 år = 70 dages varsel

            // Medarbejdere over 50 år
            // p7 >= 9 år < 12 år = 90 dages varsel
            // p8 >= 12 år = 120 dages varsel

            // Ophævelse fra medarbejderens side
            // < 6 måneder = Ingen varsel - tjek eventuelt om de har aftalt yderelige i kontrakten
            // >= 6 måneder og < 3 år = 7 dages varsel
            // >= 3 år og < 6 år = 14 dages varsel
            // >= 6 år og < 9 år = 21 dages varsel
            // >= 9 år = 28 dages varsel

            // Fratrædelsesgodtgørelse
            // >= 3 år = mulighed for fratrædelsesgodtgørelse.

            /* start calculation */
            // Vaiables
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Set current date of the server
            int age = today.Year - input.BirthdayDate.Year;

            // cD = contractDuration
            var cDYears = input.ContractTerminatedDate.Year - input.ContractStartDate.Year;
            var cDMonths = input.ContractTerminatedDate.Month - input.ContractStartDate.Month;
            var cDDays = input.ContractTerminatedDate.Day - input.ContractStartDate.Day;


            // Test output
            returnState = age + " years old" ;

            // Return answer to API stack
            return new NoticePeriodResult(returnState, noticePeriodStr, severencePayStr, extraDisplayedInfoStr);
        }
    }
}
