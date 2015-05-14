namespace Trooper.Thorny.Business.Security
{
    using Trooper.ActiveDirectory;

    public class AdCredential : Credential
    {
        public AdCredential()
        {
            var ad = new ActiveDirectoryUser();

            this.Username = ad.UserName;

            this.Groups = ad.Groups;
        }
        
    }
}
