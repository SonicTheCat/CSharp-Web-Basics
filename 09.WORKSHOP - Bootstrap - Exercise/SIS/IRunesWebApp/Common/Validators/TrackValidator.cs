namespace IRunesWebApp.Common.Validators
{
    public class TrackValidator
    {
        public static bool IsTrackNameValid(string trackName)
        {
            return !string.IsNullOrEmpty(trackName) && !string.IsNullOrWhiteSpace(trackName);
        }

        public static bool IsLinkValid(string link)
        {
            return !string.IsNullOrEmpty(link) && !string.IsNullOrWhiteSpace(link);
        }

        public static bool IsPriceValid(decimal price)
        {
            return price >= 0;
        }
    }
}