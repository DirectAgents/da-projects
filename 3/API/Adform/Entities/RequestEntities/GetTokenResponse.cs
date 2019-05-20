namespace Adform.Entities.RequestEntities
{
    internal class GetTokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}