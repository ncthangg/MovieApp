﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class MovieActor
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long ActorId { get; set; }

    public virtual Actor Actor { get; set; }

    public virtual Movie Movie { get; set; }
}