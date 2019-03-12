// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Host.Business.IDbServices;
using Host.Controllers;
using Host.Data;
using Host.DataModel;
using Host.Models;
using Host.Models.AccountViewModels;
using Host.Quickstart.Account;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRoleService _roleService;
        private readonly IEmployeeProfileService _employeeProfileService;
        private readonly ICompanyService _companyService;
        private readonly IAntiforgery _antiforgery;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IServiceProvider serviceProvider,
            IRoleService roleService,
            IEmployeeProfileService employeeProfileService,
            ICompanyService companyService,
            IAntiforgery antiforgery,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _serviceProvider = serviceProvider;
            _roleService = roleService;
            _employeeProfileService = employeeProfileService;
            _companyService = companyService;
            _antiforgery = antiforgery;
            _config = config;
        }

        /// <summary>
        /// Create New User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(Host.Models.AccountViewModels.UserInfoModel user)
        {
            if (!ModelState.IsValid)
                return View("NewHire", user);
            if(user.Id != null)
            {
                using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var userEmail = userMgr.FindByEmailAsync(user.Email).Result;
                    var userProfile = userMgr.FindByEmailAsync(user.Email);

                    userEmail.UserName = user.UserName;
                    userEmail.Email = user.Email;
                    userEmail.Status = true;
                   
                   // userEmail.PasswordHash = user.Password;
                    userEmail.NormalizedUserName = user.NormalizeUserName;

                    //var applicationUser = new ApplicationUser
                    //{
                    //    Email = user.Email,
                    //    Id = user.Id,
                    //    NormalizedEmail = user.NormalizeEmail,
                    //    AccessFailedCount = user.AccessFailedCount,
                    //    EmailConfirmed = user.EmailConfirmed,
                    //    PhoneNumber = user.PhoneNumber,
                    //    UserName = user.UserName,
                    //};

                    //await _userManager.ChangePasswordAsync(applicationUser, user.ConfrimedPassword, user.Password);
                        var result = userMgr.UpdateAsync(userEmail).Result;
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError(result.Errors.ToString(), result.Errors.ToString());
                            throw new Exception(result.Errors.First().Description);
                        }

                        _roleService.UpdateUserRole(userEmail.Id, user.RoleId);

                        Console.WriteLine("User Updated");

                        return RedirectToAction("EmployeeProfile", "Employee");
                    }
                    return View();
                
            }
            using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var userEmail = userMgr.FindByEmailAsync(user.Email).Result;
                if (userEmail == null)
                {
                    var userName = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Status = true
                    };
                    var result = userMgr.CreateAsync(userName, user.Password).Result;
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(result.Errors.ToString(),result.Errors.ToString());
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(userName, new Claim[]{
                        new Claim(JwtClaimTypes.Name, user.UserName),
                        new Claim(JwtClaimTypes.GivenName, user.UserName),
                        new Claim(JwtClaimTypes.FamilyName, user.UserName),
                        new Claim(JwtClaimTypes.Email, user.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    _roleService.AddUserRole(userName.Id, user.RoleId);

                    Console.WriteLine("User Created");
                    var employeProfile = new EmployeeProfileDto
                    {
                        FirstName = user.UserName,
                        FkInitiatedById = GetUserid().ToString(),
                        FkUserId = userName.Id,
                        CreatedOn = DateTime.Now,
                        WorkEmail = user.Email,
                    };

                    await _employeeProfileService.AddEmployeeProfile(employeProfile);

                    if (user.RoleId == "cd149620-3f5c-4081-94d1-f24b2408aa72")
                    {
                        var company = new CompanyDto
                        {
                            Name = user.UserName,
                            UserId = userName.Id
                        };
                        await _companyService.AddCompany(company);
                    }

                    return RedirectToAction("EmployeeProfile", "Employee");
                }
                return View();
            }

        }


        [HttpGet("Account/IsEmailExist/email/{email}")]
        public bool IsEmailExist([FromRoute]string email)
        {
            var isEmail = _employeeProfileService.IsEmailExist(email);
            return isEmail;
        }

        [HttpGet("Account/IsUerNameExist/usesrName/{userName}")]
        public bool IsUserNameExist([FromRoute]string userName)
        {
            var isUserName = _employeeProfileService.IsUserNameExist(userName);
            return isUserName;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ResetPasswordResult> ResetPassword(ResetPasswordViewModel requestDto)
        {
            IdentityResult result = IdentityResult.Failed(null);


            var user = await _userManager.FindByEmailAsync(requestDto.Email);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (user != null)
            {
                result = await _userManager.ResetPasswordAsync(user, resetToken, requestDto.Password);
            }

            return new ResetPasswordResult(result);
        }

        [HttpPost]
        public IActionResult CreateRoles(RolesModel rolesModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Role");
            var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var roleName = context.Roles
                           .AsNoTracking()
                           .Where(i => i.Name == rolesModel.Name)
                           .SingleOrDefault();
            if (roleName == null)
            {
                var roles = new IdentityRole
                {
                    Name = rolesModel.Name,
                    NormalizedName = rolesModel.NormalizedName
                };


                context.Roles.Add(roles);
                context.SaveChanges();
                return RedirectToAction("Role");
            }
            return RedirectToAction("Role");

        }

        [Authorize(Roles="Admin")]
        public IActionResult NewHire(string userId)
        {
            var roleList = _roleService.GetAllRoles();
            if (userId != null)
            {
                var userModel = _employeeProfileService.GetUserInfoByUserId(userId);
                userModel.Roles = new SelectList(roleList, "RoleId", "Name", userModel.RoleId);
                
                return View("NewHire", userModel);
            }
            
            var userInfoModel = new UserInfoModel
            {
                Roles = new SelectList(roleList, "RoleId", "Name")
            };
            return View("NewHire", userInfoModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Role()
        {
            var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var roleModel = _roleService.GetAllRoles();

            return View("AdminRole", roleModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {

            return View("AddRole");
        }

        /// <summary>
        /// Show login page
        /// </summary>
        /// 
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);
            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
            }

            return RedirectToAction("Sign", "Home");
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {

            if (ModelState.IsValid)
            {
                var users = _roleService.GetUserNameByEmail(model.Email);
                var result = await _signInManager.PasswordSignInAsync(users, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(users);
                    var u = await _userManager.FindByEmailAsync(model.Email);
                    var userRoles = await _userManager.GetRolesAsync(u);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(users, user.Id, users));

                    // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                    // the IsLocalUrl check is only necessary if you want to s`port additional local pages, otherwise IsValidReturnUrl is more strict
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                    {
                        //return Redirect(model.ReturnUrl);
                        return RedirectToAction("AdminDashboard", "Home");
                    }

                    //return Redirect("~/");
                    return RedirectToAction("AdminDashboard", "Home");
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));

                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            //var vm = await BuildLoginViewModelAsync(model);
            return RedirectToAction("Login", model);
        }


        [HttpPost("Account/UserLogin")]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin([FromBody] LoginInputModel model)
        {
            var users = _roleService.GetUserNameByEmail(model.Email);
            var result = await _signInManager.PasswordSignInAsync(users, model.Password, model.RememberLogin, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userRoles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                       {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    }.Union(userRoles.Select(m => new Claim(ClaimTypes.Role, m)));

                var token = new JwtSecurityToken
            (
                issuer: "Arsalan",
                audience: "You",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes("rlyaKithdrYVl6Z80ODU350md")),
                        SecurityAlgorithms.HmacSha256)
            );

               
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var employeProfile = new UserInfoDto
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    RoleName = userRoles.Select(i => i).FirstOrDefault()

                };
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) , employeProfile});
                
                // var httpcontext = HttpContext.Request.Method;
               
                //var info = await _signInManager.GetExternalLoginInfoAsync();
                //var claimsPrincipal = await this._signInManager.CreateUserPrincipalAsync(user);
                //((ClaimsIdentity)claimsPrincipal.Identity).AddClaim(new Claim("accessToken", info.AuthenticationTokens.Single(t => t.Name == "access_token").Value));
                //await HttpContext.Authentication.SignInAsync("Identity.Application", claimsPrincipal);
                //var accessToken = info.AuthenticationTokens.Single(f => f.Name == "access_token").Value;
                //var tokenType = info.AuthenticationTokens.Single(f => f.Name == "token_type").Value;
                //var expiryDate = info.AuthenticationTokens.Single(f => f.Name == "expires_at").Value;
                
                //HttpContext.GetTokenAsync("token_Name")

                //return Json(GetUserid(), user());
            }
            else
            {

                return Json("User is not Authenticate");
            }

        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // windows authentication needs special handling
                return await ProcessWindowsLoginAsync(returnUrl);
            }
            else
            {
                // start challenge and roundtrip the return URL and 
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("ExternalLoginCallback"),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
                };
                return Challenge(props, provider);
            }
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await AutoProvisionUserAsync(provider, providerUserId, claims);
            }

            // this allows us to collect any additonal claims or properties
            // for the specific prtotocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForWsFed(result, additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForSaml2p(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            // we must issue the cookie maually, and can't use the SignInManager because
            // it doesn't expose an API to issue additional claims from the login workflow
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            additionalLocalClaims.AddRange(principal.Claims);
            var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name));
            await HttpContext.SignInAsync(user.Id, name, provider, localSignInProps, additionalLocalClaims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // validate return URL and redirect back to authorization endpoint or a local page
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return RedirectToAction("Sign", "Home");
        }

        [AllowAnonymous]
        [HttpPost("Account/User/Logout")]
        public async Task<IActionResult> UserLogOut([FromBody]string userId)
        {

            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();

                // raise the logout event
            }


            return Json("User Logout Successfully");
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = context.IdP } }
                };
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the
                // user back to the external callback, in essence, tresting windows
                // auth the same as any other external authentication mechanism
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("ExternalLoginCallback"),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", AccountOptions.WindowsAuthenticationSchemeName },
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                // add the groups as claims -- be careful if the number of groups is too large
                if (AccountOptions.IncludeWindowsGroups)
                {
                    var wi = wp.Identity as WindowsIdentity;
                    var groups = wi.Groups.Translate(typeof(NTAccount));
                    var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                    id.AddClaims(roles);
                }

                await HttpContext.SignInAsync(
                    IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }
            else
            {
                // trigger windows auth
                // since windows auth don't support the redirect uri,
                // this URL is re-triggered when we call challenge
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }
        }

        private async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            // user's display name
            var name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (name != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else
            {
                var first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // email
            var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
               claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }

            var user = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString(),
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            if (filtered.Any())
            {
                identityResult = await _userManager.AddClaimsAsync(user, filtered);
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            return user;
        }

        private void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var id_token = externalResult.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }
        }

        private void ProcessLoginCallbackForWsFed(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
        }

        private void ProcessLoginCallbackForSaml2p(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
        }
    }
}