﻿using InkersCore.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.AuthEntityModels
{
    public class Common
    {
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Timestamp]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("created_by_id")]
        public UserAccount CreatedBy { get; set; }

        [Timestamp]
        [Column("last_updated_time")]
        public DateTime LastUpdatedTime { get; set; }

        [ForeignKey("last_updated_by_id")]
        public UserAccount LastUpdatedBy { get; set; }

        public Common()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedTime = DateTimeHandler.GetDateTime();
            LastUpdatedTime = DateTimeHandler.GetDateTime();
        }
    }
}
