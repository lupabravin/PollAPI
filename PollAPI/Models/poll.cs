//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PollAPI.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public partial class Poll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Poll()
        {
            this.option = new HashSet<Option>();
            this.options = new HashSet<string>();
        }

        [DataMember(Order = 0)]
        public int poll_id { get; set; }
        [DataMember(Order = 1)]
        public string poll_description { get; set; }
        public int views { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Option> option { get; set; }
        [DataMember(Order = 3)]
        public virtual ICollection<string> options { get; set; }
    }
}