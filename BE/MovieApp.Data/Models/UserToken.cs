﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MovieApp.Data.Models;

public partial class UserToken
{
    public long TokenId { get; set; }

    public long? UserId { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpires { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual User User { get; set; }
}