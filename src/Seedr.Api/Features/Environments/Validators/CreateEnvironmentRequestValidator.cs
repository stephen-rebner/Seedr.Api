using FluentValidation;
using Seedr.Api.Features.Environments.Models;

namespace Seedr.Api.Features.Environments.Validators;

public class CreateEnvironmentRequestValidator : AbstractValidator<CreateEnvironmentRequest>
{
    public CreateEnvironmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotNull()
            .MaximumLength(1000);
    }
}
