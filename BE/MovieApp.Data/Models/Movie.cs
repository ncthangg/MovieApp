﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class Movie
{
    public long MovieId { get; set; }

    public string MovieName { get; set; }

    public string Description { get; set; }

    public string VideoUrl { get; set; }

    public string PosterUrl { get; set; }

    public string Director { get; set; }

    public int ReleaseYear { get; set; }

    public long TypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

    public virtual ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();

    public virtual ICollection<MovieRate> MovieRates { get; set; } = new List<MovieRate>();

    public virtual ICollection<MovieSeason> MovieSeasons { get; set; } = new List<MovieSeason>();

    public virtual MovieType Type { get; set; }

    public virtual ICollection<UserLike> UserLikes { get; set; } = new List<UserLike>();

    public virtual ICollection<UserWatchHistory> UserWatchHistories { get; set; } = new List<UserWatchHistory>();
}