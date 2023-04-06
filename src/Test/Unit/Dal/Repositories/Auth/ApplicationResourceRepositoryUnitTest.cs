using FiraServer.Application.Interfaces.Auth;
using FiraServer.Application.Services.Auth;
using FiraServer.Infra.Dal.Contexts;
using FiraServer.Infra.Dal.Repositories.Auth;
using Moq;

namespace FiraServer.Test.Unit.Dal.Repositories.Auth;

public class ApplicationResourceRepositoryUnitTest
{
    public class GetByURIMethod
    {
        private Mock<ApplicationResourceContext> _MockDbContext;
        private ApplicationResourceRepository _Repository;

        public GetByURIMethod() 
        {
            this._MockDbContext = new Mock<ApplicationResourceContext>();
            this._Repository = new ApplicationResourceRepository(this._MockDbContext.Object);
        }

        [Fact]
        public void Returns_Most_Recent_Record_If_Multiple_Matches()
        {
            //Arrange
                           

            //Act

            //Assert
        }
    }
}
