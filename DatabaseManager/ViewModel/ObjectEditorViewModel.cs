namespace DatabaseManager.ViewModel
{
    public class ObjectEditorViewModel : ObjectEditorViewModel<object>
    {
        public ObjectEditorViewModel()
        {
        }

        public ObjectEditorViewModel(object value)
        {
            Value = value;
        }
    }
}