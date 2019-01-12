using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace PollAPI.Models.DTO
{
    public class OptionDTO : option
    {
        public int Id
        {
            get { return option_id; }
            set { option_id = value; }
        }

        public string Description
        {
            get { return option_description; }
            set { option_description = value; }
        }

        public int Poll_Id
        {
            get { return poll_id; }
            set { poll_id = value; }
        }
    }
}