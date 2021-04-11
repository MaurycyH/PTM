using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PTM.Logic;
using Newtonsoft.Json.Linq;
using Tesseract.Common;
namespace PTM.Logic.Authentication
{

    public  class TokenCacheHelper
    {
        private static readonly object mFileLock = new object();
        private SettingsManager mSettingsManager = new SettingsManager();
        /// <summary>
        /// Ustawia sciezke tokenu oraz jego nazwe
        /// </summary>
        private string mCacheFilePath;

        public TokenCacheHelper()
        {
            mCacheFilePath = Path.Combine(mSettingsManager.PathToAppData, "PTM2020", "DefaultProvider.Microsoft.bin3");
        }
        public  void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            lock (mFileLock)
            {
                args.TokenCache.DeserializeMsalV3(File.Exists(mCacheFilePath)
                        ? ProtectedData.Unprotect(File.ReadAllBytes(mCacheFilePath), null, DataProtectionScope.CurrentUser)
                        : null);
            }
        }

        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            // if the access operation resulted in a cache update
            if (args.HasStateChanged)
            {
                lock (mFileLock)
                {
                    // reflect changes in the persistent store
                    File.WriteAllBytes(mCacheFilePath,
                                       ProtectedData.Protect(args.TokenCache.SerializeMsalV3(), null, DataProtectionScope.CurrentUser));
                }
            }
        }

        public void EnableSerialization(ITokenCache tokenCache)
        {
            Ensure.ParamNotNull(tokenCache, nameof(tokenCache));
            tokenCache.SetBeforeAccess(BeforeAccessNotification);
            tokenCache.SetAfterAccess(AfterAccessNotification);

        }
    }
}
