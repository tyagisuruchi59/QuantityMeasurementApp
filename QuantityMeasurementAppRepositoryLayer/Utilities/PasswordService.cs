using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementAppRepositoryLayer.Utilities
{
    /// <summary>
    /// Provides password security services:
    /// 1. Salt generation (cryptographically random 256-bit)
    /// 2. PBKDF2-SHA512 hashing (350,000 iterations - NIST 2023)
    /// 3. AES-256-CBC encryption/decryption for sensitive data
    /// 4. Constant-time comparison to prevent timing attacks
    /// </summary>
    public class PasswordService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PasswordService> _logger;

        private const int SaltSize = 32;
        private const int HashSize = 64;
        private const int Iterations = 350_000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

        public PasswordService(IConfiguration config, ILogger<PasswordService> logger)
        {
            _config = config;
            _logger = logger;
        }

        // ─── Salt Generation ───────────────────────────────────────────────────

        /// <summary>
        /// Generates a cryptographically secure random salt.
        /// Returns Base64-encoded string for storage.
        /// </summary>
        public string GenerateSalt()
        {
            var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            var salt = Convert.ToBase64String(saltBytes);
            _logger.LogDebug("Generated new salt (length={Length})", salt.Length);
            return salt;
        }

        // ─── Password Hashing (PBKDF2) ─────────────────────────────────────────

        /// <summary>
        /// Hash a plain-text password with PBKDF2-SHA512.
        /// Combines per-user salt with global pepper for extra security.
        /// </summary>
        public string HashPassword(string plainPassword, string salt)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty", nameof(plainPassword));
            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("Salt cannot be empty", nameof(salt));

            var pepper = _config["Security:Pepper"] ?? "DefaultPepper@QMA";
            var saltedInput = $"{pepper}{plainPassword}{salt}";
            var inputBytes = Encoding.UTF8.GetBytes(saltedInput);
            var saltBytes = Convert.FromBase64String(salt);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                inputBytes,
                saltBytes,
                Iterations,
                HashAlgorithm,
                HashSize);

            var hashString = Convert.ToBase64String(hash);
            _logger.LogDebug("Password hashed successfully using PBKDF2-SHA512");
            return hashString;
        }

        /// <summary>
        /// Verify a plain-text password against a stored hash.
        /// Uses constant-time comparison to prevent timing attacks.
        /// </summary>
        public bool VerifyPassword(string plainPassword, string storedHash, string salt)
        {
            try
            {
                var computedHash = HashPassword(plainPassword, salt);
                var storedHashBytes = Convert.FromBase64String(storedHash);
                var computedHashBytes = Convert.FromBase64String(computedHash);
                return CryptographicOperations.FixedTimeEquals(storedHashBytes, computedHashBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password");
                return false;
            }
        }

        // ─── AES-256 Encryption / Decryption ──────────────────────────────────

        /// <summary>
        /// Encrypt a plain-text string using AES-256-CBC.
        /// Returns Base64-encoded ciphertext with IV prepended.
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            var key = GetAesKey();

            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Prepend IV: [IV (16 bytes)][CipherText]
            var combined = new byte[aes.IV.Length + cipherBytes.Length];
            Array.Copy(aes.IV, 0, combined, 0, aes.IV.Length);
            Array.Copy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(combined);
        }

        /// <summary>
        /// Decrypt an AES-256-CBC encrypted Base64 string.
        /// Extracts IV from the first 16 bytes.
        /// </summary>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            try
            {
                var key = GetAesKey();
                var combined = Convert.FromBase64String(cipherText);

                using var aes = Aes.Create();
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var iv = new byte[16];
                var cipher = new byte[combined.Length - 16];
                Array.Copy(combined, 0, iv, 0, 16);
                Array.Copy(combined, 16, cipher, 0, cipher.Length);
                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor();
                var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decryption failed");
                throw new CryptographicException("Decryption failed", ex);
            }
        }

        private byte[] GetAesKey()
        {
            var keyString = _config["Security:AesKey"]
                ?? throw new InvalidOperationException("AES key not configured");
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
        }
    }
}