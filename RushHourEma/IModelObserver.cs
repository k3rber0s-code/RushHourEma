namespace RushHourEma
{
    public interface IModelObserver
    {
        // The interface which the form/view must implement so that, the event will be
        // fired when a value is changed in the model.


        void valueIncremented(IModel model, ModelEventArgs e);
        void carSelected(IModel model, ModelEventArgs e);
        void carsAdded(IModel model, ModelEventArgs e);

    }
}