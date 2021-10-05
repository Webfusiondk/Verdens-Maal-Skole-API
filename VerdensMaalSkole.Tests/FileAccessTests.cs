using Xunit;

namespace VerdensMaalSkole.Tests
{
    public class FileAccessTests
    {
        [Fact]
        public void ReadConnectionString_ShouldReturnString()
        {
            //Arrange
            Verdens_Maal_Skole.FileAccess fileAccess = new Verdens_Maal_Skole.FileAccess();

            bool expected = true;


            //Act
            bool actual = false;

            if(fileAccess.ReadConnectionString() != string.Empty)
            {
                actual = true;
            }


            //Assert
            Assert.Equal(expected, actual);

        }
    }
}
