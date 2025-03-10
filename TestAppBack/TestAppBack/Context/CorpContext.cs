using Microsoft.EntityFrameworkCore;


namespace AppControlProduct.DAL.Context
{
    public class CorpContext : DbContext
    {
        public CorpContext()
        {
        }

        public CorpContext(DbContextOptions<CorpContext> options)
            : base(options)
        {
        }


    }
}
