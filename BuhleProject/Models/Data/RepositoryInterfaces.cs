using BuhleProject.Models.Entities;

namespace BuhleProject.Models.Data
{


    public interface ILoanRepository : IRepositoryBase<Loan>
    {
    }

    public interface ILoanApplicationRepository : IRepositoryBase<LoanApplication>
    {
    }

    public interface IUserRepository : IRepositoryBase<User>
    {
    }

    public interface IDocumentRepository : IRepositoryBase<Document>
    {
    }

}
