using BrainWave.Core.Entities;
using Syncfusion.Pdf;

namespace BrainWave.Core.Interfaces
{
    public interface IFileService
    {
        public PdfDocument Create(Article article);
    }
}
