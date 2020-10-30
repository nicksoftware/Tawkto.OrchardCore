using System;
using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using Tawkto.OrchardCore;
using Tawkto.OrchardCore.ViewModels;
using OrchardCore.Settings;
using Tawkto.OrchardCore.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Tawkto.OrchardCore.Drivers
{
    public class TawktoSettingsDisplayDrivers : SectionDisplayDriver<ISite, TawktoSettings>
    {

        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly IAuthorizationService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TawktoSettingsDisplayDrivers(IShellHost shellHost,
            ShellSettings shellSettings,
             IAuthorizationService authService,
             IHttpContextAccessor httpContextAccessor)
        {
            _shellHost = shellHost;
            _shellSettings = shellSettings;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async override Task<IDisplayResult> EditAsync(TawktoSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authService.AuthorizeAsync(user, Tawkto.OrchardCore.Permissions.ManageTawkto))
                return null;

            return Initialize<TawktoSettingsViewModel>("TawktoSettingsSettings_Edit", model =>
            {
                model.WidgetName = section.WidgetName?.ToString() ?? string.Empty;
                model.TokenKey= section.TokenKey?.ToString() ?? string.Empty;

            }).Location("Content").OnGroup(Constants.GroupId);
        }


        public override async Task<IDisplayResult> UpdateAsync(TawktoSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authService.AuthorizeAsync(user, Tawkto.OrchardCore.Permissions.ManageTawkto))
                return null;

            if (context.GroupId == Constants.GroupId)
            {
                var model = new TawktoSettingsViewModel();

                if (await context.Updater.TryUpdateModelAsync(model, Prefix))
                {
                    section.WidgetName = model.WidgetName?.Trim();
                    section.TokenKey = model.TokenKey?.Trim();

                    // Release the tenant to apply settings.
                    await _shellHost.ReleaseShellContextAsync(_shellSettings);
                }
            }

            return await EditAsync(section, context);
        }
    }

}