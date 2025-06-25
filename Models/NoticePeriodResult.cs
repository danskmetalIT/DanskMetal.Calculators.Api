namespace DanskMetal.Calculators.Api.Models
{
    public record NoticePeriodResult(
        string ReturnState, // returns a string used for testing. does not need to de displayed.
        string NoticePeriodStr, // Returns a text containing "[int] string" like these examples = "[int] dages varsel" or "[int] måneders varsel"
        string SeverencePayStr, // Returns a text if they can get a "". Else returns "N/A".
        string ExtraDisplayedInfoStr // Returns a text if there are extra information for the employee. Else returns "N/A".
    );
}
