﻿using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Subscriptions
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status {  get; set; }  // A(Active) - C(Canceled)
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public string UserId { get; set; } = null!;
        public int PlanId { get; set; }

        // Navigation Properties
        public RasedUser User { get; set; } = new RasedUser();
        public Plan Plan { get; set; } = new Plan();
    }
}
