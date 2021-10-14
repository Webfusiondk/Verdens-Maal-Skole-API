using System.Collections.Generic;
using System.Linq;
using Verdens_Maal_Skole;
using Xunit;

namespace VerdensMaalSkole.Tests
{
    public class TokenManagerTests
    {
        TokenManager TokenManager = new TokenManager();

        [Fact]
        public void GenerateToken_ShouldReturnUnique()
        {
            //Arrange
            bool expected = true;
            bool actual = true;
            List<SessionToken> tokenList = new List<SessionToken>();

            //Fills list with generated tokens
            for (int i = 0; i < 10000; i++)
            {
                tokenList.Add(TokenManager.GenerateToken());
            }

            //Act

            //Looks through list for duplicates and fails the test if any is found
            if (tokenList.Count != tokenList.Distinct().Count())
            {
                actual = false;
            }

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckForToken_ShouldFindNewToken()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            SessionToken testToken = TokenManager.GenerateToken();

            //Act
            if (TokenManager.CheckForToken(testToken.Token))
            {
                actual = true;
            }

            //Assert
            Assert.Equal(expected, actual);
        }


    }
}
