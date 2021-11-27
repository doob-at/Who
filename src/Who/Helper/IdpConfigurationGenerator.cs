using Who.Auth;

namespace doob.Who.Helper
{
    public static class IdpConfigurationGenerator
    {


        public static IdpConfiguration Build(StartUpConfiguration startUpConfiguration)
        {
            var idpConfig = new IdpConfiguration();

            var httpsUri = IdpUriGenerator.GenerateRedirectUri(startUpConfiguration.ListeningIP, startUpConfiguration.HttpsPort);

            idpConfig.RedirectUris.Add(httpsUri);
            idpConfig.PostLogoutUris.Add(httpsUri);

            return idpConfig;

        }
    }
}
