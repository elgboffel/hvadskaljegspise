using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace HvadSkalJegSpise.Web.Models.ComponentModels
{
    public class PageHeaderViewModel
    {

        public string ArticleType { get; set; }

        public DateTime? Date { get; set; }

        public string Headline { get; set; }

        public string Lead { get; set; }

        public string Author { get; set; }

        public string Trompet { get; set; }
    }
}
