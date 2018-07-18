using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class MakeResource : KeyValuePairResource
    {
        
        public ICollection<KeyValuePairResource> Models { get; set; }


        public MakeResource()
        {
            Models = new Collection<KeyValuePairResource>();
        }
    }
}