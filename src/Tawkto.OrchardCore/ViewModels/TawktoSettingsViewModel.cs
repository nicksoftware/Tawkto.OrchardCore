using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Tawkto.OrchardCore.ViewModels
{
    public class TawktoSettingsViewModel
    {

        public string WidgetName { get; set; }
        
        public string TokenKey { get; set; }
    }
}
