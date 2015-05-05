namespace Sitecore.FakeDb.Security.Web
{
  using System;
  using System.Threading;
  using System.Web.Security;

  public class FakeMembershipProvider : MembershipProvider, IThreadLocalProvider<MembershipProvider>
  {
    private readonly ThreadLocal<MembershipProvider> localProvider = new ThreadLocal<MembershipProvider>();

    private bool disposed;

    public virtual ThreadLocal<MembershipProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override string ApplicationName { get; set; }

    public override bool EnablePasswordReset
    {
      get { return this.IsLocalProviderSet() && this.LocalProvider.Value.EnablePasswordReset; }
    }

    public override bool EnablePasswordRetrieval
    {
      get { return this.IsLocalProviderSet() && this.LocalProvider.Value.EnablePasswordRetrieval; }
    }

    public override int MaxInvalidPasswordAttempts
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.MaxInvalidPasswordAttempts : 0; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.MinRequiredNonAlphanumericCharacters : 0; }
    }

    public override int MinRequiredPasswordLength
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.MinRequiredPasswordLength : 0; }
    }

    public override int PasswordAttemptWindow
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.PasswordAttemptWindow : 0; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.PasswordFormat : MembershipPasswordFormat.Clear; }
    }

    public override string PasswordStrengthRegularExpression
    {
      get { return this.IsLocalProviderSet() ? this.LocalProvider.Value.PasswordStrengthRegularExpression : null; }
    }

    public override bool RequiresQuestionAndAnswer
    {
      get { return this.IsLocalProviderSet() && this.LocalProvider.Value.RequiresQuestionAndAnswer; }
    }

    public override bool RequiresUniqueEmail
    {
      get { return this.IsLocalProviderSet() && this.LocalProvider.Value.RequiresUniqueEmail; }
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.ChangePassword(username, oldPassword, newPassword);
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
    }

    public override MembershipUser CreateUser(string username, string password, string email,
      string passwordQuestion, string passwordAnswer,
      bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
      if (!this.IsLocalProviderSet())
      {
        status = MembershipCreateStatus.UserRejected;
        return null;
      }

      return this.LocalProvider.Value.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.DeleteUser(username, deleteAllRelatedData);
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
      if (!this.IsLocalProviderSet())
      {
        totalRecords = 0;
        return new MembershipUserCollection();
      }

      return this.LocalProvider.Value.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
      if (!this.IsLocalProviderSet())
      {
        totalRecords = 0;
        return new MembershipUserCollection();
      }

      return this.LocalProvider.Value.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
      if (!this.IsLocalProviderSet())
      {
        totalRecords = 0;
        return new MembershipUserCollection();
      }

      return this.LocalProvider.Value.GetAllUsers(pageIndex, pageSize, out totalRecords);
    }

    public override int GetNumberOfUsersOnline()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetNumberOfUsersOnline() : 0;
    }

    public override string GetPassword(string username, string answer)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetPassword(username, answer) : null;
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetUser(providerUserKey, userIsOnline) : null;
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
      if (!this.IsLocalProviderSet())
      {
        return new MembershipUser("fake", username, new Guid(), null, null, null, true, false, DateTime.MinValue,
          DateTime.Now, DateTime.Now, DateTime.MinValue, DateTime.MinValue);
      }

      return this.LocalProvider.Value.GetUser(username, userIsOnline);
    }

    public override string GetUserNameByEmail(string email)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetUserNameByEmail(email) : null;
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override string ResetPassword(string username, string answer)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.ResetPassword(username, answer) : null;
    }

    public override bool UnlockUser(string userName)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.UnlockUser(userName);
    }

    public override void UpdateUser(MembershipUser user)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.UpdateUser(user);
      }
    }

    public override bool ValidateUser(string username, string password)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.ValidateUser(username, password);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      this.localProvider.Dispose();

      this.disposed = true;
    }
  }
}