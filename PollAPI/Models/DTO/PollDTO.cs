using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PollAPI.Models.DTO
{
    public class PollDTO : Poll
    {
        [DataMember(Order = 0)]
        public int Id { get { return poll_id; } set { poll_id = value; } }

        [DataMember(Order = 1)]
        public string Description { get { return poll_description; } set { poll_description = value; } }

        [DataMember(Order = 2)]
        public ICollection<string> OptionsList
        {
            get
            {
                return Options;
            }

        }

    }
}