namespace ICSBel.Presentation.ErrorHandling;

internal class ErrorReporter
{
    public void Report(string errorText, string caption = "Ошибка")
    {
        MessageBox.Show(errorText, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    
    public void Report(Exception exception, string errorMessage, string caption = "Ошибка")
    {
        MessageBox.Show($"{errorMessage}\n{exception.Message}", caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}