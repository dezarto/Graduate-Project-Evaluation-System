namespace GPESAPI.Domain.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }

        // Bu özelliği, token'ın süresinin dolup dolmadığını kontrol etmek için kullanabilirsiniz.
        public bool IsExpired => DateTime.UtcNow >= Expiration;

        // Token'ın hala aktif olup olmadığını kontrol etmek için kullanılır.
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
