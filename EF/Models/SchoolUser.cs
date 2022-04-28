using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("SCHOOL_USER")]
    public partial class SchoolUser
    {
        [Key]
        [Column("USER_NAME")]
        [StringLength(256)]
        public string UserName { get; set; }
        [Key]
        [Column("SCHOOL_ID")]
        public int SchoolId { get; set; }

        [ForeignKey(nameof(SchoolId))]
        [InverseProperty("SchoolUsers")]
        public virtual School School { get; set; }
        public virtual AspNetUser UserNameNavigation { get; set; }
    }
}
