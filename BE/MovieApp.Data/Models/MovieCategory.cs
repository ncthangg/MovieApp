﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class MovieCategory
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual Movie Movie { get; set; }
}