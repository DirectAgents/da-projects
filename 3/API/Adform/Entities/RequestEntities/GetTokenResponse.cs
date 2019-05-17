namespace Adform.Entities.RequestEntities
{
    public class GetTokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}