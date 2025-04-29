namespace AskIt.Models.Customizations.Exceptions.Course;

public class CourseNotFoundException : Exception
{
    public CourseNotFoundException(string message)
        : base(message)
    {}
}
