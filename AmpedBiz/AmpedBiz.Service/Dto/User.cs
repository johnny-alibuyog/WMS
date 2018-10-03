using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Person Person { get; set; }

        public Address Address { get; set; }

        public Branch Branch { get; set; }

        public Guid BranchId { get; set; }

        public List<Role> Roles { get; set; }
    }

    public class UserPageItem
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string BranchName { get; set; }

        public Person Person { get; set; }

        public Address Address { get; set; }

        public List<Role> Roles { get; set; }
    }

    public class UserInfo
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public Person Person { get; set; }
    }

    public class UserPassword
    {
        public Guid Id { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

	public class UserResetPassword
	{
		public Guid Id { get; set; }
	}

	public class UserAddress
    {
        public Guid Id { get; set; }

        public Address Address { get; set; }
    }
}
