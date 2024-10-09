using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int Seconds { get; set; }

        public NotificationDto(string title, string message, int seconds = 0)
        {
            this.Title = title;
            this.Message = message;
            this.Seconds = seconds;
        }
    }
}
