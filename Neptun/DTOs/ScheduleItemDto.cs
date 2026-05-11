namespace Neptun.DTOs
{
    public class ScheduleItemDto
    {
        /// <summary>
        /// Az óra kezdete (pl: 2026-09-15T08:00:00)
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Az óra vége (pl: 2026-09-15T09:30:00)
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Terem száma (pl: I. előadó, C/202)
        /// </summary>
        public string Room { get; set; } = string.Empty;

        /// <summary>
        /// Ha true, akkor a megadott időponttól számítva 14 héten át minden héten bekerül az órarendbe.
        /// Ha false, akkor csak egyetlen alkalom.
        /// </summary>
        public bool IsWeekly { get; set; }
    }
}
