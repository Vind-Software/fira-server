using FiraServer.Application.Entities.Auth;
using FiraServer.Application.Interfaces.Auth;
using FiraServer.Application.Services.Auth;
using Moq;

namespace FiraServer.Test.Unit.Application.Services.Auth;

public class ApplicationResourceServiceUnitTest
{
    public class GetByURIMethod : IDisposable
    {
        private Mock<IApplicationResourceRepository> _MockRepository;
        private ApplicationResourceService _Service;

        public GetByURIMethod()
        {
            this._MockRepository = new Mock<IApplicationResourceRepository>();
            this._Service = new ApplicationResourceService(this._MockRepository.Object);
        }

        [Theory]
        [InlineData("auth/access-token", false)]
        [InlineData("imaginary/uri", true)]
        public void Returns_Null_If_No_Matching_Uri_Is_Found(string uri, bool expectedResult)
        {
            //Arrange
            ApplicationResource resource = new ApplicationResource(1, "auth/access-token", new ApplicationResourceType(1, "default"));
            this._MockRepository.Setup(r => r.GetByURI("auth/access-token")).Returns(resource);

            //Act
            ApplicationResource foundResource = this._Service.GetByURI(uri);
            bool result = foundResource == null;

            //Assert
            Assert.Equal(expectedResult, result);
        }

        public void Dispose()
        {
        }
    }
}
