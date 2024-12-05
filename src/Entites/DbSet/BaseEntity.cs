﻿namespace Entities.DbSet
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Status { get; set; } = 1;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}