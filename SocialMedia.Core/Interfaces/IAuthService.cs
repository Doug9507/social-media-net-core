using SocialMedia.Core.CustomEntities;

namespace SocialMedia.Core.Interfaces
{
    public interface IAuthService
    {
        UserLoginResponse GetToken(UserLoginRequest userLoginRequest);
    }
}