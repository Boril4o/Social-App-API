using System.IO;

namespace SocialAppAPI.Pictures
{
    public static class PictureConverter
    {
        public static byte[] GetBytesFromPicture(string path = "./Pictures/DefaultProfilePicture.jpg")
        {
            // Get the size of the file located at the specified path
            long length = new FileInfo(path).Length;

            // If the file is larger than 600mb, throw an ArgumentException
            if (length > 6e+8)
            {
                throw new ArgumentException("The picture cannot be bigger than 600mb");
            }

            // Read all bytes from the file located at the specified path and return the byte array
            return File.ReadAllBytes(path);
        }
    }
}
