using System.Collections.Generic;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class ContactCardViewModel
    {
        public IPublishedContent Image { get; set; }

        public int TypedWidth { get; set; }

        public int GridSize { get; set; }

        public int ImageHeight { get; set; }

        public int ImageHeightMobile { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string WorkArea { get; set; }

        public string Department { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Mail { get; set; }

    }
}

