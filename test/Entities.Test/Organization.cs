using Entities.Models;

namespace Entities.Test;

public class OrganizationTests
{
    [Fact]
    public void Organization_ShouldHave_Name()
    {
        var org = new Organization
        {
            Name = "TestOrg"
        };
        Assert.NotNull(org.Name);
    }
}