using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTM.Logic.Authentication
{
    public interface IAvatarDownloader
    {

        /// <summary>
        /// Metoda pobierajaca avatar Usera
        /// </summary>
        /// <param name="cancellationToken">Token anulacji</param>
        /// <returns></returns>
        Task GetAvatarAsync(CancellationToken cancellationToken);
    }
}
