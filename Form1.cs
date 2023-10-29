using System.Text;

namespace Stego
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static byte[] ImageToByte(Bitmap img)       // bitmap to byte array
        {
            // Ex. (cat)
            //return new byte[] {0b11111101, 0b00000010, 0b00000000,
            //                  0b00000011, 0b00000001, 0b00000010,
            //                  0b11111100, 0b11111101, 0b00000001,
            //                  0b00000011, 0b00000001, 0b11111100};

            byte[] result = new byte[img.Width * img.Height * 3];

            int i = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color clr = img.GetPixel(x, y);

                    result[i]   = (byte)(clr.R & 0x03);
                    result[++i] = (byte)(clr.G & 0x03);
                    result[++i] = (byte)(clr.B & 0x03);

                    i += 1;
                }
            }

            return result;
        }

        private static byte[] Decode(byte[] temp)
        {
            // Decode text from sequence of bytes
            byte[] result = new byte[temp.Length / 4];

            for (int i = 0, j = 0; i < temp.Length; i += 4, j += 1)
            {
                // Ex.
                // 24, 75, 177, 24 => 52; 24 = 00011000b, LSB = 00
                // 00 << 6 = 00000000, 11 << 4 = 00110000, 01 << 2 = 00000100, 00 << 0 = 00000000
                // 00000000 | 00110000 | 00000100 | 00000000 = 00110100b = 52
                result[j] = (byte)((temp[i] << 6) | (temp[i + 1] << 4) | (temp[i + 2] << 2) | temp[i + 3]);
            }

            return result; // Char codes in ASCII
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("D:\\TestFiles\\1027.png"); // replace with your own
            label2.Text = bmp.PixelFormat.ToString();           // Pixel format ARGB or RGB. (ARGB not work)

            byte[] arr = ImageToByte(bmp);

            
            // Print
            label1.Text = Encoding.ASCII.GetString(Decode(arr)).Replace("\\n", "");    // *.Replace("\\n", "") - formatting, just for me
        }
    }
}