using ApiGateway.Client.V3.Domain;
using Shared.Models.Infrastructure.Authorization;

namespace ApiGateway.Client.V3.Application
{
    public class PlayerContext
    {
        public PlayerModel Player { get; private set; }
        public AuthorizationToken Token { get; private set; }
        public bool IsLogined => Token != null && Player != null;

        public void SetContext(PlayerModel player, AuthorizationToken token)
        {
            Player = player;
            Token = token;
        }

        public void Clear()
        {
            Player = null;
            Token = null;
        }
    }
}