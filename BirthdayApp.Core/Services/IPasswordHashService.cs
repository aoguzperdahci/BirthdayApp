namespace BirthdayApp.Core.Services
{
    public interface IPasswordHashService
    {
        /// <summary>
        /// Hash the password using the given salt
        /// </summary>
        /// <param name="password">The password will be hashed</param>
        /// <param name="salt">The salt will be used to hash</param>
        /// <returns>Hashed password</returns>
        string HashPassword(string password, byte[] salt);

        /// <summary>
        /// Generate a new randomized salt
        /// </summary>
        /// <returns>Salt</returns>
        byte[] GenerateSalt();
    }
}