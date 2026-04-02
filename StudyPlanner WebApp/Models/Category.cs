namespace StudyPlanner_WebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public ICollection<StudyTask> Tasks { get; set; }
    }
}
