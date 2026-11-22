using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface IGameSessionService
    {
        List<GameSession> GetAllSessions();
        List<GameSession> GetPagedSessions(int pageIndex, int pageSize, int? level = null);
        GameSession? GetSessionById(int id);
        GameSession? AddSession(GameSession session);
        bool UpdateSession(GameSession updatedSession);
        bool DeleteSession(int id);
    }
}

