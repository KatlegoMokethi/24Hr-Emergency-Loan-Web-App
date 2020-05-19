namespace BuhleProject.Models.Data
{
    public interface IRepositoryWrapper
    {
        IDocumentRepository DocumentRepository { get; }

        ILoanRepository LoanRepository  { get; }

        ILoanApplicationRepository LoanApplicationRepository  { get; }

        IUserRepository UserRepository { get; }
    }
}
