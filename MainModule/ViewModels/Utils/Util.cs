using Entity.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPFScbOri.Models;

namespace MainModule.ViewModels.Utils
{
    public class Util
    {
        public Util(){}

        public void PrintPdf()
        {
            Thread.Sleep(3000);
            string workingDirectory = Environment.CurrentDirectory;

            string pdfpath = System.IO.Path.GetTempPath() + "WPF";

            string imageName = "slip_transfer.png";
            string imagepath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "\\WPFScbOri\\Entity\\Images\\TransferImages\\" + imageName;


            Document doc = new Document(iTextSharp.text.PageSize.A5.Rotate(), 0, 0, 0, 0);
            var w = doc.PageSize.Width;
            var h = doc.PageSize.Height;
            string pdfOpenPath = pdfpath + "\\PDFFile" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf";

            PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(pdfOpenPath, FileMode.Create));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var fJasmineUPC = GetFontJasmineUPC();
            var fJasmineUPCSmall = GetFontJasmineUPCSmall();
            var fJasmineUPCSlash = GetFontJasmineUPCSlash();

            var UsedFont = fJasmineUPC;
            var UsedFontSmall = fJasmineUPCSmall;
            var UsedFontSlash = fJasmineUPCSlash;

            try

            {
                doc.Open();
                pdf.Open();

                PdfContentByte canvas = pdf.DirectContent;

                #region PDF Content
                Image png = Image.GetInstance(imagepath);
                png.SetAbsolutePosition(0, 0);
                png.Alignment = iTextSharp.text.Image.UNDERLYING;
                png.ScaleAbsolute(doc.PageSize.Width, doc.PageSize.Height);

                canvas.AddImage(png);


                //TranDate
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.DateDisplayWithShortMonth, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, -30f, 350f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fix
                {
                    Phrase text = new Phrase(new Chunk("อ่อนนุช", UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 85f, 350f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //TranCode
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.TransCode, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 460f, 350f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fix
                {
                    Phrase text = new Phrase(new Chunk("/", UsedFontSlash));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 59f, 332f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //FullName
                {
                    Phrase text = new Phrase(new Chunk(CustomerDetailTransferManager.GetInstance().customerDetail.AccName, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 65f, 292f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //MobileNo
                {
                    string from = CustomerDetailTransferManager.GetInstance().customerDetail.MobileNo;
                    string _mobileNo = $"{from[0]}{from[1]}{from[2]}-{from[3]}{from[4]}{from[5]}-{from[6]}{from[7]}{from[8]}{from[9]}";
                    Phrase text = new Phrase(new Chunk(_mobileNo, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 425f, 292f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fix
                {
                    Phrase text = new Phrase(new Chunk("/", UsedFontSlash));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 158f, 270f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //FromWalletId
                {
                    string from = TransactionEntityManager.GetInstance().TransactionEntity.FromWalletId;
                    string _fromWalletId = $"{from[0]}{from[1]}{from[2]}{from[3]}{from[4]}-{from[5]}{from[6]}{from[7]}-{from[8]}{from[9]}{from[10]}-{from[11]}{from[12]}{from[13]}{from[14]}";
                    Phrase text = new Phrase(new Chunk(_fromWalletId, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 308f, 265f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fix
                {
                    Phrase text = new Phrase(new Chunk("/", UsedFontSlash));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 544, 270f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Address
                {
                    Phrase text = new Phrase(new Chunk(CustomerDetailTransferManager.GetInstance().customerDetail.Address, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 70, 240f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //CitizenId
                {
                    string from = CustomerDetailTransferManager.GetInstance().customerDetail.CitizenID;
                    string _citizenId = $"{from[0]} {from[1]}{from[2]}{from[3]}{from[4]} {from[5]}{from[6]}{from[7]}{from[8]}{from[9]} {from[10]}{from[11]} {from[12]}";
                    Phrase text = new Phrase(new Chunk(_citizenId, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 480, 240f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //ToWalletName
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.ToWalletName, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 70, 205f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fix
                {
                    Phrase text = new Phrase(new Chunk("ดุสิต", UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 70, 178f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //ToWalletId
                {
                    string from = TransactionEntityManager.GetInstance().TransactionEntity.ToWalletId;
                    string _toWalletId = $"{from[0]}{from[1]}{from[2]}{from[3]}{from[4]}-{from[5]}{from[6]}{from[7]}-{from[8]}{from[9]}{from[10]}-{from[11]}{from[12]}{from[13]}{from[14]}";
                    Phrase text = new Phrase(new Chunk(_toWalletId, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 345, 178f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Amount Number Format
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.AmountDisplay, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 100, 128f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Amount String Format
                {
                    string _amountString = "(" + ChangeDecimalToText(TransactionEntityManager.GetInstance().TransactionEntity.Amount, SaleType.Baht) + ")";
                    Phrase text = new Phrase(new Chunk(_amountString, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 355, 128f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Memo
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.Memo, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 105, 102f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Fee
                {
                    Phrase text = new Phrase(new Chunk(TransactionEntityManager.GetInstance().TransactionEntity.Fee3AmountDisplay, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 174, 26f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Amount + Fee
                {
                    decimal de2 = Convert.ToDecimal(TransactionEntityManager.GetInstance().TransactionEntity.TotalAmount);
                    string tempAmount2 = de2.ToString("N");
                    string amountFee = tempAmount2;

                    Phrase text = new Phrase(new Chunk(amountFee, UsedFont));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 295, 26f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Employee Name
                {
                    Phrase text = new Phrase(new Chunk(AccountManager.GetInstance().Account.name, UsedFontSmall));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 385, 18.5f, pdf.DirectContent);
                    pdf.Add(table);
                }

                //Employee Id
                {
                    Phrase text = new Phrase(new Chunk(AccountManager.GetInstance().Account.id.ToString(), UsedFontSmall));
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 200f;
                    PdfPCell cell = new PdfPCell(text);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, 385, 12f, pdf.DirectContent);
                    pdf.Add(table);
                }
                #endregion


                doc.Close();

                //open pdf file
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(pdfOpenPath)
                {
                    UseShellExecute = true
                };
                p.Start();

            }

            catch (Exception ex)
            {
                //Log error;

            }
        }

        public static iTextSharp.text.Font GetFontJasmineUPC()
        {
            string fontName = "jasmineUPC";
            if (!FontFactory.IsRegistered(fontName))
            {
                string workingDirectory = Environment.CurrentDirectory;
                string fontttf = "upcjl.ttf";
                var fontPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "\\WPFScbOri\\Entity\\Fonts\\" + fontttf;
                FontFactory.Register(fontPath, fontName);
            }

            var FontColour = new BaseColor(0, 0, 0); // optional... ints 0, 0, 0 are red, green, blue
            int FontStyle = iTextSharp.text.Font.NORMAL;  // optional
            float FontSize = iTextSharp.text.Font.DEFAULTSIZE;  // optional

            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, FontSize, FontStyle, FontColour);
            // last 3 arguments can be removed
        }

        public static iTextSharp.text.Font GetFontJasmineUPCSmall()
        {
            string fontName = "jasmineUPC";
            if (!FontFactory.IsRegistered(fontName))
            {
                string workingDirectory = Environment.CurrentDirectory;
                string fontttf = "upcjl.ttf";
                var fontPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "\\WPFScbOri\\Entity\\Fonts\\" + fontttf;
                FontFactory.Register(fontPath, fontName);
            }

            var FontColour = new BaseColor(0, 0, 0); // optional... ints 0, 0, 0 are red, green, blue
            int FontStyle = iTextSharp.text.Font.NORMAL;  // optional
            float FontSize = 8;  // optional

            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, FontSize, FontStyle, FontColour);
            // last 3 arguments can be removed
        }

        public static iTextSharp.text.Font GetFontJasmineUPCSlash()
        {
            string fontName = "jasmineUPC";
            if (!FontFactory.IsRegistered(fontName))
            {
                string workingDirectory = Environment.CurrentDirectory;
                string fontttf = "upcjl.ttf";
                var fontPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "\\WPFScbOri\\Entity\\Fonts\\" + fontttf;
                FontFactory.Register(fontPath, fontName);
            }

            var FontColour = new BaseColor(0, 0, 0); // optional... ints 0, 0, 0 are red, green, blue
            int FontStyle = iTextSharp.text.Font.NORMAL;  // optional
            float FontSize = 20;  // optional

            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, FontSize, FontStyle, FontColour);
            // last 3 arguments can be removed
        }

        public string ChangeDecimalToText(decimal vol, SaleType type)
        {
            string result = string.Empty;
            string finalText = string.Empty;
            string middleText = string.Empty;
            Func<string, string> func;

            string numtext = vol.ToString("0.####");
            if (numtext.Contains('.'))
            {
                if (type == SaleType.Unit)
                {
                    middleText = "จุด";
                    finalText = "หน่วย";
                    func = new Func<string, string>(ChangeDecimalToTextAfterMiddel);
                }
                else if (type == SaleType.Baht)
                {
                    numtext = vol.ToString("0.##");
                    middleText = "บาท";
                    finalText = "สตางค์";
                    func = new Func<string, string>(ChangeDecimalToTextBeforeMiddel);
                }
                else
                {
                    return "";
                }
                string[] numtextArray = numtext.Split('.');
                if (numtextArray.Length > 1)
                {
                    result = ChangeDecimalToTextBeforeMiddel(numtextArray[0]) + middleText + func(numtextArray[1]) + finalText;
                }
                else
                {
                    result = ChangeDecimalToTextSingle(numtext, type);
                }
            }
            else
            {
                result = ChangeDecimalToTextSingle(numtext, type);
            }

            return result;
        }

        private string ChangeDecimalToTextSingle(string num, SaleType type)
        {
            string result = string.Empty;
            string finalText = string.Empty;
            if (type == SaleType.Unit)
            {
                finalText = "หน่วย";
            }
            else if (type == SaleType.Baht)
            {
                finalText = "บาทถ้วน";
            }
            else
            {
                return "";
            }
            result = ChangeDecimalToTextBeforeMiddel(num) + finalText;
            return result;
        }

        private string ChangeDecimalToTextBeforeMiddel(string num)
        {
            string result = string.Empty;

            if (Convert.ToDecimal(num) != 0m)
            {
                string[] thaiNum = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
                string[] thaiPosition = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
                char[] singleText = num.ToCharArray();
                int totalChar = singleText.Length;
                int loopCount = 0;
                for (int i = 0; i < totalChar; i++)
                {
                    int singleNum = ChangeCharToInt(singleText[totalChar - i - 1]);
                    if (singleNum == 0 && singleNum != -1)
                    {
                        loopCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                loopCount = totalChar - loopCount;
                for (int i = 0; i < loopCount; i++)
                {
                    int position = (totalChar - i - 1);
                    if (position > 1)
                    {
                        int adjustPos = (position) % 6;
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1)
                        {
                            if (adjustPos == 0)
                            {
                                if (singleNum != 0)
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[6]);
                                }
                                else
                                {
                                    result += thaiPosition[6];
                                }
                            }
                            else
                            {
                                if (singleNum != 0)
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[adjustPos]);
                                }
                            }
                        }
                    }
                    else if (position == 1)
                    {
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1 || singleNum != 0)
                        {
                            if (singleNum != 0)
                            {
                                if (singleNum == 1)
                                {
                                    result += thaiPosition[1];
                                }
                                else if (singleNum == 2)
                                {
                                    result += ("ยี่" + thaiPosition[1]);
                                }
                                else
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[1]);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1)
                        {
                            if (totalChar > 1 && singleNum == 1)
                            {
                                result += "เอ็ด";
                            }
                            else
                            {
                                result += thaiNum[singleNum];
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                result = "ศูนย์";
            }

            return result;
        }

        private string ChangeDecimalToTextAfterMiddel(string num)
        {
            string result = string.Empty;
            string[] thaiNum = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            char[] singleText = num.ToCharArray();
            foreach (var text in singleText)
            {
                int singleNum = ChangeCharToInt(text);
                if (singleNum != -1)
                {
                    result += thaiNum[singleNum];
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private int ChangeCharToInt(char text)
        {
            try
            {
                return Int32.Parse(text.ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
