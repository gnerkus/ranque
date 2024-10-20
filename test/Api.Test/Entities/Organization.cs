using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Api.Test.Entities;

public class OrganizationTests
{
    [Theory]
    [InlineData(null, "Address", false)]
    [InlineData("", "Address", false)]
    [InlineData("Test Leaderboard", "Address", true)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Address", false)]
    public void Organization_Name_Max50Chars(string orgName, string orgAddress, bool expectedResult)
    {
        var org = new Organization
        {
            Name = orgName,
            Address = orgAddress
        };
        var context = new ValidationContext(org);
        var results = new List<ValidationResult>();

        var actual = Validator.TryValidateObject(org, context, results, true);
        Assert.Equal(actual, expectedResult);
    }
}