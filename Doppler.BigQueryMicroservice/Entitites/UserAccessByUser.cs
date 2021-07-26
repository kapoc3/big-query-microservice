using System;

namespace Doppler.BigQueryMicroservice.Entitites
{
    /// <summary>
    /// Entity Abstraction for table
    /// [datastudio.UserAccessByUser]
    /// </summary>
    public class UserAccessByUser
    {
        public int IdUser { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
