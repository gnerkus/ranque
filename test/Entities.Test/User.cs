using Entities.Models;

namespace Entities.Test;

public class UserTests
{
    [Fact]
    public void User_ShouldHave_Name()
    {
        var user = new User();
        Assert.NotNull(user.FirstName);
        Assert.NotNull(user.LastName);
    }
}
