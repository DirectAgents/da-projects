namespace Taboola.Entities.ResponseEntities
{
    internal class TaboolaTokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}
