using PTM.Entities;
using PTM.PublicDataModel;
using Tesseract.Common;

namespace PTM.Logic.ModelConverters
{
    /// <summary>
    /// Konwerter między <see cref="User"/> i <see cref="UserPublic"/>
    /// </summary>
    public class UserConverter : IModelConverter<User, UserPublic>
    {
        private IDatabaseContext mDBContext;

        public UserConverter(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <inheritdoc/>
        public User Convert(UserPublic source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            User destination = mDBContext.Users.Find(source.ID);

            if (destination == null)
                destination = new User();

            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.OAuthID = source.OAuthID;

            return destination;
        }

        /// <inheritdoc/>
        public UserPublic Convert(User source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            UserPublic destination = new UserPublic()
            {
                ID = source.ID,
                FirstName = source.FirstName,
                LastName = source.LastName,
                OAuthID = source.OAuthID
            };

            return destination;
        }
    }
}
