namespace DanskMetal.Calculators.Api.Models
{
    public record SeverancePayResult(
        string ReturnState, // returns a string used for testing. does not need to de displayed. 
        decimal ChartColumnOne, // returns the value for column 1 *! it will always be 0.
        decimal ChartColumnTwo, // returns the value for column 2
        decimal ChartColumnThree, // returns the value for column 3
        decimal ChartColumnFour // returns the value for column 4
    );
}
