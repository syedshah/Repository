namespace Entities
{
    using System; 
    using System.Collections.Generic;
    using Microsoft.Build.Framework;

    public class NewsTicker
    {
        public NewsTicker()
        {           
        }

        public NewsTicker(int id)
            : this()
        {
        }

        public NewsTicker(string news, DateTime date)
            : this()
        {           
            News = news;
            Date = date;
        }

        public DateTime Date { get; set; }

        public int Id { get; set; }

        [Required]
        public string News { get; set; }
    }
}