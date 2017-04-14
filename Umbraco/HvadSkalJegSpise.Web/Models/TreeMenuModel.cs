using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace TreeMenu.Models
{
    public class TreeMenuModel {
        public TreeMenuModelItem Parent { get; set; }
        public List<TreeMenuModelItem> Children { get; set; }
    }

    public class TreeMenuModelItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int Level { get; set; }

        public bool HasChildren { get; set; }

        public TreeMenuModelItem(IPublishedContent context)
        {
            Id = context.Id;
            Name = context.Name;
            Url = context.Url;
            Level = context.Level;
            HasChildren = context.Children.Any();
        }

        public TreeMenuModelItem() { }
    }
}
