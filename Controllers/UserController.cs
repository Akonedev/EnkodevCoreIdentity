using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EnkodevCoreIdentity.ViewModels;
using EnkodevCoreIdentity.Interfaces;
using EnkodevCoreIdentity.Models;
using EnkodevCoreIdentity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace EnkodevCoreIdentity.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        // private readonly IPhotoService _photoService;
        private readonly ILocationService _locationService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext db, IUserRepository userRepository, UserManager<AppUser> userManager, ILocationService locationService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _db = db;
            _locationService = locationService;
            // _photoService = photoService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Pace = user.Pace,
                    City = user.City,
                    State = user.State,
                    Mileage = user.Mileage,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
                };
                result.Add(userViewModel);
            }
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();          

            var role = userRole.Select(u => u.UserId == user.Id).ToList();
            List<string> userRoles = (List<string>)await _userManager.GetRolesAsync(user);

          // var userClaimsList = await _userManager.GetClaimsAsync(user);
           // var userClaimsList = userClaims.ToList();

            // UserClaimsViewModel userClaimsViewModel = new UserClaimsViewModel();
             var existingUserClaims = await _userManager.GetClaimsAsync(user);
             var userClaimsList = new List<UserClaim>();

            foreach (Claim claim in ClaimStore.claimsList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                userClaimsList.Add(userClaim);
            }

            /*  var model = new UserClaimsViewModel()
              {
                  UserId = id
              };
              var existingUserClaims = await _userManager.GetClaimsAsync(user);
              foreach (Claim claim in ClaimStore.claimsList)
              {
                  UserClaim userClaim = new UserClaim
                  {
                      ClaimType = claim.Type
                  };
                  if (existingUserClaims.Any(c => c.Type == claim.Type))
                  {
                      userClaim.IsSelected = true;
                  }
                  model.Claims.Add(userClaim);
              }
              ViewData["userClaims"] = model;*/

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                City = user.City,
                State = user.State,
                Mileage = user.Mileage,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
                RoleList = userRoles,
            };
            userDetailViewModel.Claims= userClaimsList;

            return View(userDetailViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditProfileViewModel()
            {
                City = user.City,
                State = user.State,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(editMV);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) // only update profile image
            {
                // var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                /*if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("EditProfile", editVM);
                }*/

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    //    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                // user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await _userManager.UpdateAsync(user);

                return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            UserCreateViewModel user = new();

            var userCreateViewModel = new UserCreateViewModel();
            return View(userCreateViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(UserCreateViewModel userCreateVM)
        {

            if (!ModelState.IsValid) return View(userCreateVM);

            var user = await _userManager.FindByEmailAsync(userCreateVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already in use");
                return View(userCreateVM);
            }

            var userLocation = await _locationService.GetCityByZipCode(userCreateVM.ZipCode ?? 0);

            if (userLocation == null)
            {
                ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
                return View(userCreateVM);
            }

            var newUser = new AppUser
            {
                UserName = userCreateVM.UserName,
                Email = userCreateVM.Email,
                Address = new Address()
                {
                    State = userLocation.StateCode,
                    City = userLocation.CityName,
                    Street = userCreateVM.Street,
                    /*   State = userCreateVM.State
                     City = userCreateVM.City,,*/
                    ZipCode = userCreateVM.ZipCode ?? 0,
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, userCreateVM.Password);

            if (newUserResponse.Succeeded)
            {
                // await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "User");
        }



        [HttpGet]
        [Authorize]
        public IActionResult Delete()
        {
            var userDeleteViewModel = new UserDeleteViewModel();

            return View(userDeleteViewModel);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(UserDeleteViewModel userDeleteViewModel)
        {

            var userToDel = await _userManager.FindByNameAsync(userDeleteViewModel.UserName);

            if (userToDel == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(userToDel);

            return RedirectToAction("Index", "User");
        }


        [HttpGet]
        public async Task<IActionResult> ManageClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimStore.claimsList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel userClaimsViewModel)
        {
            var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                return View(userClaimsViewModel);
            }

            result = await _userManager.AddClaimsAsync(user,
                userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString()))
                );

            if (!result.Succeeded)
            {
                return View(userClaimsViewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}