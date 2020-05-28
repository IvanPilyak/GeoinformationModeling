using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IEnumerable<RiverParams> RiverParamsList {get;set;}
    }
}
