using ProjectsPage.Components;
using ProjectsPage.Models;
using System.Text.Json;

namespace UnitTests
{
    public class EntityModelTests
    {
        [Fact]
        public void JsonDocStoreConnectTest()
        {
            using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
            Assert.True( db.Database.CanConnect() );
        }

        [Fact]
        public void JsonDocSoreCountConfirmation()
        {
            using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
            var count = (from project in db.projects where project.site == "ProjectsPage" select project.site).Count();
            count += (from project in db.projects where project.site == "ProjectsPageDemos" select project.site).Count();
            db.Dispose();

            Assert.True( count == 12 );
        }

        [Fact]
        public void CheckAgileStockNameMatchInDatase()
        {
            using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
            
            //var DocumentTitles = db.projects.FromSqlRaw("""select document->"$.Title" from projects where site='ProjectsPage';"""); //Not an OOM: no array or list method
            var AgileStockDocument = (from project in db.projects where project.site == "ProjectsPage" && project.document.Contains("AgileStock Web") select project.document).ToList();
            db.Dispose();

            if (AgileStockDocument.Count < 1) throw new NullReferenceException();

            JsonSerializerOptions options = new JsonSerializerOptions(new JsonSerializerDefaults());
            options.PropertyNameCaseInsensitive = false;

            FrameSelectionOption jdocAgileStock = JsonSerializer.Deserialize<FrameSelectionOption>
                (AgileStockDocument.First(), options)
                ?? throw new InvalidDataException("Encountered a document serialization error.");

            Assert.Equal( "AgileStock Web", jdocAgileStock.Title.ToString() );
        }
    }
};
