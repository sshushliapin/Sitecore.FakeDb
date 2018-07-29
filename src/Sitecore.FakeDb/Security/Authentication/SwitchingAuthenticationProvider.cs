namespace Sitecore.FakeDb.Security.Authentication
{
    using Sitecore.Common;
    using Sitecore.Diagnostics;
    using Sitecore.Security.Accounts;
    using Sitecore.Security.Authentication;

    public class SwitchingAuthenticationProvider : AuthenticationProvider
    {
        private readonly User defaultActiveUser = User.FromName(@"default\Anonymous", false);

        private AuthenticationProvider defaultProvider;

        public AuthenticationProvider DefaultProvider
        {
            get { return this.defaultProvider; }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.defaultProvider = value;
            }
        }

        private AuthenticationProvider CurrentProvider
        {
            get
            {
                var switcher = Switcher<AuthenticationProvider>.CurrentValue;
                if (switcher != null)
                {
                    return switcher;
                }

                var provider = this.DefaultProvider;
                return provider ?? switcher;
            }
        }

        public override User GetActiveUser()
        {
            return this.CurrentProvider != null ? this.CurrentProvider.GetActiveUser() : this.defaultActiveUser;
        }

        public override bool Login(User user)
        {
            return this.CurrentProvider != null
                   && this.CurrentProvider.Login(user);
        }

        public override bool Login(string userName, string password, bool persistent)
        {
            return this.CurrentProvider != null
                   && this.CurrentProvider.Login(userName, password, persistent);
        }

        public override bool Login(string userName, bool persistent)
        {
            return this.CurrentProvider != null
                   && this.CurrentProvider.Login(userName, persistent);
        }

        public override void Logout()
        {
            if (this.CurrentProvider != null)
            {
                this.CurrentProvider.Logout();
            }
        }

        public override void SetActiveUser(string userName)
        {
            if (this.CurrentProvider != null)
            {
                this.CurrentProvider.SetActiveUser(userName);
            }
        }

        public override void SetActiveUser(User user)
        {
            if (this.CurrentProvider != null)
            {
                this.CurrentProvider.SetActiveUser(user);
            }
        }
    }
}