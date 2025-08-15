namespace PostgresImageMigration.Models
{
    /// <summary>
    /// Representa uma foto a ser migrada.
    /// Identificacao = nome do arquivo (string)
    /// Conteudo = bytes da imagem (byte[])
    /// </summary>
    public class Foto
    {
        public string Identificacao { get; set; }
        public byte[] Conteudo { get; set; }
    }
}
