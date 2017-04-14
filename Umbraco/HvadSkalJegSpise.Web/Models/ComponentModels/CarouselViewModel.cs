using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class CarouselViewModel
    {
        public IEnumerable<IPublishedContent> Carousel { get; set; }

        public int GridSize { get; set; }

        public int ImageHeight { get; set; }

        public int ImageHeightMobile { get; set; }

        public int GridTotalPadding { get; set; }
    }
}
