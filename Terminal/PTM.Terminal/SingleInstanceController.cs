using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PTM.Terminal
{
    /// <summary>
    /// Klasa, która umożliwa kontroluje, aby jednocześnie była włączona maksymalnie jedna instancja programu
    /// </summary>
    public class SingleInstanceController : IDisposable
    {
        private readonly Mutex m_mutex = new Mutex(true, "{17D3D0E4-EA85-4CBC-988C-EA1AD3CE9EF3}");

        /// <summary>
        /// Domyślny ctor <see cref="SingleInstanceController"/>
        /// </summary>
        public SingleInstanceController()
        {

        }

        /// <summary>
        /// Sprawdza, czy istnieje inna instancja aplikacji.
        /// </summary>
        public bool CheckIfRunning()
        {
            if (m_mutex.WaitOne(TimeSpan.Zero, true))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Usuwa obiekt z pamięci
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool bIsDisposing)
        {
            if (bIsDisposing == false)
            {
                return;
            }

            m_mutex.ReleaseMutex();
            m_mutex.Dispose();
        }
    }
}
