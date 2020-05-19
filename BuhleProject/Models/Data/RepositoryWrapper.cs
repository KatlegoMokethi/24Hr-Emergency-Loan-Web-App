namespace BuhleProject.Models.Data
{
    public class RepositoryWrapper:IRepositoryWrapper
    {
        AppDBcontext AppDBcontext { get; }
        ILoanApplicationRepository applicationRepository;
        ILoanRepository loanRepository;
        IUserRepository userRepository;
        IDocumentRepository documentRepository;
        
        public RepositoryWrapper(AppDBcontext appDBcontext)
        {
            AppDBcontext = appDBcontext;
        }

        public IDocumentRepository DocumentRepository 
        { 
            get 
            {
                if (documentRepository == null)
                {
                    documentRepository = new EF_DocumentRepository(AppDBcontext);
                }
                return documentRepository;
            } 
           
        }

        public ILoanRepository LoanRepository
        {
            get
            {
                if (loanRepository == null)
                {
                    loanRepository = new EF_LoanRepository(AppDBcontext);
                }
                return loanRepository;
            }
        }

        public ILoanApplicationRepository LoanApplicationRepository
        {
            get
            {
                if (applicationRepository == null)
                {
                    applicationRepository = new EF_LoanApplicationRepository(AppDBcontext);
                }
                return applicationRepository;

            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new EF_UserRepository(AppDBcontext);
                }
                return userRepository;

            }
        }

  

    }
}
