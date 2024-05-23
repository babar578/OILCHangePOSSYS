using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Utilities
{
    public static class InvoicePrinter
    {

        public static bool PrintToSmallPrinter(OrderViewModel order)
        {
            bool returnValue = false;
            try
            {
                returnValue = KitchenInoviceFormat(order, 1);
                returnValue = BarInoviceFormat(order, 2);
                returnValue = DessertInoviceFormat(order, 3);
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        public static bool KitchenInoviceFormat(OrderViewModel order, int departmentId)
        {
            bool returnValue = false;
            try
            {
                //var printInfo = UserServices.GetPrintInfoByDepartmentId(departmentId);


                TcpClient clientSocket = new TcpClient();
                clientSocket.Connect("172.20.100.7", 9100);

                StringBuilder sStr = new StringBuilder();
                sStr.Clear();
                sStr.AppendLine("        KITCHEN ORDER        ");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("----------DINE IN------------");
                sStr.AppendLine("");
                sStr.AppendLine("");
                sStr.AppendLine($"TABLE: {order.TableName}");
                sStr.AppendLine($"USER: {order.UserName}");
                sStr.AppendLine($"Order #: {order.Id}");

                DateTime orderDate = (DateTime)order.ModifyDate;
                orderDate.ToString("dd-MMM-yyyy h:mm tt");

                sStr.AppendLine($"Date: {orderDate}");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("");

                var kitchenOrderItems = order.OrderItems.Where(p => p.DepartmentId == departmentId).ToList();

                if (kitchenOrderItems?.Count > 0)
                {
                    foreach (var p in kitchenOrderItems)
                    {
                        sStr.AppendLine($"{p.Quantity} {p.ItemName}");

                    }
                }
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(sStr.ToString());
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                returnValue = true;



                string GS = Convert.ToString((char)29);
                string ESC = Convert.ToString((char)27);
               // -- string documentName = "x";

                string COMMAND = sStr.ToString();
                COMMAND = ESC + "@";
                COMMAND += GS + "V" + (char)1;


                //RawPrinterHelper.SendStringToPrinter(this.textBox1.Text, COMMAND, documentName);
                //RawPrinterHelper.SendStringToPrinter(sStr.ToString(), COMMAND);


            }
            catch (Exception)
            {

                throw;
            }

            return returnValue;
        }

        public static bool BarInoviceFormat(OrderViewModel order, int departmentId)
        {
            bool returnValue = false;
            try
            {
                var printInfo = UserServices.GetPrintInfoByDepartmentId(departmentId);

                TcpClient clientSocket = new TcpClient();
                clientSocket.Connect("172.20.100.7", 9100);

                StringBuilder sStr = new StringBuilder();
                sStr.Clear();
                sStr.AppendLine("          BAR ORDER          ");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("----------DINE IN------------");
                sStr.AppendLine("");
                sStr.AppendLine("");
                sStr.AppendLine($"TABLE: {order.TableName}");
                sStr.AppendLine($"USER: {order.UserName}");
                sStr.AppendLine($"Order #: {order.Id}");

                DateTime orderDate = (DateTime)order.ModifyDate;
                orderDate.ToString("dd-MMM-yyyy h:mm tt");

                sStr.AppendLine($"Date: {orderDate}");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("");

                var barOrderItems = order.OrderItems.Where(p => p.DepartmentId == departmentId).ToList();

                if (barOrderItems?.Count > 0)
                {
                    foreach (var p in barOrderItems)
                    {
                        sStr.AppendLine($"{p.Quantity} {p.ItemName}");

                    }
                }

                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(sStr.ToString());
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                returnValue = true;

            }
            catch (Exception)
            {

                throw;
            }

            return returnValue;
        }

        public static bool DessertInoviceFormat(OrderViewModel order, int departmentId)
        {
            bool returnValue = false;
            try
            {
                var printInfo = UserServices.GetPrintInfoByDepartmentId(departmentId);

                TcpClient clientSocket = new TcpClient();
                clientSocket.Connect("172.20.100.7", 9100);

                StringBuilder sStr = new StringBuilder();
                sStr.Clear();
                sStr.AppendLine("        DESSERT ORDER        ");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("----------DINE IN------------");
                sStr.AppendLine("");
                sStr.AppendLine("");
                sStr.AppendLine($"TABLE: {order.TableName}");
                sStr.AppendLine($"USER: {order.UserName}");
                sStr.AppendLine($"Order #: {order.Id}");

                DateTime orderDate = (DateTime)order.ModifyDate;
                orderDate.ToString("dd-MMM-yyyy h:mm tt");

                sStr.AppendLine($"Date: {orderDate}");
                sStr.AppendLine("-----------------------------");
                sStr.AppendLine("");

                var barOrderItems = order.OrderItems.Where(p => p.DepartmentId == departmentId).ToList();

                if (barOrderItems?.Count > 0)
                {
                    foreach (var p in barOrderItems)
                    {
                        sStr.AppendLine($"{p.Quantity} {p.ItemName}");
                    }
                }

                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(sStr.ToString());
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                returnValue = true;

            }
            catch (Exception)
            {

                throw;
            }

            return returnValue;
        }



    }
}
