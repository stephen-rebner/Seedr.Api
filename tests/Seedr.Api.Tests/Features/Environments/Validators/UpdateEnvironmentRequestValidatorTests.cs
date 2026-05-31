using FluentValidation.Results;
using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Validators;

namespace Seedr.Api.Tests.Features.Environments.Validators;

[TestFixture]
public class UpdateEnvironmentRequestValidatorTests
{
    private UpdateEnvironmentRequestValidator _validator = null!;

    [SetUp]
    public void SetUp() => _validator = new UpdateEnvironmentRequestValidator();

    [Test]
    public void Validate_WithValidRequest_IsValid()
    {
        var request = new UpdateEnvironmentRequest("Staging", "The staging environment");

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_WithEmptyName_IsInvalid()
    {
        var request = new UpdateEnvironmentRequest("", "A description");

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == nameof(UpdateEnvironmentRequest.Name)), Is.True);
    }
}
