namespace AppointmentApp.Application.Helpers
{
    public static class TokenGenerator
    {
        public static string GenerateToken(DateTime appointmentDate, int number)
        {
            string formattedNumber = number.ToString("D3");

            string token = $"A{appointmentDate:yyyyMMdd}-{formattedNumber}";

            return token;
        }
    }
}
