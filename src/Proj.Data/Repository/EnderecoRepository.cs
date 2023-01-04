using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(MeuDbContext context) : base(context) { }

        public async Task<Endereco> GetAdressByFornecedorId(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        }

        public void RemoveByFornecedorId(Guid fornecedorId)
        {
            var adressDb = DbSet.FirstOrDefault(e => e.FornecedorId == fornecedorId);

            if(adressDb != null) DbSet.Remove(adressDb);
        }
    }
}