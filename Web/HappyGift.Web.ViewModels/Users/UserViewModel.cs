namespace HappyGift.Web.ViewModels.Users
{
    using System;

    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CommingBirthdayDate { get; set; }
    }
}
