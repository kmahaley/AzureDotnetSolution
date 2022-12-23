using SqlDbApplication.Repositories.Sql.Interface;
using System;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public interface IFireAndForgetService
    {
        void ExecuteFireAndForgetJob(Func<IProductRepository, Task> jobFunction);
    }
}