using ProjectsPage.Models;

namespace UnitTests
{
    public class EntityModelTests
    {
        [Fact]
        public void JsonDocStoreConnectTest()
        {
            using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
            Assert.True(db.Database.CanConnect());
        }
    }
}
