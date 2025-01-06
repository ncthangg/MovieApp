﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class MovieEpisode
{
    public long EpisodeId { get; set; }

    public long SeasonId { get; set; }

    public string EpisodeName { get; set; }

    public string Description { get; set; }

    public string VideoUrl { get; set; }

    public string PosterUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual MovieSeason Season { get; set; }

    public virtual ICollection<UserWatchHistory> UserWatchHistories { get; set; } = new List<UserWatchHistory>();
}