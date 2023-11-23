using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public class EventSeatAvailability
    {

        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>The schedule identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>The event date.</value>
        public string EventDate { get; set; }

        /// <summary>
        /// Gets or sets the available seat.
        /// </summary>
        /// <value>The available seat.</value>
        public string AvailableSeat { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name fields.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total seats.
        /// </summary>
        /// <value>The total seats.</value>
        public int TotalSeats { get; set; }

        /// <summary>
        /// Gets or sets the total temporary booking.
        /// </summary>
        /// <value>The total temporary booking.</value>
        public int TotalTempBooking { get; set; }

        /// <summary>
        /// Gets or sets the total booked.
        /// </summary>
        /// <value>The total booked.</value>
        public int TotalBooked { get; set; }

        /// <summary>
        /// Gets or sets the total available seat.
        /// </summary>
        /// <value>The total available seat.</value>
        public int TotalAvailableSeat { get; set; }

        /// <summary>
        /// Gets or sets the total available seat.
        /// </summary>
        /// <value>The total available seat.</value>
        public int EventRateId { get; set; }

        /// <summary>
        /// Gets or sets the guest limit.
        /// </summary>
        /// <value>The guest limit.</value>
        public int GuestLimit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is maximum ticket for family member.
        /// </summary>
        /// <value><c>true</c> if this instance is maximum ticket for family member; otherwise, <c>false</c>.</value>
        public bool IsMaximumTicketForFamilyMember { get; set; }

        /// <summary>
        /// Gets or sets the maximum ticket for family member.
        /// </summary>
        /// <value>The maximum ticket for family member.</value>
        public int MaximumTicketForFamilyMember { get; set; }

        /// <summary>
        /// Gets or sets the member seats.
        /// </summary>
        /// <value>The member seats.</value>
        public int MemberSeats { get; set; }

        /// <summary>
        /// Gets or sets the guest seats.
        /// </summary>
        /// <value>The guest seats.</value>
        public int GuestSeats { get; set; }

        /// <summary>
        /// Gets or sets the available member seats.
        /// </summary>
        /// <value>The available member seats.</value>
        public int AvailableMemberSeats { get; set; }

        /// <summary>
        /// Gets or sets the available guest seats.
        /// </summary>
        /// <value>The available guest seats.</value>
        public int AvailableGuestSeats { get; set; }
    }
}