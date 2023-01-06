using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                                 IEnderecoRepository enderecoRepository,
                                 INotificador notificador, IProdutoRepository produtoRepository) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> Add(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) 
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return false;

            if (_fornecedorRepository.Find(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return false;
            }

            await _fornecedorRepository.Add(fornecedor);

            await _fornecedorRepository.SaveChanges();
            return true;
        }

        public async Task<bool> Update(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;

            var fornecedorDb = await _fornecedorRepository.GetById(fornecedor.Id);
            if (fornecedorDb == null) return false;

            if (_fornecedorRepository.Find(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento infomado.");
                return false;
            }

            _enderecoRepository.RemoveByFornecedorId(fornecedor.Id);

            fornecedorDb.Update(fornecedor);

            await _fornecedorRepository.Update(fornecedorDb);

            if(fornecedor.Endereco != null) await _enderecoRepository.Add(fornecedorDb.Endereco);

            await _fornecedorRepository.SaveChanges();
            return true;
        }

        public async Task UpdateAdress(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Update(endereco);

            await _fornecedorRepository.SaveChanges();
        }

        public async Task<bool> Remove(Guid id)
        {
            if (_fornecedorRepository.GetFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return false;
            }

            var endereco = await _enderecoRepository.GetAdressByFornecedorId(id);

            if (endereco != null) await _enderecoRepository.Remove(endereco.Id);

            await _fornecedorRepository.Remove(id);

            await _fornecedorRepository.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}