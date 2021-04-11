using PTM.Entities;
using PTM.Logic;
using PTM.Logic.Authentication;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using PTM.Services.Client.UserClient;
using PTM.Terminal.MainWindow;
using PTM.Terminal.WindowHelpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.LoginWindow
{
    /// <summary>
    /// View model dla MainWindow
    /// </summary>
    public class LoginWindowViewModel : WindowBaseViewModel, IDisposable
    {
        private ITerminalContext mContext;
        private UserPublic mAccountTemp;
        private readonly CancellationTokenSource mCancellationToken = new CancellationTokenSource();
        private bool mIsAuthenticationButtonsEnabled = true;

        /// <summary>
        /// Odpowiada za uruchomienie tylko jednego okna po zalogowaniu (true mozna naciskac false nie mozna)
        /// </summary>
        public bool IsAuthenticationButtonsEnabled
        {
            get
            {
                return mIsAuthenticationButtonsEnabled;
            }
            set
            {
                mIsAuthenticationButtonsEnabled = value;
                OnPropertyChanged(nameof(IsAuthenticationButtonsEnabled));
            }
        }

        /// <summary>
        /// Identity okna
        /// </summary>
        public override string Identity => "6183F0A1-80F4-409E-B430-450216F69957";

        /// <summary>
        /// Komenda przeprowadzająca autentykację przez google.
        /// </summary>
        public ICommand GoogleAuthentication { get; }

        /// <summary>
        /// Komenda przeprowadzająca autentykacje przez microsoft.
        /// </summary>
        public ICommand MicrosoftAuthentication { get; }

        /// <summary>
        /// Domyślny ctor VM LoginWindow.
        /// </summary>
        /// <param name="context">Kontekst terimnala</param>
        public LoginWindowViewModel(ITerminalContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));
            mContext = context;

            GoogleAuthentication = new AsyncCommand(AuthenticateWithGoogleAsync);
            MicrosoftAuthentication = new AsyncCommand(AuthenticateWithMicrosoftAsync);

            Task.Run(LoginOnStartup);
        }

        /// <summary>
        /// Dokonuje próby logowania, jeżeli zapisane jest poprzednie logowanie
        /// </summary>
        public async Task LoginOnStartup()
        {
            SettingsManager settingsManager = new SettingsManager();
            string defaultProvider = settingsManager.LoadProvider();

            if (!string.IsNullOrEmpty(defaultProvider))
            {
                if (Enum.TryParse(typeof(AuthenticationProvider), defaultProvider, out object provider))
                {
                    IsAuthenticationButtonsEnabled = false;
                    AuthenticationProvider authProvider = (AuthenticationProvider)provider;

                    AuthenticationProcessor authentication = new AuthenticationProcessor();
                    UserPublic userAccount = await authentication.AuthenticateAsync(authProvider, mCancellationToken.Token).ConfigureAwait(false);

                    if (this.ValidateAccount(userAccount, authProvider))
                    {
                        await LoginUserPTM().ConfigureAwait(false);
                        await this.OpenMainWindowAsync().ConfigureAwait(false);
                        return;
                    }

                    IsAuthenticationButtonsEnabled = true;
                }
            }
            else
            {
                settingsManager.DeleteProvider();
            }

            // Jeśli nie udało się zweryfikować usera, to pokazujemy okno
            mContext.WindowManager.ActivateWindow(Identity);
        }

        /// <summary>
        /// Dokonuje logowania przy użyciu Google (Oauth 2)
        /// </summary>
        public async Task AuthenticateWithGoogleAsync()
        {
            try
            {
                IsAuthenticationButtonsEnabled = false;
                AuthenticationProcessor authentication = new AuthenticationProcessor();
                UserPublic userAccount = await authentication.AuthenticateAsync(AuthenticationProvider.Google, mCancellationToken.Token).ConfigureAwait(false);

                if (this.ValidateAccount(userAccount, AuthenticationProvider.Google))
                {
                    await LoginUserPTM().ConfigureAwait(false);
                    await this.OpenMainWindowAsync().ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // do nothing
            }
            catch (Exception ex)
            {
                mContext.DialogBuilder.ErrorDialog((Application.Current.TryFindResource("IDS_LoginWindow_AuthenticationError") as string), ex);
            }
            finally
            {
                IsAuthenticationButtonsEnabled = true;
            }
        }

        /// <summary>
        /// Dokonuje logowania przy użyciu Microsoft (Oauth 2)
        /// </summary>
        public async Task AuthenticateWithMicrosoftAsync()
        {
            try
            {
                IsAuthenticationButtonsEnabled = false;
                AuthenticationProcessor authentication = new AuthenticationProcessor();
                UserPublic userAccount = await authentication.AuthenticateAsync(AuthenticationProvider.Microsoft, mCancellationToken.Token).ConfigureAwait(false);

                if (this.ValidateAccount(userAccount, AuthenticationProvider.Microsoft))
                {
                    await LoginUserPTM().ConfigureAwait(false);
                    await this.OpenMainWindowAsync().ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // do nothing
            }
            catch (Exception ex)
            {
                mContext.DialogBuilder.ErrorDialog((Application.Current.TryFindResource("IDS_LoginWindow_AuthenticationError") as string), ex);
            }
            finally
            {
                IsAuthenticationButtonsEnabled = true;
            }
        }

        /// <summary>
        /// Waliduje konto
        /// </summary>
        /// <param name="userAccount">Konto usera do zwalidowania</param>
        /// <param name="provider">Provider z którego pochodzi konto</param>
        private bool ValidateAccount(UserPublic userAccount, AuthenticationProvider provider)
        {
            // W przypadku, kiedy nie udało się zwalidować konta
            if (userAccount == null)
            {
                // Błąd logowania może być spowodowany wygaśnięciem refresh tokena - trzeba usunąć wpis
                CredentialsManager credentialsManager = new CredentialsManager(provider);
                credentialsManager.DeleteToken();
                return false;
            }
            else
            {
                SettingsManager settingsManager = new SettingsManager();
                settingsManager.SaveProvider(provider);
                mAccountTemp = userAccount;
                return true;
            }
        }

        /// <summary>
        /// Otwiera główne okno programu
        /// </summary>
        private async Task OpenMainWindowAsync()
        {
            // Niestety do stworzenia tego okna musimy wykorzystać disptacher, ponieważ nie jesteśmy w stanie skonstruować okna w asynchronicznym kontekście...
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                mContext.WindowManager.HideWindow(Identity);
                mContext.WindowManager.OpenDialog(new DlgMainWindow(mContext));
                mContext.WindowManager.CloseWindow(Identity);
            }));
        }

        /// <summary>
        /// Implementacja <see cref="IDisposable"/>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Usuwa zasoby niezarządzalne 
        /// </summary>
        /// <param name="isDisposed">True jeśli ma usunać zasoby niezarządzalne</param>
        protected virtual void Dispose(bool isDisposed)
        {
            if (!isDisposed)
            {
                return;
            }

            mCancellationToken.Cancel();
            mCancellationToken.Dispose();
        }

        /// <summary>
        /// Loguje użytkownika na konto PTM
        /// </summary>
        private async Task LoginUserPTM()
        {
            try
            {
                HttpUserClient client = new HttpUserClient();

                UserPublic response = await client.GetUserOAuth(mAccountTemp.OAuthID).ConfigureAwait(false);

                if (response == null)
                {
                    response = await client.CreateUser(mAccountTemp).ConfigureAwait(false);
                }

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    mContext.UserAccount = response;
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Couldn't download user account from PTM servers");
            }
        }
    }
}
