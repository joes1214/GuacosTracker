﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GuacosTracker3.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TrackerUser class
public class TrackerUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LName { get; set; }
}

