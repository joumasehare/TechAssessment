using Microsoft.EntityFrameworkCore;

namespace SettingsManager.Data
{
    public class UnitOfWorkBuilder
    {
        public IUnitOfWork Build(string connectionString)
        {
            DataContext context = new DataContext(GetOptions(connectionString));
            IUnitOfWork unitOfWork = new UnitOfWork { Context = context };
            return unitOfWork;
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), connectionString)
                .Options;
        }
    }
}
