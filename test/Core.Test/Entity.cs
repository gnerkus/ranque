using Entities.Models;

namespace Core.Test;
    

public class EntityTests
{
        [Fact]
        public void Entity_Should_Have_ExpandoObject()
        {
            var entity = new Entity();
            Assert.NotNull(entity.GetPrivate("_expando"));
        }
}
