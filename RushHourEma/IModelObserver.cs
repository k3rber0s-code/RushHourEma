namespace RushHourEma
{
    public interface IModelObserver
    {
        // The interface which the form/view must implement so that, the event will be
        // fired when a value is changed in the model.


        void carSelected(IModel model, ModelEventArgs e);
        void carsAdded(IModel model, ModelEventArgs e);
        void carMoved(IModel model, ModelEventArgs e);
        void gameOver(IModel model, ModelEventArgs e);
        void mapReseted(IModel model, ModelEventArgs e);
        void levelLoaded(IModel model, ModelEventArgs e);
        void gameCompleted(IModel model, ModelEventArgs e);

    }
}