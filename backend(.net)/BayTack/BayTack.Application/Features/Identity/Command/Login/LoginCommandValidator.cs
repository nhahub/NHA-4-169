using FluentValidation;

namespace BayTack.Application.Features.Identity.Command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .Must(BeAValidEmailOrEgyptianPhone)
                .WithMessage("Enter a valid email address or an 11-digit phone number (e.g. 01012345678).");
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.IpAddress).NotEmpty();
        }

        private static bool BeAValidEmailOrEgyptianPhone(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier)) return false;
            var value = identifier.Trim();

            // 01[0,1,2,5] + 8 digits = 11 digits total (or the +20 country-code form).
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^(01[0125]\d{8}|\+201[0125]\d{8})$"))
                return true;

            try { _ = new System.Net.Mail.MailAddress(value); return true; }
            catch { return false; }
        }
    }
}
