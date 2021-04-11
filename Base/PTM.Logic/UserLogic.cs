using System;
using System.Linq;
using PTM.Entities;
using Tesseract.Common;

namespace PTM.Logic
{
    public class UserLogic : BaseLogic
    {
        private IDatabaseContext mDatabaseContext;

        /// <summary>
        /// Konstruktor UserLogic
        /// </summary>
        public UserLogic(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));
            mDatabaseContext = context;
        }

        /// <summary>
        /// Tworzy użytkownika w bazie danych.
        /// </summary>
        public User Create(User user)
        {
            Ensure.ParamNotNull(user, nameof(user));
            return mDatabaseContext.Users.Add(user).Entity;
        }

        /// <summary>
        /// Wczytuje użytkownika z bazy danych po jego ID.
        /// </summary>
        public User Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Parameter ID has to be grater than 0, got {id}");
            }
            User user = mDatabaseContext.Users.Where(user => user.ID == id).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Wczytuje użytkownika z bazy danych po jego OAuthID.
        /// </summary>
        public User ReadOAuth(string OAuthID)
        {
            User user = mDatabaseContext.Users.Where(user => user.OAuthID == OAuthID).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Aktualizuje dane użytkownika.
        /// </summary>
        public User Update(User user)
        {
            Ensure.ParamNotNull(user, nameof(user));
            User dbUser = Read(user.ID);

            if (user == null)
            {
                return null;
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.TaskBoards = user.TaskBoards;
            return dbUser;
        }

        /// <summary>
        /// Usuwa dane użytkownika z bazy danych.
        /// </summary>
        public void Delete(int id)
        {
            User user = Read(id);

            if (user == null)
            {
                return;
            }

            mDatabaseContext.Users.Remove(user);
        }
    }
}
