namespace Scrappy.Noom
{
    public interface INavigator
    {
        void NavigateTo(string path);

        void NavigateTo(string path, object payload);
    }
}