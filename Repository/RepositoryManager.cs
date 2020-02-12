using System.Threading.Tasks;
using Contracts;
using Entities;

namespace Repository
{

    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICompanyRepository Company => _companyRepository ??= new CompanyRepository(_repositoryContext);
        public IEmployeeRepository Employee => _employeeRepository ??= new EmployeeRepository(_repositoryContext);
        
        public Task SaveAsync() =>  _repositoryContext.SaveChangesAsync();
    }
}