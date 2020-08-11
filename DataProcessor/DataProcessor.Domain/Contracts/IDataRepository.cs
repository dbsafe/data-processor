using DataProcessor.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProcessor.Domain.Contracts
{
    public interface IDataRepository
    {
        Task<InitializeFileResult> InitializeFileAsync(InitializeFileRequest request);

        void SetFileStatusWithWileLoadError(SetFileStatusWithFileLoadErrorRequest request);

        void InsertHeader(InsertRowRequest request);
        void InsertData(InsertRowRequest request);
        void InsertTrailer(InsertRowRequest request);
        void SetFileStatusWithFileLoadedCompleted(SetFileStatusWithFileLoadedCompletedRequest request);
    }

    public class SetFileStatusWithFileLoadedCompletedRequest
    {
        public Guid FileId { get; set; }
        public IList<string> Errors { get; set; }
        public ValidationResultType ValidationResult { get; set; }
    }

    public class SetFileStatusWithFileLoadErrorRequest
    {
        public Guid FileId { get; set; }
        public string Error { get; set; }
    }

    public class InsertRowRequest
    {
        public Guid FileId { get; set; }
        public ValidationResultType? ValidationResult { get; set; }
        public string Raw { get; set; }
        public string Decoded { get; set; }
        public IList<string> Errors { get; set; }
    }

    public class InitializeFileResult
    {
        public Guid FileId { get; set; }
    }

    public class InitializeFileRequest
    {
        public string Path { get; set; }
        public long SizeBytes { get; set; }
        public FileStatusType Status { get; set; }
        public DateTime DateTime { get; set; }
    }

    public enum FileStatusType
    {
        Importing,
        Imported
    }
}
