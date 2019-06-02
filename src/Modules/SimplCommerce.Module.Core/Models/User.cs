using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Module.Core.Models
{
    public class User : IdentityUser<long>, IEntityWithTypedId<long>, IExtendableObject
    {
        public User()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public const string SettingsDataKey = "Settings";

        public Guid UserGuid { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string FullName { get; set; }

        public long? VendorId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LatestUpdatedOn { get; set; }

        public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

        public UserAddress DefaultShippingAddress { get; set; }

        public long? DefaultShippingAddressId { get; set; }

        public UserAddress DefaultBillingAddress { get; set; }

        public long? DefaultBillingAddressId { get; set; }

        [StringLength(450)]
        public string RefreshTokenHash { get; set; }

        public IList<UserRole> Roles { get; set; } = new List<UserRole>();

        public IList<CustomerGroupUser> CustomerGroups { get; set; } = new List<CustomerGroupUser>();

        [StringLength(450)]
        public string Culture { get; set; }

        /// <inheritdoc />
        public string ExtensionData { get; set; }

        private bool? isGuest;
        ///
        /// Make sure Roles list must be loaded before you call this method to check user is Guest or not
        ///
        public bool IsGuest(){
            if(isGuest ==null){
                isGuest = Roles.Count == 1 && Roles[0].RoleId == (int) EnumRoles.GuestRoleId;
            } 
            return isGuest.Value;
        }
    }
}
