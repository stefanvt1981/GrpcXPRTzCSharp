using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrpcXPRTzCSharp.Repository.Entities
{
    public class XprtEntity 
    {
        [Key]
        public int Id { get; set; }
        public int BadgeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public string Skills { get; set; }
        public byte[] Foto { get; set; }
    }
}
