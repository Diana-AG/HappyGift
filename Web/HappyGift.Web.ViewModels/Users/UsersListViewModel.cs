namespace HappyGift.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UsersListViewModel
    {
        [Required(ErrorMessage = "Please select a user.")]
        public string SelectedUserId { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
