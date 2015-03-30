namespace Sitecore.FakeDb.Tests.Security
{
  using System;
  using System.Web.Security;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;

  public class FakeMembershipProviderTest : IDisposable
  {
    private const string UserName = "John";

    private const string Password = "******";

    private const string Email = "john@mail.com";

    private const string Question = "question?";

    private const string Answer = "answer!";

    private readonly FakeMembershipProvider provider;

    private readonly MembershipProvider localProvider;

    private readonly MembershipUser user;

    public FakeMembershipProviderTest()
    {
      this.localProvider = Substitute.For<MembershipProvider>();
      this.provider = new FakeMembershipProvider();
      this.provider.LocalProvider.Value = this.localProvider;

      this.user = new MembershipUser("fake", UserName, new Guid(), null, null, null, true, false, DateTime.MinValue,
        DateTime.Now, DateTime.Now, DateTime.MinValue, DateTime.MinValue);
    }

    [Fact]
    public void ShouldDoNothingIfNoLocalProviderSet()
    {
      // arrange
      var stubProvider = new FakeMembershipProvider();
      MembershipCreateStatus status;
      int totalRecords;

      // act & assert
      stubProvider.ApplicationName.Should().BeNull();
      stubProvider.ChangePassword(null, null, null).Should().BeFalse();
      stubProvider.ChangePasswordQuestionAndAnswer(null, null, null, null).Should().BeFalse();
      stubProvider.CreateUser(null, null, null, null, null, false, null, out status).Should().BeNull();
      status.Should().Be(MembershipCreateStatus.UserRejected);
      stubProvider.DeleteUser(null, false).Should().BeFalse();
      stubProvider.Description.Should().BeNull();
      stubProvider.EnablePasswordReset.Should().BeFalse();
      stubProvider.EnablePasswordRetrieval.Should().BeFalse();
      stubProvider.FindUsersByEmail(null, 0, 0, out totalRecords).Should().BeEmpty();
      totalRecords.Should().Be(0);
      stubProvider.FindUsersByName(null, 0, 0, out totalRecords).Should().BeEmpty();
      totalRecords.Should().Be(0);
      stubProvider.GetAllUsers(0, 0, out totalRecords).Should().BeEmpty();
      totalRecords.Should().Be(0);
      stubProvider.GetNumberOfUsersOnline().Should().Be(0);
      stubProvider.GetPassword(null, null).Should().BeNull();
      stubProvider.GetUser((object)null, false).Should().BeNull();
      stubProvider.GetUser("John", false).UserName.Should().Be("John");
      stubProvider.GetUserNameByEmail(null).Should().BeNull();
      stubProvider.MaxInvalidPasswordAttempts.Should().Be(0);
      stubProvider.MinRequiredNonAlphanumericCharacters.Should().Be(0);
      stubProvider.MinRequiredPasswordLength.Should().Be(0);
      stubProvider.Name.Should().BeNull();
      stubProvider.PasswordAttemptWindow.Should().Be(0);
      stubProvider.PasswordFormat.Should().Be(MembershipPasswordFormat.Clear);
      stubProvider.PasswordStrengthRegularExpression.Should().BeNull();
      stubProvider.RequiresQuestionAndAnswer.Should().BeFalse();
      stubProvider.RequiresUniqueEmail.Should().BeFalse();
      stubProvider.ResetPassword(null, null).Should().BeNull();
      stubProvider.UnlockUser(null).Should().BeFalse();
      stubProvider.UpdateUser(null);
      stubProvider.ValidateUser(null, null).Should().BeFalse();
    }

    [Fact]
    public void ShouldBeThreadLocalProvider()
    {
      // assert
      this.provider.Should().BeAssignableTo<IThreadLocalProvider<MembershipProvider>>();
    }

    [Fact]
    public void ShouldGetEnablePasswordReset()
    {
      // arrange
      this.localProvider.EnablePasswordReset.Returns(true);

      // act & assert
      this.provider.EnablePasswordReset.Should().BeTrue();
    }

    [Fact]
    public void ShouldGetEnablePasswordRetrieval()
    {
      // arrange
      this.localProvider.EnablePasswordRetrieval.Returns(true);

      // act & assert
      this.provider.EnablePasswordRetrieval.Should().BeTrue();
    }

    [Fact]
    public void ShouldGetMaxInvalidPasswordAttempts()
    {
      // arrange
      this.localProvider.MaxInvalidPasswordAttempts.Returns(1);

      // act & assert
      this.provider.MaxInvalidPasswordAttempts.Should().Be(1);
    }

    [Fact]
    public void ShouldGetMinRequiredNonAlphanumericCharacters()
    {
      // arrange
      this.localProvider.MinRequiredNonAlphanumericCharacters.Returns(1);

      // act & assert
      this.provider.MinRequiredNonAlphanumericCharacters.Should().Be(1);
    }

    [Fact]
    public void ShouldGetMinRequiredPasswordLength()
    {
      // arrange
      this.localProvider.MinRequiredPasswordLength.Returns(1);

      // act & assert
      this.provider.MinRequiredPasswordLength.Should().Be(1);
    }

    [Fact]
    public void ShouldGetPasswordAttemptWindow()
    {
      // arrange
      this.localProvider.PasswordAttemptWindow.Returns(1);

      // act & assert
      this.provider.PasswordAttemptWindow.Should().Be(1);
    }

    [Fact]
    public void ShouldGet()
    {
      // arrange
      this.localProvider.PasswordFormat.Returns(MembershipPasswordFormat.Encrypted);

      // act & assert
      this.provider.PasswordFormat.Should().Be(MembershipPasswordFormat.Encrypted);
    }

    [Fact]
    public void ShouldGetPasswordStrengthRegularExpression()
    {
      // arrange
      this.localProvider.PasswordStrengthRegularExpression.Returns("^");

      // act & assert
      this.provider.PasswordStrengthRegularExpression.Should().Be("^");
    }

    [Fact]
    public void ShouldGetRequiresQuestionAndAnswer()
    {
      // arrange
      this.localProvider.RequiresQuestionAndAnswer.Returns(true);

      // act & assert
      this.provider.RequiresQuestionAndAnswer.Should().BeTrue();
    }

    [Fact]
    public void ShouldGetRequiresUniqueEmail()
    {
      // arrange
      this.localProvider.RequiresUniqueEmail.Returns(true);

      // act & assert
      this.provider.RequiresUniqueEmail.Should().BeTrue();
    }

    [Fact]
    public void ShouldChangePassword()
    {
      // arrange
      this.localProvider.ChangePassword(UserName, "123456", "******").Returns(true);

      // act & assert
      this.provider.ChangePassword(UserName, "123456", "******").Should().BeTrue();
    }

    [Fact]
    public void ShouldChangePasswordQuestionAndAnswer()
    {
      // arrange
      this.localProvider.ChangePasswordQuestionAndAnswer(UserName, Password, Question, Answer).Returns(true);

      // act & assert
      this.provider.ChangePasswordQuestionAndAnswer(UserName, Password, Question, Answer).Should().BeTrue();
    }

    [Fact]
    public void ShouldCreateUser()
    {
      // arrange
      var key = new object();
      var status = MembershipCreateStatus.UserRejected;

      this.localProvider
        .CreateUser(UserName, Password, Email, Question, Answer, true, key, out status)
        .Returns(x =>
          {
            x[7] = MembershipCreateStatus.Success;
            return this.user;
          });

      // act & assert
      this.provider
        .CreateUser(UserName, Password, Email, Question, Answer, true, key, out status)
        .Should().BeSameAs(this.user);
    }

    [Fact]
    public void ShouldDeleteUser()
    {
      // arrange
      this.localProvider.DeleteUser(UserName, true).Returns(true);

      // act & assert
      this.provider.DeleteUser(UserName, true).Should().BeTrue();
    }

    [Fact]
    public void ShouldFindUserByEmail()
    {
      // arrange
      var users = new MembershipUserCollection();
      int totalRecords;

      this.localProvider
        .FindUsersByEmail(Email, 5, 20, out totalRecords)
        .Returns(x =>
          {
            x[3] = 200;
            return users;
          });

      // act
      var result = this.provider.FindUsersByEmail(Email, 5, 20, out totalRecords);

      // assert
      result.Should().BeSameAs(users);
      totalRecords.Should().Be(200);
    }

    [Fact]
    public void ShouldFindUsersByName()
    {
      // arrange
      var users = new MembershipUserCollection();
      int totalRecords;

      this.localProvider
        .FindUsersByName(UserName, 5, 20, out totalRecords)
        .Returns(x =>
        {
          x[3] = 200;
          return users;
        });

      // act
      var result = this.provider.FindUsersByName(UserName, 5, 20, out totalRecords);

      // assert
      result.Should().BeSameAs(users);
      totalRecords.Should().Be(200);
    }

    [Fact]
    public void ShouldGetAllUsers()
    {
      // arrange
      var users = new MembershipUserCollection();
      int totalRecords;

      this.localProvider
        .GetAllUsers(5, 20, out totalRecords)
        .Returns(x =>
        {
          x[2] = 200;
          return users;
        });

      // act
      var result = this.provider.GetAllUsers(5, 20, out totalRecords);

      // assert
      result.Should().BeSameAs(users);
      totalRecords.Should().Be(200);
    }

    [Fact]
    public void ShouldGetNumberOfUsersOnline()
    {
      // arrange
      this.localProvider.GetNumberOfUsersOnline().Returns(1);

      // act & assert
      this.provider.GetNumberOfUsersOnline().Should().Be(1);
    }

    [Fact]
    public void ShouldGetPassword()
    {
      // arrange
      this.localProvider.GetPassword(UserName, Answer).Returns(Password);

      // act & assert
      this.provider.GetPassword(UserName, Answer).Should().Be(Password);
    }

    [Fact]
    public void ShouldGetUserByKey()
    {
      // arrange
      var key = new object();

      this.localProvider.GetUser(key, true).Returns(this.user);

      // act & assert
      this.provider.GetUser(key, true).Should().Be(this.user);
    }

    [Fact]
    public void ShouldGetUserByName()
    {
      // arrange
      this.localProvider.GetUser(UserName, true).Returns(this.user);

      // act & assert
      this.provider.GetUser(UserName, true).Should().Be(this.user);
    }

    [Fact]
    public void ShouldGetUserNameByEmail()
    {
      // arrange
      this.localProvider.GetUserNameByEmail(Email).Returns(UserName);

      // act & assert
      this.provider.GetUserNameByEmail(Email).Should().Be(UserName);
    }

    [Fact]
    public void ShouldResetPassword()
    {
      // arrange
      this.localProvider.ResetPassword(UserName, Answer).Returns(Password);

      // act & assert
      this.provider.ResetPassword(UserName, Answer).Should().Be(Password);
    }

    [Fact]
    public void ShouldUnlockUser()
    {
      // arrange
      this.localProvider.UnlockUser(UserName).Returns(true);

      // act & assert
      this.provider.UnlockUser(UserName).Should().Be(true);
    }

    [Fact]
    public void ShouldUpdateUser()
    {
      // act
      this.provider.UpdateUser(this.user);

      // assert
      this.localProvider.Received().UpdateUser(this.user);
    }

    [Fact]
    public void ShouldValidateUser()
    {
      // arrange
      this.localProvider.ValidateUser(UserName, Password).Returns(true);

      // act & assert
      this.provider.ValidateUser(UserName, Password).Should().BeTrue();
    }

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}