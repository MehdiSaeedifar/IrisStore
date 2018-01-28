﻿using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region VerifyCodeViewModel
    public class VerifyCodeViewModel
    {
        #region Properties
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
        #endregion
    }
    #endregion
}