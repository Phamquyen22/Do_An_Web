namespace DoanWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sessions
    {
        [Key]
        [StringLength(50)]
        public string id_ses { get; set; }

        [StringLength(50)]
        public string ses { get; set; }

        public int? soluong { get; set; }

        [StringLength(50)]
        public string id_user { get; set; }

        [StringLength(50)]
        public string size_sp { get; set; }

        public virtual User_role User_role { get; set; }
    }
}
