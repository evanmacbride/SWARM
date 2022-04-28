using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("ASP_NET_USERS")]
    [Index(nameof(UserName), Name = "ASP_NET_USERS_UK1", IsUnique = true)]
    [Index(nameof(NormalizedEmail), Name = "EMAILINDEX")]
    [Index(nameof(NormalizedUserName), Name = "USERNAMEINDEX", IsUnique = true)]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            SchoolUsers = new HashSet<SchoolUser>();
        }

        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Required]
        [Column("USER_NAME")]
        [StringLength(256)]
        public string UserName { get; set; }
        [Column("NORMALIZED_USER_NAME")]
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [Column("EMAIL")]
        [StringLength(256)]
        public string Email { get; set; }
        [Column("NORMALIZED_EMAIL")]
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        [Column("EMAIL_CONFIRMED")]
        public bool EmailConfirmed { get; set; }
        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; }
        [Column("SECURITY_STAMP")]
        public string SecurityStamp { get; set; }
        [Column("CONCURRENCY_STAMP")]
        public string ConcurrencyStamp { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("PHONE_NUMBER_CONFIRMED")]
        public bool PhoneNumberConfirmed { get; set; }
        [Column("TWO_FACTOR_ENABLED")]
        public bool TwoFactorEnabled { get; set; }
        [Column("LOCKOUT_END")]
        public DateTimeOffset? LockoutEnd { get; set; }
        [Column("LOCKOUT_ENABLED")]
        public bool LockoutEnabled { get; set; }
        [Column("ACCESS_FAILED_COUNT")]
        public int AccessFailedCount { get; set; }

        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        [InverseProperty(nameof(AspNetUserToken.User))]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; }
    }
}
