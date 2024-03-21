using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Culture
{
    public static class CultureRegistration
    {
        public static void AddCultureRegistrations(this IApplicationBuilder app)
        {
            var mainCulture = new CultureInfo("tr-TR");
            var supportedCultures = new[] { mainCulture, new CultureInfo("en-US"), new CultureInfo("en-GB") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("tr-TR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            mainCulture.NumberFormat.CurrencySymbol = "₺";
            CultureInfo.DefaultThreadCurrentCulture = mainCulture;
            CultureInfo.DefaultThreadCurrentUICulture = mainCulture;
            CultureInfo.CurrentCulture = mainCulture;
            CultureInfo.CurrentUICulture = mainCulture;
        }
    }
}
