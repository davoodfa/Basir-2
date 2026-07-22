using Basir.Application.Globalization.ViewModels;
using FluentValidation;

namespace Basir.Application.Globalization.Validators;

public class PreferencesValidator : AbstractValidator<PreferencesViewModel>
{
    public PreferencesValidator()
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language is required.");

        RuleFor(x => x.ThemeId)
            .GreaterThan(0).WithMessage("Theme is required.");

        RuleFor(x => x.TimeZoneId)
            .NotEmpty().WithMessage("Time zone is required.");

        RuleFor(x => x.CalendarType)
            .NotEmpty().WithMessage("Calendar type is required.");

        RuleFor(x => x.NumberFormat)
            .NotEmpty().WithMessage("Number format is required.");

        RuleFor(x => x.ItemsPerPage)
            .InclusiveBetween(5, 100).WithMessage("Items per page must be between 5 and 100.");
    }
}
