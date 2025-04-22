namespace AskIt.Models.Customizations.Exceptions.Question
{
    public class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException(int id)
            :base($"Nessuna domanda trovata con id #{id}")
        {
            Id = id;
        }

        public int Id { get; }
    }
}
