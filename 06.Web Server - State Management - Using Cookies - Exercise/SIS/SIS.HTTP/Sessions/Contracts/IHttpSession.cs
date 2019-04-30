namespace SIS.HTTP.Sessions.Contracts
{
    public interface IHttpSession
    {
        string Id { get; }

        object GeParameter(string name);

        bool ContainsParameter(string name);

        void AddParameter(string name, object parameter);

        void ClearParameters(); 
    }
}