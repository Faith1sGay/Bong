namespace Bong.Services.Config
{
    public class Config
    {
        string token;
        string prefix;
        string topggToken;
        ulong owners;
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
            }
        }
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
            }
        }
        public string TopGGToken
        {
            get
            {
                return topggToken;
            }
            set
            {
                topggToken = value;
            }
        }
        public ulong Owners
        {
            get
            {
                return owners;
            }
            set
            {
                owners = value;
            }
        }
    }
}