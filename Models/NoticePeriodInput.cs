namespace DanskMetal.Calculators.Api.Models
{
    public class NoticePeriodInput
    {
        public int SelectedCollectiveAgreement { get; set; } // Represent a value from 1 to x. Each number represent an collective agreement.
        public int TerminatingParty { get; set; } // Represent a value between 0 and 1. It represent who initiated the termination of the contract.
        public DateOnly ContractStartDate { get; set; } // The date of when the employee started.
        public DateOnly ContractTerminatedDate { get; set; } // The date the employee was/is notified of the termination of their employment.
        public DateOnly BirthdayDate { get; set; } // The birthdate of the terminated employee.
        public int SalariedEmployee {  get; set; } // Represent a value from 0 to 2. Description below.
    }
}

/* Update list and values informations on both this class and NoticePeriodCalculator */
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

