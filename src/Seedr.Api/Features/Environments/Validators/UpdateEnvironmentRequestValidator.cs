using FluentValidation;
using Seedr.Api.Features.Environments.Models;

namespace Seedr.Api.Features.Environments.Validators;

public class UpdateEnvironmentRequestValidator : AbstractValidator<UpdateEnvironmentRequest>
{
    public UpdateEnvironmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotNull()
            .MaximumLength(1000);
    }
}
