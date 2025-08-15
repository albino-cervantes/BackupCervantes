using MigracaoPostgreSQL.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Services
{
    /// <summary>
    /// Serviço responsável pelo carregamento e processamento de fotos
    /// </summary>
    public class PhotoService
    {
        private readonly Logger _logger;
        private const string BASE_PHOTOS_PATH = @"C:\FotosProdutos\";

        public PhotoService(Logger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Carrega uma foto do sistema de arquivos e converte para byte array
        /// </summary>
        public async Task<byte[]> LoadPhotoAsync(string photoFileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(photoFileName))
                    return null;

                var fullPath = Path.Combine(BASE_PHOTOS_PATH, photoFileName);

                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning($"Arquivo de foto não encontrado: {fullPath}");
                    return null;
                }

                var photoBytes = File.ReadAllBytes(fullPath);

                //var photoBytes = await Task.Run(() => File.ReadAllBytes(fullPath));

                if (photoBytes.Length == 0)
                {
                    _logger.LogWarning($"Arquivo de foto vazio: {fullPath}");
                    return null;
                }

                _logger.LogInfo($"Foto carregada com sucesso: {photoFileName} ({photoBytes.Length} bytes)");
                return photoBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao carregar foto {photoFileName}: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Valida se o arquivo é uma imagem válida baseado na extensão
        /// </summary>
        public bool IsValidImageFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                   extension == ".webp" || extension == ".bmp" || extension == ".gif";
        }

        /// <summary>
        /// Cria o diretório base para fotos se não existir
        /// </summary>
        public void EnsurePhotosDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(BASE_PHOTOS_PATH))
                {
                    Directory.CreateDirectory(BASE_PHOTOS_PATH);
                    _logger.LogInfo($"Diretório de fotos criado: {BASE_PHOTOS_PATH}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar diretório de fotos: {ex.Message}", ex);
            }
        }
    }
}
