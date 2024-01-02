namespace DoanWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User_role()
        {
            Orders = new HashSet<Orders>();
            Sessions = new HashSet<Sessions>();
        }

        [Key]
        [StringLength(50)]
        public string id_user { get; set; }

        [Required]
        [StringLength(50)]
        public string tk { get; set; }

        [Required]
        public string mk { get; set; }

        [StringLength(50)]
        public string role { get; set; }

        [StringLength(50)]
        public string img { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public string address { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        [Column("lock")]
        [StringLength(50)]
        public string _lock { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string ngaysinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sessions> Sessions { get; set; }
    }
}
