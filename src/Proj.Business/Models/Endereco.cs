namespace DevIO.Business.Models
{
    public class Endereco : Entity
    {
        public Guid FornecedorId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        /* EF Relation */
        public Fornecedor Fornecedor { get; set; }





        public void Update(Endereco endereco)
        {
            this.Logradouro = endereco.Logradouro;
            this.Numero = endereco.Numero;
            this.Complemento = endereco.Numero;
            this.Cep = endereco.Cep;
            this.Bairro = endereco.Bairro;
            this.Cidade = endereco.Cidade;
            this.Estado = endereco.Estado;
        }
    }
}