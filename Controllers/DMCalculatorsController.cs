using Microsoft.AspNetCore.Mvc;
using DanskMetal.Calculators.Api.Models;
using DanskMetal.Calculators.Api.Services;
using Microsoft.AspNetCore.Mvc.TagHelpers;
//using Umbraco.Cms.Web.Common.Controllers; Enable this line if you are using UmbracoApiController instead of ControllerBase

namespace DanskMetal.Calculators.Api.Controllers
{
    [ApiController] // Comment out or remove this, when using Umbraco standard routing.
    [Route("api/[controller]")] // Comment out to use standard umbraco routing or use it to enable a costum route.

    /* Comment this class out when using : UmbracoApiController*/
    public class DMCalculatorsController : ControllerBase
    {
        [HttpPost("severancepay")]
        public ActionResult<SeverancePayResult> CalculateSeverancePay(SeverancePayInput input)
        {
            // Checks if input values are not empty or 0.
            // only FritvalgsPercent is allowed to be 0.
            if (input is null ||
                input.SalaryPerHour <= 0 ||
                input.WorkingHours <= 0 ||
                input.FritvalgsPercent < 0)
            {
                return BadRequest("All values has to be above 0 except FritvalgsPercent.");
            }

            var result = SeverancePayCalculator.Calculate(
                input.SalaryPerHour,
                input.WorkingHours,
                input.FritvalgsPercent,
                input.UnemploymentMonthlyRate // The danish standard monthly unemployment rate.
            );

            return Ok(result);
        }

        [HttpPost("restPeriod")]
        public ActionResult<RestPeriodResult> CalculateRestPeiod(RestPeriodInput input)
        {
            // Checks if inputs types are correct. 
            // If not. Then return BadRequest "All values has to be included and valid"
            if (input == null ||
                (input.IsEightHourRules != 0 && input.IsEightHourRules != 1) ||
                (input.T1 != 0 && input.T1 != 1) ||
                (input.T2 != 0 && input.T2 != 1) ||
                (input.T3 != 0 && input.T3 != 1) ||
                (input.T4 != 0 && input.T4 != 1) ||
                (input.T5 != 0 && input.T5 != 1) ||
                (input.T6 != 0 && input.T6 != 1) ||
                (input.T7 != 0 && input.T7 != 1) ||
                (input.T8 != 0 && input.T8 != 1) ||
                (input.T9 != 0 && input.T9 != 1) ||
                (input.T10 != 0 && input.T10 != 1) ||
                (input.T11 != 0 && input.T11 != 1) ||
                (input.T12 != 0 && input.T12 != 1) ||
                (input.T13 != 0 && input.T13 != 1) ||
                (input.T14 != 0 && input.T14 != 1) ||
                (input.T15 != 0 && input.T15 != 1) ||
                (input.T16 != 0 && input.T16 != 1) ||
                (input.T17 != 0 && input.T17 != 1) ||
                (input.T18 != 0 && input.T18 != 1) ||
                (input.T19 != 0 && input.T19 != 1) ||
                (input.T20 != 0 && input.T20 != 1) ||
                (input.T21 != 0 && input.T21 != 1) ||
                (input.T22 != 0 && input.T22 != 1) ||
                (input.T23 != 0 && input.T23 != 1) ||
                (input.T24 != 0 && input.T24 != 1))
            {
                return BadRequest("All values has to be included and valid");
            }

            var result = RestPeriodCalculator.Calculate(
                input.IsEightHourRules,
                input.T1,
                input.T2,
                input.T3,
                input.T4,
                input.T5,
                input.T6,
                input.T7,
                input.T8,
                input.T9,
                input.T10,
                input.T11,
                input.T12,
                input.T13,
                input.T14,
                input.T15,
                input.T16,
                input.T17,
                input.T18,
                input.T19,
                input.T20,
                input.T21,
                input.T22,
                input.T23,
                input.T24
            );

            return Ok(result);
        }
        
        [HttpPost("noticePeriod")]
        public ActionResult<NoticePeriodResult> CalculateNoticePeriod(NoticePeriodInput input)
        {
            // Checks if input values are valid
            if (input is null ||
                input.SelectedCollectiveAgreement < 0 || input.SelectedCollectiveAgreement > 9 ||
                input.TerminatingParty < 0 || input.TerminatingParty > 1 ||
                input.SalariedEmployee < 0 || input.SalariedEmployee > 2 ||
                input.ContractStartDate == default ||
                input.ContractTerminatedDate == default ||
                input.BirthdayDate == default) 
            {
                return BadRequest("Input values are not correct. Please send correct values");
            }

            var result = NoticePeriodCalculator.Calculate(input);

            // Replace C# newlines and likes to html version.
            // To display raw in Umbraco you can use
            // @html.Raw(Model.ExtraDisplayedInfoStr) or what else you have called it.
            var updatedResult = result with
            {
                ExtraDisplayedInfoStr = result.ExtraDisplayedInfoStr
                    .Replace("\r\n", "<br />")
                    .Replace("\n", "<br />")
                        };

            return Ok(updatedResult);
        }
        
