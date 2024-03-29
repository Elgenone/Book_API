﻿using System;
using System.Collections.Generic;

namespace SecandAPI.Models
{
    public partial class UserInform
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
