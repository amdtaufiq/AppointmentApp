using AppointmentApp.Application.Helpers;

namespace AppointmentApp.Application.Tests.Helpers
{
    public class TokenGeneratorTests
    {
        [Theory]
        [InlineData("2025-04-28", 1, "A20250428-001")]
        [InlineData("2025-04-28", 25, "A20250428-025")]
        [InlineData("2025-12-31", 99, "A20251231-099")]
        public void GenerateToken_ShouldReturnCorrectToken(DateTime appointmentDate, int number, string expectedToken)
        {
            // Act
            var result = TokenGenerator.GenerateToken(appointmentDate, number);

            // Assert
            Assert.Equal(expectedToken, result);
        }
    }
}
