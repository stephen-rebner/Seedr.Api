using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Seedr.Api.Features.Environments.Models;
using Testcontainers.PostgreSql;

namespace Seedr.Api.Tests.Features.Environments.Endpoints;

[TestFixture]
public class EnvironmentEndpointsTests
{
    private const string BaseUrl = "/api/v1/environments";
    private const int MissingId = 2147483647;

    private PostgreSqlContainer _container = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .Build();

        await _container.StartAsync();

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:DefaultConnection", _container.GetConnectionString());
        });

        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await _container.DisposeAsync();
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsCreatedEnvironment()
    {
        var request = new CreateEnvironmentRequest("Create-Test", "Created via test");

        var response = await _client.PostAsJsonAsync(BaseUrl, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var created = await response.Content.ReadFromJsonAsync<EnvironmentResponse>();
        Assert.That(created, Is.Not.Null);
        Assert.That(created!.Id, Is.GreaterThan(0));
        Assert.That(created.Name, Is.EqualTo(request.Name));
        Assert.That(created.Description, Is.EqualTo(request.Description));
    }

    [Test]
    public async Task Create_WithInvalidRequest_ReturnsBadRequest()
    {
        var request = new CreateEnvironmentRequest("", "Missing name");

        var response = await _client.PostAsJsonAsync(BaseUrl, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetById_WhenExists_ReturnsEnvironment()
    {
        var created = await CreateEnvironmentAsync("GetById-Test", "For get by id");

        var response = await _client.GetAsync($"{BaseUrl}/{created.Id}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var fetched = await response.Content.ReadFromJsonAsync<EnvironmentResponse>();
        Assert.That(fetched, Is.Not.Null);
        Assert.That(fetched!.Id, Is.EqualTo(created.Id));
    }

    [Test]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BaseUrl}/{MissingId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetAll_ReturnsCreatedEnvironments()
    {
        await CreateEnvironmentAsync("GetAll-Test", "For get all");

        var environments = await _client.GetFromJsonAsync<List<EnvironmentResponse>>(BaseUrl);

        Assert.That(environments, Is.Not.Null);
        Assert.That(environments!.Any(e => e.Name == "GetAll-Test"), Is.True);
    }

    [Test]
    public async Task Update_WhenExists_ReturnsUpdatedEnvironment()
    {
        var created = await CreateEnvironmentAsync("Update-Test", "Before update");
        var request = new UpdateEnvironmentRequest("Update-Test-Renamed", "After update");

        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{created.Id}", request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updated = await response.Content.ReadFromJsonAsync<EnvironmentResponse>();
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Name, Is.EqualTo(request.Name));
        Assert.That(updated.Description, Is.EqualTo(request.Description));
    }

    [Test]
    public async Task Update_WhenMissing_ReturnsNotFound()
    {
        var request = new UpdateEnvironmentRequest("Missing", "Does not exist");

        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{MissingId}", request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Delete_WhenExists_ReturnsNoContentAndRemovesEnvironment()
    {
        var created = await CreateEnvironmentAsync("Delete-Test", "To be deleted");

        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{created.Id}");
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        var getResponse = await _client.GetAsync($"{BaseUrl}/{created.Id}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Delete_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/{MissingId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task<EnvironmentResponse> CreateEnvironmentAsync(string name, string description)
    {
        var response = await _client.PostAsJsonAsync(BaseUrl, new CreateEnvironmentRequest(name, description));
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<EnvironmentResponse>();
        return created!;
    }
}
