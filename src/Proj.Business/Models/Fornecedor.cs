namespace DevIO.Business.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
        public IEnumerable<Produto> Produtos { get; set; }



        public void Update(Fornecedor fornecedor)
        {
            Nome = fornecedor.Nome;
            Documento = fornecedor.Documento;
            TipoFornecedor = fornecedor.TipoFornecedor;
            Ativo = fornecedor.Ativo;

            if(fornecedor.Endereco != null)
            {
                this.Endereco = new Endereco
                {
                    FornecedorId = this.Id,
                    Logradouro = fornecedor.Endereco.Logradouro,
                    Numero = fornecedor.Endereco.Numero,
                    Complemento = fornecedor.Endereco.Complemento,
                    Cep = fornecedor.Endereco.Cep,
                    Bairro = fornecedor.Endereco.Bairro,
                    Cidade = fornecedor.Endereco.Cidade,
                    Estado = fornecedor.Endereco.Estado,
                };
            }

        }


    }
}