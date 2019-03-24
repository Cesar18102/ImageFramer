using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace PhotoCutter {

    class Program {

        static void Main(string[] args) {

            string PATH = "C:/Server/data/htdocs/Lyrics/pictures/draw_pictures/Cutted/";
            string CuttedDir = "../";
            Color ColorCutted = Color.White;

            if (!Directory.Exists(PATH))
                return;

            if (!Directory.Exists(PATH + CuttedDir))
                Directory.CreateDirectory(PATH + CuttedDir);
            

            DirectoryInfo DI = new DirectoryInfo(PATH);
            FileInfo[] FIS = DI.GetFiles();

            int q = 5;

            foreach (FileInfo FI in FIS)
                if (FI.Extension == ".jpg" || FI.Extension == ".JPG") {

                    Bitmap BMP = new Bitmap(FI.FullName);

                    int[,] D = new int[4, 2] {

                        { 0, 1 },
                        { 1, 0 },
                        { 1, 0 },
                        { 0, 1 }
                    };

                    for (int i = 0; i < 4; i++) {

                        bool ToCut = true;

                        while (ToCut) {

                            int[, ,] StartEnd = new int[4, 2, 2] {

                                { { 0, BMP.Width }, { 0, 1 } },
                                { { 0, 1 }, { 0, BMP.Height } },
                                { { BMP.Width - 1, BMP.Width }, { 0, BMP.Height } },
                                { { 0, BMP.Width }, { BMP.Height - 1, BMP.Height } }
                            };

                            for (int j = StartEnd[i, 0, 0]; j < StartEnd[i, 0, 1]; j++) {
                                for (int k = StartEnd[i, 1, 0]; k < StartEnd[i, 1, 1]; k++) {

                                    Color Pixel = BMP.GetPixel(j, k);
                                    if (Pixel.R != ColorCutted.R || Pixel.G != ColorCutted.G || Pixel.B != ColorCutted.B) {

                                        ToCut = false;
                                        break;
                                    }
                                }

                                if (!ToCut) break;
                            }


                            if (ToCut) {

                                Bitmap BT = new Bitmap(BMP.Width - D[i, 0], BMP.Height - D[i, 1]);
                                Graphics G = Graphics.FromImage(BT);

                                G.DrawImage(BMP, new Rectangle(0, 0, BT.Width, BT.Height), new Rectangle(i == 1? 1 : 0, i == 0? 1 : 0, BMP.Width - (i == 2? 1 : 0), BMP.Height - (i == 3? 1 : 0)), GraphicsUnit.Pixel);

                                G.Save();

                                BMP.Dispose();
                                BMP = BT;
                                G.Dispose();
                            }
                        }

                    }

                    BMP.Save(PATH + CuttedDir + FI.Name, System.Drawing.Imaging.ImageFormat.Jpeg);
                    BMP.Dispose();
                }
        }
    }
}
