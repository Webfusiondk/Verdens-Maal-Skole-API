using Verdens_Maal_Skole;
using Xunit;

namespace VerdensMaalSkole.Tests
{
    public class DataAccessTests
    {
        [Fact]
        public void GetAllReaders_ShouldReturnData()
        {
            //Arrange
            bool expected = true;
            bool actual = false;

            //Act
            if (DataAccess.GetAllData() != null)
            {
                actual = true;
            }

            //Assert
            Assert.Equal(expected, actual);
        }

    }
}
