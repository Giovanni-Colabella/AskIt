namespace AskIt.Models.Enums;

public enum QuestionStatus
{
    Open, // La domanda è aperta e può ricevere risposte
    Closed, // La domanda è stata chiusa e non può più ricevere risposte
    Answered, // La domanda ha ricevuto una risposta accettata e non può più ricevere risposte
    Deleted // La domanda è stata eliminata e non è più visibile
}
