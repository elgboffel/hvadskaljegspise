using Our.Umbraco.LinksPicker.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class HeroViewModel
    {
        public IEnumerable<IPublishedContent> Image { get; set; }

        public bool FluidHero { get; set; }

        public int GridSize { get; set; }

        public int GridTotalPadding { get; set; }

        public int ImageHeightLargeDesktop { get; set; }

        public int ImageHeight { get; set; }

        public int ImageHeightMobile { get; set; }

        public string Headline { get; set; }

        public HeroViewModel()
        {
            this.FluidHero = false;
        }
    }
}
