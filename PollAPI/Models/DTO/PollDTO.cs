using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PollAPI.Models.DTO
{
    public class PollDTO : poll
    {
        [DataMember(Order = 0)]
        //private int id { get; set; }
        public int Id { get { return poll_id; } set { poll_id = value; } }

        [DataMember(Order = 1)]
        //private string description { get; set; }
        public string Description { get { return poll_description; } set { poll_description = value; } }

        [DataMember(Order = 2)]
        //private ICollection<string> options { get; set; }
        public ICollection<string> Options
        {
            get
            {
                return options;
            }

        }

    }
}