﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class UserStatus
{
    public long StatusId { get; set; }

    public string StatusName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}