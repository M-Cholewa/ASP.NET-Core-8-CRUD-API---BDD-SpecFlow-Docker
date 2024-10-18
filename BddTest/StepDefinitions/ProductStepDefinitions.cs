using Business.Entity;
using Business.Repository;
using Domain.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddTest.StepDefinitions
{
    [Binding]
    public class ProductStepDefinitions
    {
        public WebAppFactory _factory = new WebAppFactory();
        public HttpClient _client { get; set; } = null!;
        private HttpResponseMessage _response { get; set; } = null!;

        public ProductStepDefinitions()
        {
            _client = _factory.CreateDefaultClient(new Uri($"http://localhost/"));
        }

        [Given(@"The database is empty")]
        public async Task GivenTheDatabaseIsEmpty()
        {
            await _factory.InitProductDB(new List<Product>());
        }

        [Given(@"Database contains products:")]
        public async Task GivenDatabaseContainsProducts(Table table)
        {
            var products = table.CreateSet<Product>();
            await _factory.InitProductDB(products);
        }

        [When(@"A GET request is made to '([^']*)'")]
        public async Task WhenAGETRequestIsMadeTo(string path)
        {
            _response = await _client.GetAsync(path);
        }

        [When(@"A POST request is made to '([^']*)' with the following data:")]
        public async Task WhenAPOSTRequestIsMadeToWithTheFollowingData(string path, Table table)
        {
            var product = table.CreateInstance<Product>();
            _response = await _client.PostAsJsonAsync(path, product);
        }

        [When(@"A PUT request is made to '([^']*)' with the following updated data:")]
        public async Task WhenAPUTRequestIsMadeToWithTheFollowingUpdatedData(string path, Table table)
        {
            var product = table.CreateInstance<Product>();
            _response = await _client.PutAsJsonAsync(path, product);
        }

        [When(@"A DELETE request is made to '([^']*)'")]
        public async Task WhenADELETERequestIsMadeTo(string path)
        {
            _response = await _client.DeleteAsync(path);
        }

        [Then(@"The products should be retrieved successfully with status code (.*)")]
        public void ThenTheProductsShouldBeRetrievedSuccessfullyWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"The product should be added successfully with status code (.*)")]
        public void ThenTheProductShouldBeAddedSuccessfullyWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"The product should be updated successfully with status code (.*)")]
        public void ThenTheProductShouldBeUpdatedSuccessfullyWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"The product should be deleted successfully with status code (.*)")]
        public void ThenTheProductShouldBeDeletedSuccessfullyWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"The result should be:")]
        public async Task ThenTheResultShouldBe(Table table)
        {
            var expected = table.CreateSet<ProductSimplifiedDTO>();
            var actual = await _response.Content.ReadFromJsonAsync<List<ProductSimplifiedDTO>>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"The product in the database should be:")]
        public async Task ThenTheProductInTheDatabaseShouldBe(Table table)
        {
            var expectedProduct = table.CreateInstance<Product>();
            var actualProduct = await _client.GetFromJsonAsync<Product>($"/Product/{expectedProduct.Id}");
            actualProduct.Should().BeEquivalentTo(expectedProduct);
        }

        [Then(@"The product should no longer exist in the database")]
        public async Task ThenTheProductShouldNoLongerExistInTheDatabase()
        {
            var products = await _client.GetFromJsonAsync<List<Product>>("/Product");
            products.Should().BeEmpty();
        }
    }
}
