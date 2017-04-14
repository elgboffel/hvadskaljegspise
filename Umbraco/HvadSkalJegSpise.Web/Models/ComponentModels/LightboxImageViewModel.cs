using Our.Umbraco.LinksPicker.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class LightboxImageViewModel
    {
        public string Id { get; set; }

        public IPublishedContent Image { get; set; }

        public int GridSize { get; set; }

        public int ImageHeightLargeDesktop { get; set; }

        public int ImageHeight { get; set; }

        public int ImageHeightMobile { get; set; }

        public bool ImageIsFluid { get; set; }

        public string Headline { get; set; }

        public string Text { get; set; }

        public string Byline { get; set; }

        public LightboxImageViewModel()
        {
            this.ImageIsFluid = false;
        }

    }
}
