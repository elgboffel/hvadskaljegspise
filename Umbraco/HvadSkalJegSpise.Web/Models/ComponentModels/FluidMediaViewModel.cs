using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class FluidMediaViewModel
    {
        public IPublishedContent Image { get; set; }

        public int GridSize { get; set; }

        public int ImageHeightLargeDesktop { get; set; }

        public int ImageHeight { get; set; }

        public int ImageHeightMobile { get; set; }

        public int GridTotalPadding { get; set; }

        public FluidMediaViewModel()
        {
            this.GridTotalPadding = 30;
        }
    }
}
