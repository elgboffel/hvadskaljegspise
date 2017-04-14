using System;
using System.Collections.Generic;
using System.Web;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class CardViewModel
    {
        public IPublishedContent Image { get; set; }

        public int TypedWidth { get; set; }

        public int GridSize { get; set; }

        public int ImageHeight { get; set; }

        public int MobileImageHeight { get; set; }

        public string Headline { get; set; }

        public string Lead { get; set; }

        public IEnumerable<IPublishedContent> ArticleType { get; set; }

        public string Trumpet { get; set; }

        public string Url { get; set; }

        public string Date { get; set; }

        public bool AltDatePosition { get; set; }

        public string Category { get; set; }

        public IPublishedContent PrimaryTag { get; set; }

        public bool ShowLargePrimaryTag { get; set; }

        public int Truncate { get; set; }

        public IHtmlString RTE { get; set; }

        public string EventPlace { get; set; }

        public CardViewModel()
        {
            this.ShowLargePrimaryTag = false;
            this.AltDatePosition = false;
            this.Truncate = 200;
        }
    }
}
