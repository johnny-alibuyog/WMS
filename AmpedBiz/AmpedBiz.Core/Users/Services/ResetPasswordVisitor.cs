using AmpedBiz.Core.Common.Services.Generators;
using AmpedBiz.Core.Settings.Services;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Users.Services
{
	public class ResetPasswordVisitor : IVisitor<User>
	{
		private readonly ISettingsFacade _settings;

		private readonly IHashProvider _hashProvider;

		public ResetPasswordVisitor(ISettingsFacade settings, IHashProvider hashProvider = null)
		{
			this._settings = settings;
			this._hashProvider = hashProvider ?? new HashProvider();
		}

		public void Visit(User target)
		{
			this._hashProvider.GetHashAndSaltString(this._settings.Users.DefaultPassword, out var hash, out var salt);

			target.PasswordHash = hash;

			target.PasswordSalt = salt;
		}
	}
}
