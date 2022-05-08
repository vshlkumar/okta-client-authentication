using DemoApplication.Localization;
using DemoApplication.Model;
using FluentValidation;
using FluentValidation.Resources;
using System.Globalization;

namespace DemoApplication.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            //globally set the culture to "hindi" for all messages
            //ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("hi");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(x => ValidatorOptions.Global.LanguageManager.GetString("NotEmptyValidator",
                new CultureInfo("hi")));

            RuleFor(x => x.LastName).NotEmpty().WithMessage(x => ValidatorOptions.Global.LanguageManager.GetString("NotEmptyValidator",
                new CultureInfo("fr")));

            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Age).NotEqual(0);
        }
    }
}
