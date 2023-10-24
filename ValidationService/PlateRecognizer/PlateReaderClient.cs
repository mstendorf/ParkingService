namespace PlateRecognizer
{
    public class PlateReaderClient
    {
        private static string postUrl = "https://api.platerecognizer.com/v1/plate-reader/";
        private static string regions = "dk";
        private static string token = "your-token-here";

        public PlateReaderClient() { }

        public string GetPlateFromImage(byte[] image)
        {
            PlateReaderResult result = PlateReader.Read(
                postUrl,
                null,
                image,
                regions,
                token,
                false
            );

            if (result.Results.Count == 0)
            {
                return null;
            }

            return result.Results[0].Plate;
        }
    }
}
