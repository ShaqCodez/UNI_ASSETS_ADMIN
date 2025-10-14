using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace UNI_ASSETS.Models
{
    public class Asset
    {
        
        public string AssetId { get; set; }
        public string Name { get; set; }
       
        public string Description { get; set; }
        public string Default_Location { get; set; }
        public string ImageUrl { get; set; }
    }
}
