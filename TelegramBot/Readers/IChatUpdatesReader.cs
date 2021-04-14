namespace ChatBot.Core.Readers
{
    public interface IChatUpdatesReader<T> where T: class
    {
        T GetUpdate(int offset);
    }
}
