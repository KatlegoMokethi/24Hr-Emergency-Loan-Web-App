using BuhleProject.Models.Entities;

namespace BuhleProject.Models.Data
{
    public class EF_Classes
    {
        
    }



    public class EF_LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public EF_LoanRepository(AppDBcontext appDBcontext) : base(appDBcontext)
        {
         }
    }

    public class EF_LoanApplicationRepository : RepositoryBase<LoanApplication>, ILoanApplicationRepository
    {
        public EF_LoanApplicationRepository(AppDBcontext appDBcontext) : base(appDBcontext)
        {
        }
    }

    public class EF_UserRepository : RepositoryBase<User>, IUserRepository
    {
        public EF_UserRepository(AppDBcontext appDBcontext) : base(appDBcontext)
        {
        }
    }

    public class EF_DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public EF_DocumentRepository(AppDBcontext appDBcontext) : base(appDBcontext)
        {
        }
    }


}
