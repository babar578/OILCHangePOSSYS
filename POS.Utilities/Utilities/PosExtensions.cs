using POS.Database.DatabaseModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Utilities
{
    public static class PosExtensions
    {
        public static void Enlarged(this BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)32);
            bw.Write(text);
            bw.Write(AsciiControlChars.Newline);
        }
        public static void High(this BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)16);
            bw.Write(text); //Width,enlarged
            bw.Write(AsciiControlChars.Newline);
        }
        public static void LargeText(this BinaryWriter bw, string text)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)48);
            bw.Write(text);
            bw.Write(AsciiControlChars.Newline);
        }
        public static void FeedLines(this BinaryWriter bw, int lines)
        {
            bw.Write(AsciiControlChars.Newline);
            if (lines > 0)
            {
                bw.Write(AsciiControlChars.Escape);
                bw.Write('d');
                bw.Write((byte)lines - 1);
            }
        }

        public static void Finish(this BinaryWriter bw)
        {
            bw.FeedLines(1);
            bw.NormalFont("---  Thank You, Come Again ---");
            bw.FeedLines(1);
            bw.Write(AsciiControlChars.Newline);
        }

        public static void NormalFont(this BinaryWriter bw, string text, bool line = true)
        {
            bw.Write(AsciiControlChars.Escape);
            bw.Write((byte)33);
            bw.Write((byte)8);
            bw.Write(" " + text);
            if (line)
                bw.Write(AsciiControlChars.Newline);
        }

        public static string PrinterName
        {
            get { return Environment.MachineName; }
            //get { return @"\\" + Environment.MachineName + @"\POS58"; }
        }

        public static void btnPrintReceipt(OrderViewModel order = null)
        {
            Print(PrinterName, GetDocument(order));
        }

        private static void Print(string printerName, byte[] document)
        {

            NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;

            documentInfo = new NativeMethods.DOC_INFO_1();
            documentInfo.pDataType = "RAW";
            documentInfo.pDocName = "Receipt";

            printerHandle = new IntPtr(0);

            if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
            {
                if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
                {
                    int bytesWritten;
                    byte[] managedData;
                    IntPtr unmanagedData;

                    managedData = document;
                    unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                    Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);

                    if (NativeMethods.StartPagePrinter(printerHandle))
                    {
                        NativeMethods.WritePrinter(
                            printerHandle,
                            unmanagedData,
                            managedData.Length,
                            out bytesWritten);
                        NativeMethods.EndPagePrinter(printerHandle);
                    }
                    else
                    {
                        //throw new Win32Exception();
                    }

                    Marshal.FreeCoTaskMem(unmanagedData);

                    NativeMethods.EndDocPrinter(printerHandle);
                }
                else
                {
                    //throw new Win32Exception();
                }

                NativeMethods.ClosePrinter(printerHandle);
            }
            else
            {
                //throw;
            }

        }

        public static byte[] GetDocument(OrderViewModel order = null)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Reset the printer bws (NV images are not cleared)
                bw.Write(AsciiControlChars.Escape);
                bw.Write('@');

                // Render the logo
                //RenderLogo(bw);
                PrintReceipt(bw, order);

                // Feed 3 vertical motion units and cut the paper with a 1 point cut
                bw.Write(AsciiControlChars.GroupSeparator);
                bw.Write('V');
                bw.Write((byte)66);
                bw.Write((byte)3);

                bw.Flush();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// This is the method we print the receipt the way we want. Note the spaces.
        /// Wasted a lot of paper on this to get it right.
        /// </summary>
        /// <param name="bw"></param>
        public static void PrintReceipt(BinaryWriter bw, OrderViewModel order = null)
        {

            bw.LargeText("     Dock 27");
            bw.LargeText("  Enterprises");
            bw.NormalFont("  M:071-2628126 T:045-2271300");
            bw.FeedLines(1);
            bw.NormalFont("Invoice #: " + order.InvoiceNumber);
            bw.NormalFont("Date: " + order.CreationDate);
            bw.NormalFont("Customer: " + "None");
            bw.FeedLines(1);

            bw.NormalFont("Itm     Qty     Price    Tot");
            bw.NormalFont("-----------------------------");
            foreach (var item in order.OrderItems)
            {
                // var idx = InvoiceItems.IndexOf(item) + 1;
                bw.NormalFont(item.ItemName);
                bw.NormalFont($"       {item.Quantity}   {item.Price}  {item.TotalPrice}", false);

            }
            bw.FeedLines(2);
            bw.NormalFont(" Discount:  " + order.DiscountCharged);
            bw.NormalFont("Sub Total:  " + order.GrossAmount);
            bw.FeedLines(1);
            bw.High("     Total:  " + order.TotalNetAmount);
            bw.FeedLines(1);
            bw.NormalFont("  Payment:  " + "None");
            bw.NormalFont("  Balance:  " + "None");
            bw.Finish();

        }




    }


    /*
27 33 0     ESC ! NUL    Master style: pica                              ESC/P
27 33 1     ESC ! SOH    Master style: elite                             ESC/P
27 33 2     ESC ! STX    Master style: proportional                      ESC/P
27 33 4     ESC ! EOT    Master style: condensed                         ESC/P
27 33 8     ESC ! BS     Master style: emphasised                        ESC/P
27 33 16    ESC ! DLE    Master style: enhanced (double-strike)          ESC/P
27 33 32    ESC ! SP     Master style: enlarged (double-width)           ESC/P
27 33 64    ESC ! @      Master style: italic                            ESC/P
27 33 128   ESC ! ---    Master style: underline                         ESC/P
                 Above values can be added for combined styles.

    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)0);
    bw.Write("test"); //Default, Pica
    bw.Write(AsciiControlChars.Newline);

    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)4);
    bw.Write("test"); //condensed
    bw.Write(AsciiControlChars.Newline);
    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)8);
    bw.Write("test"); //emphasised
    bw.Write(AsciiControlChars.Newline);
    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)16);
    bw.Write("test"); //Height,enhanced
    bw.Write(AsciiControlChars.Newline);
    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)32);
    bw.Write("test"); //Width,enlarged
    bw.Write(AsciiControlChars.Newline);
    bw.Write(AsciiControlChars.Escape);
    bw.Write((byte)33);
    bw.Write((byte)48);
    bw.Write("test");   //WxH
    bw.Write(AsciiControlChars.Newline);
         */
}

