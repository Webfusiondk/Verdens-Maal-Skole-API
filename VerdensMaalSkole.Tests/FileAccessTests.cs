using Verdens_Maal_Skole;
using Xunit;

namespace VerdensMaalSkole.Tests
{
    public class FileAccessTests
    {
        [Fact]
        public void ReadConnectionString_ShouldReadAndReturnString()
        {
            //Arrange

            bool expected = true;
            

            //Act
            bool actual = false;

            if(FileAccess.ReadConnectionString() != string.Empty)
            {
                actual = true;
            }


            //Assert
            Assert.Equal(expected, actual);

        }
    }
}
