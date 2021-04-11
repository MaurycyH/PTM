using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tesseract.Common;

namespace PTM.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="databaseContext">Kontekst bazy danych z którego ma korzystać serwis.</param>
        public UserRepository(IDatabaseContext databaseContext)
        {
            Ensure.ParamNotNull(databaseContext, nameof(databaseContext));

            mDBContext = databaseContext;
        }

        /// <inheritdoc/>
        public UserPublic CreateUser(UserPublic user)
        {
            UserLogic logic = new UserLogic(mDBContext);
            UserConverter converter = new UserConverter(mDBContext);

            try
            {
                User createdUser = logic.Create(converter.Convert(user));

                mDBContext.SaveChanges();

                return converter.Convert(createdUser);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public UserPublic GetUser(int ID)
        {
            UserLogic logic = new UserLogic(mDBContext);
            UserConverter converter = new UserConverter(mDBContext);

            try
            {
                return converter.Convert(logic.Read(ID));
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public UserPublic GetUserOAuth(string OAuthID)
        {
            UserLogic logic = new UserLogic(mDBContext);
            UserConverter converter = new UserConverter(mDBContext);

            try
            {
                return converter.Convert(logic.ReadOAuth(OAuthID));
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public UserPublic UpdateUser(UserPublic user)
        {
            UserLogic logic = new UserLogic(mDBContext);
            UserConverter converter = new UserConverter(mDBContext);

            try
            {
                UserPublic updatedUser = converter.Convert(logic.Update(converter.Convert(user)));
                mDBContext.SaveChangesAsync(CancellationToken.None);
                return updatedUser;
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void DeleteUser(int ID)
        {
            UserLogic logic = new UserLogic(mDBContext);

            logic.Delete(ID);
            mDBContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
