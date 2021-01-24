﻿using Framework.Models;
using System;

namespace Movies.Models
{
    public class MovieModel : IAggregateModel
    {
        public Guid MovieIdentity { get; set; }

        public String Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool? IsDelete { get; set; }

        // Miss

        public DateTime? ReleaseStartDate { get; set; }

        public DateTime? ReleaseEndDate { get; set; }

        public Guid? AggregateId { get; set; }
    }
}