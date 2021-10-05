using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //Act
            bool actual = false;

            if (Verdens_Maal_Skole.DataAccess.GetAllReaders() != null)
            {
                actual = true;
            }

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
