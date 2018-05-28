using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Generators;
using Humanizer;
using System;

namespace AmpedBiz.Core.Services.Users
{
    public class SetPasswordVisitor : IVisitor<User>
    {
        private readonly IHashProvider _hashProvider;

        public virtual string OldPassword { get; set; }

        public virtual string NewPassword { get; set; }

        public virtual string ConfirmPassword { get; set; }

        public SetPasswordVisitor(IHashProvider hashProvider = null)
        {
            this._hashProvider = hashProvider ?? new HashProvider();
        }

        public void Visit(User target)
        {
            this.Validate(target);

            var hash = default(string);

            var salt = default(string);

            this._hashProvider.GetHashAndSaltString(this.NewPassword, out hash, out salt);

            target.PasswordHash = hash;

            target.PasswordSalt = salt;
        }

        private void Validate(User target)
        {
            this.NewPassword.Ensure(
                that: instance => !string.IsNullOrWhiteSpace(instance),
                message: $"{nameof(this.NewPassword).Humanize(LetterCasing.Title)} should have a value."
            );

            this.ConfirmPassword.Ensure(
                that: instance => !string.IsNullOrWhiteSpace(instance),
                message: $"{nameof(this.ConfirmPassword).Humanize(LetterCasing.Title)} should have a value."
            );

            new Tuple<string, string>(this.NewPassword, this.ConfirmPassword).Ensure(
                that: instance => instance.Item1 == instance.Item2,
                message: $"Password does not match."
            );

            if (target.HasPassword())
            {
                this.OldPassword.Ensure(
                    that: instance => !string.IsNullOrWhiteSpace(instance),
                    message: $"{nameof(this.OldPassword).Humanize(LetterCasing.Title)} should have a value."
                );

                var verified = false;

                target.Accept(new VerifyPasswordVisitor(this._hashProvider)
                {
                    Password = this.OldPassword,
                    ResultCallback = (result) => verified = result
                });

                if (!verified)
                    throw new ArgumentException("Invalid Password");
            }

        }
    }
}
