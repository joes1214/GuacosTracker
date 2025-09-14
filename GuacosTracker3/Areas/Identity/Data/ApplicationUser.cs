using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GuacosTracker3.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TrackerUser class
public class ApplicationUser : IdentityUser
{
    public int Id { get; set; }
    public string Auth0Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
    public DateTime CreatedAt { get; set; }
}

