using CoreService.Domain.DomainEvents.Profile;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Company;
using CoreService.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Profile
{
    public sealed class ProfileEntity : BaseEntityWithSoftDelete<Guid>
    {
        public ProfileEntity()
        {
            Age = 0;
            Firstname = string.Empty;
            Lastname = string.Empty;
            BirthDate = DateTimeOffset.UtcNow;
            User = null;
            UserId = null;
            Company = null;
            CompanyId = null;
        }

        private ProfileEntity(UInt16 age, Guid userId, string firstname, string lastname, DateTimeOffset birthDate)
        {
            Age = age;
            UserId = userId;
            Firstname = firstname;
            Lastname = lastname;
            BirthDate = birthDate;
        }

        [Range(1, 100, ErrorMessage = "Age should be between 1 and 100")]
        public UInt16 Age { get; private set; }

        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public DateTimeOffset BirthDate { get; private set; }
        public UserEntity? User { get; private set; }
        public Guid? UserId { get; private set; }
        public CompanyEntity? Company { get; private set; }
        public Guid? CompanyId { get; private set; }

        public void CreateProfilePictureWhenProfileCreated(string userId, string userProfileId, byte[] profilePictureData, string profilePictureExtension, string profilePictureName)
        {
            AddDomainEvents(new CreateProfilePictureWhenProfileCreatedDomainEvent(userId, userProfileId, profilePictureData, profilePictureExtension, profilePictureName));
        }

        public static ProfileEntity CreateNewProfile(UInt16 age, Guid userId, string firstname, string lastname, ulong birthDate)
        {
            return new ProfileEntity(age, userId, firstname, lastname, DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(birthDate)).AddHours(2)); //UTCden dolayı 2 saat ekliyoruz.
        }
    }
}