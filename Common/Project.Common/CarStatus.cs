using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Common
{
    public class CarStatus
    {
        [Key]
        public Guid Id { get; set; }

        public const string Awaited = "изчакваща";
        public const string Canceled = "отменена";
        public const string Active = "активна";
        public const string Used = "използвана";
        public const string OverDue = "просрочена";

        public string Name { get; set; }
    }
}
