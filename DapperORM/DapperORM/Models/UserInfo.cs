using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperORM.Models
{
    public class UserInfo
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }
        public string Email { get; set; }

    }

    public class LoginLog
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public DateTime CreateTime { get; set; }

    }
}