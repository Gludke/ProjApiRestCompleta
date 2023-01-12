
namespace Proj.Business.Utils
{
    public class Utils 
    {
        private static readonly string _baseDirectory = $"{Directory.GetCurrentDirectory()}/{"wwwroot"}";

        public static void UploadDocBase64(string docBase64, string docName)
        {
            try
            {
                var docByte = Convert.FromBase64String(docBase64);

                var filePath = Path.Combine(GetBaseDirectory(), docName);

                if (System.IO.File.Exists(filePath))
                {
                    throw new Exception("Já existe um arquivo com esse nome");
                }

                System.IO.File.WriteAllBytes(filePath, docByte);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void UploadDocStream(Stream docStream, string docName)
        {
            try
            {
                var docByte = StreamToByte(docStream);

                var filePath = Path.Combine(GetBaseDirectory(), docName);

                if (System.IO.File.Exists(filePath))
                {
                    throw new Exception("Já existe um arquivo com esse nome");
                }

                System.IO.File.WriteAllBytes(filePath, docByte);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string ApenasNumeros(string valor)
        {
            var onlyNumber = "";
            foreach (var s in valor)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }

        public static string GetBaseDirectory()
        {
            if (!Directory.Exists(_baseDirectory)) Directory.CreateDirectory(_baseDirectory);

            return _baseDirectory;
        }

        public static byte[] StreamToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }



    }
}
