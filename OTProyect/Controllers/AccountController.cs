using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OTProyect.Models;
using OTProyect.ViewModels.assistant;
using Microsoft.AspNet.Identity.EntityFramework;
using OTProyect.ViewModels.Security;
using System.Data.Entity;
using PagedList;
using System.Collections.Generic;

namespace OTProyect.Controllers
{
    /// <summary>
    /// Seguridad
    /// </summary>
    [Authorize(Roles ="Administrador")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Lista los usuarios
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="CurrentUserName"></param>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, int? page, DataPageUser search, string CurrentUserName)
        {

            try
            {
                //deaclaramos valores iniciales para ordenacion
                ViewBag.CurrentSort = sortOrder;
                ViewBag.Param1SortParm = String.IsNullOrEmpty(sortOrder) ? "UserName_desc" : "";

                //mapear los datos sobre los filtro actuales y mantener
                #region Map CurrentFilter
                var currentFilter = new DataPageUser
                {
                    UserName = CurrentUserName
                };
                #endregion

                //verificar pagina inicial o filtro
                if (search.isValid() && !currentFilter.isValid())
                {
                    page = 1;
                }
                else
                {
                    search = currentFilter;
                }

                ViewBag.currentFilter = search;

                using (var db = new ApplicationDbContext())
                {
                    //Predicado
                    var query = db.Users.Include(x => x.Roles).Where(x => x.UserName != null);

                    //busqueda de los datos
                    if (ModelState.IsValid)
                    {

                        //Buscar por nombre
                        if (search.UserName != null)
                        {
                            query = query.Where(x => x.UserName.Contains(search.UserName));
                        }
                    }

                    ViewBag.CurrentSort = sortOrder;

                    switch (sortOrder)
                    {
                        case "UserName_desc":
                            query = query.OrderByDescending(s => s.UserName);
                            break;
                        default:
                            query = query.OrderBy(s => s.UserName);
                            break;
                    }



                    //page
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);


                    //View Page
                    var ModelView = new DataPageUser
                    {
                        Lista = query.ToPagedList(pageNumber, pageSize)
                    };

                    if (search != null)
                    {
                        if (search.UserName != null) { ModelView.UserName = search.UserName; }
                    }

                    //count
                    var Count = query.Count();
                    ViewBag.Count = Count;

                    return View(ModelView);
                }
            }
            catch(Exception e){
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
            
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("index","home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("index", "home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try{
                if (ModelState.IsValid)
                {
                    //Nueva Instancia
                    var user = new ApplicationUser { UserName = model.UserName };
                    //Crear cuenta de usuario
                    var result = await UserManager.CreateAsync(user, model.Password);

                    //Si fue exitosa la creación de la cuenta
                    if (result.Succeeded)
                    {

                        //Guardar roles
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                        foreach (var newRole in model.Roles)
                        {
                            if (!await RoleManager.RoleExistsAsync(newRole))
                            {
                                var roleResult = await RoleManager.CreateAsync(new IdentityRole(newRole));
                                await UserManager.AddToRoleAsync(user.Id, newRole);
                            }
                            else
                            {
                                await UserManager.AddToRoleAsync(user.Id, newRole);
                            }
                        }
                        

                        //Se ha agregado una cuenta de usuario exitosamente
                        return View("Message", new MessageResult
                        {
                            Type = MessageResult.TypeMessage.InformationSuccess,
                            Title = "Operación ejecutada exitosamente",
                            Message = "Se ha creado exitosamente la cuenta de usuario: " + model.UserName
                        });
                    }
                    AddErrors(result);
                }

                // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
                return View(model);
            }
            catch(Exception e) {
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
 
        }

        //
        // Get: //Acount/Update
        public ActionResult Update(string Id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var Usuario = db.Users.Where(x => x.Id == Id).FirstOrDefault();
                    if (Usuario == null)
                    {
                        return View("Message", new MessageResult
                        {
                            Type = MessageResult.TypeMessage.InformationHelper,
                            Title = "No se encontro el usuario correspondiente",
                            Message = "Se ha intentado buscar el usuario correspondiente, pero no se encontraron resultados"
                        });
                    }

                    var Model = new UpdateUserViewModel
                    {
                        Id = Usuario.Id,
                        UserName=Usuario.UserName
                    };

                    var roles = Usuario.Roles.ToList();
                    List<string> RolesLista = new List<string>();
                    foreach (var rol in roles)
                    {
                        var rolBd = db.Roles.Where(x => x.Id == rol.RoleId).FirstOrDefault();
                        RolesLista.Add(rolBd.Name);
                    }
                    Model.Roles = RolesLista;
                    return View(Model);
                }
            }
            catch(Exception e)
            {
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
        }

        //
        // Post: //Acount/update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UpdateUserViewModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var bd = new ApplicationDbContext())
                    {

                        var usuarioIntegro = bd.Users.Where(x => x.Id == Model.Id).FirstOrDefault();

                        if (usuarioIntegro == null)
                        {
                            //Mostrar exection no controlada
                            return View("Message", new MessageResult
                            {
                                Type = MessageResult.TypeMessage.InformationFail,
                                Title = "No encontrado",
                                Message = "No se encontro ningun usuario correspondiente"
                            });
                        }

                        //validar que no actulize a otro usuario
                        var usuarioxNombre = bd.Users.Where(x => x.UserName == Model.UserName && x.Id!=Model.Id).FirstOrDefault();
                        if (usuarioxNombre != null)
                        {
                            ModelState.AddModelError("UserName","No puede actualizar a otro nombre de usuario existente, intente con otro" );
                            return View(Model);
                        }

                        List<string> RolesNew = new List<string>();
                        List<string> RolIdDetails = new List<string>();
                        List<string> RolNameIntegro = new List<string>();
                        var User = UserManager.FindById(usuarioIntegro.Id);


                        foreach (var item in User.Roles)
                        {
                            RolNameIntegro.Add(bd.Roles.Where(x => x.Id == item.RoleId).FirstOrDefault().Name);
                        }
                        //Delete
                        foreach (var rol in RolNameIntegro)
                        {

                            if (Model.Roles.Contains(rol) == false)
                            {
                                RolIdDetails.Add(rol);
                            }
                        }

                        //new
                        foreach (var Rolnew in Model.Roles)
                        {
                            foreach (var RoldOld in User.Roles)
                            {
                                var RolnewIntegro = bd.Roles.Where(x => x.Name == Rolnew).FirstOrDefault();
                                if (RoldOld.RoleId != RolnewIntegro.Id && Model.Roles.Contains(Rolnew) && !RolesNew.Contains(Rolnew))
                                {
                                    RolesNew.Add(Rolnew);
                                }
                            }
                        }

                        //work delete
                        foreach (var delete in RolIdDetails.ToList())
                        {
                            await UserManager.RemoveFromRoleAsync(usuarioIntegro.Id, delete);
                        }
                        //work add
                        foreach (var role in RolesNew)
                        {
                            await UserManager.AddToRoleAsync(usuarioIntegro.Id, role);
                        }

                        usuarioIntegro.UserName = Model.UserName;
                        bd.SaveChanges();

                        //Se ha agregado una cuenta de usuario exitosamente
                        return View("Message", new MessageResult
                        {
                            Type = MessageResult.TypeMessage.InformationSuccess,
                            Title = "Operación ejecutada exitosamente",
                            Message = "Se ha actualizado exitosamente la cuenta de usuario: " + Model.UserName
                        });

                    }
                }
                return View(Model);
            }
            catch(Exception e)
            {
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // No revelar que el usuario no existe o que no está confirmado
                    return View("ForgotPasswordConfirmation");
                }

                // Para obtener más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
                // Enviar correo electrónico con este vínculo
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generar el token y enviarlo
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Si el usuario no tiene ninguna cuenta, solicitar que cree una
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtener datos del usuario del proveedor de inicio de sesión externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Inicio", "Inicio");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}