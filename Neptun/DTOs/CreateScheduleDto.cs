namespace Neptun.DTOs
{
    public class CreateScheduleDto
    {
        public List<ScheduleItemDto> Items { get; set; } = new();
    }
}
