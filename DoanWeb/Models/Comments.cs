namespace DoanWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comments
    {
        [Key]
        public int commentID { get; set; }

        [StringLength(50)]
        public string proID { get; set; }

        public string commentMessage { get; set; }

        [StringLength(50)]
        public string id_user { get; set; }

        [StringLength(50)]
        public string datepost { get; set; }

        public virtual Products Products { get; set; }
    }
}
