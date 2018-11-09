using AmpedBiz.Core.Common.Services.Generators;
using System;

namespace AmpedBiz.Core.Users.Services
{
	public class VerifyPasswordVisitor : IVisitor<User>
    {
        private readonly IHashProvider _hashProvider;

        public virtual string Password { get; set; }

        public virtual bool Verified { get; private set; }

        public virtual Action<bool> ResultCallback { get; set; }

        public VerifyPasswordVisitor(IHashProvider hashProvider = null)
        {
            this._hashProvider = hashProvider ?? new HashProvider();
        }

        public virtual void Visit(User target)
        {
            this.Verified = this._hashProvider.VerifyHashString(this.Password, target.PasswordHash, target.PasswordSalt);

            if (this.ResultCallback != null)
                this.ResultCallback(this.Verified);
        }
    }
}
