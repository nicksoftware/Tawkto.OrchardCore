using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tawkto.OrchardCore.ViewModels
{
    public class TawktoSettingsViewModel
    {
        [Required]
        public string WidgetName { get; set; }
        
        [Required]
        public string TokenKey { get; set; }
    }
}
