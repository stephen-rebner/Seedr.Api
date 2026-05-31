using FluentValidation.Results;
using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Validators;

namespace Seedr.Api.Tests.Features.Environments.Validators;

[TestFixture]
public class CreateEnvironmentRequestValidatorTests
{
    private CreateEnvironmentRequestValidator _validator = null!;

    [SetUp]
    public void SetUp() => _validator = new CreateEnvironmentRequestValidator();

    [Test]
    public void Validate_WithValidRequest_IsValid()
    {
        var request = new CreateEnvironmentRequest("Production", "The live environment");

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_WithEmptyName_IsInvalid()
    {
        var request = new CreateEnvironmentRequest("", "A description");

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == nameof(CreateEnvironmentRequest.Name)), Is.True);
    }

    [Test]
    public void Validate_WithNameExceedingMaxLength_IsInvalid()
    {
        var request = new CreateEnvironmentRequest(new string('a', 201), "A description");

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void Validate_WithNullDescription_IsValid()
    {
        var request = new CreateEnvironmentRequest("Production", null);

        ValidationResult result = _validator.Validate(request);

        Assert.That(result.IsValid, Is.True);
    }
}
