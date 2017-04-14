using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class SoMeShareViewModel
    {
        public IPublishedContent CurrentPage { get; set; }

        public bool EnableFacebook { get; set; }

        public bool EnableEmail { get; set; }

        public bool EnableTwitter { get; set; }

        public bool EnableGooglePlus { get; set; }

        public bool EnableLinkedIn { get; set; }

        public bool EnablePrint { get; set; }

        public bool EnableCopyUrl { get; set; }

        public SoMeShareViewModel()
        {
            this.EnableFacebook = false;
            this.EnableTwitter = false;
            this.EnableGooglePlus = false;
            this.EnableLinkedIn = false;
            this.EnableEmail = false;
            this.EnableCopyUrl = false;

        }
    }
}
