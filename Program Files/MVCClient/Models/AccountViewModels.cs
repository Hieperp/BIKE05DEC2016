using System.ComponentModel.DataAnnotations;
// New namespace imports:
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;


namespace MVCClient.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



        // New Fields added to extend Application User class:

        [Required]

        [Display(Name = "First Name")]

        public string FirstName { get; set; }



        [Required]

        [Display(Name = "Last Name")]

        public string LastName { get; set; }



        // Return a pre-poulated instance of AppliationUser:

        public ApplicationUser GetUser()
        {

            var user = new ApplicationUser()

            {
                Email = this.Email,

                FirstName = this.FirstName,

                LastName = this.LastName

            };

            return user;

        }


    }



    public class EditUserViewModel
    {

        public EditUserViewModel() { }


        // Allow Initialization with an instance of ApplicationUser:

        public EditUserViewModel(ApplicationUser user)
        {

            this.Id = user.Id;

            this.Email = user.Email;

            this.FirstName = user.FirstName;

            this.LastName = user.LastName;

        }


        [Required]

        public string Id { get; set; }

        [Required]

        public string Email { get; set; }


        [Required]

        [Display(Name = "First Name")]

        public string FirstName { get; set; }



        [Required]

        [Display(Name = "Last Name")]

        public string LastName { get; set; }

    }


    public class SelectUserRolesViewModel
    {

        public SelectUserRolesViewModel()
        {

            this.Roles = new List<SelectRoleEditorViewModel>();

        }





        // Enable initialization with an instance of ApplicationUser:

        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {

            this.Email = user.Email;

            this.FirstName = user.FirstName;

            this.LastName = user.LastName;



            var Db = new ApplicationDbContext();



            // Add all available roles to the list of EditorViewModels:

            var allRoles = Db.Roles;

            foreach (var role in allRoles)
            {

                // An EditorViewModel will be used by Editor Template:

                var rvm = new SelectRoleEditorViewModel(role);

                this.Roles.Add(rvm);

            }



            // Set the Selected property to true for those roles for 

            // which the current user is a member:

            foreach (var userRole in user.Roles)
            {

                var checkUserRole =

                    this.Roles.Find(r => r.RoleId == userRole.RoleId);

                checkUserRole.Selected = true;

            }

        }



        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }

    }


    // Used to display a single role with a checkbox, within a list structure:

    public class SelectRoleEditorViewModel
    {

        public SelectRoleEditorViewModel() { }

        public SelectRoleEditorViewModel(IdentityRole role)
        {

            this.RoleName = role.Name;
            this.RoleId = role.Id;
            
        }



        public bool Selected { get; set; }



        [Required]

        public string RoleId { get; set; }

        [Required]

        public string RoleName { get; set; }

    }



    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
