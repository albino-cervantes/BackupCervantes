using BackupAutomaticoCervantes.DestinoBackup;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.DestinoBackup.Ftp;
using BackupAutomaticoCervantes.DestinoBackup.Amazon;
using BackupAutomaticoCervantes.DestinoBackup.OneDrive;

namespace BackupAutomaticoCervantes.DestinoBackup
{


    /// <summary>
    /// Converte JSON para instâncias concretas de IDestinoConfig
    /// sem recursão infinita.
    /// </summary>
    public class DestinoConfigConverter : JsonConverter
    {
        private static readonly Dictionary<DestinoTipo, Type> TipoParaClasse = new Dictionary<DestinoTipo, Type>
    {
        { DestinoTipo.GoogleDrive, typeof(GoogleDriveConfig) },
        { DestinoTipo.Ftp,typeof(FtpConfig)},
        { DestinoTipo.S3,typeof(S3Config)},
        { DestinoTipo.OneDrive,typeof(OneDriveConfig)}
    };

        /// <summary>
        /// Só aplica o conversor quando o tipo pedido for exatamente IDestinoConfig.
        /// Isso impede que o mesmo converter seja reaplicado nos tipos concretos,
        /// quebrando o ciclo de recursão infinita.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            // note a comparação de igualdade, não IsAssignableFrom
            return objectType == typeof(IDestinoConfig);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Estamos em um token StartObject (o elemento da lista), então podemos carregar o JObject
            var jObject = JObject.Load(reader);

            // Extrai o enum DestinoTipo
            if (!jObject.TryGetValue("Tipo", out JToken tipoToken))
                throw new JsonSerializationException("Campo 'Tipo' é obrigatório para desserializar IDestinoConfig.");

            var tipoEnum = (DestinoTipo)tipoToken.ToObject(typeof(DestinoTipo));

            // Mapeia para a classe concreta
            if (!TipoParaClasse.TryGetValue(tipoEnum, out Type tipoConcreto))
                throw new JsonSerializationException($"Tipo '{tipoEnum}' não é suportado.");

            // Desserializa apenas no tipo concreto — aqui não voltamos a passar por CanConvert,
            // já que objectType (IDestinoConfig) ≠ tipoConcreto (p.ex. GoogleDriveDestinoConfig).
            return jObject.ToObject(tipoConcreto, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Simplesmente converte o objeto em JObject e escreve
            JObject obj = JObject.FromObject(value, serializer);
            obj.WriteTo(writer);
        }
    }
}