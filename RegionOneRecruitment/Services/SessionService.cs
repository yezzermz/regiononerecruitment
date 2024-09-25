namespace RegionOneRecruitment.Services
{
    public interface ISessionService
    {
        void CreateSession(int userId, bool isAdmin);
        void EndSession();
        bool IsUserLoggedIn();
        bool IsAdmin();
        int GetUserId();
        event Action OnLoginStateChanged;
    }

    public class SessionService : ISessionService
    {
        private readonly Dictionary<string, object> _session = [];

        public event Action? OnLoginStateChanged;

        public void CreateSession(int userId, bool isAdmin)
        {
            _session["UserId"] = userId;
            _session["IsAdmin"] = isAdmin;
            OnLoginStateChanged?.Invoke();
        }

        public void EndSession()
        {
            _session.Clear();
            OnLoginStateChanged?.Invoke();
        }

        public bool IsUserLoggedIn()
        {
            return _session.ContainsKey("UserId");
        }

        public bool IsAdmin()
        {
            return _session.TryGetValue("IsAdmin", out var isAdmin) && (bool)isAdmin;
        }

        public int GetUserId()
        {
            if (_session.TryGetValue("UserId", out var userId))
            {
                return (int)userId;
            }

            return -1; // return -1 if no userid
        }
    }
}
