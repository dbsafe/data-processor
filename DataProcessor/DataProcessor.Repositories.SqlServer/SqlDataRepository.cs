using DataProcessor.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataProcessor.Repositories.SqlServer
{
    public class SqlDataRepository : IDataRepository
    {
        public InitializeFileResult InitializeFile(InitializeFileRequest request)
        {
            throw new NotImplementedException();
        }

        public void InsertData(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void InsertHeader(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void InsertTrailer(InsertRowRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetFileStatusWithFileLoadedCompleted(SetFileStatusWithFileLoadedCompletedRequest request)
        {
            throw new NotImplementedException();
        }

        public void SetFileStatusWithWileLoadError(SetFileStatusWithFileLoadErrorRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
