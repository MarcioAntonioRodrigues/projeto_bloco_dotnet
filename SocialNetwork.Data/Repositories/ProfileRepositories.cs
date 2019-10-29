using SocialNetwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Repositories
{
    class ProfileRepository
    {
        private static IDictionary<string, Profile> _profiles;

        public ProfileRepository()
        {
            _profiles = new Dictionary<string, Profile>();
        }

        public void Create(Profile profile)
        {
            if (!_profiles.ContainsKey(profile.AccountId))
            {
                _profiles.Add(profile.AccountId, profile);
            }
        }
    }
}
