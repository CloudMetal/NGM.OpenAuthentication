﻿using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Providers.OpenId;
using NGM.OpenAuthentication.Providers.OpenId.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdProviderAuthenticator _openIdProviderAuthenticator;
        private readonly IOrchardServices _orchardServices;

        public OpenIdAccountController(IOpenIdProviderAuthenticator openIdProviderAuthenticator,
            IOrchardServices orchardServices)
        {
            _openIdProviderAuthenticator = openIdProviderAuthenticator;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (!_openIdProviderAuthenticator.IsOpenIdCallback) {
                var viewModel = new CreateViewModel();
                TryUpdateModel(viewModel);
                _openIdProviderAuthenticator.EnternalIdentifier = viewModel.ExternalIdentifier;
            }
            var result = _openIdProviderAuthenticator.Authenticate(returnUrl);

            if (result.Status == Statuses.AssociateOnLogon) {
                return new RedirectResult(Url.LogOn(returnUrl));
            }

            if (result.Result != null) return result.Result;

            return HttpContext.Request.IsAuthenticated ? this.RedirectLocal(returnUrl, "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}