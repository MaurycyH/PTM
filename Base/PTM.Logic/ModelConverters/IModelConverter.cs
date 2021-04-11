using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.Logic.ModelConverters
{
    /// <summary>
    /// Interfejs dostarczający metody potrzebne do konwersji obiektów między modelami serwisów a EF
    /// </summary>
    /// <typeparam name="TEntity">Typ obiektu Entity Framework</typeparam>
    /// <typeparam name="TService">Typ obiektu sewisu</typeparam>
    public interface IModelConverter<TEntity, TService>
    {
        /// <summary>
        /// Konwertuje model z serwisu do modelu EF.
        /// </summary>
        /// <param name="source">Obiekt do przekonwertowania</param>
        /// <returns>Przekonwertowany serwis do EF</returns>
        TEntity Convert(TService source);

        /// <summary>
        /// Konwertuje model EF do modelu serwisu.
        /// </summary>
        /// <param name="source">Obiekt do przekonwertowania</param>
        /// <returns>Przekonwertowany EF do serwisu</returns>
        TService Convert(TEntity source);
    }
}
