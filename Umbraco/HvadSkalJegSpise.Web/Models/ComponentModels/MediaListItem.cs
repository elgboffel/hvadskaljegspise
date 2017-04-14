using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class MediaListItem
    {
        public IPublishedContent Content { get; set; }
        public object Image { get; set; }
        public string Type { get; set; }
        public string Heading { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
		public string Url { get; set; }

		public MediaListItem() { }

		public MediaListItem(IPublishedContent x)
		{
			Content = x;
			Type = x.DocumentTypeAlias;
			Heading = x.Name;

			switch (x.ItemType)
			{
				case PublishedItemType.Content: this.ParseContent(x); break;
				case PublishedItemType.Media: this.ParseMedia(x); break;
			}
		}

		private void ParseContent(IPublishedContent x)
		{
			Image = x.GetPropertyValue<IPublishedContent>("heroImage", null);
			Heading = x.GetPropertyValue<string>("teaserHeadline", x.GetPropertyValue<string>("headline", x.Name));
			Date = x.GetPropertyValue("date", (DateTime?)null);
			Description = x.GetPropertyValue<string>("teaserLead", x.GetPropertyValue<string>("lead", string.Empty));
		}

		private void ParseMedia(IPublishedContent x)
		{
			switch (x.DocumentTypeAlias)
			{
				case "File":
					break;
				case "Image":
					break;
			}
		}
    }
}