        // Placeholder for the other Services coming from Simon Larsen Dansk Metal
        // [HttpPost("somethingelse")]
        // public ActionResult<...> CalculateSomethingElse(...) { ... }
    }

    /* Use this one below if using : UmbracoApiController in your setup*/
    /* OBS! Has to umbraco version 9+ but we are using 12.3.10 currently */
    //public class DMCalculatorsApiController : UmbracoApiController
    //{
    //    [HttpPost("severancepay")]
    //    public ActionResult<SeverancePayResult> CalculateSeverancePay(SeverancePayInput input)
    //    {
    //        if (input is null ||
    //            input.SalaryPerHour <= 0 ||
    //            input.WorkingHours <= 0 ||
    //            input.FritvalgsPercent < 0)
    //        {
    //            return BadRequest("All values have to be above 0 except FritvalgsPercent.");
    //        }

    //        var result = SeverancePayCalculator.Calculate(
    //            input.SalaryPerHour,
    //            input.WorkingHours,
    //            input.FritvalgsPercent,
    //            input.UnemploymentMonthlyRate
    //        );

    //        return Ok(result);
    //    }

    //    [HttpPost("restPeriod")]
    //    public ActionResult<RestPeriodResult> CalculateRestPeriod(RestPeriodInput input)
    //    {
    //        if (input == null ||
    //            (input.IsEightHourRules != 0 && input.IsEightHourRules != 1) ||
    //            (input.T1 != 0 && input.T1 != 1) ||
    //            (input.T2 != 0 && input.T2 != 1) ||
    //            (input.T3 != 0 && input.T3 != 1) ||
    //            (input.T4 != 0 && input.T4 != 1) ||
    //            (input.T5 != 0 && input.T5 != 1) ||
    //            (input.T6 != 0 && input.T6 != 1) ||
    //            (input.T7 != 0 && input.T7 != 1) ||
    //            (input.T8 != 0 && input.T8 != 1) ||
    //            (input.T9 != 0 && input.T9 != 1) ||
    //            (input.T10 != 0 && input.T10 != 1) ||
    //            (input.T11 != 0 && input.T11 != 1) ||
    //            (input.T12 != 0 && input.T12 != 1) ||
    //            (input.T13 != 0 && input.T13 != 1) ||
    //            (input.T14 != 0 && input.T14 != 1) ||
    //            (input.T15 != 0 && input.T15 != 1) ||
    //            (input.T16 != 0 && input.T16 != 1) ||
    //            (input.T17 != 0 && input.T17 != 1) ||
    //            (input.T18 != 0 && input.T18 != 1) ||
    //            (input.T19 != 0 && input.T19 != 1) ||
    //            (input.T20 != 0 && input.T20 != 1) ||
    //            (input.T21 != 0 && input.T21 != 1) ||
    //            (input.T22 != 0 && input.T22 != 1) ||
    //            (input.T23 != 0 && input.T23 != 1) ||
    //            (input.T24 != 0 && input.T24 != 1))
    //        {
    //            return BadRequest("All values have to be included and valid");
    //        }

    //        var result = RestPeriodCalculator.Calculate(
    //            input.IsEightHourRules,
    //            input.T1, input.T2, input.T3, input.T4, input.T5, input.T6,
    //            input.T7, input.T8, input.T9, input.T10, input.T11, input.T12,
    //            input.T13, input.T14, input.T15, input.T16, input.T17, input.T18,
    //            input.T19, input.T20, input.T21, input.T22, input.T23, input.T24
    //        );

    //        return Ok(result);
    //    }

    //    [HttpPost("noticePeriod")]
    //    public ActionResult<NoticePeriodResult> CalculateNoticePeriod(NoticePeriodInput input)
    //    {
    //        if (input is null ||
    //            input.SelectedCollectiveAgreement < 0 || input.SelectedCollectiveAgreement > 9 ||
    //            input.TerminatingParty < 0 || input.TerminatingParty > 1 ||
    //            input.SalariedEmployee < 0 || input.SalariedEmployee > 2 ||
    //            input.ContractStartDate == default ||
    //            input.ContractTerminatedDate == default ||
    //            input.BirthdayDate == default)
    //        {
    //            return BadRequest("Input values are not correct. Please send correct values");
    //        }

    //        var result = NoticePeriodCalculator.Calculate(input);
    //        return Ok(result);
    //    }

    //    // Placeholder for the other Services coming from Simon Larsen Dansk Metal
    //}
}
