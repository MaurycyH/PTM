using System.Collections.ObjectModel;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.Schedule
{
    class GridOverlayViewModel : BindableBase
    {
        /// <summary>
        /// Kolekcja z nazwami kontrolek
        /// </summary>
        public ObservableCollection<string> GridPiece { get; }

        public GridOverlayViewModel()
        {
            GridPiece = new ObservableCollection<string>();
            for(int i=0; i<24; i++)
            {
                GridPiece.Add(i < 10 ? $"0{i}:00" : $"{i}:00");
            }
        }
    }
}
