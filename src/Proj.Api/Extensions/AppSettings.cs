namespace Proj.Api.Extensions
{
    //Classe de config para o JWT.
    //Deve ser colocada dentro do appsettings.json
    public class AppSettings
    {
        public string Secret { get; set; }//chave de criptografia
        public int ExpiracaoHoras { get; set; }//duração token
        public string Emissor { get; set; }//quem emite (nossa Aplicação)
        public string ValidoEm { get; set; }//urls em q o token é válido
    }
}
