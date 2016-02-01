namespace Loki.UI.Office
{
    public class ActionMessage
    {
        public ActionMessage(string action)
        {
            Action = action;
        }

        public string Action { get; private set; }
    }
}