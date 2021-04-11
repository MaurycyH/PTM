using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Text;
using PTM.Utilities;
using PTM.Logic.Authentication;

namespace PTM.Logic
{
    public class CredentialsManager
    {
        public AuthenticationProvider Provider { get; private set; }

        public CredentialsManager(AuthenticationProvider Provider)
        {
            this.Provider = Provider;
        }

        /// <summary>
        /// Wpisuje wartość do managera poświadczeń Windows (Google)
        /// </summary>
        public void SaveToken(string Token)
        {
            using (Credential Cred = new Credential())
            {
                Cred.Password = Token;
                Cred.Target = Provider.GetDescription();
                Cred.Type = CredentialType.Generic;
                Cred.PersistanceType = PersistanceType.LocalComputer;
                Cred.Save();
            }
        }

        /// <summary>
        /// Wczytuje wartość refresh tokena z menedżera poświadczeń Windows (Google)
        /// </summary>
        public string LoadToken()
        {
            using (var Cred = new Credential())
            {
                Cred.Target = Provider.GetDescription();
                Cred.Load();
                return Cred.Password;
            }
        }

        /// <summary>
        /// Usuwa wartość tokena z menedżera poświadczeń Windows (Google)
        /// </summary>
        public void DeleteToken()
        {
            using (var Cred = new Credential() {Target = Provider.GetDescription() })
            {
                Cred.Delete();
            }
        }
    }
}
