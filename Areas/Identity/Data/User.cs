using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jenjennewborncare.Models;
using Microsoft.AspNetCore.Identity;

namespace jenjennewborncare.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public override string Id { get => base.Id; set => base.Id = value; }
    public override string UserName { get => base.UserName; set => base.UserName = value; }
    public override string Email { get => base.Email; set => base.Email = value; }
    public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public ICollection<Schedule> ScheduleItems { get; set; }
}

