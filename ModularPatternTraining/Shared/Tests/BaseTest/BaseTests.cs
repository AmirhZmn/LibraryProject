namespace ModularPatternTraining.Shared.Tests.BaseTest
{
    using Microsoft.EntityFrameworkCore;
    using Data.AppDbContext;
    using ModularPatternTraining.Modules.BookModule.Models;
    using Moq;

    public abstract class BaseTests
    {
        protected readonly Mock<AppDbContext> MockDbContext;
        protected readonly Mock<DbSet<Book>> MockBookDbSet;

        public BaseTests()
        {
      
            MockBookDbSet = new Mock<DbSet<Book>>();

       
            MockDbContext = new Mock<AppDbContext>();

          
            MockDbContext.Setup(c => c.Books).Returns(MockBookDbSet.Object);

            
            MockDbContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }
    }

}
