namespace IRunesWebApp.Common.Validators
{
    public class AlbumValidator
    {
        public static bool IsAlbumValid(string albumName)
        {
            if (string.IsNullOrEmpty(albumName) || string.IsNullOrWhiteSpace(albumName))
            {
                return false; 
            }

            return true; 
        }

        public static bool IsCoverUrlValid(string coverUrl)
        {
            if (string.IsNullOrEmpty(coverUrl) || string.IsNullOrWhiteSpace(coverUrl))
            {
                return false;
            }

            //if (!coverUrl.StartsWith("http://") && !coverUrl.StartsWith("https://"))
            //{
            //    return false;
            //}

            return true;
        }
    }
}